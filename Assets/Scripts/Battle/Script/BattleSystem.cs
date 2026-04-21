using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleSystem : MonoBehaviour
{
  [SerializeField] private InventoryTetris inventoryTetris;
  [SerializeField] private CooldownModifierSystem modifierSystem;
  [SerializeField] private ItemBattleDataSO[] battleDataSOs;

  private readonly Dictionary<PlacedObject, BattleItemRunner> runners = new();
  private bool inBattle;

  private void Start()
  {
    inventoryTetris.OnObjectPlaced += OnItemPlaced;
    inventoryTetris.OnObjectRemoved += OnItemRemoved;
  }

  private void OnDestroy()
  {
    inventoryTetris.OnObjectPlaced -= OnItemPlaced;
    inventoryTetris.OnObjectRemoved -= OnItemRemoved;
  }

  private void OnItemPlaced(object sender, PlacedObject placedObject)
  {
    if (runners.ContainsKey(placedObject)) return;

    var itemSO = placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO;
    if (itemSO == null) return;

    var battleData = FindBattleData(itemSO.name);
    if (battleData == null)
    {
      Debug.LogWarning($"ไม่พบ BattleData สำหรับ {itemSO.name}");
      return;
    }

    var runner = new BattleItemRunner(battleData, modifierSystem);

    runner.OnAction += log => Debug.Log($"Action: {log.item} -> {log.type}");
    runner.OnCooldownProgress += progress =>
    {
      placedObject.SetCooldownVisual(progress);
    };

    runners.Add(placedObject, runner);
  }

  void OnItemRemoved(object sender, PlacedObject placedObject)
  {
    if (!runners.ContainsKey(placedObject))
      return;

    var runner = runners[placedObject];

    runner.Stop();

    runners.Remove(placedObject);
  }

  private void Update()
  {
    if (Keyboard.current.tKey.wasPressedThisFrame)
    {
      modifierSystem.ApplyModifier(
        CooldownModifierType.SpeedUp,
        1.5f,
        5f);

      Debug.Log("Speed Buff Applied");
      modifierSystem.DebugPrintModifiers();
    }

    if (Keyboard.current.yKey.wasPressedThisFrame)
    {
      modifierSystem.ApplyModifier(
        CooldownModifierType.SlowDown,
        -0.5f,
        5f);

      Debug.Log("Slow Debuff Applied");
      modifierSystem.DebugPrintModifiers();
    }

    if (Keyboard.current.sKey.wasPressedThisFrame)
      StartTurn();

    if (Keyboard.current.dKey.wasPressedThisFrame)
      EndTurn();

    if (!inBattle) return;
    float dt = Time.deltaTime;
    foreach (var r in runners.Values) r.Tick(dt);
  }

  private void StartTurn()
  {
    if (inBattle) return;
    inBattle = true;
    Debug.Log("=== Turn Start ===");
    foreach (var r in runners.Values) r.Start();
  }

  private void EndTurn()
  {
    if (!inBattle) return;
    inBattle = false;
    Debug.Log("=== Turn End ===");
    foreach (var r in runners.Values) r.Stop();
  }

  private ItemBattleDataSO FindBattleData(string itemName)
  {
    foreach (var so in battleDataSOs)
      if (so.itemName == itemName) return so;
    return null;
  }
}
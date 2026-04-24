using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
  [Header("Inventory")]
  [SerializeField] private InventoryTetris inventoryTetris;
  [SerializeField] private ItemBattleDataSO[] battleDataSOs;

  [Header("Modifier")]
  [SerializeField] private CooldownModifierSystem modifierSystem;

  [Header("Battle Setup")]
  [SerializeField] private int playerMaxHp = 100;
  [SerializeField] private Button bootButton;

  [Header("Floating Text")]
  [SerializeField] private FloatingTextConfig floatingTextConfig;

  private FloatingTextService floatingTextService;

  // เพิ่ม field
  private CharacterView playerView;

  private int goldRewardOnWin = 20;

  // units
  private CombatUnit player;
  private readonly List<CombatUnit> enemies = new();

  // runners
  private readonly Dictionary<PlacedObject, BattleItemRunner> playerRunners = new();
  private readonly Dictionary<CombatUnit, List<BattleItemRunner>> enemyRunners = new();

  private bool inBattle;

  public event Action<BattleActionLog> OnBattleAction;
  public static event Action<bool> OnBattleEnded;
  // ─────────────────────────────────────────
  // Unity lifecycle
  // ─────────────────────────────────────────

  private void Start()
  {
    CreatePlayerUnit();

    floatingTextService = new FloatingTextService(
      floatingTextConfig.Prefab,
      floatingTextConfig.Styles
    );

    inventoryTetris.OnObjectPlaced += OnItemPlaced;
    inventoryTetris.OnObjectRemoved += OnItemRemoved;

    bootButton.onClick.AddListener(BootEnergyDrink);

    GameStateManager.OnStateChanged += OnStateChanged;
  }

  // method ใหม่ — เรียกจาก BattleSceneSpawner
  public void BindPlayerView(CharacterView view)
  {
    playerView = view;

    if (player == null) CreatePlayerUnit();
    player.BindView(view);
  }

  private void CreatePlayerUnit()
  {
    player = new CombatUnit("Player", playerMaxHp);
    player.OnHpChanged += (cur, max) => Debug.Log($"[Player HP] {cur}/{max}");
    player.OnDied += _ => EndBattle("Enemy");
  }

  public void InitializedEnemy(List<EnemyUnitSO> enemys, List<CharacterView> views, int goldRewardOnWin)
  {
    enemies.Clear();
    enemyRunners.Clear();

    this.goldRewardOnWin = goldRewardOnWin;

    for (int i = 0; i < enemys.Count; i++)
    {
      var enemyUnitSO = enemys[i];
      var view = i < views.Count ? views[i] : null;

      EnemyPosition position = i == 0
          ? EnemyPosition.Front
          : EnemyPosition.Back;

      var newEnemy = new CombatUnit(
          enemyUnitSO.enemyName,
          enemyUnitSO.hp,
          position,
          view);

      enemies.Add(newEnemy);

      newEnemy.OnHpChanged += (cur, max) =>
          Debug.Log($"[{newEnemy.unitName} HP] {cur}/{max}");

      newEnemy.OnDied += _ =>
      {
        enemies.Remove(newEnemy);
        RefreshEnemyFormation();
        if (enemies.Count == 0) EndBattle("Player");
      };

      foreach (var actionSO in enemyUnitSO.actions)
      {
        var runner = new BattleItemRunner(actionSO);
        runner.OnAction += ProcessAction;

        if (!enemyRunners.ContainsKey(newEnemy))
          enemyRunners[newEnemy] = new List<BattleItemRunner>();

        enemyRunners[newEnemy].Add(runner);
      }
    }
  }
  private void OnDestroy()
  {
    inventoryTetris.OnObjectPlaced -= OnItemPlaced;
    inventoryTetris.OnObjectRemoved -= OnItemRemoved;

    GameStateManager.OnStateChanged -= OnStateChanged;
  }

  private void OnStateChanged(GameState prev, GameState next)
  {
    if (next == GameState.Battle) StartTurn();
    if (prev == GameState.Battle && next != GameState.Battle) EndTurn();
  }

  private void Update()
  {

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
    foreach (var r in playerRunners.Values) r.Tick(dt);
    foreach (var runners in enemyRunners.Values)
      foreach (var r in runners) r.Tick(dt);
  }

  private void BootEnergyDrink()
  {
    if(!PlayerData.Instance.HasEnergyDrink) return;

    PlayerData.Instance.ConsumeEnergyDrink();
    modifierSystem.ApplyModifier(
      CooldownModifierType.SpeedUp,
      0.5f,
      5f);
  }

  private void OnItemPlaced(object sender, PlacedObject placedObject)
  {
    if (playerRunners.ContainsKey(placedObject)) return;

    var itemSO = placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO;
    if (itemSO == null) return;

    var battleData = FindBattleData(itemSO.name);
    if (battleData == null)
    {
      Debug.LogWarning($"ไม่พบ BattleData สำหรับ {itemSO.name}");
      return;
    }

    var runner = new BattleItemRunner(battleData, modifierSystem);

    runner.OnAction += ProcessAction;
    runner.OnCooldownProgress += p => placedObject.SetCooldownVisual(p);
    playerRunners.Add(placedObject, runner);
  }
  private void OnItemRemoved(object sender, PlacedObject placedObject)
  {
    if (!playerRunners.TryGetValue(placedObject, out var runner)) return;
    runner.Stop();
    playerRunners.Remove(placedObject);
  }

  // ─────────────────────────────────────────
  // Turn control
  // ─────────────────────────────────────────

  private void StartTurn()
  {
    if (inBattle) return;
    inBattle = true;
    Debug.Log("=== Turn Start ===");
    foreach (var r in playerRunners.Values) r.Start();
    foreach (var runners in enemyRunners.Values)
      foreach (var r in runners) r.Start();
  }

  private void EndTurn()
  {
    if (!inBattle) return;
    inBattle = false;
    Debug.Log("=== Turn End ===");
    foreach (var r in playerRunners.Values) r.Stop();
    foreach (var runners in enemyRunners.Values)
      foreach (var r in runners) r.Stop();
  }

  private void EndBattle(string winner)
  {
    inBattle = false;
    foreach (var r in playerRunners.Values) r.Stop();
    foreach (var runners in enemyRunners.Values)
      foreach (var r in runners) r.Stop();

    bool playerWon = winner == "Player";
    Debug.Log($"=== Battle End — {winner} wins! ===");
    player.RefillHp();

    if (playerWon && PlayerData.Instance != null)
      PlayerData.Instance.AddGold(goldRewardOnWin);

    OnBattleEnded?.Invoke(playerWon);
  }

  // ─────────────────────────────────────────
  // Action resolver
  // ─────────────────────────────────────────

  private void ProcessAction(BattleActionLog log)
  {
    Debug.Log($"{{item: \"{log.item}\", value: {log.value}, type: {log.type}, target: {log.targetType}}}");
    OnBattleAction?.Invoke(log);

    bool isPlayerAction = IsPlayerItem(log.item);

    CombatUnit attacker = isPlayerAction ? player : enemies[0];

    if (log.targetType == TargetType.AllEnemies)
    {
      ApplyDamageAllEnemies(log, isPlayerAction);
      Debug.Log($"AOE HIT {enemies.Count} enemies");
      return;
    }

    CombatUnit defender = ResolveTarget(log, isPlayerAction);

    if (defender == null) return;
    if (attacker.IsDead || defender.IsDead) return;

    switch (log.type)
    {
      case BattleActionType.Damage:
        ApplyDamage(log, defender);

        SpawnFloatingText(
          FloatingTextType.Damage,
          defender,
          log.value
        );

        break;

      case BattleActionType.Heal:
        attacker.Heal(log.value);

        SpawnFloatingText(
          FloatingTextType.Heal,
          attacker,
          log.value
        );
        break;

      case BattleActionType.ApplyStatusEffect:
        ApplyStatusEffect(log, defender);
        break;

      case BattleActionType.Gold:
        Debug.Log($"[Gold] +{log.value}");
        break;
    }
  }

  private void ApplyDamage(
  BattleActionLog log,
  CombatUnit defender)
  {
    defender.TakeDamage(log.value);
  }

  private CombatUnit ResolveTarget(
  BattleActionLog log,
  bool isPlayerAction)
  {
    if (!isPlayerAction)
      return player;

    switch (log.targetType)
    {
      case TargetType.FrontEnemy:
        return enemies.Find(e =>
          e.Position == EnemyPosition.Front);

      case TargetType.BackEnemy:
        return enemies.Find(e =>
          e.Position == EnemyPosition.Back);

      case TargetType.RandomEnemy:
        if (enemies.Count == 0)
          return null;
        return enemies[
          UnityEngine.Random.Range(0, enemies.Count)
        ];

      default:
        return enemies.Count > 0
          ? enemies[0]
          : null;
    }
  }
  private void ApplyStatusEffect(
  BattleActionLog log,
  CombatUnit defender)
  {
    if (!enemyRunners.ContainsKey(defender))
      return;

    foreach (var runner in enemyRunners[defender])
    {
      runner.ApplyStatusEffect(
        log.statusEffect,
        log.duration);

      switch (log.statusEffect)
      {
        case StatusEffectType.Freeze:

          SpawnFloatingText(
            FloatingTextType.Freeze,
            defender,
            0
          );

          break;

        case StatusEffectType.SlowAttack:

          SpawnFloatingText(
            FloatingTextType.Slow,
            defender,
            0
          );

          break;
      }
    }
  }


  // ─────────────────────────────────────────
  // View
  // ─────────────────────────────────────────

  private async void SpawnFloatingText(
  FloatingTextType type,
  CombatUnit target,
  int value)
  {
    if (target == null) return;

    await floatingTextService.Spawn(
      type,
      target.WorldPosition,
      value
    );
  }

  // ─────────────────────────────────────────
  // Helpers
  // ─────────────────────────────────────────

  private void RefreshEnemyFormation()
  {
    for (int i = 0; i < enemies.Count; i++)
    {
      enemies[i].SetPosition(
        i < 2
        ? EnemyPosition.Front
        : EnemyPosition.Back);
    }
  }
  private void ApplyDamageAllEnemies(
  BattleActionLog log,
  bool isPlayerAction)
  {
    if (isPlayerAction)
    {
      var snapshot = new List<CombatUnit>(enemies);

      foreach (var enemy in snapshot)
      {
        if (enemy.IsDead) continue;

        enemy.TakeDamage(log.value);

        SpawnFloatingText(
          FloatingTextType.Damage,
          enemy,
          log.value
        );
      }
    }
    else
    {
      player.TakeDamage(log.value);

      SpawnFloatingText(
        FloatingTextType.Damage,
        player,
        log.value
      );
    }
  }

  private bool IsPlayerItem(string itemName)
  {
    foreach (var so in battleDataSOs)
      if (so.itemName == itemName) return true;
    return false;
  }

  private ItemBattleDataSO FindBattleData(string itemName)
  {
    foreach (var so in battleDataSOs)
      if (so.itemName == itemName) return so;
    return null;
  }
}
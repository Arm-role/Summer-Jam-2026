using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
  public static PlayerData Instance { get; private set; }

  public event Action<int> OnGoldChanged;
  public event Action<ItemBattleDataSO, int> OnConsumableChanged;

  [Header("Starting Values")]
  [SerializeField] private int startingGold = 50;

  private int currentGold;
  public int Gold => currentGold;

  private readonly Dictionary<ItemBattleDataSO, int> consumableStacks = new();

  private void Awake()
  {
    if (Instance != null && Instance != this) { Destroy(gameObject); return; }
    Instance = this;
  }

  private void Start() => ResetData();

  public void ResetData()
  {
    currentGold = startingGold;
    consumableStacks.Clear();
    OnGoldChanged?.Invoke(currentGold);
  }

  // ── Gold ──────────────────────────────────────────────────────────────
  public bool CanAfford(int amount) => currentGold >= amount;

  public bool TrySpend(int amount)
  {
    if (!CanAfford(amount)) return false;
    currentGold -= amount;
    OnGoldChanged?.Invoke(currentGold);
    return true;
  }

  public void AddGold(int amount)
  {
    if (amount <= 0) return;
    currentGold += amount;
    OnGoldChanged?.Invoke(currentGold);
  }

  // ── Consumable ────────────────────────────────────────────────────────
  public void AddConsumable(ItemBattleDataSO item)
  {
    if (!consumableStacks.ContainsKey(item))
      consumableStacks[item] = 0;

    consumableStacks[item]++;
    OnConsumableChanged?.Invoke(item, consumableStacks[item]);
  }

  public bool HasConsumable(ItemBattleDataSO item)
      => consumableStacks.TryGetValue(item, out var count) && count > 0;

  public bool TryConsumeItem(ItemBattleDataSO item)
  {
    if (!HasConsumable(item)) return false;
    consumableStacks[item]--;
    OnConsumableChanged?.Invoke(item, consumableStacks[item]);
    return true;
  }

  public int GetConsumableCount(ItemBattleDataSO item)
      => consumableStacks.TryGetValue(item, out var count) ? count : 0;
}
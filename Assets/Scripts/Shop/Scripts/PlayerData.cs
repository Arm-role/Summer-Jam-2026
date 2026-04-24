using System;
using TMPro;
using UnityEngine;
/// <summary>
/// เก็บข้อมูล runtime ของ Player
/// Singleton + DontDestroyOnLoad → คงอยู่ข้าม scene
/// เรียก ResetData() ตอน restart
/// </summary>
public class PlayerData : MonoBehaviour
{
  public static PlayerData Instance { get; private set; }

  public event Action<int> OnGoldChanged;

  [Header("Starting Values")]
  [SerializeField] private int startingGold = 50;

  [Header("UI")]
  [SerializeField] private TMP_Text energyCount;


  private int energyDrink;
  public bool HasEnergyDrink => energyDrink > 0;

  private int currentGold;
  public int Gold => currentGold;

  private void Awake()
  {
    if (Instance != null && Instance != this) { Destroy(gameObject); return; }
    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void Start()
  {
    ResetData();
  }

  public void ResetData()
  {
    currentGold = startingGold;
    energyCount.text = energyDrink.ToString();
    OnGoldChanged?.Invoke(currentGold);
  }

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
    Debug.Log($"[PlayerData] +{amount}G | รวม {currentGold}G");
  }

  public void TryBuyEnergyDrink()
  {
    energyDrink++;
    energyCount.text = energyDrink.ToString();
  }
  public void ConsumeEnergyDrink()
  {
    energyDrink--;
    energyCount.text = energyDrink.ToString();
  }
}

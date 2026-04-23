using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ระบบ Shop หลัก
/// TryBuy() → หักเงิน → Spawn DroppedWorldItem ที่ spawnPoint
/// </summary>
public class ShopSystem : MonoBehaviour
{
  public static ShopSystem Instance { get; private set; }

  public event Action<ShopItemSO> OnItemBought;
  public event Action<string> OnBuyFailed;

  [Header("Shop Items (กำหนดใน Inspector)")]
  [SerializeField] private List<ShopItemSO> shopItems;

  [Header("Spawn Point")]
  [Tooltip("item จะ spawn รอบๆ จุดนี้ในโลกเกม")]
  [SerializeField] private Transform spawnPoint;
  [SerializeField] private float spawnScatter = 0.4f;

  private void Awake() => Instance = this;

  public IReadOnlyList<ShopItemSO> ShopItems => shopItems;

  public bool TryBuy(ShopItemSO shopItem)
  {
    if (shopItem?.itemTetrisSO == null)
    {
      OnBuyFailed?.Invoke("Item data missing");
      return false;
    }

    if (PlayerData.Instance == null || !PlayerData.Instance.CanAfford(shopItem.price))
    {
      OnBuyFailed?.Invoke("Not enough money!");
      return false;
    }

    PlayerData.Instance.TrySpend(shopItem.price);

    Vector3 pos = GetSpawnPos();
    DroppedWorldItem.Spawn(shopItem.itemTetrisSO, pos);

    OnItemBought?.Invoke(shopItem);
    Debug.Log($"[Shop] ซื้อ {shopItem.DisplayName} {shopItem.price}G | เหลือ {PlayerData.Instance.Gold}G");
    return true;
  }

  private Vector3 GetSpawnPos()
  {
    Vector3 origin = spawnPoint != null ? spawnPoint.position : transform.position;
    Vector2 scatter = UnityEngine.Random.insideUnitCircle * spawnScatter;
    return origin + new Vector3(scatter.x, scatter.y, 0f);
  }
}

using UnityEngine;

public class ShopItemButton : MonoBehaviour
{
  [Header("Item")]
  [SerializeField] private ItemTetrisSO itemSO;

  [Header("Spawner")]
  [SerializeField] private DroppedWorldItemSpawner spawner;

  public void OnClickBuyItem()
  {
    if (itemSO == null)
    {
      Debug.LogWarning("ItemSO missing!");
      return;
    }

    if (spawner == null)
    {
      Debug.LogWarning("Spawner missing!");
      return;
    }

    spawner.Spawn(itemSO);
  }
}

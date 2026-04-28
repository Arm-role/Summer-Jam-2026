using UnityEngine;
using UnityEngine.UI;

public class ShopItemCardUI2 : MonoBehaviour
{
  [SerializeField] private ItemBattleDataSO consumable;
  [SerializeField] private ShopItemSO shopItem;

  [SerializeField] private Button buyButton;

  private void Start()
  {
    buyButton.onClick.AddListener(OnBuyClicked);
    Refresh(PlayerData.Instance.Gold);

    if (PlayerData.Instance != null)
      PlayerData.Instance.OnGoldChanged += Refresh;

    var tooltipTrigger = gameObject.AddComponent<TooltipTrigger>();
    tooltipTrigger.SetData(new ShopItemTooltipData(shopItem, consumable));
  }

  private void OnDestroy()
  {
    if (PlayerData.Instance != null)
      PlayerData.Instance.OnGoldChanged -= Refresh;
  }

  private void OnBuyClicked()
  {
    if (PlayerData.Instance.TrySpend(shopItem.price))
      PlayerData.Instance.AddConsumable(consumable);
  }

  private void Refresh(int gold)
  {
    if (buyButton != null)
      buyButton.interactable = PlayerData.Instance.CanAfford(shopItem.price);
  }
}
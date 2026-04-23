using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Card UI สำหรับ item แต่ละชิ้นใน Shop
///
/// Prefab ควรมี:
///   Image        → iconImage
///   TMP_Text × 4 → nameText, sizeText, priceText, descriptionText
///   Button       → buyButton
///   GameObject   → cantAffordOverlay (optional, dim เมื่อไม่มีเงิน)
/// </summary>
public class ShopItemCardUI : MonoBehaviour
{
  [SerializeField] private TMP_Text sizeText;
  [SerializeField] private TMP_Text priceText;
  [SerializeField] private Button buyButton;
  [SerializeField] private ShopItemSO shopItem;

  // ── Setup ─────────────────────────────────────────────────────────────────

  public void Start()
  {
    if (sizeText != null) sizeText.text = shopItem.SizeLabel;
    if (priceText != null) priceText.text = $"{shopItem.price}G";

    buyButton?.onClick.RemoveAllListeners();
    buyButton?.onClick.AddListener(OnBuyClicked);

    RefreshAffordability();

    if (PlayerData.Instance != null)
      PlayerData.Instance.OnGoldChanged += _ => RefreshAffordability();
  }

  private void OnDestroy()
  {
    if (PlayerData.Instance != null)
      PlayerData.Instance.OnGoldChanged -= _ => RefreshAffordability();
  }

  // ── Internal ──────────────────────────────────────────────────────────────

  private void OnBuyClicked() => ShopSystem.Instance?.TryBuy(shopItem);

  private void RefreshAffordability()
  {
    bool can = PlayerData.Instance != null && PlayerData.Instance.CanAfford(shopItem.price);
    if (buyButton != null) buyButton.interactable = can;
  }
}

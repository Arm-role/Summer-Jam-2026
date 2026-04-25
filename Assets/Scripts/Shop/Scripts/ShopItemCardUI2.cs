using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCardUI2 : MonoBehaviour
{
  [SerializeField] private Button buyButton;

  // ── Setup ─────────────────────────────────────────────────────────────────

  public void Start()
  {
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

  private void OnBuyClicked()
  {
    if (PlayerData.Instance.TrySpend(30))
      PlayerData.Instance?.TryBuyEnergyDrink();
  }

  private void RefreshAffordability()
  {
    bool can = PlayerData.Instance != null && PlayerData.Instance.CanAfford(30);
    if (buyButton != null) buyButton.interactable = can;
  }
}

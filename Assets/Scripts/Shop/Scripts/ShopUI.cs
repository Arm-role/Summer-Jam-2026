using TMPro;
using UnityEngine;
/// <summary>
/// UI หลักของ Shop Panel
///
/// Hierarchy:
///   ShopUI (active=false ตอนเริ่ม)
///     ├── GoldText  (TMP)
///     ├── ErrorText (TMP)
///     └── ItemContainer (GridLayoutGroup)
///           └── [ShopItemCardUI prefab]
/// </summary>
public class ShopUI : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private ShopSystem shopSystem;

  [Header("UI")]
  [SerializeField] private TMP_Text goldTextBattle;
  [SerializeField] private TMP_Text goldTextShop;

  // ── Unity ─────────────────────────────────────────────────────────────────

  private void OnEnable()
  {
    if (PlayerData.Instance != null)
    {
      RefreshGold(PlayerData.Instance.Gold);
      PlayerData.Instance.OnGoldChanged += RefreshGold;
    }

    if (shopSystem != null)
    {
      shopSystem.OnBuyFailed += ShowError;
      shopSystem.OnItemBought += _ => { }; // hook ไว้ขยายได้
    }
  }

  private void Start()
  {
    if (PlayerData.Instance != null)
    {
      RefreshGold(PlayerData.Instance.Gold);
      PlayerData.Instance.OnGoldChanged += RefreshGold;
    }
  }


  private void OnDisable()
  {
    if (PlayerData.Instance != null)
      PlayerData.Instance.OnGoldChanged -= RefreshGold;

    if (shopSystem != null)
      shopSystem.OnBuyFailed -= ShowError;
  }

  // ── Callbacks ─────────────────────────────────────────────────────────────

  private void RefreshGold(int gold)
  {
    if (goldTextBattle != null) goldTextBattle.text = $"{gold}";
    if (goldTextShop != null) goldTextShop.text = $"{gold}";
  }

  private void ShowError(string msg)
  {
    TooltipCanvas.ShowTooltip_Static(msg);
  }
}
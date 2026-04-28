using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableButtonUI : MonoBehaviour
{
  [SerializeField] private ItemBattleDataSO consumable;
  [SerializeField] private Button button;
  [SerializeField] private TMP_Text countText;

  [Header("Modifier")]
  [SerializeField] private CooldownModifierSystem modifierSystem;

  private void Start()
  {
    button.onClick.AddListener(OnClick);
    Refresh(PlayerData.Instance.GetConsumableCount(consumable));

    if (PlayerData.Instance != null)
      PlayerData.Instance.OnConsumableChanged += OnConsumableChanged;

    var tooltipTrigger = button.gameObject.AddComponent<TooltipTrigger>();
    tooltipTrigger.SetData(new BattleItemTooltipData(consumable));
  }

  private void OnDestroy()
  {
    if (PlayerData.Instance != null)
      PlayerData.Instance.OnConsumableChanged -= OnConsumableChanged;
  }

  private void OnConsumableChanged(ItemBattleDataSO item, int count)
  {
    if (item == consumable) Refresh(count);
  }

  private void Refresh(int count)
  {
    countText.text = count.ToString();
    button.interactable = count > 0;
  }

  private void OnClick()
  {
    if (!PlayerData.Instance.TryConsumeItem(consumable)) return;

    switch (consumable.actionType)
    {
      case BattleActionType.Damage:
      case BattleActionType.Heal:
        // ขยายได้
        break;

      case BattleActionType.ApplyStatusEffect:
        modifierSystem.ApplyModifier(
            CooldownModifierType.SpeedUp,
            consumable.effectStrength,
            consumable.duration);
        break;
    }
  }
}
public interface ITooltipData
{
  string ItemName { get; }
  string BuildTooltip();
}
public class ShopItemTooltipData : ITooltipData
{
  private readonly ShopItemSO _so;
  private readonly ItemBattleDataSO _battle;

  public ShopItemTooltipData(ShopItemSO so, ItemBattleDataSO battle)
  {
    _so = so;
    _battle = battle;
  }

  public string ItemName => _so.DisplayName;

  public string BuildTooltip()
  {
    var sb = new System.Text.StringBuilder();
    sb.AppendLine($"<b>{ItemName}</b>");
    sb.AppendLine($"Price: {_so.price}g");

    if (_battle != null)
      sb.Append(TooltipBuilder.FromBattleData(_battle));

    return sb.ToString().TrimEnd();
  }
}

public class BattleItemTooltipData : ITooltipData
{
  private readonly ItemBattleDataSO _so;

  public BattleItemTooltipData(ItemBattleDataSO so) => _so = so;

  public string ItemName => _so.itemName;

  public string BuildTooltip()
  {
    var sb = new System.Text.StringBuilder();
    sb.AppendLine($"<b>{ItemName}</b>");
    sb.Append(TooltipBuilder.FromBattleData(_so));
    return sb.ToString().TrimEnd();
  }
}
public static class TooltipBuilder
{
  public static string FromBattleData(ItemBattleDataSO data)
  {
    var sb = new System.Text.StringBuilder();

    switch (data.actionType)
    {
      case BattleActionType.Damage:
        sb.AppendLine($"Target: {data.targetType}");
        sb.AppendLine($"Damage: {data.value}");
        sb.AppendLine($"Cooldown: {data.cooldown:F1}s");
        break;

      case BattleActionType.Heal:
        sb.AppendLine($"Heal: {data.value}");
        sb.AppendLine($"Cooldown: {data.cooldown:F1}s");
        break;

      case BattleActionType.ApplyStatusEffect:
        sb.AppendLine($"Target: {data.targetType}");
        sb.AppendLine($"Effect: {data.statusEffectType}");
        if (data.effectStrength > 0)
          sb.AppendLine($"Strength: {data.effectStrength * 100:F0}%");

        sb.AppendLine($"Duration: {data.duration:F1}s");
        sb.AppendLine($"Cooldown: {data.cooldown:F1}s");
        break;

      case BattleActionType.Gold:
        sb.AppendLine($"Gold: +{data.value}");
        sb.AppendLine($"Cooldown: {data.cooldown:F1}s");
        break;
    }

    return sb.ToString();
  }
}
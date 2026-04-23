using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Shop Item")]
public class ShopItemSO : ScriptableObject
{
  [Header("Item")]
  public ItemTetrisSO itemTetrisSO;

  [Header("Display")]
  public string displayName;
  [TextArea(1, 3)]
  public string description;
  public Sprite icon;

  [Header("Economy")]
  public int price = 10;

  public string DisplayName =>
      string.IsNullOrEmpty(displayName)
      ? (itemTetrisSO != null ? itemTetrisSO.name : name)
      : displayName;

  public string SizeLabel =>
      itemTetrisSO != null ? $"{itemTetrisSO.width}x{itemTetrisSO.height}" : "?";

  public Sprite DisplayIcon
  {
    get
    {
      if (icon != null) return icon;
      if (itemTetrisSO?.visual == null) return null;
      var sr = itemTetrisSO.visual.GetComponentInChildren<SpriteRenderer>();
      if (sr != null) return sr.sprite;
      var img = itemTetrisSO.visual.GetComponentInChildren<UnityEngine.UI.Image>();
      return img != null ? img.sprite : null;
    }
  }
}

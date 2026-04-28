using UnityEngine;

public class DropZoneChecker : MonoBehaviour
{
  public static DropZoneChecker Instance { get; private set; }

  [SerializeField] private RectTransform worldZoneRect;      // UI rect คลุมพื้นที่ world
  [SerializeField] private RectTransform inventoryZoneRect;  // UI rect คลุม inventory panel
  [SerializeField] private Camera uiCamera;                  // null ถ้า Canvas เป็น Screen Space Overlay

  private void Awake() => Instance = this;

  public enum DropZone { World, Inventory, None }

  public DropZone GetZoneAtMouse()
  {
    Vector2 mousePos = InputHelper.Position;

    if (IsInsideRect(worldZoneRect, mousePos)) return DropZone.World;
    if (IsInsideRect(inventoryZoneRect, mousePos)) return DropZone.Inventory;
    return DropZone.None;
  }

  private bool IsInsideRect(RectTransform rect, Vector2 screenPos)
  {
    return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, uiCamera);
  }
}

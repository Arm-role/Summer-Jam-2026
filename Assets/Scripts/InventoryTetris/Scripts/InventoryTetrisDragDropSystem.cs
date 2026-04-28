using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryTetrisDragDropSystem : MonoBehaviour
{
  public static InventoryTetrisDragDropSystem Instance { get; private set; }

  [SerializeField] private List<InventoryTetris> inventoryTetrisList;

  // ── PATCH: เพิ่มแค่บรรทัดนี้ ──────────────────────────────────────────────
  [SerializeField] private InventoryTetrisDropToWorld dropToWorld; // drag from Inspector
                                                                   // ─────────────────────────────────────────────────────────────────────────

  private InventoryTetris draggingInventoryTetris;
  private PlacedObject draggingPlacedObject;
  private Vector2Int mouseDragGridPositionOffset;
  private Vector2 mouseDragAnchoredPositionOffset;
  private PlacedObjectTypeSO.Dir dir;

  private void Awake()
  {
    Instance = this;
  }

  private void Start()
  {
    foreach (InventoryTetris inventoryTetris in inventoryTetrisList)
    {
      inventoryTetris.OnObjectPlaced += (object sender, PlacedObject placedObject) => { };
    }
  }

  private void Update()
  {
    if (InputHelper.SecondaryPressed)
    {
      dir = PlacedObjectTypeSO.GetNextDir(dir);
      if (draggingPlacedObject != null)
        draggingPlacedObject.FixCooldownRotation();
    }

    if (draggingPlacedObject != null)
    {
      RectTransformUtility.ScreenPointToLocalPointInRectangle(draggingInventoryTetris.GetItemContainer(), InputHelper.Position, null, out Vector2 targetPosition);
      targetPosition += new Vector2(-mouseDragAnchoredPositionOffset.x, -mouseDragAnchoredPositionOffset.y);

      Vector2Int rotationOffset = draggingPlacedObject.GetPlacedObjectTypeSO().GetRotationOffset(dir);
      targetPosition += new Vector2(rotationOffset.x, rotationOffset.y) * draggingInventoryTetris.GetGrid().GetCellSize();

      targetPosition /= 10f;
      targetPosition = new Vector2(Mathf.Floor(targetPosition.x), Mathf.Floor(targetPosition.y));
      targetPosition *= 10f;

      draggingPlacedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(draggingPlacedObject.GetComponent<RectTransform>().anchoredPosition, targetPosition, Time.deltaTime * 20f);
      draggingPlacedObject.transform.rotation = Quaternion.Lerp(draggingPlacedObject.transform.rotation, Quaternion.Euler(0, 0, -draggingPlacedObject.GetPlacedObjectTypeSO().GetRotationAngle(dir)), Time.deltaTime * 15f);

      draggingPlacedObject.FixCooldownRotation();
    }
  }

  public void StartedDragging(InventoryTetris inventoryTetris, PlacedObject placedObject)
  {
    draggingInventoryTetris = inventoryTetris;
    draggingPlacedObject = placedObject;

    Cursor.visible = false;

    RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), InputHelper.Position, null, out Vector2 anchoredPosition);
    Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);

    mouseDragGridPositionOffset = mouseGridPosition - placedObject.GetGridPosition();
    mouseDragAnchoredPositionOffset = anchoredPosition - placedObject.GetComponent<RectTransform>().anchoredPosition;

    dir = placedObject.GetDir();

    Vector2Int rotationOffset = draggingPlacedObject.GetPlacedObjectTypeSO().GetRotationOffset(dir);
    mouseDragAnchoredPositionOffset += new Vector2(rotationOffset.x, rotationOffset.y) * draggingInventoryTetris.GetGrid().GetCellSize();
  }

  public void StoppedDragging(InventoryTetris fromInventoryTetris, PlacedObject placedObject)
  {
    draggingInventoryTetris = null;
    draggingPlacedObject = null;

    Cursor.visible = true;

    fromInventoryTetris.RemoveItemAt(placedObject.GetGridPosition());

    InventoryTetris toInventoryTetris = null;

    foreach (InventoryTetris inventoryTetris in inventoryTetrisList)
    {
      Vector3 screenPoint = InputHelper.Position;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), screenPoint, null, out Vector2 anchoredPosition);
      Vector2Int placedObjectOrigin = inventoryTetris.GetGridPosition(anchoredPosition);
      placedObjectOrigin = placedObjectOrigin - mouseDragGridPositionOffset;

      if (inventoryTetris.IsValidGridPosition(placedObjectOrigin))
      {
        toInventoryTetris = inventoryTetris;
        break;
      }
    }

    if (toInventoryTetris != null)
    {
      Vector3 screenPoint = InputHelper.Position;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(toInventoryTetris.GetItemContainer(), screenPoint, null, out Vector2 anchoredPosition);
      Vector2Int placedObjectOrigin = toInventoryTetris.GetGridPosition(anchoredPosition);
      placedObjectOrigin = placedObjectOrigin - mouseDragGridPositionOffset;

      bool tryPlaceItem = toInventoryTetris.TryPlaceItem(placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO, placedObjectOrigin, dir);

      if (!tryPlaceItem)
      {
        fromInventoryTetris.TryPlaceItem(placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO, placedObject.GetGridPosition(), placedObject.GetDir());
      }
    }
    else
    {
      var zone = DropZoneChecker.Instance.GetZoneAtMouse();

      switch (zone)
      {
        case DropZoneChecker.DropZone.World:
          if (dropToWorld != null)
            dropToWorld.OnItemDroppedOutsideInventory(
                placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO);
          break;

        case DropZoneChecker.DropZone.None:
        default:
          TooltipCanvas.ShowTooltip_Static("Cannot Drop Item Here!");
          FunctionTimer.Create(() => TooltipCanvas.HideTooltip_Static(),
              2f, "HideTooltip", true, true);
          fromInventoryTetris.TryPlaceItem(
              placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO,
              placedObject.GetGridPosition(),
              placedObject.GetDir());
          break;
      }
    }
  }


}
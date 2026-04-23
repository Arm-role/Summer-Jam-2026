using UnityEngine;

[RequireComponent(typeof(DroppedWorldItem))]
public class WorldItemDragHandler : MonoBehaviour
{
  private DroppedWorldItem droppedWorldItem;

  private ItemTetrisSO itemSO;

  private bool isDragging;

  private Vector3 originWorldPos;

  private InventoryTetrisDropToWorld dropToWorld;

  private void Awake()
  {
    droppedWorldItem =
        GetComponent<DroppedWorldItem>();

    dropToWorld =
        FindObjectOfType<InventoryTetrisDropToWorld>();
  }

  private void OnMouseDown()
  {
    itemSO =
        droppedWorldItem.GetItemTetrisSO();

    if (itemSO == null)
      return;

    originWorldPos = transform.position;

    isDragging = true;

    Cursor.visible = false;

    InventoryTetrisManualPlacement
    .Instance
    .SelectItem(itemSO);

    gameObject.SetActive(false);
  }

  private void OnMouseUp()
  {
    if (!isDragging)
      return;

    isDragging = false;

    Cursor.visible = true;

    var zone =
        DropZoneChecker.Instance.GetZoneAtMouse();

    switch (zone)
    {
      case DropZoneChecker.DropZone.Inventory:

        TryPlaceIntoInventory();

        break;

      case DropZoneChecker.DropZone.World:

        MoveInsideWorld();

        break;

      default:

        Respawn();

        break;
    }

    InventoryTetrisManualPlacement
       .Instance
       .ClearSelection();
  }

  // ─────────────────────────────
  // Placement logic
  // ─────────────────────────────

  private void TryPlaceIntoInventory()
  {
    if (dropToWorld == null)
    {
      Respawn();
      return;
    }

    bool placed =
        dropToWorld
        .TryPlaceIntoInventory(itemSO);

    if (placed)
    {
      Destroy(gameObject);
    }
    else
    {
      Respawn();
    }
  }

  private void MoveInsideWorld()
  {
    if (dropToWorld == null)
    {
      Respawn();
      return;
    }

    dropToWorld
        .OnItemDroppedOutsideInventory(itemSO);

    Destroy(gameObject);
  }

  private void Respawn()
  {
    DroppedWorldItem.Spawn(
        itemSO,
        originWorldPos);

    Destroy(gameObject);
  }
}

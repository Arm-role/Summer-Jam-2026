using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Attach this to the same GameObject as <see cref="InventoryTetrisDragDropSystem"/>.
///
/// How it works:
///   • Hooks into <see cref="InventoryTetrisDragDropSystem.OnItemDroppedOutside"/> (see patch notes).
///   • Converts the screen-space mouse position to world space.
///   • Calls <see cref="DroppedWorldItem.Spawn"/> — fully decoupled from inventory internals.
///
/// Setup:
///   1. Assign <see cref="worldCamera"/> (the camera rendering your 2-D scene).
///   2. (Optional) assign a world canvas / UI camera if your game uses a separate one.
/// </summary>
public class InventoryTetrisDropToWorld : MonoBehaviour
{
  [Header("Required")]
  [Tooltip("The camera that renders your 2-D game world (NOT the UI camera).")]
  [SerializeField] private Camera worldCamera;

  [Header("Optional spawn offset")]
  [Tooltip("Nudge the spawn position so the item doesn't overlap the UI panel.")]
  [SerializeField] private Vector3 spawnOffset = Vector3.zero;

  // ── Unity messages ────────────────────────────────────────────────────────

  private void Awake()
  {
    if (worldCamera == null)
      worldCamera = Camera.main;
  }

  // ── Called by InventoryTetrisDragDropSystem (see patch) ───────────────────

  public bool TryPlaceIntoInventory(
    ItemTetrisSO itemSO)
  {
    var inventory =
        InventoryTetris.Instance;

    if (inventory == null)
      return false;

    RectTransformUtility
        .ScreenPointToLocalPointInRectangle(
            inventory.GetItemContainer(),
            InputHelper.Position,
            null,
            out Vector2 anchoredPos);

    Vector2Int gridPos =
        inventory.GetGridPosition(
            anchoredPos);

    return inventory.TryPlaceItem(
        itemSO,
        gridPos,
        PlacedObjectTypeSO.Dir.Down);
  }


  /// <summary>
  /// Call this from <see cref="InventoryTetrisDragDropSystem.StoppedDragging"/>
  /// when no inventory panel was found under the mouse.
  /// </summary>
  public void OnItemDroppedOutsideInventory(ItemTetrisSO itemTetrisSO)
  {
    if (itemTetrisSO == null) return;

    Vector3 worldPos = ScreenToWorld(InputHelper.Position);
    worldPos += spawnOffset;

    DroppedWorldItem.Spawn(itemTetrisSO, worldPos);

    Debug.Log($"[DropToWorld] Spawned {itemTetrisSO.name} at {worldPos}");
  }

  // ── Helpers ───────────────────────────────────────────────────────────────

  private Vector3 ScreenToWorld(Vector2 screenPos)
  {
    Vector3 pos = worldCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, worldCamera.nearClipPlane));
    pos.z = 0f; // keep on 2-D plane
    return pos;
  }
}

using UnityEngine;

/// <summary>
/// Attached to a world GameObject spawned when an item is dragged out of the inventory.
/// Requires: SpriteRenderer, Rigidbody2D, Collider2D (auto-added).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class DroppedWorldItem : MonoBehaviour
{
  [Header("Pick-up Settings")]
  [Tooltip("How close the player must be to pick this item up (set 0 to disable auto pick-up)")]
  [SerializeField] private float pickUpRadius = 1.5f;

  private ItemTetrisSO itemTetrisSO;
  private Rigidbody2D rb;

  // ── Public factory ────────────────────────────────────────────────────────

  /// <summary>
  /// Spawn a world item at <paramref name="worldPosition"/> for the given SO.
  /// Uses <see cref="ItemTetrisSO.worldPrefab"/> if assigned, otherwise builds one on-the-fly.
  /// </summary>
  public static DroppedWorldItem Spawn(ItemTetrisSO itemTetrisSO, Vector3 worldPosition)
  {
    GameObject go;

    if (itemTetrisSO.worldPrefab != null)
    {
      go = Instantiate(itemTetrisSO.worldPrefab, worldPosition, Quaternion.identity);
    }
    else
    {
      go = new GameObject($"WorldItem_{itemTetrisSO.name}");
      go.transform.position = worldPosition;

      // Visual
      SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
      // Try to grab the first sprite from the SO's visual prefab
      sr.sprite = TryGetSprite(itemTetrisSO);
      sr.sortingLayerName = "Default";
      sr.sortingOrder = 1;

      // Physics
      go.AddComponent<Rigidbody2D>();
      BoxCollider2D col = go.AddComponent<BoxCollider2D>();
      if (sr.sprite != null)
        col.size = sr.sprite.bounds.size;

      go.AddComponent<DroppedWorldItem>();
    }

    DroppedWorldItem dropped = go.GetComponent<DroppedWorldItem>();
    if (dropped == null)
      dropped = go.AddComponent<DroppedWorldItem>();

    dropped.Init(itemTetrisSO);

    if (dropped.GetComponent<WorldItemDragHandler>() == null)
      dropped.gameObject.AddComponent<WorldItemDragHandler>();

    return dropped;
  }

  // ── Init ──────────────────────────────────────────────────────────────────

  private void Init(ItemTetrisSO so)
  {
    itemTetrisSO = so;
    rb = GetComponent<Rigidbody2D>();

    // Small random toss so items don't stack perfectly
    rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 3f));
  }

  // ── Helpers ───────────────────────────────────────────────────────────────

  private static Sprite TryGetSprite(ItemTetrisSO so)
  {
    if (so.visual == null) return null;

    // Check if the visual prefab itself (or its first child) has a SpriteRenderer
    SpriteRenderer sr = so.visual.GetComponentInChildren<SpriteRenderer>();
    if (sr != null) return sr.sprite;

    // Fallback: UnityEngine.UI.Image (canvas item)
    UnityEngine.UI.Image img = so.visual.GetComponentInChildren<UnityEngine.UI.Image>();
    if (img != null) return img.sprite;

    return null;
  }

  // ── Pick-up trigger ───────────────────────────────────────────────────────

  public ItemTetrisSO GetItemTetrisSO() => itemTetrisSO;

  /// <summary>Call this from your Player script to pick the item back up.</summary>
  public void PickUp()
  {
    // Hook into your own pick-up logic here, e.g. add back to inventory.
    // Then destroy the world object.
    Destroy(gameObject);
  }

  private void OnDrawGizmosSelected()
  {
    if (pickUpRadius <= 0) return;
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, pickUpRadius);
  }
}

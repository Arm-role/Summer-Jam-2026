using UnityEngine;
using UnityEngine.InputSystem;

public class DroppedWorldItemSpawner : MonoBehaviour
{
  [Tooltip("Spawn around this transform")]
  [SerializeField] private Transform spawnPoint;
  [SerializeField] private ItemTetrisSO itemSO;


  private void Start()
  {
    Spawn(itemSO);
  }

  public void Spawn(ItemTetrisSO itemSO)
  {
    if (itemSO == null)
      return;

    Vector3 spawnPos =
        spawnPoint != null
        ? spawnPoint.position
        : transform.position;

    DroppedWorldItem.Spawn(
        itemSO,
        spawnPos
    );
  }
}

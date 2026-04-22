using UnityEngine;
using UnityEngine.InputSystem;

public class FloatingTextTester : MonoBehaviour
{
  [SerializeField] private FloatingTextConfig config;

  private FloatingTextService service;

  private void Start()
  {
    service = new FloatingTextService(
      config.Prefab,
      config.Styles
    );
  }

  private async void Update()
  {
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
      Vector3 worldPos = GetMouseWorldPosition();

      await service.Spawn(
        FloatingTextType.Damage,
        worldPos,
        Random.Range(5, 50)
      );
    }

    if (Mouse.current.rightButton.wasPressedThisFrame)
    {
      Vector3 worldPos = GetMouseWorldPosition();

      await service.Spawn(
        FloatingTextType.Heal,
        worldPos,
        Random.Range(5, 50)
      );
    }
  }

  private Vector3 GetMouseWorldPosition()
  {
    Camera cam = Camera.main;

    Vector2 mousePos = Mouse.current.position.ReadValue();

    Ray ray = cam.ScreenPointToRay(mousePos);

    Plane plane = new Plane(Vector3.forward, Vector3.zero);

    if (plane.Raycast(ray, out float distance))
      return ray.GetPoint(distance);

    return Vector3.zero;
  }
}

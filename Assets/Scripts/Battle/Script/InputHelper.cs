using UnityEngine;
using UnityEngine.InputSystem;

public static class InputHelper
{
  private static bool HasActiveTouch =>
      Touchscreen.current != null
      && Touchscreen.current.touches.Count > 0
      && Touchscreen.current.touches[0].press.isPressed;

  public static Vector2 Position
  {
    get
    {
      if (HasActiveTouch)
        return Touchscreen.current.touches[0].position.ReadValue();

      return Mouse.current.position.value;
    }
  }

  public static bool PrimaryPressed
  {
    get
    {
      if (HasActiveTouch)
        return Touchscreen.current.touches[0].phase.ReadValue()
            == UnityEngine.InputSystem.TouchPhase.Began;

      return Mouse.current.leftButton.wasPressedThisFrame;
    }
  }

  public static bool PrimaryReleased
  {
    get
    {
      if (HasActiveTouch)
        return Touchscreen.current.touches[0].phase.ReadValue()
            == UnityEngine.InputSystem.TouchPhase.Ended;

      return Mouse.current.leftButton.wasReleasedThisFrame;
    }
  }

  public static bool SecondaryPressed
  {
    get
    {
      if (Touchscreen.current != null
          && Touchscreen.current.touches.Count >= 2
          && Touchscreen.current.touches[1].press.isPressed)
        return Touchscreen.current.touches[1].phase.ReadValue()
            == UnityEngine.InputSystem.TouchPhase.Began;

      return Mouse.current.rightButton.wasPressedThisFrame;
    }
  }
}
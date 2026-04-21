using UnityEngine;

public class TargetFramrate : MonoBehaviour
{
  private void Awake()
  {
    Application.targetFrameRate = 100;
  }
}
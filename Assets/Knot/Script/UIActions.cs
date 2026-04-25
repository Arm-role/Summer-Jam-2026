using UnityEngine;

public class UIActions : MonoBehaviour
{
  public void OnClickNextLevel()
  {
    SceneController.instance?.Nextlevel();
  }

  public void OnClickPlaySFX(AudioClip clip)
  {
    AudioManager.instance?.PlaySFX(clip);
  }

  public void OnExit()
  {
    Application.Quit();
  }
}
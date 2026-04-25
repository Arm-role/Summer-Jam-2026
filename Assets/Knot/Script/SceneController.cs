using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
  public static SceneController instance;
  public Animator transitionAnim;
  public GameObject fadeObject;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);

    }
    else
    {
      Destroy(gameObject);
    }
  }
  private void Start()
  {

    if (fadeObject != null)
    {
      fadeObject.SetActive(false);
    }
  }

  public void Nextlevel()
  {
    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
  }
  public void TryGameAgain(int index)
  {
    StartCoroutine(LoadLevel(index));
  }
  IEnumerator LoadLevel(int index)
  {
    if (fadeObject != null)
    {
      fadeObject.SetActive(true);
    }
    transitionAnim.SetTrigger("End");
    yield return new WaitForSeconds(1.05f);
    SceneManager.LoadSceneAsync(index);

    transitionAnim.SetTrigger("Start");
    {
      yield return new WaitForSeconds(1.05f);
      if (fadeObject != null)
      {
        fadeObject.SetActive(false);
      }
    }


  }
  public void GoToNextLevel()
  {

    if (SceneController.instance != null)
    {

      SceneController.instance.Nextlevel();
    }
    else
    {
      Debug.LogWarning("หา SceneController ไม่เจอ! (อาจจะไม่ได้เริ่มเล่นจากฉากแรก)");
    }
  }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
  [SerializeField] private GameObject root;
  [SerializeField] private Animator animator;
  [SerializeField] private Button restartButton;

  private void Awake()
  {
    root.SetActive(false);
  }

  private void OnEnable()
  {
    GameStateManager.OnStateChanged += OnStateChanged;
  }
  private void Start()
  {
    restartButton.onClick.AddListener(OnClickRestart);
  }

  private void OnDisable()
  {
    GameStateManager.OnStateChanged -= OnStateChanged;
  }

  private void OnStateChanged(GameState prev, GameState next)
  {
    if (next == GameState.GameOver)
    {
      Show();
    }
  }

  public void OnClickRestart()
  {
    if (SceneController.instance != null)
    {
      SceneController.instance.TryGameAgain(SceneManager.GetActiveScene().buildIndex);
    }
    else
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  private void Show()
  {
    root.SetActive(true);
    animator?.SetTrigger("Show");
  }

  public void Hide()
  {
    root.SetActive(false);
  }
}

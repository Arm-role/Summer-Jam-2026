using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
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
    restartButton.onClick.AddListener(OnClickToHome);
  }

  private void OnDisable()
  {
    GameStateManager.OnStateChanged -= OnStateChanged;
  }

  private void OnStateChanged(GameState prev, GameState next)
  {
    if (next == GameState.GameEnd)
    {
      Show();
    }
  }

  public void OnClickToHome()
  {
    if (SceneController.instance != null)
    {
      SceneController.instance.TryGameAgain(0);
    }
    else
    {
      SceneManager.LoadScene(0);
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
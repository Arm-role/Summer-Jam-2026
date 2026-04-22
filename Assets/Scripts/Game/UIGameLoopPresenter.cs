using UnityEngine;
using UnityEngine.UI;

public class UIGameLoopPresenter : MonoBehaviour
{
  [SerializeField] private GameLoopController gameLoopController;
  [SerializeField] Button startBattleButton;
  [SerializeField] Button endShopButton;

  private void Start()
  {
    gameLoopController.OnPrepareEventChanged += HandleEventChanged;
    startBattleButton.onClick.AddListener(OnStartBattlePressed);
    endShopButton.onClick.AddListener(OnEndShopPressed);
  }

  private void HandleEventChanged(string label)
  {
    startBattleButton.gameObject.SetActive(label == "Enemy");
    endShopButton.gameObject.SetActive(label == "Shop");
  }

  public void OnStartBattlePressed()
  {
    gameLoopController.RequestStartBattle();
  }

  public void OnEndShopPressed()
  {
    gameLoopController.RequestEndShop();
  }

  public void OnRestartPressed()
  {
    gameLoopController.RequestRestart();
  }
}

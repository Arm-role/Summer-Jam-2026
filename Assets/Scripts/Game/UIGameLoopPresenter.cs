using UnityEngine;
using UnityEngine.UI;

public class UIGameLoopPresenter : MonoBehaviour
{
  [SerializeField] private GameLoopController gameLoopController;
  [SerializeField] Button startBattleButton;
  [SerializeField] Button endShopButton;
  [SerializeField] GameObject[] battleObjects;
  [SerializeField] GameObject[] shopObjects;

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

    foreach (var obj in battleObjects)
      obj.gameObject.SetActive(label == "Enemy");

    foreach (var obj in shopObjects)
       obj.gameObject.SetActive(label == "Shop");
  }

  public void OnStartBattlePressed()
  {
    gameLoopController.RequestStartBattle();
  }

  public void OnEndShopPressed()
  {
    gameLoopController.RequestEndShop();

    foreach (var obj in shopObjects)
      obj.gameObject.SetActive(false);
  }

  public void OnRestartPressed()
  {
    gameLoopController.RequestRestart();
  }
}

using UnityEngine;

public class MockShopEvent : MonoBehaviour, IPrepareEvent
{
  public string EventLabel => "Shop";

  [SerializeField] private GameObject shopUIPanel;

  public void OnEventBegin()
  {
    Debug.Log("[MockShopEvent] open shop");
    shopUIPanel?.SetActive(true);
  }

  public void OnBattleEnd(bool playerWon)
  {
    shopUIPanel?.SetActive(false);
  }
}
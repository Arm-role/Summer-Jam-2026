using UnityEngine;

public class MockShopEvent : MonoBehaviour, IPrepareEvent
{
  public string EventLabel => "Shop";

  public void OnEventBegin()
  {
    Debug.Log("[MockShopEvent] open shop");
  }

  public void OnBattleEnd(bool playerWon) { }
}
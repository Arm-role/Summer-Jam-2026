using UnityEngine;

public class MockShopEvent : MonoBehaviour, IPrepareEvent
{
  [SerializeField] private GameObject shopUIPanel;
  public string EventLabel => "Shop";
  public void OnEventBegin() => shopUIPanel?.SetActive(true);
  public void OnBattleEnd(bool playerWon) => shopUIPanel?.SetActive(false);
}
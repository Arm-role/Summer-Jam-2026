using UnityEngine;

public interface IPrepareEvent
{
  string EventLabel { get; }         
  void OnEventBegin();               
  void OnBattleEnd(bool playerWon);  
}
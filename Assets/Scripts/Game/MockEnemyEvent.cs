using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MockEnemyEvent : MonoBehaviour, IPrepareEvent
{
  [SerializeField] BattleSceneSpawner battleSceneSpawner;
  [SerializeField] private List<EnemyUnitSO> enemyUnitSOs;

  public string EventLabel => "Enemy";

  public void OnEventBegin()
  {
    battleSceneSpawner.SpawnBattle(enemyUnitSOs);
  }

  public void OnBattleEnd(bool playerWon)
  {
    Debug.Log($"[MockEnemyEvent] battle ended — playerWon: {playerWon}");
  }
}

using UnityEngine;

public class MockEnemyEvent : MonoBehaviour, IPrepareEvent
{
  [SerializeField] private BattleSceneSpawner battleSceneSpawner;

  public string EventLabel => "Enemy";
  private EnemyLevelSO levelData;

  public void SetLevelData(EnemyLevelSO data) => levelData = data;

  public void OnEventBegin()
  {
    if (levelData == null)
    {
      Debug.LogWarning("[MockEnemyEvent] Missing LevelData");
      return;
    }
    battleSceneSpawner.SpawnBattle(levelData.enemies, levelData.goldReward);
  }

  public void OnBattleEnd(bool playerWon)
  {
    Debug.Log($"[MockEnemyEvent] playerWon: {playerWon}");
  }
}

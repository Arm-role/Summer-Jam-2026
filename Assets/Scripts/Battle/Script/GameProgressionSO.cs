using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Game Progression")]
public class GameProgressionSO : ScriptableObject
{
  public List<StageData> stages;
}

public enum StageEventType { Battle, Shop }

[System.Serializable]
public class StageEvent
{
  public StageEventType type;
  public EnemyLevelSO enemyLevel; 
}

[System.Serializable]
public class StageData
{
  public string stageName;
  public List<StageEvent> events;
}
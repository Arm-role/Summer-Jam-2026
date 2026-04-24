using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Enemy Level")]
public class EnemyLevelSO : ScriptableObject
{
  [Header("Enemies")]
  public List<EnemyUnitSO> enemies;

  [Header("Reward")]
  public int goldReward = 10;
}


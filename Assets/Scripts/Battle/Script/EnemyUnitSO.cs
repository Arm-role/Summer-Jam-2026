using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Enemy Unit")]
public class EnemyUnitSO : ScriptableObject
{
  public string enemyName;
  public int hp;
  public List<ItemBattleDataSO> actions;

  [Header("Visual")]
  public CharacterView prefab;      
  public Vector3 spawnOffset;
}
using System;
using UnityEngine;

public enum BattleActionType
{
  Damage,
  Heal,
  Gold,
  ApplyStatusEffect
}
public enum StatusEffectType
{
  None,
  Freeze,
  SlowAttack
}

public enum TargetType
{
  Self,
  Enemy,
  AllEnemies,
  AllPlayers,
  RandomEnemy,
  FrontEnemy,
  BackEnemy
}

[CreateAssetMenu(menuName = "Battle/Item Battle Data")]
public class ItemBattleDataSO : ScriptableObject
{
  public string itemName;
  public TargetType targetType;

  [Header("Action")]
  public BattleActionType actionType;
  public int value;
  public float cooldown;

  [Header("Status Effect")]
  public StatusEffectType statusEffectType;
  public float duration;
  public float effectStrength;
}

[Serializable]
public struct BattleActionLog
{
  public string item;
  public TargetType targetType;

  public int value;
  public BattleActionType type;
  public StatusEffectType statusEffect;
  public float duration;
}

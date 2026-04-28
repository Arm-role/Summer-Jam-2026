using System;
using UnityEngine;
public enum OwnerType
{
  Player,
  Enemy
}
public enum ParticleEffectType
{
  Hit,
  Heal,
  Freeze,
  Slow,
  Death,
  Gold,
  LevelUp
}

[CreateAssetMenu(menuName = "ParticleEffect/Style Config")]
public class ParticleEffectStyleConfig : ScriptableObject
{
  [SerializeField] private string itemName;
  [SerializeField] private BattleActionType actionType;
  [SerializeField] private OwnerType ownerType;
  [SerializeField] private TargetType targetType;
  [SerializeField] private StatusEffectType statusEffectType = StatusEffectType.None;
  [SerializeField] private GameObject prefab;

  public string ItemName => itemName;
  public BattleActionType ActionType => actionType;
  public OwnerType OwnerType => ownerType;
  public TargetType TargetType => targetType;
  public StatusEffectType StatusEffectType => statusEffectType;
  public GameObject Prefab => prefab;

  public bool Matches(ParticleEffectKey key)
  {
    return itemName == key.itemName
        && actionType == key.actionType
        && ownerType == key.ownerType
        && targetType == key.targetType
        && statusEffectType == key.statusEffectType;
  }
}

[Serializable]
public struct ParticleEffectKey
{
  public string itemName;
  public BattleActionType actionType;
  public OwnerType ownerType;
  public TargetType targetType;
  public StatusEffectType statusEffectType;

  public ParticleEffectKey(
    string itemName,
    BattleActionType actionType,
    OwnerType ownerType,
    TargetType targetType,
    StatusEffectType statusEffectType = StatusEffectType.None)
  {
    this.itemName = itemName;
    this.actionType = actionType;
    this.ownerType = ownerType;
    this.targetType = targetType;
    this.statusEffectType = statusEffectType;
  }
}
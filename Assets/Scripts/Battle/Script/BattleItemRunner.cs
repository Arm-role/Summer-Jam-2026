using System;
using UnityEngine;

public class BattleItemRunner
{
  public event Action<BattleActionLog> OnAction;
  public event Action<float> OnCooldownProgress;
  public event Action<StatusEffectType> OnStatusApplied;

  private readonly ItemBattleDataSO data;
  private readonly CooldownModifierSystem modifierSystem;

  private float timer;
  private bool running;

  private float freezeTimer;
  private float slowTimer;
  private float slowMultiplier = 1f;


  public BattleItemRunner(ItemBattleDataSO data, CooldownModifierSystem modifierSystem = null)
  {
    this.data = data;
    this.modifierSystem = modifierSystem;

    timer = data.cooldown;
  }

  public void Tick(float deltaTime)
  {
    if (!running) return;

    if (freezeTimer > 0)
    {
      freezeTimer -= deltaTime;
      return;
    }

    float modifierMultiplier =
      modifierSystem?.GetCooldownMultiplier() ?? 1f;

    if (slowTimer > 0)
    {
      slowTimer -= deltaTime;
      modifierMultiplier *= slowMultiplier;
    }

    timer -= deltaTime * modifierMultiplier;

    float progress =
      Mathf.Clamp01(1f - (timer / data.cooldown));

    OnCooldownProgress?.Invoke(progress);

    if (timer > 0) return;

    timer += data.cooldown;

    var log = new BattleActionLog
    {
      item = data.itemName,
      targetType = data.targetType,
      value = data.value,
      type = data.actionType,
      statusEffect = data.statusEffectType,
      duration = data.duration
    };

    OnAction?.Invoke(log);
  }

  public void ApplyStatusEffect(
    StatusEffectType effect,
    float duration)
  {
    switch (effect)
    {
      case StatusEffectType.Freeze:

        freezeTimer = duration;

        break;

      case StatusEffectType.SlowAttack:

        slowTimer = duration;
        slowMultiplier = 0.5f;

        break;
    }

    OnStatusApplied?.Invoke(effect);
  }


  public void Start()
  {
    running = true;
    timer = data.cooldown;

    OnCooldownProgress?.Invoke(0f);
  }

  public void Stop()
  {
    running = false;
    timer = data.cooldown;

    OnCooldownProgress?.Invoke(0f);
  }
}
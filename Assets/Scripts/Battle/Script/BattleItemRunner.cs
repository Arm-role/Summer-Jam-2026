using System;
using UnityEngine;

public class BattleItemRunner
{
  public event Action<BattleActionLog> OnAction;
  public event Action<float> OnCooldownProgress;

  private readonly ItemBattleDataSO data;
  private readonly CooldownModifierSystem modifierSystem;

  private float timer;
  private bool running;

  public BattleItemRunner(ItemBattleDataSO data, CooldownModifierSystem modifierSystem)
  {
    this.data = data;
    this.modifierSystem = modifierSystem;

    timer = data.cooldown;
  }

  public void Tick(float deltaTime)
  {
    if (!running) return;

    float multiplier = modifierSystem.GetCooldownMultiplier();

    timer -= deltaTime * multiplier;

    float progress = 1f - (timer / data.cooldown);
    OnCooldownProgress?.Invoke(progress);

    if (timer > 0) return;

    timer = data.cooldown;

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
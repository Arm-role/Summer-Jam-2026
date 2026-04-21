using Codice.Client.BaseCommands;
using System.Collections.Generic;
using UnityEngine;

public class CooldownModifierSystem : MonoBehaviour
{
  private List<CooldownModifierInstance> modifiers = new();

  public float GetCooldownMultiplier()
  {
    float value = 1f;

    foreach (var mod in modifiers)
      value += mod.value;

    return Mathf.Clamp(value, 0.2f, 2f);
  }

  public void ApplyModifier(
  CooldownModifierType type,
  float value,
  float duration)
  {
    foreach (var mod in modifiers)
    {
      if (mod.type == type)
      {
        mod.timer = duration;
        Debug.Log($"Refresh {type}");
        return;
      }
    }

    modifiers.Add(
      new CooldownModifierInstance(
        type,
        value,
        duration));
  }

  private void Update()
  {
    float dt = Time.deltaTime;

    for (int i = modifiers.Count - 1; i >= 0; i--)
    {
      modifiers[i].Tick(dt);

      if (modifiers[i].IsExpired())
        modifiers.RemoveAt(i);
    }
  }

  public void DebugPrintModifiers()
  {
    foreach (var mod in modifiers)
      Debug.Log(mod.type + " : " + mod.timer);
  }
}
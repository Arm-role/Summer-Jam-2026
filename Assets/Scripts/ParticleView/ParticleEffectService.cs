using UnityEngine;

public class ParticleEffectService
{
  private readonly ParticleEffectStyleConfig[] _styles;

  public ParticleEffectService(ParticleEffectStyleConfig[] styles)
  {
    _styles = styles;
  }

  public void Spawn(ParticleEffectKey key, Vector3 worldPos)
  {
    var style = FindStyle(key);

    if (style == null)
    {
      Debug.LogWarning($"Missing prefab for " +
          $"action={key.actionType} " +
          $"owner={key.ownerType} " +
          $"target={key.targetType} " +
          $"status={key.statusEffectType}");
      return;
    }

    var instance = Object.Instantiate(style.Prefab, worldPos, Quaternion.identity);

    var ps = instance.GetComponent<ParticleSystem>();
    float destroyDelay = ps != null
        ? ps.main.duration + ps.main.startLifetime.constantMax
        : 2f;

    Object.Destroy(instance, destroyDelay);
  }

  private ParticleEffectStyleConfig FindStyle(ParticleEffectKey key)
  {
    foreach (var s in _styles)
      if (s.Matches(key)) return s;
    return null;
  }
}
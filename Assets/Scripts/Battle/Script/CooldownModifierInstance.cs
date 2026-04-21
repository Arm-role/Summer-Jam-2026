public enum CooldownModifierType
{
  SpeedUp,
  SlowDown
}

public class CooldownModifierInstance
{
  public CooldownModifierType type;

  public float value;

  public float timer;

  public bool IsBuff =>
    type == CooldownModifierType.SpeedUp;

  public CooldownModifierInstance(
    CooldownModifierType type,
    float value,
    float duration)
  {
    this.type = type;
    this.value = value;
    timer = duration;
  }

  public void Tick(float dt)
  {
    timer -= dt;
  }

  public bool IsExpired()
  {
    return timer <= 0;
  }
}

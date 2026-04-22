using System;
using UnityEngine;

public enum EnemyPosition { Front, Back }

public class CombatUnit
{
  public string unitName;
  public int maxHp;
  public int currentHp;
  public bool IsDead => currentHp <= 0;
  public EnemyPosition Position { get; private set; }

  public Vector3 WorldPosition => view != null
      ? view.WorldPosition
      : Vector3.zero;

  public event Action<int, int> OnHpChanged;
  public event Action<CombatUnit> OnDied;

  private CharacterView view;

  public CombatUnit(string name, int hp,
      EnemyPosition position = EnemyPosition.Front,
      CharacterView view = null)
  {
    unitName = name;
    maxHp = hp;
    currentHp = hp;
    Position = position;
    this.view = view;
  }

  public void BindView(CharacterView characterView)
  {
    view = characterView;
  }

  public void TakeDamage(int amount)
  {
    if (IsDead) return;
    currentHp = Mathf.Max(0, currentHp - amount);
    view?.PlayHit();
    OnHpChanged?.Invoke(currentHp, maxHp);
    if (IsDead)
    {
      view?.PlayDie();
      view?.DestroySelf();
      OnDied?.Invoke(this);
    }
  }

  public void Heal(int amount)
  {
    if (IsDead) return;
    currentHp = Mathf.Min(maxHp, currentHp + amount);
    OnHpChanged?.Invoke(currentHp, maxHp);
  }

  public void RefillHp()
  {
    currentHp = maxHp;
    OnHpChanged?.Invoke(currentHp, maxHp);
  }

  public void SetPosition(EnemyPosition position) => Position = position;
}

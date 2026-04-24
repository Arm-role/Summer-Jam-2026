using UnityEngine;

public class CharacterView : MonoBehaviour
{
  [SerializeField] private Animator animator;
  [SerializeField] private SpriteRenderer spriteRenderer;

  private static readonly int IdleTrigger = Animator.StringToHash("Idle");
  private static readonly int HitTrigger = Animator.StringToHash("Hit");
  private static readonly int AttackTrigger = Animator.StringToHash("Attack");
  private static readonly int RunTrigger = Animator.StringToHash("Run");
  private static readonly int DieTrigger = Animator.StringToHash("Die");

  public Vector3 WorldPosition => transform.position;

  public void PlayIdle()
  {
    animator?.SetTrigger(IdleTrigger);
  }

  public void PlayAttack()
  {
    animator?.SetTrigger(AttackTrigger);
  }

  public void PlayRun()
  {
    animator?.SetTrigger(RunTrigger);
  }

  public void PlayHit()
  {
    animator?.SetTrigger(HitTrigger);
  }

  public void PlayDie()
  {
    animator?.SetTrigger(DieTrigger);
  }

  public void DestroySelf()
  {
    Destroy(gameObject);
  }
}

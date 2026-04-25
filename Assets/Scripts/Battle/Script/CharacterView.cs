using UnityEngine;

public class CharacterView : MonoBehaviour
{
  [SerializeField] private Animator animator;
  [SerializeField] private SpriteRenderer spriteRenderer;

  private static readonly int AttackTrigger = Animator.StringToHash("Attack");
  private static readonly int IsMove = Animator.StringToHash("IsMove");
  private static readonly int HitTrigger = Animator.StringToHash("Hit");  

  public Vector3 WorldPosition => transform.position;

  public void PlayAttack()
  {
    Debug.Log("Attack1");
    animator?.SetTrigger(AttackTrigger);
  }

  /// <param name="isMoving">true = วิ่ง, false = หยุด</param>
  public void PlayRun(bool isMoving)
  {

    animator?.SetBool(IsMove, isMoving);
  }

  public void PlayHit()
  {
    animator?.SetTrigger(HitTrigger);
  }

  public void DestroySelf()
  {
    Destroy(gameObject);
  }
}
using UnityEngine;

public class CharacterView : MonoBehaviour
{
  //[SerializeField] private Animator animator;
  [SerializeField] private SpriteRenderer spriteRenderer;

  // Animator parameter names — ตั้งให้ตรงกับ Animator Controller
  private static readonly int HitTrigger = Animator.StringToHash("Hit");
  private static readonly int DieTrigger = Animator.StringToHash("Die");
  private static readonly int IdleTrigger = Animator.StringToHash("Idle");

  public Vector3 WorldPosition => transform.position;

  public void PlayIdle()
  {
    Debug.Log("Idle");
    //animator?.SetTrigger(IdleTrigger);
  }

  public void PlayHit()
  {
    Debug.Log("Hit");
    //animator?.SetTrigger(HitTrigger);
  }

  public void PlayDie()
  {
    Debug.Log("Die");
    //animator?.SetTrigger(DieTrigger);
  }

  public void DestroySelf() => Destroy(gameObject);

  // flip sprite เมื่อ enemy อยู่ขวา
  public void FaceLeft() => spriteRenderer.flipX = false;
  public void FaceRight() => spriteRenderer.flipX = true;
}
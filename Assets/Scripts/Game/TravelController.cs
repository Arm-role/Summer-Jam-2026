using System;
using System.Collections.Generic;
using UnityEngine;

public class TravelController : MonoBehaviour
{
  [System.Serializable]
  public struct TravelEvent
  {
    public string label;         // "Enemy", "Shop", "Nothing"
    [Range(0, 100)] public int weight;
    public EnemyUnitSO enemy;    // null ถ้าไม่ใช่ battle event
  }

  [SerializeField] private List<TravelEvent> events;
  [SerializeField] private float travelDuration = 2f;  // วินาทีก่อนสุ่ม event

  private float timer;
  private bool travelling;

  private void OnEnable()
  {
    // เริ่ม travel ทันทีที่ screen นี้ active
    timer = travelDuration;
    travelling = true;
  }

  private void Update()
  {
    if (!travelling) return;
    timer -= Time.deltaTime;
    if (timer > 0) return;

    travelling = false;
    TriggerRandomEvent();
  }

  private void TriggerRandomEvent()
  {
    int total = 0;
    foreach (var e in events) total += e.weight;

    int roll = UnityEngine.Random.Range(0, total);
    int acc = 0;
    foreach (var e in events)
    {
      acc += e.weight;
      if (roll >= acc) continue;

      Debug.Log($"[Travel] event: {e.label}");

      if (e.label == "Shop")
        GameStateManager.GoTo(GameState.Shop);
      else if (e.label == "Enemy")
      {
        // ส่ง enemy ไปให้ BattleSystem ก่อนเปลี่ยน state
        // (inject ผ่าน event หรือ ScriptableObject channel ก็ได้)
        GameStateManager.GoTo(GameState.Prepare);
      }
      else
        GameStateManager.GoTo(GameState.Prepare);  // Nothing

      return;
    }
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
  public event System.Action<string> OnPrepareEventChanged;

  [Header("CutScene")]
  [SerializeField] private float cutSceneDuration = 3f;

  [Header("Events — ลาก MockEvent หรือ RealEvent ใส่นี้")]
  [SerializeField] private List<MonoBehaviour> prepareEvents;
  private IPrepareEvent currentEvent;
  private bool playerWon;

  // ─────────────────────────────────────────
  private void Start()
  {
    GameStateManager.OnStateChanged += OnStateChanged;
    BattleSystem.OnBattleEnded += OnBattleEnded;

    EnterPrepare();
  }

  private void OnDestroy()
  {
    GameStateManager.OnStateChanged -= OnStateChanged;
    BattleSystem.OnBattleEnded -= OnBattleEnded;
  }

  // ─────────────────────────────────────────
  private void OnStateChanged(GameState prev, GameState next)
  {
    Debug.Log(prev.ToString() + " " + next.ToString());
  }

  // ─────────────────────────────────────────
  // Prepare
  // ─────────────────────────────────────────
  private void EnterPrepare()
  {
    currentEvent = PickNextEvent();
    currentEvent?.OnEventBegin();
    GameStateManager.GoTo(GameState.Prepare);
    OnPrepareEventChanged?.Invoke(currentEvent?.EventLabel);
  }

  private IPrepareEvent PickNextEvent()
  {
    if (prepareEvents == null || prepareEvents.Count == 0) return null;
    int index = _eventIndex % prepareEvents.Count;
    _eventIndex++;
    return prepareEvents[index] as IPrepareEvent;
  }
  private int _eventIndex = 0;

  // ─────────────────────────────────────────
  // Battle
  // ─────────────────────────────────────────

  // เรียกจาก UI Button "Start Battle"
  public void RequestStartBattle()
  {
    if (GameStateManager.Current != GameState.Prepare) return;
    GameStateManager.GoTo(GameState.Battle);
  }

  // BattleSystem ยิง static event นี้เมื่อ HP ฝั่งใดฝั่งหนึ่งหมด
  private void OnBattleEnded(bool didPlayerWin)
  {
    playerWon = didPlayerWin;
    currentEvent?.OnBattleEnd(playerWon);

    if (!playerWon)
    {
      GameStateManager.GoTo(GameState.GameOver);
      return;
    }

    GameStateManager.GoTo(GameState.Travel);
    StartCoroutine(CutSceneRoutine());
  }

  public void RequestEndShop()
  {
    if (GameStateManager.Current != GameState.Prepare)
      return;

    GameStateManager.GoTo(GameState.Travel);
    StartCoroutine(CutSceneRoutine());
  }

  private IEnumerator CutSceneRoutine()
  {
    yield return new WaitForSeconds(cutSceneDuration);
    EnterPrepare();   // วน loop
  }



  // ─────────────────────────────────────────
  // UI Buttons
  // ─────────────────────────────────────────

  public void RequestRestart()
  {
    PlayerData.Instance?.ResetData();
    EnterPrepare();
  }
}

using System;
using System.Collections;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
  public event Action<string> OnPrepareEventChanged;

  [Header("Progression")]
  [SerializeField] private GameProgressionSO progression;

  [Header("Event Handlers")]
  [SerializeField] private MockEnemyEvent enemyEvent;
  [SerializeField] private MockShopEvent shopEvent;


  [Header("Debug")]
  [SerializeField] private GameState initialGameState = GameState.Prepare;

  [Header("CutScene")]
  [SerializeField] private PanelSlider parnalSlider;
  [SerializeField] private float cutSceneDuration = 3f;

  private CharacterView playerCharacterView;

  private int stageIndex;
  private int eventIndex;
  private IPrepareEvent currentEvent;
  private bool playerWon;

  public StageData CurrentStage => progression.stages[stageIndex];
  public StageEvent CurrentStageEvent => CurrentStage.events[eventIndex];
  public int StageIndex => stageIndex;
  public int EventIndex => eventIndex;

  // ── Unity ─────────────────────────────────────────────────────────────────

  private void Start()
  {
    GameStateManager.OnStateChanged += OnStateChanged;
    BattleSystem.OnBattleEnded += OnBattleEnded;
    parnalSlider.OnPageChanged += NextWave;

    EnterInitialState();
  }

  private void OnDestroy()
  {
    GameStateManager.OnStateChanged -= OnStateChanged;
    BattleSystem.OnBattleEnded -= OnBattleEnded;
  }
  public void SetUpPlayerView(CharacterView playerCharacterView)
  {
    this.playerCharacterView = playerCharacterView;
  }
  private void OnStateChanged(GameState prev, GameState next)
  {
    Debug.Log($"[State] {prev} → {next}");
  }

  // ── Initial ───────────────────────────────────────────────────────────────

  private void EnterInitialState()
  {
    switch (initialGameState)
    {
      case GameState.Battle: GameStateManager.GoTo(GameState.Battle); break;
      case GameState.GameOver: GameStateManager.GoTo(GameState.GameOver); break;
      default: EnterPrepare(); break;
    }
  }

  // ── Prepare ───────────────────────────────────────────────────────────────

  private void EnterPrepare()
  {
    SetPlayerRunning(false);

    if (stageIndex >= progression.stages.Count)
    {
      Debug.Log("=== Game Clear! ===");
      GameStateManager.GoTo(GameState.GameEnd);
      return;
    }

    currentEvent = PickNextEvent();
    currentEvent?.OnEventBegin();

    GameStateManager.GoTo(GameState.Prepare);
    OnPrepareEventChanged?.Invoke(currentEvent?.EventLabel);
  }

  private IPrepareEvent PickNextEvent()
  {
    var stageEvent = CurrentStageEvent;
    switch (stageEvent.type)
    {
      case StageEventType.Battle:
        enemyEvent.SetLevelData(stageEvent.enemyLevel);
        return enemyEvent;
      case StageEventType.Shop:
        return shopEvent;
      default:
        return null;
    }
  }

  // ── Battle ────────────────────────────────────────────────────────────────

  public void RequestStartBattle()
  {
    if (GameStateManager.Current != GameState.Prepare) return;
    if (CurrentStageEvent.type != StageEventType.Battle) return;
    GameStateManager.GoTo(GameState.Battle);
  }

  private void OnBattleEnded(bool didPlayerWin)
  {
    playerWon = didPlayerWin;
    currentEvent?.OnBattleEnd(playerWon);

    if (!playerWon)
    {
      GameStateManager.GoTo(GameState.GameOver);
      return;
    }

    StartCoroutine(AdvanceRoutine());
  }

  // ── Shop ──────────────────────────────────────────────────────────────────

  public void RequestEndShop()
  {
    if (GameStateManager.Current != GameState.Prepare) return;
    StartCoroutine(AdvanceRoutine());
  }

  // ── Advance ───────────────────────────────────────────────────────────────

  private IEnumerator AdvanceRoutine()
  {
    GameStateManager.GoTo(GameState.Travel);

    // ── เริ่มวิ่ง ─────────────────────────────────────────────────────────
    SetPlayerRunning(true);

    eventIndex++;

    if (eventIndex >= CurrentStage.events.Count)
    {
      parnalSlider.Next();
    }
    else
    {
      yield return new WaitForSeconds(cutSceneDuration);
      NextWave(0);
    }
  }

  private void NextWave(int _)
  {
    if (eventIndex >= CurrentStage.events.Count)
    {
      eventIndex = 0;
      stageIndex++;
    }

    EnterPrepare();
  }

  // ── Helpers ───────────────────────────────────────────────────────────────

  private void SetPlayerRunning(bool isRunning)
  {
    playerCharacterView?.PlayRun(isRunning);
  }

  // ── UI ────────────────────────────────────────────────────────────────────

  public void RequestRestart()
  {
    stageIndex = 0;
    eventIndex = 0;
    PlayerData.Instance?.ResetData();
    EnterPrepare();
  }
}
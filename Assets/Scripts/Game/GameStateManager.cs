using System;

public enum GameState
{
  Prepare,
  Battle,
  Travel,
  Shop,
  GameOver,
  GameEnd
}

public static class GameStateManager
{
  public static GameState Current { get; private set; } = GameState.Prepare;

  public static event Action<GameState, GameState> OnStateChanged;

  public static void GoTo(GameState next)
  {
    if (Current == next) return;
    var prev = Current;
    Current = next;
    OnStateChanged?.Invoke(prev, next);
  }
}

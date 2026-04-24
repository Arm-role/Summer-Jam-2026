using System.Collections.Generic;
using UnityEngine;

public class BattleSceneSpawner : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private BattleSystem battleSystem;

  [Header("Spawn Points")]
  [SerializeField] private Transform playerSpawnPoint;
  [SerializeField] private Transform[] enemySpawnSlots;

  [Header("Player")]
  [SerializeField] private CharacterView playerPrefab;

  private CharacterView spawnedPlayer;
  private readonly List<CharacterView> spawnedEnemies = new();

  // ─────────────────────────────────────────
  // เรียกจาก GameLoopController ตอน IPrepareEvent.OnEventBegin()
  // ─────────────────────────────────────────
  public void SpawnBattle(List<EnemyUnitSO> enemySOs, int goldReward)
  {
    ClearAll();
    SpawnPlayer();
    SpawnEnemies(enemySOs, goldReward);
  }

  public void ClearAll()
  {
    if (spawnedPlayer != null)
      Destroy(spawnedPlayer.gameObject);

    foreach (var v in spawnedEnemies)
      if (v != null) Destroy(v.gameObject);

    spawnedEnemies.Clear();
  }

  // ─────────────────────────────────────────
  private void SpawnPlayer()
  {
    spawnedPlayer = Instantiate(
        playerPrefab,
        playerSpawnPoint.position,
        Quaternion.identity);

    spawnedPlayer.PlayIdle();

    battleSystem.BindPlayerView(spawnedPlayer);
  }

  private void SpawnEnemies(List<EnemyUnitSO> enemySOs, int goldReward)
  {
    var viewList = new List<CharacterView>();

    for (int i = 0; i < enemySOs.Count; i++)
    {
      var so = enemySOs[i];

      if (so.prefab == null)
      {
        Debug.LogWarning(
          $"EnemyUnitSO '{so.enemyName}' ไม่มี prefab");

        viewList.Add(null);
        continue;
      }

      if (i >= enemySpawnSlots.Length)
      {
        Debug.LogWarning(
          "Not enough enemy spawn slots!");

        viewList.Add(null);
        continue;
      }

      var spawnPoint = enemySpawnSlots[i];

      var view = Instantiate(
        so.prefab,
        spawnPoint.position,
        Quaternion.identity);

      view.PlayIdle();

      spawnedEnemies.Add(view);
      viewList.Add(view);
    }

    battleSystem.InitializedEnemy(
      enemySOs,
      viewList,
      goldReward);
  }
}
using TD.Rules;
using TD.Data;
using UnityEngine;
using System;

namespace TD.SceneGlue
{
    public sealed class LevelBootstrapper : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private LevelPath levelPath;
        [SerializeField] private PathLineView pathView;
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private EnemyData testEnemyData; // temp test
        private WaypointPath path;
        private bool HasAllReferences()
        {
            bool ready = true;
            if (levelData == null)
            {
                Debug.LogError($"Missing: {nameof(levelData)}", this);
                ready = false;
            }
            if (levelPath == null)
            {
                Debug.LogError($"Missing: {nameof(levelPath)}", this);
                ready = false;
            }
            if (pathView == null)
            {
                Debug.LogError($"Missing: {nameof(pathView)}", this);
                ready = false;
            }
            if (enemyPrefab == null)
            {
                Debug.LogError($"Missing: {nameof(enemyPrefab)}", this);
                ready = false;
            }
            if (testEnemyData == null)
            {
                Debug.LogError($"Missing: {nameof(testEnemyData)}", this);
                ready = false;
            }
            return ready;
        }
        void Awake()
        {
            if (HasAllReferences() == false)
            {
                enabled = false;
                return;
            }

            path = levelPath.BuildPath();
            pathView.Initialize(path);

            SpawnTestEnemy();

            Debug.Log(levelData.DisplayName + " - path " + path.TotalLength + " units, " + levelData.StartingCash + " cash, " + levelData.StartingLives + " lives");
        }
        private void SpawnTestEnemy()
        {
            // enemyPrefab.Initialize(testEnemyData, path);
            Enemy enemy = Instantiate(enemyPrefab);
            enemy.Died += HandleEnemyDied;
            enemy.ReachedEnd += HandleEnemyReachedEnd;

            enemy.Initialize(testEnemyData, path);

            Debug.Log("Spawned " + enemy.Data.DisplayName + " " + enemy.Data.LivesCost + " Lives cost " + enemy.Data.Speed + " Speed " + enemy.Data.Reward + " Reward");
        }
        private void HandleEnemyDied(Enemy enemy)
        {
            Debug.Log(enemy.Data.DisplayName + " popped (+" + enemy.Data.Reward + " cash)");
            Despawn(enemy);
        }
        private void HandleEnemyReachedEnd(Enemy enemy)
        {
            Debug.Log(enemy.Data.DisplayName + " reached the end(-" + enemy.Data.LivesCost + " lives)");
            Despawn(enemy);

        }
        private void Despawn(Enemy enemy)
        {
            enemy.Died -= HandleEnemyDied;
            enemy.ReachedEnd -= HandleEnemyReachedEnd;

            Destroy(enemy.gameObject);
        }
    }
}



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
        [SerializeField] EnemySpawner enemySpawner;
        [SerializeField] EnemyData testEnemyData;
        // Replaced by WaveManager
        [SerializeField, Min(0.1f)] private float testSpawnInterval = 1f;
        private float spawnTimer = 0f;
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
            if (enemySpawner == null)
            {
                Debug.LogError($"Missing: {nameof(enemySpawner)}", this);
                ready = false;
            }
            if (testEnemyData == null)
            {
                ready = false;
                Debug.LogError($"Missing: {nameof(testEnemyData)}", this);

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

            Debug.Log(levelData.DisplayName + " - path " + path.TotalLength + " units, " + levelData.StartingCash + " cash, " + levelData.StartingLives + " lives");

            enemySpawner.Initialize(path);
            enemySpawner.EnemyDied += HandleEnemyDied;
            enemySpawner.EnemyLeaked += HandleEnemyReachedEnd;
        }
        private void HandleEnemyDied(Enemy enemy)
        {
            Debug.Log(enemy.Data.DisplayName + " popped (+" + enemy.Data.Reward + " cash)");
        }
        private void HandleEnemyReachedEnd(Enemy enemy)
        {
            Debug.Log(enemy.Data.DisplayName + " reached the end(-" + enemy.Data.LivesCost + " lives)");

        }
        private void Update() {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer > 0f) return;
            
            enemySpawner.Spawn(testEnemyData);
            spawnTimer = testSpawnInterval;
        }

        private void OnDestroy() {
            if (enemySpawner == null) return;

            enemySpawner.EnemyDied -= HandleEnemyDied;
            enemySpawner.EnemyLeaked -= HandleEnemyReachedEnd;
            
        }
    }
}



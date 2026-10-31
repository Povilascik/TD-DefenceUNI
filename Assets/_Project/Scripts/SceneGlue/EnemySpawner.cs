using System;
using TD.Data;
using TD.Rules;
using UnityEngine;

namespace TD.SceneGlue
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField, Min(0)] private int prewarmCount = 20;
        private ObjectPool<Enemy> pool;
        private IPath path;
        public event Action<Enemy> EnemyDied;
        public event Action<Enemy> EnemyLeaked;
        public void Initialize(IPath path)
        {
            if(path==null) throw new ArgumentNullException(nameof(path),"Path is null");
            this.path = path;

            pool = new ObjectPool<Enemy>(CreateEnemy, OnGet, OnRelease, prewarmCount);
        }
        public Enemy Spawn(EnemyData data)
        {
            if(pool == null) throw new InvalidOperationException("Spawner used before Initialize was called.");
            if (data == null) throw new ArgumentNullException(nameof(data));
            Enemy enemy = pool.Get();
            enemy.Died += HandleDied;
            enemy.ReachedEnd += HandleReachedEnd;
            enemy.Initialize(data, path);

            return enemy;
        }
        private Enemy CreateEnemy()
        {
            return Instantiate(enemyPrefab, transform);
        }
        private void OnGet(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }
        private void OnRelease(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
        private void HandleDied(Enemy enemy)
        {
            EnemyDied?.Invoke(enemy);
            Despawn(enemy);
        }
        private void HandleReachedEnd(Enemy enemy)
        {
            EnemyLeaked?.Invoke(enemy);
            Despawn(enemy);
        }
        private void Despawn(Enemy enemy)
        {
            enemy.Died -= HandleDied;
            enemy.ReachedEnd -= HandleReachedEnd;
            pool.Release(enemy);
        }

    }
}

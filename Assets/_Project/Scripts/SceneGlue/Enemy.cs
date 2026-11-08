using UnityEngine;
using System;
using TD.Data;
using TD.Rules;

namespace TD.SceneGlue
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private EnemyView enemyView;
        public float Distance => enemyMovement.Distance;
        private void Reset()
        {
            enemyHealth = GetComponent<EnemyHealth>();
            enemyMovement = GetComponent<EnemyMovement>();
            enemyView = GetComponent<EnemyView>();
        }
        public EnemyData Data{ get; private set; }
        public event Action<Enemy> Died;
        public event Action<Enemy> ReachedEnd;
        public void Initialize(EnemyData data, IPath path)
        {
            if(data == null) throw new ArgumentNullException(nameof(data));
            if(path == null) throw new ArgumentNullException(nameof(path));
            
            Data = data;
            enemyHealth.Initialize(data.MaxHealth);
            enemyMovement.Initialize(path, Data.Speed);
            enemyView.Initialize(data.Sprite, data.Tint, data.Scale);
        }
        private void OnEnable() 
        {
            enemyHealth.Died += HandleDied;
            enemyMovement.ReachedEnd += HandleReachedEnd;
        }
        private void OnDisable()
        {
            enemyHealth.Died -= HandleDied;
            enemyMovement.ReachedEnd -= HandleReachedEnd;
        }
        private void HandleDied()
        {
            Died?.Invoke(this);
        }
        private void HandleReachedEnd() => ReachedEnd?.Invoke(this);
    }
}

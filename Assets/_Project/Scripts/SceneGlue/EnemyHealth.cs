using System;
using TD.Rules;
using UnityEngine;

namespace TD.SceneGlue
{
    public sealed class EnemyHealth : MonoBehaviour
    {
        private Health health;
        public event Action Died;
        public void Initialize(int maxHealth)
        {
            health = new Health(maxHealth);
        }
        public void TakeDamage(int amount)
        {
            if (health == null) throw new InvalidOperationException("EnemyHealth used before Initialize was called.");
            
            if (health.TakeDamage(amount)) Died?.Invoke();

        }
        [ContextMenu("Debug: Take 1 Damage")]
        private void DebugTakeOneDamage() => TakeDamage(1);
    }
}

using UnityEngine;
using System;
using System.Collections.Generic;

namespace TD.SceneGlue
{
    public sealed class RangeDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask enemyLayers;
        private float range;
        private ContactFilter2D filter;
        private readonly List<Collider2D> hits = new List<Collider2D>();
        private readonly List<Enemy> enemiesInRange = new List<Enemy>();
        public void Initialize(float range)
        {
            if (range <= 0) throw new ArgumentOutOfRangeException(nameof(range));
            this.range = range;

            filter = new ContactFilter2D();
            filter.SetLayerMask(enemyLayers);
            filter.useTriggers = true;  
        }
        public IReadOnlyList<Enemy> FindEnemiesInRange()
        {
            enemiesInRange.Clear();
            Physics2D.OverlapCircle(transform.position, range, filter, hits);
            foreach(Collider2D hit in hits)
            {
                if (hit.TryGetComponent(out Enemy enemy)) enemiesInRange.Add(enemy);
            }
            return enemiesInRange;
        }
        private void OnDrawGizmosSelected() {

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}

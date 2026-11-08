using UnityEngine;

namespace TD.SceneGlue
{
    public sealed class TowerAim : MonoBehaviour
    {
        [SerializeField] private Transform turret;
        [SerializeField] private RangeDetector rangeDetector;
        private void Update()
        {
            Enemy target = FindFirst();
            if (target == null) return;

            Vector3 direction = target.transform.position - turret.position;
            turret.right = direction;
        }

        private Enemy FindFirst()
        {
            Enemy first = null;
            foreach (Enemy enemy in rangeDetector.FindEnemiesInRange())
            {
                if (first == null || enemy.Distance>first.Distance) first = enemy;
            }
            return first;
        }
    }
}

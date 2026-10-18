using System;
using TD.Rules;
using UnityEngine;

namespace TD.SceneGlue
{
    public sealed class EnemyMovement : MonoBehaviour
    {
        private IPath path;
        private float speed;
        private bool hasReachedEnd;
        public float Distance { get; private set; }
        public event Action ReachedEnd;
        public void Initialize(IPath path, float speed)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            this.path = path;
            this.speed = speed;
            Distance = 0;
            hasReachedEnd = false;

            transform.position = path.PositionAt(Distance).ToUnity();
        }
        void Update()
        {
            if (path == null || hasReachedEnd) return;

            Distance += speed * Time.deltaTime;

            transform.position = path.PositionAt(Distance).ToUnity();

            if(path.HasReachedEnd(Distance))
            {
                hasReachedEnd = true;
                ReachedEnd?.Invoke();
            }
        }
    }
}

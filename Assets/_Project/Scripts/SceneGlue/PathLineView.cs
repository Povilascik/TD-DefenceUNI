using System;
using TD.Rules;
using UnityEngine;

namespace TD.SceneGlue
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class PathLineView : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;

        private void Reset()
        {
            line = GetComponent<LineRenderer>();
        }

        public void Initialize(IPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            line.positionCount = path.Waypoints.Count;

            for (int i = 0; i < line.positionCount; i++)
            {
                line.SetPosition(i,VectorConversions.ToUnity(path.Waypoints[i]));
            }
        }
    }
}

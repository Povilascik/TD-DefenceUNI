using UnityEngine;
using System;
using System.Collections.Generic;
using NumericsVector2 = System.Numerics.Vector2;
using TD.Rules;
using System.IO;

namespace TD.SceneGlue
{
    public sealed class LevelPath : MonoBehaviour
    {
        [Tooltip("Points of path to follow, in order")]
        [SerializeField] private Transform[] waypoints;

        private const float GizmosRadius = 0.3f;

        private void OnDrawGizmos()
        {
            if (waypoints == null) return;
            Gizmos.color = Color.yellow;

            DrawWaypointCircles();

            DrawPathLines();

        }

        private void DrawWaypointCircles()
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                Transform waypoint = waypoints[i];

                if (waypoint == null) continue;
                Gizmos.DrawWireSphere(waypoint.position, GizmosRadius);
            }
        }

        private void DrawPathLines()
        {
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Transform from = waypoints[i];
                Transform to = waypoints[i + 1];

                if (from == null || to == null) continue;

                Gizmos.DrawLine(from.position, to.position);
            }
        }

        private void OnValidate()
        {
            if (waypoints == null || waypoints.Length < 2)
            {
                Debug.LogWarning("LevelPath needs at least 2 waypoints. Drag them into the Waypoints list.", this);
                return;
            }

            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) Debug.LogWarning($"Waypoint {i} is empty (NULL)", this);
            }
        }

        public WaypointPath BuildPath()
        {
            var points = new List<NumericsVector2>(waypoints.Length);

            for (int i = 0; i < waypoints.Length; i++)
            {
                Transform waypoint = waypoints[i];
                if (waypoint == null) throw new InvalidOperationException("Slot " + i + " is Empty");

                points.Add(waypoint.position.ToNumerics());
            }
            return new WaypointPath(points);
        }

        [ContextMenu("Log Path Length")]
        private void LogPathLength()
        {
            WaypointPath path = BuildPath();
            Debug.Log($"Path length: {path.TotalLength:F2}", this);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Numerics;

namespace TD.Rules
{
    public sealed class WaypointPath : IPath
    {
        private readonly Vector2[] points;
        private readonly float[] cumulativeDistances;
        public float TotalLength { get; }

        public IReadOnlyList<Vector2> Waypoints { get; }
        // Constructor
        public WaypointPath(IReadOnlyList<Vector2> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if (points.Count < 2) throw new ArgumentException("A path needs at least 2 points", nameof(points));
            
            this.points = new Vector2[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                this.points[i] = points[i];
            }
            // Total distance so far
            cumulativeDistances = new float[this.points.Length];
            // Calculates total distance
            for (int i = 1; i < this.points.Length; i++)
            {
                float segmentLength = Vector2.Distance(this.points[i - 1], this.points[i]);
                cumulativeDistances[i] = cumulativeDistances[i - 1] + segmentLength;
            }

            TotalLength = cumulativeDistances[cumulativeDistances.Length - 1];

            Waypoints = Array.AsReadOnly(this.points);
        }
        // Returns a Vector direction of where the enemy is looking at
        public Vector2 DirectionAt(float distance)
        {
            int segment = FindSegmentIndex(distance);
            // (b - a), to find the direction
            Vector2 delta = points[segment + 1] - points[segment];
            if (delta == Vector2.Zero) return Vector2.Zero;
            return Vector2.Normalize(delta);
        }

        public bool HasReachedEnd(float distance) => distance >= TotalLength;

        public Vector2 PositionAt(float distance)
        {
            // Clamps distance to a specific range
            float clampedDistance = Math.Clamp(distance, 0f, TotalLength);
            int segment = FindSegmentIndex(clampedDistance);

            float segmentStart = cumulativeDistances[segment];
            float segmentLength = cumulativeDistances[segment + 1] - segmentStart;

            if (segmentLength <= 0f) return points[segment];

            float t = (clampedDistance - segmentStart) / segmentLength;

            return Vector2.Lerp(points[segment], points[segment + 1], t);
        }

        private int FindSegmentIndex(float distance)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                if (distance <= cumulativeDistances[i + 1]) return i;
            }
            return points.Length - 2;
        }
    }
}

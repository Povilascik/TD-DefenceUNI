using System.Collections.Generic;
using System.Numerics;

namespace TD.Rules
{
    public interface IPath
    {
        /// <summary>
        /// How long is the whole route?
        /// </summary>
        float TotalLength { get; }
        /// <summary>
        /// Where are the corners?
        /// </summary>
        IReadOnlyList<Vector2> Waypoints { get; }
        /// <summary>
        /// After walking X units, where am I?
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        Vector2 PositionAt(float distance);
        /// <summary>
        /// Which way am I facing?
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        
        Vector2 DirectionAt(float distance);
        /// <summary>
        /// Have I finished the route?
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        bool HasReachedEnd(float distance);

    } 
}

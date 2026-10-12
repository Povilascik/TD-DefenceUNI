using UnityEngine;

namespace TD.SceneGlue
{
    public static class VectorConversions
    {
        /// <summary>
        /// System numerics vector uses uppercase X and Y. Takes Vector3, because transform.position is a Vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static System.Numerics.Vector2 ToNumerics(this UnityEngine.Vector3 v) => new(v.x, v.y);
        
        /// <summary>
        /// Unity Vector uses lowercase x and y. Drops z, because the game is 2D
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector2 v) => new(v.X,v.Y);
        
    }
}

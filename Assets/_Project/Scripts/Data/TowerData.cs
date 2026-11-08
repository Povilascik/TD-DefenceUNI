using UnityEngine;

namespace TD.Data
{
    [CreateAssetMenu(fileName = "TowerData", menuName = "TD/Tower Data")]

    public sealed class TowerData : ScriptableObject
    {
        [SerializeField] private string displayName;
        [Tooltip("Radius of rhe range circle, in world units")]
        [Min(0.5f)]
        [SerializeField] private float range = 3f;

        public string DisplayName => displayName;
        public float Range => range;
    }
}

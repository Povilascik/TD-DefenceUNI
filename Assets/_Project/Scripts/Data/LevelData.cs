using UnityEngine;


// ScriptableObject as the base class.

// [CreateAssetMenu], which adds a right-click menu item for making the asset.

// [SerializeField] private: Unity saves the field and shows it in the Inspector, but other scripts can't touch it.

// A read-only property (=>): other scripts can read the value but never change it.

// sealed: no class may inherit from this one.

namespace TD.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "TD/LevelData")]
    public sealed class LevelData : ScriptableObject
    {
        [SerializeField] private string displayName;
        [Min(0)]
        [SerializeField] private int startingCash;
        [Min(1)]
        [SerializeField] private int startingLives;

        public string DisplayName => displayName;
        public int StartingCash => startingCash;
        public int StartingLives => startingLives;
    }
}

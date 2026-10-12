using UnityEngine;

namespace TD.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "TD/EnemyData")]
    public sealed class EnemyData : ScriptableObject
    {
        [Tooltip("Enemy Name")]
        [SerializeField] private string displayName;

        [Tooltip("Max Health of an enemy")]
        [Min(1)]
        [SerializeField] private int maxHealth = 1;

        [Tooltip("Speed of an enemy, World units per second")]
        // Speed is missing Min because enemies can move backwards if pushed back
        [SerializeField] private float speed = 2f;

        [Tooltip("Reward in Cash for defeating the enemy")]
        [Min(0)]
        [SerializeField] private int reward = 1;

        [Tooltip("Lives lost when reached the base")]
        [Min(1)]
        [SerializeField] private int livesCost = 1;

        [Tooltip("Image to show")]
        [SerializeField] private Sprite sprite;

        [Tooltip("Color laid over the sprite")]
        [SerializeField] private Color tint = Color.white;

        [Tooltip("Size multiplier")]
        [Min(0.1f)]
        [SerializeField] private float scale = 1f;






        public string DisplayName => displayName;
        public int MaxHealth => maxHealth;
        public float Speed => speed;
        public int Reward => reward;
        public int LivesCost => livesCost;
        public Sprite Sprite => sprite;
        public Color Tint => tint;
        public float Scale => scale;
    }
}

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TD.SceneGlue
{
    public sealed class EnemyView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private void Reset()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void Initialize(Sprite sprite, Color tint, float scale)
        {
            if (spriteRenderer == null) throw new InvalidOperationException("SpriteRenderer is not assigned");

            spriteRenderer.sprite = sprite;
            spriteRenderer.color = tint;
            spriteRenderer.transform.localScale = Vector3.one * scale;
        }

    }
}

using UnityEngine;
using System;
using TD.Data;

namespace TD.SceneGlue
{
    public sealed class Tower : MonoBehaviour
    {
        [SerializeField] private RangeDetector rangeDetector;
        public TowerData Data { get; private set; }
        public void Initialize(TowerData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            Data = data;
            rangeDetector.Initialize(Data.Range);
        }
    }
}

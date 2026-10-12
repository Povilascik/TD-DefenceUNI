using TD.Rules;
using TD.Data;
using UnityEngine;
using System;

namespace TD.SceneGlue
{
    public sealed class LevelBootstrapper : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private LevelPath levelPath;
        [SerializeField] private PathLineView pathView;
        private WaypointPath path;
        private bool HasAllReferences()
        {
            bool ready = true;
            if (levelData == null)
            {
                Debug.LogError($"Missing: {nameof(levelData)}", this);
                ready = false;
            }
            if (levelPath == null)
            {
                Debug.LogError($"Missing: {nameof(levelPath)}", this);
                ready = false;
            }
            if (pathView == null)
            {
                Debug.LogError($"Missing: {nameof(pathView)}", this);
                ready = false;
            }
            return ready;
        }
        void Awake()
        {
            if (HasAllReferences() == false)
            {
                enabled = false;
                return;
            }
            
            path = levelPath.BuildPath();
            pathView.Initialize(path);

            Debug.Log(levelData.DisplayName + " - path " + path.TotalLength + " units, " + levelData.StartingCash + " cash, " + levelData.StartingLives + " lives");
        }
    }
}



using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheckpointSystem
{

    public abstract class BaseTrack: MonoBehaviour
    {
        [SerializeField] private List<Checkpoint> checkpoints;
        
        [HideInInspector] public bool IsStarted;
        
        protected CheckpointsManager _checkpointsManager;
        private void Start()
        {
            checkpoints[0].OnLapComplete += OnLapComplete;
            _checkpointsManager = new CheckpointsManager(checkpoints);
        }
        
        [ContextMenu("Update Checkpoints List")]
        private void UpdateCheckpointsList()
        {
            checkpoints = GetComponentsInChildren<Checkpoint>().ToList();
        }

        
        [ContextMenu("Draw lines Between Checkpoints")]
        private void DrawLineBetweenCheckpoints()
        {
            CurveDrawer.DrawLineBetweenCheckpoints(checkpoints);
        }

        protected void TrackComplete()
        {
            if (IsStarted)
            {
                Debug.Log("Track is complete");
                _checkpointsManager.DeactivateAllCheckpoints();
            }
        }

        protected abstract void OnLapComplete();
        
        public abstract void ResetTrack();
        
    }
}

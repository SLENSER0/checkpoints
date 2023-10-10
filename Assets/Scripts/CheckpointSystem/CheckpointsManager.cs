using System.Collections.Generic;
using UnityEngine;

namespace CheckpointSystem
{
    public class CheckpointsManager
    {
        private List<Checkpoint> _checkpoints;
        public int CurrentCheckpoint = -1;
        private int _nextCheckpoint => (CurrentCheckpoint + 1) % _checkpoints.Count;


        public CheckpointsManager(List<Checkpoint> checkpoints)
        {
            _checkpoints = checkpoints;
            AttachCheckpointEventHandlers();
            DeactivateAllCheckpointsExceptFirst();
        }
        
        private void AttachCheckpointEventHandlers()
        {
            foreach (var checkpoint in _checkpoints)
            {
                checkpoint.OnCheckpointPassed += OnCheckpointPassed;
            }
        }
        public void DeactivateAllCheckpoints()
        {
            foreach (var checkpoint in _checkpoints)
            {
                checkpoint.gameObject.SetActive(false);
            }
        }
        
        private void OnCheckpointPassed(int checkpointIndex)
        {
            if (checkpointIndex == _nextCheckpoint)
            {
                CurrentCheckpoint = checkpointIndex;
            }
            
            Debug.Log($"CurrentCheckpoint {CurrentCheckpoint}");
            Debug.Log($"NextCheckpoint {_nextCheckpoint}");
            DeactivateCurrentCheckpointAndActivateNext();
        }

        private void DeactivateCurrentCheckpointAndActivateNext()
        {
            _checkpoints[CurrentCheckpoint].gameObject.SetActive(false);
            
            if (_nextCheckpoint < _checkpoints.Count)
            {
                _checkpoints[_nextCheckpoint].gameObject.SetActive(true);
            }
            _checkpoints[_nextCheckpoint].ObjectAlongSplineAnimate.Restart(true);
            
        }

        public void DeactivateAllCheckpointsExceptFirst()
        {
            foreach (var checkpoint in _checkpoints)
            {
                checkpoint.gameObject.SetActive(false);
                
            }
            _checkpoints[0].gameObject.SetActive(true);
            
            _checkpoints[0].Spline.gameObject.SetActive(false);
            _checkpoints[0].ObjectAlongSplineAnimate.gameObject.SetActive(false);
        }

        public void StartTrack()
        {
            _checkpoints[0].Spline.gameObject.SetActive(true);
            _checkpoints[0].ObjectAlongSplineAnimate.gameObject.SetActive(true);
        }
    }
}

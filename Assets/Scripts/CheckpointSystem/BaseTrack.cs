using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CheckpointSystem
{
    public abstract class BaseTrack: MonoBehaviour
    {
        [SerializeField] private List<Checkpoint> checkpoints;
        
        
        protected int _currentCheckpoint = -1;
        protected bool _isStarted;
        
        
        protected int _getLastCheckpoint => checkpoints.Count-1;
        private int _nextCheckpoint => (_currentCheckpoint + 1) % checkpoints.Count;
        
        
        private void Start()
        {
            
            if (checkpoints == null || checkpoints.Count < 2)
            {
                throw new InvalidOperationException("Checkpoints list is null or has insufficient elements");
            }
            
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.OnCheckpointPassed += OnCheckpointPassed;
            }
            DeactivateAllCheckpointsExceptFirst();
        }
        

        [ContextMenu("Update Checkpoints List")]
        private void UpdateCheckpoints()
        {
            checkpoints = transform.GetComponentsInChildren<Checkpoint>().ToList();
        }
        
        
        [ContextMenu("Draw lines Between Checkpoints")]
        private void DrawLineBetweenCheckpoints()
        {
            CurveDrawer.DrawLineBetweenCheckpoints(checkpoints);
        }


        private void DeactivateAllCheckpoints()
        {
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.gameObject.SetActive(false);
            }
        }

        private void OnCheckpointPassed(int checkpointIndex)
        {
            if (checkpointIndex == _nextCheckpoint)
            {
                _currentCheckpoint = checkpointIndex;
            }
            
            Debug.Log($"CurrentCheckpoint {_currentCheckpoint}");
            Debug.Log($"NextCheckpoint {_nextCheckpoint}");
            ActivateNextCheckpoint();
            
        }

        protected void DeactivateCurrentCheckpointAndActivateNext()
        {
            checkpoints[_currentCheckpoint].transform.gameObject.SetActive(false);
            
            if (_nextCheckpoint < checkpoints.Count)
            {
                checkpoints[_nextCheckpoint].transform.gameObject.SetActive(true);
            }
            checkpoints[_nextCheckpoint].ObjectAlongSplineAnimate.Restart(true);
            
        }

        protected void DeactivateAllCheckpointsExceptFirst()
        {
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.transform.gameObject.SetActive(false);
                
            }
            checkpoints[0].transform.gameObject.SetActive(true);
            
            checkpoints[0].Spline.gameObject.SetActive(false);
            checkpoints[0].ObjectAlongSplineAnimate.gameObject.SetActive(false);;
        }

        protected void StartTrack()
        {
            checkpoints[0].Spline.gameObject.SetActive(true);
            checkpoints[0].ObjectAlongSplineAnimate.gameObject.SetActive(true);
            _isStarted = true;
        }

        public virtual void ResetTrack()
        {
            _currentCheckpoint = -1;
            DeactivateAllCheckpointsExceptFirst();
            _isStarted = false;
        }

        protected void OnTrackComplete()
        {
            Debug.Log("Track is complete");
            DeactivateAllCheckpoints();
        }

        protected abstract void OnLapComplete();
        protected abstract void ActivateNextCheckpoint();
    }
}
using UnityEngine;

namespace CheckpointSystem
{
    public class FinalTrack : BaseTrack
    {
        private bool _isStarted;
        public override void ResetTrack()
        {
            _currentCheckpoint = -1;
            DeactivateAllCheckpointsExceptFirst();
            _isStarted = false;
        }
        

        protected override void OnLapComplete()
        {
            Debug.Log("Track is complete");
            DeactivateAllCheckpoints();
            
        }

        protected override void ActivateNextCheckpoint()
        {
            сheckpoints[_currentCheckpoint].gameObject.SetActive(false);
            
            if (_nextCheckpoint < сheckpoints.Count)
            {
                сheckpoints[_nextCheckpoint].gameObject.SetActive(true);
            }
            if(_nextCheckpoint == 1 && _isStarted)
            {
                OnLapComplete();
            }
            _isStarted = true;
        
        }
    
    }
}

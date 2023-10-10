using UnityEngine;

namespace CheckpointSystem
{
    public class FinalTrack : BaseTrack
    {
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
            DeactivateCurrentCheckpointAndActivateNext();
            
            if(_nextCheckpoint == 1 && _isStarted)
            {
                OnLapComplete();
            }

            StartTrack();

        }
    
    }
}

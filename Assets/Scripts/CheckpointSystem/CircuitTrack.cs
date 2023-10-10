using UnityEngine;

namespace CheckpointSystem
{
    public class CircuitTrack : BaseTrack
    {
        private int _rounds;

        public override void ResetTrack()
        {
            _rounds = 0;
            _checkpointsManager.CurrentCheckpoint = -1;
            _checkpointsManager.DeactivateAllCheckpointsExceptFirst();
            IsStarted = false;
        }

        protected override void OnLapComplete()
        {
            if (IsStarted)
            {
                _rounds += 1;
                Debug.Log($"round {_rounds}");
            }
            else
            {
                IsStarted = true;
            }
            
        }
        
    }
}

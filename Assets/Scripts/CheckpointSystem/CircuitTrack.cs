using UnityEngine;

namespace CheckpointSystem
{
    public class CircuitTrack : BaseTrack
    {
        private int _rounds = 0;

        public override void ResetTrack()
        {
            _rounds = 0;
            CurrentCheckpoint = 0;
            DeactivateAllCheckpointsExceptFirst();
        }

        protected override void OnLapComplete()
        {
            _rounds += 1;
            Debug.Log(_rounds);
        }

        protected override void ActivateNextCheckpoint()
        {
            
            Checkpoints[NextCheckpoint-1].gameObject.SetActive(false);
            if (NextCheckpoint < Checkpoints.Count)
            {
                Checkpoints[NextCheckpoint].gameObject.SetActive(true);
            }
            else
            {
                CurrentCheckpoint = 0;
                Checkpoints[0].gameObject.SetActive(true);
            }
        }
    }
}

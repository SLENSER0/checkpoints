using UnityEngine;

namespace CheckpointSystem
{
    public class CircuitTrack : BaseTrack
    {
        private int _rounds;

        public override void ResetTrack()
        {
            _rounds = 0;
            _currentCheckpoint = -1;
            DeactivateAllCheckpointsExceptFirst();
            _isStarted = false;
        }

        protected override void OnLapComplete()
        {
            _rounds += 1;
            Debug.Log($"round {_rounds}");
        }

        protected override void ActivateNextCheckpoint()
        {

            DeactivateCurrentCheckpointAndActivateNext();

            if (_currentCheckpoint == 0 && _isStarted)
            {
                OnLapComplete();
            }

            _isStarted = true;
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        }
    }
}

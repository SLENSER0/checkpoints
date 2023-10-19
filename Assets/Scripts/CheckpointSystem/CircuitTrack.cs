using System;
using UnityEngine;

namespace CheckpointSystem
{
    public class CircuitTrack : BaseTrack
    {
        private int _rounds;
        [SerializeField] private int maxRounds;

        private void OnValidate()
        {
            if (maxRounds < 1)
                maxRounds = 1;
        }

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

            if (_rounds == maxRounds)
            {
                OnTrackComplete();
            }
        }

        protected override void ActivateNextCheckpoint()
        {

            DeactivateCurrentCheckpointAndActivateNext();

            if (_currentCheckpoint == 0 && _isStarted)
            {
                OnLapComplete();
            }

            StartTrack();

        }
    }
}
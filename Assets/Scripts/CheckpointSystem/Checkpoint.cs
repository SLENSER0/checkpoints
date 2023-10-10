using System;
using UnityEngine;
using UnityEngine.Splines;

namespace CheckpointSystem
{
    public class Checkpoint : MonoBehaviour
    {
        public event Action<int> OnCheckpointPassed;
        public event Action OnLapComplete;

        public SplineContainer Spline;
        public SplineAnimate ObjectAlongSplineAnimate;
        
        private void OnTriggerEnter(Collider other)
        {
            HandleCheckpoint(other);
        }

        private void HandleCheckpoint(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnCheckpointPassed?.Invoke(GetCheckpointIndex());
                
                if (GetCheckpointIndex() == 0)
                {
                    OnLapComplete?.Invoke();
                }
            }
        }
        
        private int GetCheckpointIndex()
        {
            return transform.GetSiblingIndex();
        }
    }
}

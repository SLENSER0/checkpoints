using System;
using UnityEngine;
using UnityEngine.Splines;

namespace CheckpointSystem
{
    public class Checkpoint : MonoBehaviour
    {
        public event Action<int> OnCheckpointPassed;

        public SplineContainer Spline;
        public SplineAnimate ObjectAlongSplineAnimate;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnCheckpointPassed?.Invoke(GetCheckpointIndex());
            }
        }
        
        private int GetCheckpointIndex()
        {
            return transform.GetSiblingIndex();
        }
    }
}

using System;
using UnityEngine;

namespace CheckpointSystem
{
    public class Checkpoint : MonoBehaviour
    {
        public event Action<int> OnCheckpointPassed;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnCheckpointPassed?.Invoke(GetCheckpointIndex());
            }
        }
        
        private int GetCheckpointIndex()
        {
            return transform.parent.GetSiblingIndex();
        }
    }
}

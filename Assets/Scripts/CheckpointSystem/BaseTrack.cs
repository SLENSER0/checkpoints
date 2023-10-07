using System.Collections.Generic;
using UnityEngine;
namespace CheckpointSystem
{
    public abstract class BaseTrack: MonoBehaviour
    {
        [SerializeField] protected List<Checkpoint> сheckpoints;
        protected int _currentCheckpoint = -1;
        protected int _nextCheckpoint => GetNextCheckpoint();
        
        
        private void Start()
        {
            foreach (var checkpoint in сheckpoints)
            {
                checkpoint.OnCheckpointPassed += OnCheckpointPassed;
            }
            DeactivateAllCheckpointsExceptFirst();
        }
        

        [ContextMenu("Update Checkpoints List")]
        private void UpdateCheckpoints()
        {
            сheckpoints.Clear();
            foreach(Transform child in transform)
            {
                Checkpoint checkpoint = child.GetComponent<Checkpoint>();
                if (checkpoint != null)
                {
                    сheckpoints.Add(checkpoint);
                }
            }
        }

        protected void DeactivateAllCheckpoints()
        {
            foreach (var checkpoint in сheckpoints)
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
        
        public abstract void ResetTrack();

        
        protected void DeactivateAllCheckpointsExceptFirst()
        {
            foreach (var checkpoint in сheckpoints)
            {
                checkpoint.gameObject.SetActive(false);
                
            }
            сheckpoints[0].gameObject.SetActive(true);
        }

        protected int GetNextCheckpoint()
        {
            return (_currentCheckpoint + 1) % сheckpoints.Count;
        }

        protected abstract void OnLapComplete();
        protected abstract void ActivateNextCheckpoint();
    }
}

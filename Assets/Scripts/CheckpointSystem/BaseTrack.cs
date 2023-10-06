using System;
using System.Collections.Generic;
using UnityEngine;

namespace CheckpointSystem
{
    public abstract class BaseTrack: MonoBehaviour
    {
        public List<Checkpoint> Checkpoints;
        public int CurrentCheckpoint = 0;
        public int TrackID;
        public int NextCheckpoint => GetNextCheckpoint();
        
        
        private void Start()
        {
            foreach (var checkpoint in Checkpoints)
            {
                checkpoint.OnCheckpointPassed += OnCheckpointPassed;
            }

            DeactivateAllCheckpointsExceptFirst();
        }
        

        [ContextMenu("Update Checkpoints List")]
        private void UpdateCheckpoints()
        {
            Checkpoints.Clear();
            foreach(Transform child in transform)
            {
                Checkpoint checkpoint = child.GetComponent<Checkpoint>();
                if (checkpoint != null)
                {
                    Checkpoints.Add(checkpoint);
                    Debug.Log(checkpoint.transform.position);
                    
                }
            }
        }

        private void OnCheckpointPassed(int checkpointIndex)
        {

            if (checkpointIndex == NextCheckpoint)
            {
                CurrentCheckpoint = checkpointIndex;
                if (CurrentCheckpoint == Checkpoints.Count - 1)
                {
                    OnLapComplete();
                }
            }
            ActivateNextCheckpoint();
        }
        
        public abstract void ResetTrack();
        private int GetNextCheckpoint()
        {
            return CurrentCheckpoint + 1;
        }
        
        protected void DeactivateAllCheckpointsExceptFirst()
        {
            foreach (var checkpoint in Checkpoints)
            {
                checkpoint.gameObject.SetActive(false);
                
            }
            Checkpoints[0].gameObject.SetActive(true);
        }
        
        protected void DeactivateAllCheckpoints()
        {
            foreach (var checkpoint in Checkpoints)
            {
                checkpoint.gameObject.SetActive(false);
                
            }
        }
        
        protected abstract void OnLapComplete();
        protected abstract void ActivateNextCheckpoint();
    }
}

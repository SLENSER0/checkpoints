using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace CheckpointSystem
{
    public abstract class BaseTrack: MonoBehaviour
    {
        [SerializeField] protected List<Checkpoint> checkpoints;
        protected int _currentCheckpoint = -1;
        protected bool _isStarted;
        protected int _nextCheckpoint => GetNextCheckpoint();
        
        
        private void Start()
        {
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.OnCheckpointPassed += OnCheckpointPassed;
            }
            DeactivateAllCheckpointsExceptFirst();
        }
        

        [ContextMenu("Update Checkpoints List")]
        private void UpdateCheckpoints()
        {
            checkpoints.Clear();
            foreach(Transform child in transform)
            {
                Checkpoint checkpoint = child.GetComponent<Checkpoint>();
                if (checkpoint != null)
                {
                    checkpoints.Add(checkpoint);
                }
            }
        }
        
        
        [ContextMenu("Draw lines Between Checkpoints")]
        private void DrawLineBetweenCheckpoints()
        {
            for (int i = 1; i < checkpoints.Count; i++)
            {
                DrawLineBetweenCheckpointsByIndex(i, i - 1);

            }

            DrawLineBetweenCheckpointsByIndex(0, checkpoints.Count - 1);
        }

        private void DrawLineBetweenCheckpointsByIndex(int index1, int index2)
        {
            var spline = checkpoints[index1].Spline;
            var secondSpline =  checkpoints[index2].Spline;
            
            int splineLength = spline.Spline.Count;

            for (int j = splineLength; j < 2; j++)
            {
                spline.Spline.Add(new BezierKnot(float3.zero));
            }
                
            var firstKnot = spline.Spline.ToArray()[0];
            var lastKnot = spline.Spline.ToArray().Last();

                
            firstKnot.Position = Vector3.zero;
            lastKnot.Position = spline.transform.InverseTransformPoint(secondSpline.transform.position);
                
            firstKnot.Rotation = Quaternion.LookRotation(lastKnot.Position - firstKnot.Position);
            lastKnot.Rotation = Quaternion.LookRotation(firstKnot.Position - lastKnot.Position);

            int lastKnotIndex = spline.Spline.ToArray().Length - 1;
            
            spline.Spline.SetKnot(0,lastKnot);
            spline.Spline.SetKnot(lastKnotIndex,firstKnot);
        }

        protected void DeactivateAllCheckpoints()
        {
            foreach (var checkpoint in checkpoints)
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

        protected void DeactivateCurrentCheckpointAndActivateNext()
        {
            checkpoints[_currentCheckpoint].transform.gameObject.SetActive(false);
            
            if (_nextCheckpoint < checkpoints.Count)
            {
                checkpoints[_nextCheckpoint].transform.gameObject.SetActive(true);
            }
            checkpoints[_nextCheckpoint].ObjectAlongSplineAnimate.Restart(true);
            
        }

        protected void DeactivateAllCheckpointsExceptFirst()
        {
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.transform.gameObject.SetActive(false);
                
            }
            checkpoints[0].transform.gameObject.SetActive(true);
            
            checkpoints[0].Spline.gameObject.SetActive(false);
            checkpoints[0].ObjectAlongSplineAnimate.gameObject.SetActive(false);;
        }

        protected void StartTrack()
        {
            checkpoints[0].Spline.gameObject.SetActive(true);
            checkpoints[0].ObjectAlongSplineAnimate.gameObject.SetActive(true);
            _isStarted = true;
        }

        protected int GetNextCheckpoint()
        {
            return (_currentCheckpoint + 1) % checkpoints.Count;
        }

        protected abstract void OnLapComplete();
        public abstract void ResetTrack();
        protected abstract void ActivateNextCheckpoint();
    }
}

using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

namespace CheckpointSystem
{
    public abstract class BaseTrack: MonoBehaviour
    {
        [SerializeField] protected List<Checkpoint> сheckpoints;
        protected int _currentCheckpoint = -1;
        protected bool _isStarted;
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
                Checkpoint checkpoint = child.GetComponentInChildren<Checkpoint>();
                if (checkpoint != null)
                {
                    сheckpoints.Add(checkpoint);
                }
            }
        }
        
        
        [ContextMenu("Draw lines Between Checkpoints")]
        private void DrawLineBetweenCheckpoints()
        {
            for (int i = 1; i < сheckpoints.Count; i++)
            {
                DrawLineBetweenCheckpointsByIndex(i, i - 1);

            }

            DrawLineBetweenCheckpointsByIndex(0, сheckpoints.Count - 1);
        }

        private void DrawLineBetweenCheckpointsByIndex(int index1, int index2)
        {
            var spline = transform.GetChild(index1).GetComponentInChildren<SplineContainer>();
            var previousSpline = transform.GetChild(index2).GetComponentInChildren<SplineContainer>();

            if (spline.Spline.ToArray().Length < 2)
            {
                for (int j = spline.Spline.ToArray().Length; j <2; j++)
                {
                    spline.Spline.Add(new BezierKnot(float3.zero));
                }
            }
                
            var firstKnot = spline.Spline.ToArray()[0];
            var lastKnot = spline.Spline.ToArray().Last();

                
            firstKnot.Position = Vector3.zero;
            lastKnot.Position = spline.transform.InverseTransformPoint(previousSpline.transform.position);
                
            firstKnot.Rotation = Quaternion.LookRotation(lastKnot.Position - firstKnot.Position);
            lastKnot.Rotation = Quaternion.LookRotation(firstKnot.Position - lastKnot.Position);
                
            spline.Spline.SetKnot(0,lastKnot);
            spline.Spline.SetKnot(spline.Spline.ToArray().Length-1,firstKnot);
        }

        protected void DeactivateAllCheckpoints()
        {
            foreach (var checkpoint in сheckpoints)
            {
                checkpoint.transform.parent.gameObject.SetActive(false);
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
            сheckpoints[_currentCheckpoint].transform.parent.gameObject.SetActive(false);
            
            if (_nextCheckpoint < сheckpoints.Count)
            {
                сheckpoints[_nextCheckpoint].transform.parent.gameObject.SetActive(true);
            }
            transform.GetChild(_nextCheckpoint).GetChild(2).GetComponentInChildren<SplineAnimate>().Restart(true);
            
        }

        protected void DeactivateAllCheckpointsExceptFirst()
        {
            foreach (var checkpoint in сheckpoints)
            {
                checkpoint.transform.parent.gameObject.SetActive(false);
                
            }
            сheckpoints[0].transform.parent.gameObject.SetActive(true);
            
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }

        protected void ActivateSplineAndObjectForSpline(int splineIndex, int objectForSplineIndex)
        {
            transform.GetChild(0).GetChild(splineIndex).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(objectForSplineIndex).gameObject.SetActive(true);
            _isStarted = true;
        }

        protected int GetNextCheckpoint()
        {
            return (_currentCheckpoint + 1) % сheckpoints.Count;
        }

        protected abstract void OnLapComplete();
        public abstract void ResetTrack();
        protected abstract void ActivateNextCheckpoint();
    }
}

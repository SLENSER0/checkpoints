using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace CheckpointSystem
{
    public static class CurveDrawer
    {

        public static void DrawLineBetweenCheckpoints(List<Checkpoint> checkpoints)
        {
            for (int i = 1; i < checkpoints.Count; i++)
            {
                DrawLineBetweenCheckpointsByIndex(checkpoints, i, i - 1);
            }
            // draw between first and last checkpoint
            DrawLineBetweenCheckpointsByIndex(checkpoints, 0, checkpoints.Count - 1);
        }

        private static void DrawLineBetweenCheckpointsByIndex(List<Checkpoint> checkpoints,int index1, int index2)
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
    }
}

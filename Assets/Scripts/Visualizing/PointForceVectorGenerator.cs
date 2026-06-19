using System;
using System.Collections.Generic;
using Models;
using Tsne;
using UnityEngine;
using Utils;

namespace Visualizing
{
    public static class PointForceVectorGenerator
    {
        public enum Mode
        {
            RepulsiveForces,
            AttractiveForce,
            SummedForces
        }
        
        public static void Render(Mode mode)
        {
            if (Manager.Settings.N <= 1) return;

            List<Vector3> forces = mode switch
            {
                Mode.RepulsiveForces => TSneManager.RepulsiveForces(),
                Mode.AttractiveForce => TSneManager.AttractiveForces(),
                Mode.SummedForces => TSneManager.Forces(),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            
            List<Point> points = Manager.Settings._points;
            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                Vector3 f = forces[i];
                
                float size = f.magnitude * Manager.Settings.arrowSize * 1000;

                // add slight offset so that the vector isnt inside the point
                Vector3 pos = p.transform.position + (Manager.Settings.pointSize/2) * f.normalized;
                
                ArrowUtils.SpawnArrow(pos, f.normalized, size);
            }
        }
    }
}
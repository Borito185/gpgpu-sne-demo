using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts
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
            if (Manager.Settings.nSamples <= 1) return;
            
            List<Vector3> forces = mode switch
            {
                Mode.RepulsiveForces => TSneManager.RepulsiveForces(),
                Mode.AttractiveForce => TSneManager.AttractiveForces(),
                Mode.SummedForces => TSneManager.Forces(),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            
            
            List<Settings.Point> points = Manager.Settings._points;
            Transform parent = Manager.Settings.s_arrowParent;

            float norm = forces.Max(a => a.magnitude);
            for (int i = 0; i < points.Count; i++)
            {
                Settings.Point p = points[i];
                Vector3 f = forces[i];
                
                float size = f.magnitude / norm * Manager.Settings.arrowSize;

                Vector3 pos = p.transform.position + (Manager.Settings.pointSize/2) * f.normalized;
                
                Utils.SpawnArrow(pos, f, size, parent);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts
{
    public static class TSneManager
    {
        public static void Reset()
        {
            SimilarityCompute.UpdateSimilarities();
        }

        public static float Similarity(Settings.Point a, Settings.Point b)
        {
            return Manager.Settings._similarities[new Settings.PointTuple(a, b)];
        }
        
        public static Field RepulsiveForceField()
        {
            Settings settings = Manager.Settings;

            List<Vector2> points = settings._points.ConvertAll(p =>
            {
                Transform t = p.transform;
                return new Vector2(t.position.x, t.position.z);
            });

            Field f = new(-settings.s_spawnRange, settings.s_spawnRange, settings.FieldResolution);
            Field kernel = FieldKernelGenerator.Generate(settings.KernelSize, settings.KernelResolution);
            
            foreach (Vector2 point in points) 
                f.AddKernel(kernel, point);
            
            return f;
        }

        public static List<Vector3> AttractiveForces(float z = -1, Field field = null)
        {
            if (z < 0) 
                z = Z(field);
            
            var points = Manager.Settings._points;
            List<Vector3> forces = new(points.Count);
            foreach (Settings.Point i in points)
            {
                Vector3 force = Vector3.zero;

                foreach (Settings.Point j in points)
                {
                    if (i == j) continue;
                    
                    Vector3 delta = j.transform.position - i.transform.position;
                    delta.y = 0;
                    
                    float distSqr = delta.sqrMagnitude;
                    if (distSqr < 1e-8f) continue;
                    
                    float pij = Similarity(i, j);
                    // actually I should do 1f / ((1f + distSqr) * z)
                    // and then later multiply with z again
                    // but since that cancels, i dont
                    // but be aware that this isnt actually qij
                    float qij = 1f / (1f + distSqr); 
                    
                    force += delta * pij * qij;
                }
                
                forces.Add(force);
            }

            return forces;
        }

        public static List<Vector3> RepulsiveForces(Field field = null, float z = -1)
        {
            if (field == null) 
                field = RepulsiveForceField();
            if (z < 0) 
                z = Z(field);

            var points = Manager.Settings._points;
            List<Vector3> forces = new(points.Count);
            foreach (Settings.Point a in points)
            {
                Vector3 position = a.transform.position;
                Vector2 pos = new Vector2(position.x, position.z);
                
                Vector3 v = field.GetInterpolatedValue(pos);
                
                Vector3 force = new Vector3(v.y, 0, v.z) / z;
                forces.Add(force);
            }

            return forces;
        }

        private static float Z(Field f = null)
        {
            if (f == null)
                f = RepulsiveForceField();
            
            float z = Manager.Settings._points
                .Select(p => p.transform.position)
                .Select(p => new Vector2(p.x, p.z))
                .Select(p => f.GetInterpolatedValue(p))
                .Sum(v => v.x-1);
            return z;
        }

        public static List<Vector3> Forces()
        {
            Field field = RepulsiveForceField();
            float z = Z(field);
            
            var attractiveForces = AttractiveForces(z);
            var repulsiveForces = RepulsiveForces(field, z);

            return attractiveForces
                .Zip(repulsiveForces, (a, r) => 4 * (a + r))
                .ToList();
        }

        public static void SimulateStep()
        {
            var forces = Forces();
            var points = Manager.Settings._points;
            
            for (var i = 0; i < points.Count; i++)
            {
                Settings.Point p = points[i];
                Vector3 f = forces[i];
                
                p.transform.position += f * Manager.Settings.stepSize;
            }
        }
    }
}
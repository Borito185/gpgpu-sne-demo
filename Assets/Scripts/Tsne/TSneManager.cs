using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace Tsne
{
    /**
     * Single api for t-SNE.
     * mostly follows the description as in the paper.
     */
    public static class TSneManager
    {
        public static void Reset()
        {
            SimilarityCompute.UpdateSimilarities();
        }

        public static float Similarity(Point a, Point b) 
            => Manager.Settings._similarities[new PointTuple(a, b)];

        public static Field RepulsiveForceField() 
            => RepulsiveForceFieldGenerator.RepulsiveForceField();

        public static List<Vector3> AttractiveForces()
        {
            var points = Manager.Settings._points;
            List<Vector3> forces = new(points.Count);
            foreach (Point i in points)
            {
                Vector3 force = Vector3.zero;

                foreach (Point j in i.nearestNeighbours)
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
                    
                    force += delta * (pij * qij);
                }
                
                forces.Add(force);
            }

            return forces;
        }

        public static List<Vector3> RepulsiveForces()
        {
            Field field = RepulsiveForceField();
            float z = Z(field);

            var points = Manager.Settings._points;
            List<Vector3> forces = new(points.Count);
            foreach (Point a in points)
            {
                Vector3 position = a.transform.position;
                Vector2 pos = new Vector2(position.x, position.z);
                
                Vector3 v = field.GetInterpolatedValue(pos);
                
                Vector3 force = new Vector3(v.y, 0, v.z) / z;
                forces.Add(force);
            }

            return forces;
        }

        public static float Z(Field f = null)
        {
            f ??= RepulsiveForceField();
            
            float z = Manager.Settings._points
                .Select(p => p.transform.position)
                .Select(p => new Vector2(p.x, p.z))
                .Select(p => f.GetInterpolatedValue(p))
                .Sum(v => v.x-1); // -1 to remove a points own density at this spot
            return z;
        }

        public static List<Vector3> Forces()
        {
            if (Manager.Settings.N == 1)
                return new List<Vector3>(new []{Vector3.zero});
            
            var attractiveForces = AttractiveForces();
            var repulsiveForces = RepulsiveForces();

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
                Point p = points[i];
                Vector3 f = forces[i];
                
                p.transform.position += f;
            }
        }
    }
}
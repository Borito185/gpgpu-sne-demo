using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace _Project.Scripts
{
    public static class RepulsiveForcesFieldGenerator
    {
        private const float RESOLUTION = 2f;
        private const float KERNEL_RESOLUTION = 5f;
        
        public static Field Generate(List<Vector2> points)
        {
            (Vector2 min, Vector2 max) = MinMax(points);

            Field f = new(min, max, RESOLUTION);
            Field kernel = FieldKernelGenerator.Generate(new Vector2Int(5, 5), KERNEL_RESOLUTION);
            
            foreach (Vector2 point in points) 
                f.AddKernel(kernel, point);
            
            return f;
        }

        private static Tuple<Vector2, Vector2> MinMax(List<Vector2> points)
        {
            Vector2 min = Vector2.one * float.MaxValue;
            Vector2 max = -min;
            
            foreach (Vector2 p in points)
            {
                min.x = Mathf.Min(min.x, p.x);
                min.y = Mathf.Min(min.y, p.y);
                
                max.x = Mathf.Max(max.x, p.x);
                max.y = Mathf.Max(max.y, p.y);
            }
            
            return new Tuple<Vector2, Vector2>(min, max);
        }
    }
}
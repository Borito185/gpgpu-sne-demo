using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class RepulsiveProvider : FieldProvider
    {
        public Transform pointParent;
        
        public override Field GetField()
        {
            int n = pointParent.childCount;
            List<Vector2> points = new(n);
            for (int i = 0; i < n; i++)
            {
                Vector3 pos = pointParent.GetChild(i).position;
                points.Add(new Vector2(pos.x, pos.z));
            }
            
            return RepulsiveForcesFieldGenerator.Generate(points);
        }
    }
}
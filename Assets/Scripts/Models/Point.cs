using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class Point
    {
        public Color color;
        public Transform transform;
        public float sigma;
        public List<Point> nearestNeighbours;
            
        public Point(Color color, Transform transform)
        {
            this.color = color;
            this.transform = transform;
        }
    }
}
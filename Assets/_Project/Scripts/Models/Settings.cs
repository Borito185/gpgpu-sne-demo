using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    [System.Serializable]
    public class Settings
    {
        // general naming rules:
        // _ -> dont allow user to change
        // s -> shouldnt really change during runtime
        
        // === point settings ===
        public int nSamples;
        public bool showPoints;
        public float pointSize;
        public Vector2 s_spawnRange = new(5f, 5f);
        public GameObject s_pointPrefab;
        public Transform s_pointParent;
        public float s_pointHeight = 2;

        // === draw settings ===
        public DrawMode drawMode;

        // === field settings ===
        public float fieldHeight;
        public float FieldResolution = 2f;
        public float KernelResolution = 5f;
        public Vector2Int KernelSize = new(5, 5);
        public MeshFilter s_fieldMesh;


        // === arrow settings ===
        public float arrowSize;
        public GameObject s_arrowPrefab;
        public Transform s_arrowParent;

        // === simulation step size ===
        public float stepSize = .1f;
        public float sigma = 0.25f;
        
        // === state ===
        public List<Point> _points;
        public float cooldownSeconds;
        public bool simulate = false;
        public Dictionary<PointTuple, float> _similarities = new();
        
        public class Point
        {
            public Color color;
            public Transform transform;

            public Point(Color color, Transform transform)
            {
                this.color = color;
                this.transform = transform;
            }
        }

        public struct PointTuple : IEquatable<PointTuple>
        {
            public Point a;
            public Point b;

            public PointTuple(Point a, Point b)
            {
                this.a = a;
                this.b = b;
            }

            public bool Equals(PointTuple other)
            {
                return (Equals(a, other.a) && Equals(b, other.b)) ||
                       (Equals(a, other.b) && Equals(b, other.a));
            }

            public override bool Equals(object obj)
            {
                return obj is PointTuple other && Equals(other);
            }

            public override int GetHashCode()
            {
                return a.GetHashCode() ^ b.GetHashCode();
            }
        }
        
        public enum DrawMode
        {
            VectorField,
            DensityField,
            GradientXField,
            GradientYField,
            PointRepulsive,
            PointAttractive,
            PointSummedForce
        }
    }
}
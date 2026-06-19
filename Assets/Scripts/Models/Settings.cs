using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    /**
     * Stores most settings for the tool.
     * Can be accessed globally through the manager.
     */
    [System.Serializable]
    public class Settings
    {
        // general naming rules:
        // _ -> dont allow user to change
        // s -> shouldnt really change during runtime
        
        // === point settings ===
        public int nSamples;
        public int N => _points.Count;
        public bool showPoints = true;
        public float pointSize = 0.2f;
        public Vector2 s_spawnRange = new(5f, 5f);
        public GameObject s_pointPrefab;
        public Transform s_pointParent;
        public float s_pointHeight = 2;

        // === draw settings ===
        public DrawMode drawMode;

        // === field settings ===
        public float FieldResolution = 2f;
        public float KernelResolution = 5f;
        public Vector2Int KernelSize = new(5, 5);
        public Vector2 s_fieldSize = new(9, 6);
        public MeshFilter s_fieldMesh;
        public Material s_fieldMaterial;

        // === arrow settings ===
        public float arrowSize = 2f;
        public GameObject s_arrowPrefab;

        // === simulation ===
        public bool simulate = false;
        public float perplexity = 20;
        public float targetRate = 100;
        public int knn => 3 * Mathf.RoundToInt(perplexity);

        // === state ===
        public List<Point> _points;
        public Dictionary<PointTuple, float> _similarities = new();

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
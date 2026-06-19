using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Project.Scripts
{
    public static class ArrowUtils
    {
        private const int BatchSize = 1023;

        private static readonly List<Matrix4x4> matrices = new(BatchSize);
        private static readonly Matrix4x4[] batch = new Matrix4x4[BatchSize];

        private static Mesh mesh;
        private static Material material;
        private static Matrix4x4 meshLocalMatrix;

        public static void Reset()
        {
            ClearArrows();
            
            GameObject arrow = Manager.Settings.s_arrowPrefab;
            
            mesh = arrow.GetComponentInChildren<MeshFilter>().sharedMesh;
            material = arrow.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            material.enableInstancing = true;

            Transform t = arrow.transform.GetChild(0);
            meshLocalMatrix = Matrix4x4.TRS(
                t.localPosition,
                t.localRotation,
                t.localScale
            );
        }

        public static void ClearArrows()
        {
            matrices.Clear();
        }

        public static void SpawnArrow(Vector3 position, Vector3 direction, float arrowSize)
        {
            if (direction.sqrMagnitude < 0.000001f)
                return;
            
            Quaternion rotation = Quaternion.LookRotation(direction.normalized);
            Vector3 scale = Vector3.one * arrowSize;

            Matrix4x4 pivotMatrix = Matrix4x4.TRS(position, rotation, scale);

            matrices.Add(pivotMatrix * meshLocalMatrix);
        }

        public static void DrawArrows()
        {
            if (mesh == null || material == null || matrices.Count == 0)
                return;

            RenderParams rp = new RenderParams(material)
            {
                shadowCastingMode = ShadowCastingMode.Off,
                receiveShadows = false
            };

            for (int i = 0; i < matrices.Count; i += BatchSize)
            {
                int count = Mathf.Min(BatchSize, matrices.Count - i);

                for (int j = 0; j < count; j++)
                    batch[j] = matrices[i + j];

                Graphics.RenderMeshInstanced(rp, mesh, 0, batch, count);
            }
        }
    }
}
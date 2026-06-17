using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(FieldVisualizer), typeof(MeshFilter), typeof(MeshRenderer))]
    public class FieldVisualizer : MonoBehaviour
    {
        public GameObject arrowPrefab;

        public float height;
        public float size;
        
        private Field GetField()
        {
            return GetComponent<FieldProvider>().GetField();
        }

        private void SetMesh(Mesh mesh)
        {
            GetComponent<MeshFilter>().sharedMesh = mesh;
            GetComponent<MeshRenderer>().enabled = true;
        }

        private void _Reset()
        {
            Utils.DestroyChildren(transform);
            GetComponent<MeshRenderer>().enabled = false;
        }
        
        [Button]
        public void VectorField()
        {
            _Reset();
            
            Field f = GetField();
            if (f == null)
                return;

            Vector3Int shape = f.Shape;

            float norm = f.Value.Max(v => new Vector2(v.y, v.z).magnitude);
            
            for (int i = 0; i < shape.x; i++)
            {
                for (int j = 0; j < shape.y; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);
                    
                    Vector3 v = f.Get(index);
                    Vector2 pos = f.GetPosition(index);
                    Vector3 vector = new(-v.y, 0, -v.z);
                    float arrowSize = vector.magnitude / norm * size;
                    
                    GameObject o = Instantiate(arrowPrefab, transform);
                    Transform t = o.transform;
                    t.position = new Vector3(pos.x, 0, pos.y);
                    t.rotation = Quaternion.LookRotation(vector.normalized);
                    t.localScale = Vector3.one * arrowSize;
                }
            }
        }

        [Button] public void DensityField() => HeightMap(0);
        [Button] public void GradientXField() => HeightMap(1);
        [Button] public void GradientYField() => HeightMap(2);

        public void HeightMap(int channel)
        {
            _Reset();
            
            Field f = GetField();
            if (f == null)
                return;

            Mesh mesh = FieldMeshGenerator.Generate(f, channel, height);
            
            SetMesh(mesh);
        }
    }
}
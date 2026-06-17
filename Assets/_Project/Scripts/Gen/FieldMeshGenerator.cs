using System.Linq;
using UnityEngine;

namespace _Project.Scripts
{
    public static class FieldMeshGenerator
    {
        public static Mesh Generate(Field field, int channel, float meshHeight)
        {
            int width = field.Shape.x;
            int height = field.Shape.y;

            Vector3[] vertices = new Vector3[width * height];
            Vector2[] uvs = new Vector2[width * height];

            float dx = (field.max.x - field.min.x) / (width - 1);
            float dy = (field.max.y - field.min.y) / (height - 1);

            float offset = field.Value.Min(v => v[channel]);
            float norm = field.Value.Max(v => v[channel])
                - offset;
            
            // Vertices
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = y * width + x;

                    float v = field.Get(new Vector2Int(x, y))[channel];
                    float h = v / norm * meshHeight - offset;
                    
                    vertices[i] = new Vector3(
                        field.min.x + x * dx,
                        h,
                        field.min.y + y * dy
                    );

                    uvs[i] = new Vector2(
                        (float)x / (width - 1),
                        (float)y / (height - 1)
                    );
                }
            }

            // Triangles
            int[] triangles = new int[(width - 1) * (height - 1) * 6];
            int t = 0;

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int bl = y * width + x;
                    int br = bl + 1;
                    int tl = bl + width;
                    int tr = tl + 1;

                    triangles[t++] = bl;
                    triangles[t++] = tl;
                    triangles[t++] = tr;

                    triangles[t++] = bl;
                    triangles[t++] = tr;
                    triangles[t++] = br;
                }
            }

            Mesh mesh = new Mesh();
            mesh.indexFormat = vertices.Length > 65535
                ? UnityEngine.Rendering.IndexFormat.UInt32
                : UnityEngine.Rendering.IndexFormat.UInt16;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
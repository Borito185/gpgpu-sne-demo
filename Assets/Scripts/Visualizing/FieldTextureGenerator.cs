using System.Linq;
using UnityEngine;

namespace _Project.Scripts
{
    public static class FieldTextureGenerator
    {
        private static readonly int FieldTexId = Shader.PropertyToID("_FieldTex");
        private static readonly int FieldMinId = Shader.PropertyToID("_FieldMin");
        private static readonly int FieldMaxId = Shader.PropertyToID("_FieldMax");
        private static readonly int ChannelId = Shader.PropertyToID("_Channel");

        public static Mesh Render(int channel)
        {
            Field field = TSneManager.RepulsiveForceField();

            Mesh mesh = CreateQuadMesh(field);
            Texture2D texture = CreateFieldTexture(field, channel, out float min, out float max);

            Material material = Manager.Settings.s_fieldMaterial;
            material.SetTexture(FieldTexId, texture);
            material.SetFloat(FieldMinId, min);
            material.SetFloat(FieldMaxId, max);
            material.SetFloat(ChannelId, channel);

            return mesh;
        }

        private static Mesh CreateQuadMesh(Field field)
        {
            Vector3[] vertices =
            {
                new(field.min.x, 0f, field.min.y),
                new(field.max.x, 0f, field.min.y),
                new(field.min.x, 0f, field.max.y),
                new(field.max.x, 0f, field.max.y)
            };

            Vector2[] uvs =
            {
                new(0f, 0f),
                new(1f, 0f),
                new(0f, 1f),
                new(1f, 1f)
            };

            int[] triangles =
            {
                0, 2, 3,
                0, 3, 1
            };

            Mesh mesh = new Mesh
            {
                name = "Field Quad",
                vertices = vertices,
                uv = uvs,
                triangles = triangles
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private static Texture2D CreateFieldTexture(
            Field field,
            int channel,
            out float min,
            out float max)
        {
            int width = field.Shape.x;
            int height = field.Shape.y;

            min = field.Value.Min(v => v[channel]);
            max = field.Value.Max(v => v[channel]);

            Texture2D texture = new Texture2D(
                width,
                height,
                TextureFormat.RFloat,
                false,
                true
            );

            texture.name = "Field Texture";
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 v = field.Get(new Vector2Int(x, y));
                    float value = v[channel];

                    float vec = 0;
                    if (channel > 0)
                    {
                        vec = new Vector2(v[1], v[2]).normalized[channel - 1];
                    }
                    
                    texture.SetPixel(
                        x,
                        y,
                        new Color(value, vec, 0f, 1f)
                    );
                }
            }

            texture.Apply(false, false);

            return texture;
        }
    }
}
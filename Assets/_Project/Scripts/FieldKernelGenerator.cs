using UnityEngine;

namespace _Project.Scripts
{
    public static class FieldKernelGenerator
    {
        public static Field Generate(Vector2Int size, float resolution = 2)
        {
            Field f = new Field(Vector2.zero, size, resolution);
            Vector2 sizeFloat = size;
            Vector2 sizeFloat_Half = sizeFloat * 0.5f;
            f.min = -sizeFloat_Half;
            f.max = sizeFloat_Half;
            
            Vector3Int shape = f.Shape;

            for (int i = 0; i < shape.x; i++)
            {
                for (int j = 0; j < shape.y; j++)
                {
                    Vector2Int fieldPos = new Vector2Int(i, j);
                    float density = GetDensity(f, fieldPos);
                    Vector2 gradient = GetGradient(f, fieldPos);

                    Vector3 value = new Vector3(density, gradient.x, gradient.y);
                    
                    f.Set(fieldPos, value);
                }
            }

            return f;
        }
        
        private static float GetDensity(Field f, Vector2Int fieldPos)
        {
            Vector2 worldPos = f.GetPosition(fieldPos);
                    
            float distSqr = worldPos.sqrMagnitude;
            float value = 1 / (1 + distSqr);

            return value;
        }
        
        private static Vector2 GetGradient(Field f, Vector2Int fieldPos)
        {
            Vector2 worldPos = f.GetPosition(fieldPos);
                    
            float distSqr = worldPos.sqrMagnitude;
            float valueSqrt = 1 / (1 + distSqr);
            
            Vector2 value = (valueSqrt * valueSqrt) * worldPos;

            return value;
        }
    }
}
using UnityEngine;

namespace _Project.Scripts
{
    public static class VectorFieldGenerator
    {
        public static void Render()
        {
            Field f = TSneManager.RepulsiveForceField();

            Vector3Int shape = f.Shape;

            float z = TSneManager.Z(f);
            
            for (int i = 0; i < shape.x; i++)
            {
                for (int j = 0; j < shape.y; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);
                    Vector2 pos = f.GetPosition(index);
                    Vector3 v = f.Get(index);
                    Vector3 dir = new Vector3(v.y, 0, v.z).normalized;
                    
                    float size = (new Vector3(v.y, 0, v.z).magnitude / z) * Manager.Settings.arrowSize * 1000;
                    
                    if (size < 0.1f) 
                        continue;
                    
                    ArrowUtils.SpawnArrow(new Vector3(pos.x, 0, pos.y), dir, size);
                }
            }
        }
    }
}
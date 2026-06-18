using System.Linq;
using UnityEngine;

namespace _Project.Scripts
{
    public static class VectorFieldGenerator
    {
        public static void Render()
        {
            Transform parent = Manager.Settings.s_arrowParent;
            Field f = TSneManager.RepulsiveForceField();

            Vector3Int shape = f.Shape;

            float norm = f.Value.Max(v => new Vector2(v.y, v.z).magnitude);
            
            for (int i = 0; i < shape.x; i++)
            {
                for (int j = 0; j < shape.y; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);
                    Vector2 pos = f.GetPosition(index);
                    Vector3 v = f.Get(index);
                    Vector3 dir = new Vector3(v.y, 0, v.z).normalized;
                    
                    float size = (v.magnitude / norm) * Manager.Settings.arrowSize;
                    
                    if (size < 0.1f) 
                        continue;
                    
                    Utils.SpawnArrow(new Vector3(pos.x, 0, pos.y), dir, size, parent);
                }
            }
        }
    }
}
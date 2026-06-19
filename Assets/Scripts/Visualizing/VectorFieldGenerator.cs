using System.Linq;
using Models;
using Tsne;
using UnityEngine;
using Utils;

namespace Visualizing
{
    public static class VectorFieldGenerator
    {
        public static void Render()
        {
            Field f = TSneManager.RepulsiveForceField();

            Vector3Int shape = f.Shape;

            // I wanted to use Z here also
            // but it seems so hard to balance for different values of N
            // so I chose to just normalize it by max
            float norm = f.Value.Max(v => new Vector2(v.y, v.z).magnitude);
            
            for (int i = 0; i < shape.x; i++)
            {
                for (int j = 0; j < shape.y; j++)
                {
                    Vector2Int index = new Vector2Int(i, j);
                    Vector2 pos = f.GetPosition(index);
                    Vector3 v = f.Get(index);
                    Vector3 dir = new Vector3(v.y, 0, v.z).normalized;
                    
                    // mul by 2 is arbitrary, preferred how it looked over no scaling
                    float size = (new Vector3(v.y, 0, v.z).magnitude / norm) * Manager.Settings.arrowSize * 2;
                    
                    if (size < 0.1f) 
                        continue;
                    
                    ArrowUtils.SpawnArrow(new Vector3(pos.x, 0, pos.y), dir, size);
                }
            }
        }
    }
}
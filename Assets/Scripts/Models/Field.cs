using System;
using UnityEngine;

namespace Models
{
    /**
     * Stores the field/kernel texture.
     * Supports sampling and space conversion helper functions.
     */
    public class Field
    {
        public Vector2 min;
        public Vector2 max;
        public Vector3Int Shape; // x, y, channels
        public Vector3[] Value;
        
        public Field(Vector2 min, Vector2 max, float resolution = 2)
        {
            this.min = min;
            this.max = max;

            var sizeFloat = (max - min) * resolution;

            Shape = new Vector3Int(
                (int)Math.Ceiling(sizeFloat.x),
                (int)Math.Ceiling(sizeFloat.y),
                3
            );

            Value = new Vector3[Shape.x * Shape.y];
        }

        private int Index(in Vector2Int pos) 
            => pos.y * Shape.x + pos.x;

        public Vector3 Get(in Vector2Int pos) 
            => Value[Index(pos)];

        public Vector3 Set(in Vector2Int pos, Vector3 value) 
            => Value[Index(pos)] = value;

        /**
         * Bilinear interpolates between the field pixel values
         */
        public Vector3 GetInterpolatedValue(in Vector2 pos)
        {
            var tx = Mathf.InverseLerp(min.x, max.x, pos.x) * (Shape.x - 1);
            var ty = Mathf.InverseLerp(min.y, max.y, pos.y) * (Shape.y - 1);

            int x0 = Mathf.Clamp(Mathf.FloorToInt(tx), 0, Shape.x - 1);
            int y0 = Mathf.Clamp(Mathf.FloorToInt(ty), 0, Shape.y - 1);

            int x1 = Mathf.Clamp(x0 + 1, 0, Shape.x - 1);
            int y1 = Mathf.Clamp(y0 + 1, 0, Shape.y - 1);

            float fx = tx - x0;
            float fy = ty - y0;

            Vector3 v00 = Get(new Vector2Int(x0, y0));
            Vector3 v10 = Get(new Vector2Int(x1, y0));
            Vector3 v01 = Get(new Vector2Int(x0, y1));
            Vector3 v11 = Get(new Vector2Int(x1, y1));

            Vector3 vx0 = Vector3.Lerp(v00, v10, fx);
            Vector3 vx1 = Vector3.Lerp(v01, v11, fx);

            return Vector3.Lerp(vx0, vx1, fy);
        }

        public Vector2 GetPosition(in Vector2Int index)
        {
            var tx = Mathf.InverseLerp(0, Shape.x - 1, index.x);
            var ty = Mathf.InverseLerp(0, Shape.y - 1, index.y);

            return new Vector2(
                Mathf.Lerp(min.x, max.x, tx),
                Mathf.Lerp(min.y, max.y, ty)
            );
        }
    }
}
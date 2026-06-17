using UnityEngine;

namespace _Project.Scripts
{
    public class KernelProvider : FieldProvider
    {
        public Vector2Int size = new Vector2Int(5, 5);
        public float resolution = 2f;
        
        public override Field GetField()
        {
            return FieldKernelGenerator.Generate(size, resolution);
        }
    }
}
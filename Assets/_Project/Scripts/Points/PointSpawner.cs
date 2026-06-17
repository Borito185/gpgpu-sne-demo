using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Points
{
    public class PointSpawner : MonoBehaviour
    {
        public GameObject pointPrefab;
        
        [Button]
        public void Spawn()
        {
            Utils.DestroyChildren(transform);

            int n = Manager.Settings.nSamples;
            var colors = ColorGenerator.Generate(n);
            
            foreach (Color color in colors)
            {
                Vector3 position = new  Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

                GameObject instantiate = Instantiate(pointPrefab, position, Quaternion.identity, transform);
                Renderer renderer = instantiate.GetComponent<Renderer>();
                
                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor("_BaseColor", color);
                renderer.SetPropertyBlock(block);
            }
        }
    }
}
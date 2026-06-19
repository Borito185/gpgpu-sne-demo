using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class Utils
    {
        public static void Destroy(Object o)
        {
            #if UNITY_EDITOR
            GameObject.DestroyImmediate(o);
            #else
            GameObject.Destroy(o);
            #endif
        }

        public static void DestroyChildren(Transform transform)
        {
            int count = transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void SetColor(GameObject go, Color color)
        {
            Renderer renderer = go.GetComponent<Renderer>();
                
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor("_BaseColor", color);
            renderer.SetPropertyBlock(block);
        }

        public static void MarkAsTransient(GameObject go)
        {
            go.hideFlags = HideFlags.DontSave;
        }

        public static void RecenterPoints()
        {
            var points = Manager.Settings._points;

            Vector3 center = Vector3.zero;
            foreach (var p in points)
                center += p.transform.position;

            center /= points.Count;
            center.y = 0f;

            foreach (var p in points)
                p.transform.position -= center;
        }
    }
}
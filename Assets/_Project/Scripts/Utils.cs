using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts
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
    }
}
using System;
using System.Collections.Generic;
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

        public static List<Transform> GetChildren(Transform transform)
        {
            int count = transform.childCount;
            List<Transform> result = new(count); 
            for (int i = count - 1; i >= 0; i--)
            {
                result.Add(transform.GetChild(i));
            }
            
            return result;
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

        public static GameObject SpawnArrow(Vector3 position, Vector3 direction, float arrowSize, Transform parent = null)
        {
            GameObject arrow = Manager.Settings.s_arrowPrefab;
            arrow = Object.Instantiate(arrow, position, Quaternion.LookRotation(direction), parent);
            arrow.transform.localScale = new Vector3(arrowSize, arrowSize, arrowSize);
            MarkAsTransient(arrow);
            return arrow;
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
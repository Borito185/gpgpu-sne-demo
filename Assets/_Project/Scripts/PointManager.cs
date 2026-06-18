using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public static class PointManager
    {
        public static void Reset()
        {
            Settings settings = Manager.Settings;
            int n             = settings.nSamples;
            GameObject prefab = settings.s_pointPrefab;
            Vector2 range     = settings.s_spawnRange;
            Transform parent  = settings.s_pointParent;
            
            // destroy old
            Utils.DestroyChildren(parent);
            
            // generate new
            List<Settings.Point> points = new(n);
            for (int i = 0; i < n; i++)
            {
                // random non-transparent color
                Color color = Random.ColorHSV();
                color.a = 1f;
                
                // random position
                Vector3 position = new(
                    Random.Range(-range.x, range.x), 
                    settings.s_pointHeight, 
                    Random.Range(-range.y, range.y)
                );

                // instantiate and set stuff
                GameObject go = Object.Instantiate(prefab, position, Quaternion.identity, parent);
                Utils.SetColor(go, color);
                Utils.MarkAsTransient(go);
                
                points.Add(new Settings.Point(color, go.transform));
            }
            
            settings._points = points;
        }

        public static void Render()
        {
            Settings settings = Manager.Settings;
            foreach (Settings.Point p in settings._points)
            {
                Transform t = p.transform;
                t.transform.localScale = settings.pointSize * Vector3.one;
                t.gameObject.SetActive(settings.showPoints);
            }
        }
    }
}
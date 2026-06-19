using System.Collections.Generic;
using Models;
using UnityEngine;

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
        Utils.Utils.DestroyChildren(parent);
            
        // generate new
        List<Point> points = new(n);
        for (int i = 0; i < n; i++)
        {
            // random non-transparent color
            Color color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f), 
                1
            );
                
            // random position
            Vector3 position = new(
                Random.Range(-range.x, range.x), 
                settings.s_pointHeight, 
                Random.Range(-range.y, range.y)
            );

            // instantiate and set stuff
            GameObject go = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            Utils.Utils.SetColor(go, color);
            Utils.Utils.MarkAsTransient(go);
                
            points.Add(new Point(color, go.transform));
        }
            
        settings._points = points;
    }

    public static void Render()
    {
        Settings settings = Manager.Settings;
        foreach (Point p in settings._points)
        {
            Transform t = p.transform;
            t.transform.localScale = settings.pointSize * Vector3.one;
            t.gameObject.SetActive(settings.showPoints);
        }
    }
}
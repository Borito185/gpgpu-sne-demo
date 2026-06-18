using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public static class SimilarityCompute
    {

        public static void UpdateSimilarities()
        {
            Settings settings = Manager.Settings;
            int n = settings.nSamples;
            var similarities = settings._similarities;
            similarities.Clear();

            Dictionary<Tuple<Settings.Point, Settings.Point>, float> conditionals = new(n * n - n); 
            foreach (Settings.Point i in settings._points)
            {
                foreach (Settings.Point j in settings._points)
                {
                    if (i == j) continue;
                    float conditionalP = ConditionalP(i, j);
                    
                    conditionals.Add(new Tuple<Settings.Point, Settings.Point>(i, j), conditionalP);
                }
            }
            
            foreach (Settings.Point i in settings._points)
            {
                foreach (Settings.Point j in settings._points)
                {
                    if (i == j) continue;
                    if (similarities.ContainsKey(new(i,j))) 
                        continue;
                    
                    float p_j_given_i = conditionals[new Tuple<Settings.Point, Settings.Point>(i, j)];
                    float p_i_given_j = conditionals[new Tuple<Settings.Point, Settings.Point>(j, i)];

                    float value = (p_j_given_i + p_i_given_j) / (2f * n);
                    
                    similarities.Add(new(i,j), value);
                }
            }
        }
        
        public static float ConditionalP(Settings.Point i, Settings.Point j)
        {
            var points = Manager.Settings._points;

            Vector3 xi = Features(i);
            Vector3 xj = Features(j);

            float sigma = Manager.Settings.sigma;
            float sigma2 = 2f * sigma * sigma;

            float numerator = Mathf.Exp(-(xi - xj).sqrMagnitude / sigma2);

            float denominator = 0f;
            foreach (var k in points)
            {
                if (k == i) continue;

                Vector3 xk = Features(k);
                denominator += Mathf.Exp(-(xi - xk).sqrMagnitude / sigma2);
            }

            return numerator / denominator;
        }
        
        public static Vector3 Features(Settings.Point point)
        {
            Color.RGBToHSV(point.color, out float h, out float s, out float v);

            float angle = h * Mathf.PI * 2f;

            return new Vector3(
                Mathf.Cos(angle) * s,
                Mathf.Sin(angle) * s,
                v
            );
        }
    }
}
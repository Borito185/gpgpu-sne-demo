using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace _Project.Scripts
{
    public static class SimilarityCompute
    {
        public static void UpdateSimilarities()
        {
            Settings settings = Manager.Settings;
            int n = settings.N;
            var similarities = settings._similarities;
            similarities.Clear();

            Dictionary<Tuple<Point, Point>, float> conditionals = new(n * n - n); 
            foreach (Point i in settings._points)
            {
                ComputeSigma(i);
                float denominator = Denominator(i);

                foreach (Point j in settings._points)
                {
                    if (i == j) continue;
                    float conditionalP = ConditionalP(i, j, denominator);
                    
                    conditionals.Add(new Tuple<Point, Point>(i, j), conditionalP);
                }
            }
            
            foreach (Point i in settings._points)
            {
                PriorityQueue<Point, float> nn = new PriorityQueue<Point, float>();
                
                foreach (Point j in settings._points)
                {
                    if (i == j) continue;
                    
                    if (!similarities.TryGetValue(new(i, j), out float value))
                    {
                        float p_j_given_i = conditionals[new Tuple<Point, Point>(i, j)];
                        float p_i_given_j = conditionals[new Tuple<Point, Point>(j, i)];

                        value = (p_j_given_i + p_i_given_j) / (2f * n);
                    
                        similarities.Add(new(i,j), value);
                    }

                    nn.Enqueue(j, value);
                    if (nn.Count > settings.knn) nn.Dequeue();
                }

                i.nearestNeighbours = nn.UnorderedItems.Select(x => x.Element).ToList();
            }
        }
        
        public static float ConditionalP(Point i, Point j, float denom)
        {
            if (denom <= 1e-12f || float.IsNaN(denom) || float.IsInfinity(denom))
                return 0f;
            
            Vector3 xi = Features(i);
            Vector3 xj = Features(j);

            float sigma2 = 2f * i.sigma * i.sigma;
            float numerator = Mathf.Exp(-(xi - xj).sqrMagnitude / sigma2);
            
            return numerator / denom;
        }

        public static float Denominator(Point i)
        {
            var points = Manager.Settings._points;
            float denominator = 0f;
            Vector3 xi = Features(i);
            float sigma2 = 2f * i.sigma * i.sigma;
            foreach (var k in points)
            {
                if (k == i) continue;

                Vector3 xk = Features(k);
                denominator += Mathf.Exp(-(xi - xk).sqrMagnitude / sigma2);
            }
            return Mathf.Max(denominator, 1e-12f);
        }
        
        public static Vector3 Features(Point point)
        {
            Color c = point.color;
            float r = c.r, g = c.g, b = c.b;
            float y  =  0.299f    * r + 0.587f    * g + 0.114f    * b;
            float cb = -0.168736f * r - 0.331264f * g + 0.5f      * b;
            float cr =  0.5f      * r - 0.418688f * g - 0.081312f * b;

            return new Vector3(y, cb, cr);
        }
        
        public static void ComputeSigma(Point i)
        {
            Settings settings = Manager.Settings;
            var points = settings._points;

            float min = 1e-5f;
            float max = 2;

            float log2Perp = Mathf.Log(settings.perplexity, 2);
            
            for (int x = 0; x < 30; x++)
            {
                i.sigma = (min + max) * 0.5f;

                float denominator = Denominator(i);

                float shannonEntropy = 0;
                foreach (Point j in points)
                {
                    if (i == j) continue;

                    float pji = ConditionalP(i, j, denominator);
                    
                    shannonEntropy -= pji * Mathf.Log(pji, 2);
                }

                if (Mathf.Approximately(shannonEntropy, log2Perp)) break;
                if (shannonEntropy < log2Perp)
                    min = i.sigma;
                else
                    max = i.sigma;
            }
            
            i.sigma = (min + max) * 0.5f;
        }
    }
}
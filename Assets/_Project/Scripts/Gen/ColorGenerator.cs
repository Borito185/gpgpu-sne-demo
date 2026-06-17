using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public static class ColorGenerator
    {
        public static List<Color> Generate(int nSamples)
        {
            List<Color> result = new(nSamples);

            for (int i = 0; i < nSamples; i++)
            {
                Color c = Random.ColorHSV();
                c.a = 1;
                result.Add(c);
            }
            
            return result;
        }
    }
}
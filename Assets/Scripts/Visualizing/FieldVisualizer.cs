using System;
using UnityEngine;

namespace _Project.Scripts
{
    public static class FieldVisualizer
    {
        private static Settings.DrawMode previous = Settings.DrawMode.VectorField-1;
        
        public static void Reset()
        {
            Settings settings = Manager.Settings;

            ArrowUtils.Reset();
            settings.s_fieldMesh.gameObject.SetActive(false);
        }

        private static void SetMesh(Mesh mesh)
        {
            Settings settings = Manager.Settings;

            settings.s_fieldMesh.sharedMesh = mesh;
            settings.s_fieldMesh.gameObject.SetActive(true);
        }
        
        public static void Render()
        {
            Reset();
            
            switch (Manager.Settings.drawMode)
            {
                case Settings.DrawMode.VectorField:
                    VectorFieldGenerator.Render();
                    break;
                case Settings.DrawMode.DensityField:
                    SetMesh(FieldTextureGenerator.Render(0));
                    break;
                case Settings.DrawMode.GradientXField:
                    SetMesh(FieldTextureGenerator.Render(1));
                    break;
                case Settings.DrawMode.GradientYField:
                    SetMesh(FieldTextureGenerator.Render(2));
                    break;
                case Settings.DrawMode.PointRepulsive:
                    PointForceVectorGenerator.Render(PointForceVectorGenerator.Mode.RepulsiveForces);
                    break;
                case Settings.DrawMode.PointAttractive:
                    PointForceVectorGenerator.Render(PointForceVectorGenerator.Mode.AttractiveForce);
                    break;
                case Settings.DrawMode.PointSummedForce:
                    PointForceVectorGenerator.Render(PointForceVectorGenerator.Mode.SummedForces);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            ArrowUtils.DrawArrows();
        }
    }
}
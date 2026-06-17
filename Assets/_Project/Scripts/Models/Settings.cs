namespace _Project.Scripts
{
    [System.Serializable]
    public class Settings
    {
        public int nSamples;
        public bool showPoints;
        public DrawMode drawMode;
        
        public enum DrawMode
        {
            VectorField,
            DensityField,
            GradientXField,
            GradientYField,
            PointRepulsive,
            PointAttractive
        }
    }
}
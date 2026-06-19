using System;

namespace _Project.Scripts
{
    public struct PointTuple : IEquatable<PointTuple>
    {
        public Point a;
        public Point b;

        public PointTuple(Point a, Point b)
        {
            this.a = a;
            this.b = b;
        }

        public bool Equals(PointTuple other)
        {
            return (Equals(a, other.a) && Equals(b, other.b)) ||
                   (Equals(a, other.b) && Equals(b, other.a));
        }

        public override bool Equals(object obj)
        {
            return obj is PointTuple other && Equals(other);
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }
    }
}
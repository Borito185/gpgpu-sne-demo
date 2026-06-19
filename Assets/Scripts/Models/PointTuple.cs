using System;

namespace Models
{
    /**
     * Represents a datapoint pair.
     * Equates such that PointTuple(a,b) == PointTuple(b,a)
     * Used for dictionaries (maps in java terms)
     */
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
            => obj is PointTuple other && Equals(other);

        public override int GetHashCode() 
            => a.GetHashCode() ^ b.GetHashCode();
    }
}
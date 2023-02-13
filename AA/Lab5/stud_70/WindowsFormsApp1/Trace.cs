using System.Numerics;
using System.Drawing;

namespace ComputerGraphic
{
    internal class Trace
    {
        public Trace(Vector3 p, Color c)
        {
            Color = c;
            Point = p;
        }
        public bool IsShadowed { get; set; }
        public Vector3 Point { get; set; }
        public Color Color { get; set; }
        public static bool operator <(Trace t1, Trace t2)
        {
            return float.IsInfinity(t2.Point.X)
|| (!float.IsInfinity(t1.Point.X) && t1.Point.LengthSquared() < t2.Point.LengthSquared());
        }
        public static bool operator >(Trace t1, Trace t2)
        {
            return float.IsInfinity(t1.Point.X)
|| (!float.IsInfinity(t2.Point.X) && t1.Point.LengthSquared() > t2.Point.LengthSquared());
        }
    }
}

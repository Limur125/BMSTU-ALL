using System.Numerics;
using System.Drawing;
using System;

namespace ComputerGraphic
{
    internal class Sphere : SceneObject
    {
        private Vector3 Position;
        private readonly float radius;
        private readonly Color color;
        public Sphere(Vector3 Position, float radius, Color color)
        {
            this.Position = Position;
            this.radius = radius;
            this.color = color;
        }

        public override Trace Intersection(Ray ray, LightSource l)
        {
            Vector3 oc = Position - ray.Start;

            float k1 = ray.Direct.LengthSquared();
            float k2 = Vector3.Dot(oc, ray.Direct);
            float k3 = oc.LengthSquared() - (radius * radius);

            float d = (k2 * k2) - (k1 * k3);
            if (d < 0)
                return new Trace(new Vector3(float.PositiveInfinity), Color.Gray);

            double t1 = -(-k2 + Math.Sqrt(d)) / k1;
            double t2 = -(-k2 - Math.Sqrt(d)) / k1;
            if (t1 < 1 && t2 < 1)
                return new Trace(new Vector3(float.PositiveInfinity), Color.Gray);
            double t = t1 > 1 ? t1 : double.MaxValue;
            t = t2 > 1 && t2 < t ? t2 : t;
            Vector3 inters = ray.Start + (ray.Direct * (float)t);
            Vector3 norm = Vector3.Normalize(inters - Position);
            Vector3 lv = Vector3.Normalize(l.Position - inters);
            float i = Vector3.Dot(norm, lv);

            return new Trace(inters, Intense(i));
        }

        private Color Intense(float i)
        {
            if (i < 0.1)
                i = 0.1f;
            return Color.FromArgb((int)(color.R * i), (int)(color.G * i), (int)(color.B * i));
        }
        public override void Rotate(float ax, float ay, float az)
        {
            double x, y, z;
            y = (Position.Y * Math.Cos(ax)) - (Position.Z * Math.Sin(ax));
            z = (Position.Y * Math.Sin(ax)) + (Position.Z * Math.Cos(ax));
            Position.Y = (float)y;
            Position.Z = (float)z;
            x = (Position.X * Math.Cos(az)) - (Position.Y * Math.Sin(az));
            y = (Position.X * Math.Sin(az)) + (Position.Y * Math.Cos(az));
            Position.X = (float)x;
            Position.Y = (float)y;
            z = (Position.Z * Math.Cos(ay)) - (Position.X * Math.Sin(ay));
            x = (Position.Z * Math.Sin(ay)) + (Position.X * Math.Cos(ay));
            Position.Z = (float)z;
            Position.X = (float)x;
        }
        public override void Move(float dx, float dy, float dz)
        {
            Vector3 o = new Vector3(dx, dy, dz);
            Position += o;
        }
        public override bool CastShadow(Trace t, Vector3 l)
        {
            Ray r = new Ray(l - t.Point, t.Point);

            Vector3 oc = Position - r.Start;

            float k1 = r.Direct.LengthSquared();
            float k2 = Vector3.Dot(oc, r.Direct);
            float k3 = oc.LengthSquared() - (radius * radius);

            float d = (k2 * k2) - (k1 * k3);
            if (d <= 0.001f)
                return false;
            double t1 = -(-k2 + Math.Sqrt(d)) / k1;
            double t2 = -(-k2 - Math.Sqrt(d)) / k1;
            return (t1 <= 1 && t1 >= 0.001) || (t2 <= 1 && t2 >= 0.001);
        }
    }
}

using System.Numerics;
using System.Drawing;
using System;

namespace ComputerGraphic
{
    internal class Particle : IDisposable
    {
        private Vector3 position;
        private Vector3 velocity = new Vector3(0, 1, 0);
        private double lifeTime = 0;
        private readonly double maxLifeTime;
        public Color color;
        private readonly float radius;
        public bool IsShadowed { get; set; } = true;

        private Random r = new Random((int)DateTime.UtcNow.Ticks);

        public Particle(Vector3 position, float radius, int maxLifeTime, Color color)
        {
            this.position = position;
            this.maxLifeTime = maxLifeTime;
            this.color = color;
            this.radius = radius;
        }
        public Color CastShadow(Vector3 t, Vector3 l, Color tc)
        {
            Vector3 oc = position - t;

            float k1 = (l - t).LengthSquared();
            float k2 = Vector3.Dot(oc, l - t);
            float k3 = oc.LengthSquared() - (radius * radius);

            float d = (k2 * k2) - (k1 * k3);
            if (d <= 0.001f)
                return tc;

            double t1 = -(-k2 + Math.Sqrt(d)) / k1;
            double t2 = -(-k2 - Math.Sqrt(d)) / k1;
            if ((t1 <= 1 && t1 >= 0.001) || (t2 <= 1 && t2 >= 0.001))
                return Color.FromArgb((int)(tc.R * 0.9f), (int)(tc.G * 0.9f), (int)(tc.B * 0.9f));
            return tc;
        }
        public void Enable(Vector3 t, Vector3 l)
        {
            if (!IsShadowed)
                return;
            Vector3 oc = position - t;

            float k1 = (l - t).LengthSquared();
            float k2 = Vector3.Dot(oc, l - t);
            float k3 = oc.LengthSquared() - (radius * radius);

            float d = (k2 * k2) - (k1 * k3);
            if (d <= 0.001f)
                return;
            double t1 = -(-k2 + Math.Sqrt(d)) / k1;
            double t2 = -(-k2 - Math.Sqrt(d)) / k1;
            if ((t1 <= 1 && t1 >= 0.001) || (t2 <= 1 && t2 >= 0.001))
                IsShadowed = false;
        }
        public bool Update(double dtime, Vector3 smokerPosition)
        {
            float fdtime = (float) dtime;
            Vector3 pos = position - smokerPosition;
            velocity = new Vector3(2f * pos.Y * pos.Z, velocity.Y, -2f * pos.X * pos.Y);
            velocity += new Vector3(Random(-3, 3), 0.1f * Random(-1, 1), Random(-3, 3));
            position += velocity * fdtime;
            if (lifeTime > maxLifeTime)
                return true;
            lifeTime += dtime;
            return false;
        }
        public void Dispose() { }

        public Color Intersection(Vector3 p1, Vector3 p2, Color c)
        {
            Color pc = color;
            if (IsShadowed)
                pc = Color.FromArgb(color.A, (int)(color.R * 0.3f), (int)(color.G * 0.3f), (int)(color.B * 0.3f));
            Vector3 oc = position - p1;
            float k1 = (p2 - p1).LengthSquared();
            float k2 = Vector3.Dot(oc, p2 - p1);
            float k3 = oc.LengthSquared() - (radius * radius);

            float d = (k2 * k2) - (k1 * k3);
            if (d <= 0.001f)
                return c;

            double t1 = -(-k2 + Math.Sqrt(d)) / k1;
            double t2 = -(-k2 - Math.Sqrt(d)) / k1;
            return (t1 <= 1 && t1 >= 0.001) || (t2 <= 1 && t2 >= 0.001)
                ? Color.FromArgb(c.A, ((4 * c.R) + pc.R) / 5, ((4 * c.G) + pc.G) / 5, ((4 * c.B) + pc.G) / 5)
                : c;
        }

        private float Random(float l, float u)
        {
            float f = l + ((u - l) * ((float)r.Next() / int.MaxValue));
            return f;
        }
        public void Rotate(float ax, float ay, float az)
        {
            double x, y, z;
            y = (position.Y * Math.Cos(ax)) - (position.Z * Math.Sin(ax));
            z = (position.Y * Math.Sin(ax)) + (position.Z * Math.Cos(ax));
            position.Y = (float)y;
            position.Z = (float)z;
            x = (position.X * Math.Cos(az)) - (position.Y * Math.Sin(az));
            y = (position.X * Math.Sin(az)) + (position.Y * Math.Cos(az));
            position.X = (float)x;
            position.Y = (float)y;
            z = (position.Z * Math.Cos(ay)) - (position.X * Math.Sin(ay));
            x = (position.Z * Math.Sin(ay)) + (position.X * Math.Cos(ay));
            position.Z = (float)z;
            position.X = (float)x;
        }
        public void Move(float dx, float dy, float dz)
        {
            Vector3 o = new Vector3(dx, dy, dz);
            position += o;
        }
    }
}

using System.Numerics;
using System.Drawing;
using System;
using System.Windows.Forms;

namespace ComputerGraphic
{
    internal class Cube : SceneObject
    {
        private struct SphereShell
        {
            public SphereShell(Vector3[] v)
            {
                Position = Vector3.Zero;
                foreach (var vec in v)
                    Position += vec;
                Position /= v.Length;
                float max_d = Vector3.DistanceSquared(Position, v[0]);
                foreach (var vec in v)
                {
                    float d = Vector3.DistanceSquared(Position, vec);
                    if (d > max_d)
                        max_d = d;
                }
                radius = (float)Math.Sqrt(max_d);
            }
            public Vector3 Position;
            public float radius;
            public bool Intersection(Ray ray)
            {
                Vector3 oc = Position - ray.Start;

                float k1 = ray.Direct.LengthSquared();
                float k2 = Vector3.Dot(oc, ray.Direct);
                float k3 = oc.LengthSquared() - (radius * radius);

                float d = (k2 * k2) - (k1 * k3);
                return d >= 0;
            }
        }

        private SphereShell shell;
        private readonly Vector3[] vertices;
        private readonly int[][] polygons;
        private readonly Color color;
        public Cube(Vector3[] v, int[][] poly, Color color)
        {
            vertices = v;
            polygons = poly;
            this.color = color;
            shell = new SphereShell(v);
        }
        private Vector3? SurfaceIntersection(Ray r, Vector3 n, float d)
        {
            float c = Vector3.Dot(r.Direct, n);
            if (Math.Abs(c) <= 1e-6)
                return null;
            float a = (d - Vector3.Dot(r.Start, n)) / c;
            if (a < 0)
                return null;
            Vector3 q = r.Start + (a * r.Direct);
            return q;
        }
        private Vector3 TriangleIntersection(Ray ray, Vector3[] tri)
        {
            Vector3 e1 = tri[1] - tri[0];
            Vector3 e2 = tri[2] - tri[0];
            Vector3 n = Vector3.Cross(e1, e2);
            float d = Vector3.Dot(n, tri[0]);
            float a = e1.LengthSquared();
            float b = Vector3.Dot(e1, e2);
            float c = e2.LengthSquared();
            float D = (a * c) - (b * b);
            float A = a / D;
            float B = b / D;
            float C = c / D;
            Vector3 Ub = (C * e1) - (B * e2);
            Vector3 Vb = (A * e2) - (B * e1);
            Vector3? t = SurfaceIntersection(ray, n, d);
            if (t is null)
                return new Vector3(float.PositiveInfinity);
            Vector3 q = (Vector3)t;
            Vector3 r = q - tri[0];
            float beta = Vector3.Dot(Ub, r);
            if (beta < 0)
                return new Vector3(float.PositiveInfinity);
            float g = Vector3.Dot(Vb, r);
            if (g < 0)
                return new Vector3(float.PositiveInfinity);
            float alp = 1 - beta - g;
            return alp < 0 ? (new Vector3(float.PositiveInfinity)) : q;
        }
        public override Trace Intersection(Ray ray, LightSource l)
        {
            if (!shell.Intersection(ray))
                return new Trace(new Vector3(float.PositiveInfinity), Color.Gray);
            Vector3 min = new Vector3(float.PositiveInfinity);
            Color cl = color;
            foreach (var ipoly in polygons)
            {
                Vector3[] poly = { vertices[ipoly[0]], vertices[ipoly[1]], vertices[ipoly[2]] };
                Vector3 tmp = TriangleIntersection(ray, poly);
                if (float.IsInfinity(tmp.X))
                    continue;
                if (tmp.LengthSquared() < min.LengthSquared())
                {
                    min = tmp;
                    cl = Intense(Vector3.Dot(Vector3.Normalize(Vector3.Cross(poly[1] - poly[0], poly[2] - poly[0])), Vector3.Normalize(l.Position - min)));
                }
            }
            return !float.IsInfinity(min.X) ? (new Trace(min, cl)) : (new Trace(min, Color.Gray));
        }

        private Color Intense(float i)
        {
            if (i < 0.1)
                i = 0.1f;
            return Color.FromArgb((int)(color.R * i), (int)(color.G * i), (int)(color.B * i));
        }
        public override void Rotate(float ax, float ay, float az)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                double x, y, z;
                y = (vertices[i].Y * Math.Cos(ax)) - (vertices[i].Z * Math.Sin(ax));
                z = (vertices[i].Y * Math.Sin(ax)) + (vertices[i].Z * Math.Cos(ax));
                vertices[i].Y = (float)y;
                vertices[i].Z = (float)z;
                x = (vertices[i].X * Math.Cos(az)) - (vertices[i].Y * Math.Sin(az));
                y = (vertices[i].X * Math.Sin(az)) + (vertices[i].Y * Math.Cos(az));
                vertices[i].X = (float)x;
                vertices[i].Y = (float)y;
                z = (vertices[i].Z * Math.Cos(ay)) - (vertices[i].X * Math.Sin(ay));
                x = (vertices[i].Z * Math.Sin(ay)) + (vertices[i].X * Math.Cos(ay));
                vertices[i].Z = (float)z;
                vertices[i].X = (float)x;
            }
            shell = new SphereShell(vertices);
        }
        public override void Move(float dx, float dy, float dz)
        {
            Vector3 o = new Vector3(dx, dy, dz);
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] += o;
            shell.Position += o;
        }

        public override bool CastShadow(Trace t, Vector3 l)
        {
            Ray r = new Ray(Vector3.Normalize(l - t.Point), t.Point);
            if (!shell.Intersection(r))
                return false;
            foreach (var ipoly in polygons)
            {
                Vector3[] poly = { vertices[ipoly[0]], vertices[ipoly[1]], vertices[ipoly[2]] };
                Vector3 tmp = TriangleIntersection(r, poly);
                if (!float.IsInfinity(tmp.X))
                {
                    float tp = (tmp - r.Start).X / (l - t.Point).X;
                    if (tp > 0.001f && tp < 1)
                        return true;
                }
            }
            return false;
        }
    }
}

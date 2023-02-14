using System.Diagnostics;
using System.Numerics;
namespace ComputerGraphic
{
    [Serializable]
    internal class Scene
    {
        private static readonly int Ch = 768;
        private static readonly int Cw = 1024;
        private readonly float Vh;
        private readonly float Vw;
        private readonly float d = 0.7f;
        private Point bmp_size = new(Cw, Ch);
        private readonly Bitmap bmp;
        private readonly LockBitmap lbmp;
        private readonly Composite Composite = new Composite("composite");
        private readonly LightSource light = new(new(0.01f, 3.8f, 4f));
        Smoker smoker;
        public Bitmap Bmp { get { return bmp; } }

        public Point Bmp_size { get => bmp_size; set => bmp_size = value; }

        private Smoke smoke;

        public Color ParticleColor
        {
            set => smoke.ParticleColor = value;
        }
        public Scene()
        {
            Vh = Ch / (float)Ch;
            Vw = Cw / (float)Ch;
            bmp = new(Bmp_size.X, Bmp_size.Y);
            lbmp = new(bmp);

            smoke = new();
            
            Composite.Add(new Cube(new Vector3[]
            {
                new(-4, -2, -1),
                new(4, -2, -1),
                new(4, -2, 9),
                new(-4, -2, 9),
                new(-4, 4, -1),
                new(4, 4, -1),
                new(4, 4, 9),
                new(-4, 4, 9)
            },
            new int[][]
            {
                new[]{ 0, 2, 1 },
                new[]{ 0, 3, 2 },
                new[]{ 0, 1, 5 },
                new[]{ 0, 5, 4 },
                new[]{ 1, 2, 6 },
                new[]{ 1, 6, 5 },
                new[]{ 0, 7, 3 },
                new[]{ 0, 4, 7 },
                new[]{ 2, 3, 6 },
                new[]{ 3, 7, 6 },
                new[]{ 4, 5, 6 },
                new[]{ 4, 6, 7 }
            }, Color.Blue, "cube_3"));
        }
        public void SetLightIntensity(float i)
        {
            light.intens = i;
        }
        public void SimulateSmoke(Color particleColor, TimeSpan time, float particleSize, int intensity)
        {
            smoke = new();
            smoker = new(smoke, new(0, -1.9f, 4), intensity, particleSize, particleColor);
            smoker.Run(time);
        }
        public void RenderSingle()
        {
            for (int i = 0; i < Cw; i++)
                for (int j = 0; j < Ch; j++)
                {
                    Trace t = TraceRay(i - (Cw / 2), -j + (Ch / 2));
                    CastShadow(t);
                    RenderSmoke(t);
                    bmp.SetPixel(i, j, t.Color);
                }
        }
        public void Render()
        {
            lbmp.LockBits();
            ThreadPool.SetMaxThreads(96, 6);
            for (int i = 0; i < Cw; i++)
                for (int j = 0; j < Ch; j++)
                    ThreadPool.QueueUserWorkItem(RenderPiece, new[] { i, j });
            while (ThreadPool.PendingWorkItemCount != 0) ;
            lbmp.UnlockBits();
        }
        private void RenderPiece(object? param)
        {
            int[] p = (int[])(param ?? new[] { 0, 0 });
            int i = p[0];
            int j = p[1];
            Trace t = TraceRay(i - (Cw / 2), -j + (Ch / 2));
            CastShadow(t);
            RenderSmoke(t);
            lbmp.SetPixel(i, j, t.Color);
        }
        private Trace TraceRay(int x, int y)
        {
            Ray r = new(new(x * Vw / Cw, y * Vh / Ch, d), new(0, 0, 0));
            return Composite.TraceRay(r, light);
        }
        private void CastShadow(Trace trace)
        {
            trace.IsShadowed = Composite.CastShadow(trace, light.Position);
            if (!trace.IsShadowed)
            {
                trace.Color = smoke.CastShadow(trace.Point, light.Position, trace.Color);
                smoke.EnableShadowedParticles(trace.Point, light.Position);
            }
            else
            {
                trace.Color = Color.FromArgb((int)(trace.Color.R * 0.3f), (int)(trace.Color.G * 0.3f), (int)(trace.Color.B * 0.3f));
            }
        }
        private void RenderSmoke(Trace trace)
        {
            trace.Color = smoke.Intersection(new(0, 0, 0), trace.Point, trace.Color);
        }
        public void Move(float dx, float dy, float dz)
        {
            Composite.Move(dx, dy, dz);
            light.Move(dx, dy, dz);
            smoke.Move(dx, dy, dz);
            smoker?.Move(dx, dy, dz);
        }
        public void Move(float dx, float dy, float dz, string name)
        {
            Composite.Move(dx, dy, dz, name);
        }
        public void Rotate(float ax, float ay, float az, string name)
        {
            Composite.Move(ax, ay, az, name);
        }
        public void Rotate(float ax, float ay, float az)
        {
            Composite.Rotate(ax, ay, az);
            light.Rotate(ax, ay, az);
            smoke.Rotate(ax, ay, az);
            smoker?.Rotate(ax, ay, az);
        }
        public void AddObject(SceneObject o)
        {
            Composite.Add(o);
        }
    }
}

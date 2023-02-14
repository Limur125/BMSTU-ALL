using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Drawing;
using System;
using System.Net;

namespace ComputerGraphic
{
    internal class Scene
    {
        private static readonly int Ch = 720;
        private static readonly int Cw = 640;
        private readonly float Vh;
        private readonly float Vw;
        private readonly float d = 0.5f;
        private Point bmp_size = new Point(Cw, Ch);
        private readonly Bitmap bmp;
        private readonly LockBitmap lbmp;
        private readonly Composite Composite = new Composite();
        private readonly LightSource light = new LightSource(new Vector3(0.2f, 3.8f, 4.1f));
        public Bitmap Bmp { get { return bmp; } }
        private bool[] sem;
        public Point Bmp_size { get => bmp_size; set => bmp_size = value; }
        Random r;
        Smoke smoke;
        Smoker smoker;
        public Scene()
        {
            Vh = Ch / (float)Ch;
            Vw = Cw / (float)Ch;
            bmp = new Bitmap(Bmp_size.X, Bmp_size.Y);
            lbmp = new LockBitmap(bmp);

            Composite.Add(new Sphere(new Vector3(4, -2, 9), 2f, Color.Red));
            Composite.Add(new Sphere(new Vector3(0, -1.9f, 4), 0.5f, Color.Blue));
            Composite.Add(new Cube(new Vector3[]
            {
                new Vector3(-2, 0, 7),
                new Vector3(1, 0, 7),
                new Vector3(2, -0.5f, 9),
                new Vector3(0, 3, 8)
            },
            new int[][]
            {
                new[]{ 0, 3, 1 },
                new[]{ 1, 3, 2 },
                new[]{ 0, 1, 2 },
                new[]{ 0, 2, 3 }
            }, Color.Green));
            r = new Random();
            //Composite.Add(new Cube(new Vector3[]
            //{
            //    new Vector3(-0.3f, 2.7f, 3.7f),
            //    new Vector3(0.3f, 2.7f, 3.7f),
            //    new Vector3(0, 2.7f, 4.3f),
            //    new Vector3(-0.3f, 3.9f, 3.7f),
            //    new Vector3(0.3f, 3.9f, 3.7f),
            //    new Vector3(0, 3.9f, 4.3f)
            //},
            //new int[][]
            //{
            //    new[]{ 3, 5, 4 },
            //    new[]{ 0, 4, 3 },
            //    new[]{ 0, 1, 4 },
            //    new[]{ 1, 5, 4 },
            //    new[]{ 1, 2, 5 },
            //    new[]{ 0, 3, 5 },
            //    new[]{ 0, 5, 2 }
            //}, Color.Green));
            Composite.Add(new Cube(new Vector3[]
            {
                new Vector3(-4, -2, -1),
                new Vector3 (4, -2, -1),
                new Vector3 (4, -2, 9),
                new Vector3 (-4, -2, 9),
                new Vector3 (-4, 4, -1),
                new Vector3 (4, 4, -1),
                new Vector3 (4, 4, 9),
                new Vector3 (-4, 4, 9)
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
            }, Color.Blue));
        }
        class Params
        {
            public int WidthStart;
            public int HeightStart;
            public int WidthEnd;
            public int HeightEnd;
            public int SemaphoreIndex;
            public Params(int widthStart, int heightStart, int widthEnd, int heightEnd, int semaphoreIndex)
            {
                WidthStart = widthStart;
                HeightStart = heightStart;
                WidthEnd = widthEnd;
                HeightEnd = heightEnd;
                SemaphoreIndex = semaphoreIndex;
            }
        }
        public void SimulateSmoke(Color particleColor, TimeSpan time, float particleSize, int intensity)
        {
            smoke = new Smoke();
            smoker = new Smoker(smoke, new Vector3(0, -1.9f, 4), intensity, particleSize, particleColor);
            smoker.Run(time);
        }
        public void AddSphere(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Composite.Add(new Sphere(GetRandomVector3(), 0.1f, Color.Gold));
            }
        }

        Vector3 GetRandomVector3()
        {
            return new Vector3(Random(-4, 4), Random(-2, 4), Random(-1, 9));
        }
        private float Random(float l, float u)
        {
            float f = l + ((u - l) * ((float)r.Next() / int.MaxValue));
            return f;
        }
        public void Render(int threadCount)
        {
            Thread[] threads = new Thread[3 * (int)Math.Pow(2, threadCount + 1)];
            sem = new bool[3 * (int)Math.Pow(2, threadCount + 1)];
            int dtc = threadCount / 2;
            int mtc = threadCount % 2;
            int jh = 3 * (int)Math.Pow(2, dtc);
            int iw = (int)Math.Pow(2, dtc + mtc + 1);
            // Console.WriteLine($"{iw} {jh}");
            int k = 0;
            lbmp.LockBits();
            for (int i = 0; i < iw; i++)
                for (int j = 0; j < jh; j++)
                {
                    sem[k] = true;
                    
                    threads[k] = new Thread(RenderPiece);
                    threads[k].Start(new Params(i * Cw / iw, j * Ch / jh, (i + 1) * Cw / iw, (j + 1) * Ch / jh, k));
                    k++;
                }
            bool f = true;
            while (f)
            { 
                f = false;
                for (int i = 0; i < sem.Length; i++)
                    if (sem[i])
                        f = true;
            }
            lbmp.UnlockBits();
        }
        public void RenderFollow()
        {
            sem = new bool[] { true };
            lbmp.LockBits();
            RenderPiece(new Params(0, 0, Cw, Ch, 0));
            lbmp.UnlockBits();
        }
        public void RenderSingle()
        {
            Thread thread = new Thread(RenderPiece);
            sem = new bool[] { true };
            lbmp.LockBits();
            thread.Start(new Params(0, 0, Cw, Ch, 0));

            bool f = true;
            while (f)
            {
                f = false;
                for (int i = 0; i < sem.Length; i++)
                    if (sem[i])
                        f = true;
            }
            lbmp.UnlockBits();
        }
        private void RenderPiece(object parametrs)
        {
            Params p = (Params)parametrs;
            int sw = p.WidthStart;
            int ew = p.WidthEnd;
            int sh = p.HeightStart;
            int eh = p.HeightEnd;
            // Console.WriteLine($"{sw} {sh} {ew} {eh}");
            for (int i = sw; i < ew; i++)
                for (int j = sh; j < eh; j++)
                {
                    Trace t = TraceRay(i - (Cw / 2), -j + (Ch / 2));
                    CastShadow(t);
                    RenderSmoke(t);
                    lbmp.SetPixel(i, j, t.Color);
                }
            // Console.WriteLine($"{sw} {sh} Done");
            sem[p.SemaphoreIndex] = false;
        }
        private Trace TraceRay(int x, int y)
        {
            Ray r = new Ray(new Vector3(x * Vw / Cw, y * Vh / Ch, d), new Vector3(0, 0, 0));
            return Composite.TraceRay(r, light);
        }
        private void CastShadow(Trace trace)
        {
            trace.IsShadowed = Composite.CastShadow(trace, light.Position);
            if (trace.IsShadowed)
                trace.Color = Color.FromArgb((int)(trace.Color.R * 0.3f), (int)(trace.Color.G * 0.3f), (int)(trace.Color.B * 0.3f));
        }
        private void RenderSmoke(Trace trace)
        {
            trace.Color = smoke.Intersection(new Vector3(0, 0, 0), trace.Point, trace.Color);
        }
    }
}

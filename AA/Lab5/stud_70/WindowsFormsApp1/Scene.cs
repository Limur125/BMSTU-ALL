using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Drawing;
using System;
using System.Net;
using WindowsFormsApp1;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ComputerGraphic
{
    internal class Scene
    {
        private int Ch = 720;
        private int Cw = 640;
        private readonly float Vh = 1f;
        private readonly float Vw = 1f;
        private readonly float d = 0.5f;
        private Point bmp_size;
        private readonly Bitmap bmp;
        private readonly LockBitmap lbmp;
        private readonly Composite Composite = new Composite();
        private readonly LightSource light = new LightSource(new Vector3(0.2f, 3.8f, 4.1f));
        public Bitmap Bmp { get { return bmp; } }
        Random r;
        public Point Bmp_size { get => bmp_size; set => bmp_size = value; }
        Smoke smoke;
        Smoker smoker;
        public Scene(int cw, int ch)
        {
            bmp_size = new Point(Cw, Ch);
            Ch = ch;
            Cw = cw;
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
            smoke = new Smoke();
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
        public void SimulateSmoke(Color particleColor, TimeSpan time, float particleSize, int intensity)
        {
            smoke = new Smoke();
            smoker = new Smoker(smoke, new Vector3(0, -1.9f, 4), intensity, particleSize, particleColor);
            smoker.Run(time);
        }
        public List<Query> RenderConveyor(int xmax, int ymax)
        {
            Cw = xmax;
            Ch = ymax;
            Thread[] threads = new Thread[4];
            LineBase[] lines = new LineBase[4];
            Stopwatch start = new Stopwatch();
            int k = 0;
            List<Query> gen = new List<Query>();
            for (int i = 0; i < Cw; i++)
                for (int j = 0; j < Ch; j++)
                    gen.Add(new Query(k++, i, j));
            gen.Add(new Query(-1, 0, 0, true));
            LineSetPixel lsp = new LineSetPixel(this, start);
            lines[3] = lsp; 
            LineRenderSmoke lrs = new LineRenderSmoke(lsp, this, start);
            lines[2] = lrs;
            LineCastShadow lcs = new LineCastShadow(lrs, this, start);
            lines[1] = lcs;
            LineTraceRay ltr = new LineTraceRay(lcs, gen, this, start);
            lines[0] = ltr;
            lbmp.LockBits();
            start.Start();
            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(lines[i].RunLine);
                threads[i].Start();
            }            
            foreach (Thread thread in threads)
                thread.Join();
            lbmp.UnlockBits();
            return lsp.result;
        }
        public void RenderFollow(int xmax, int ymax)
        {
            Cw = xmax;
            Ch = ymax;
            for (int i = 0; i < Cw; i++)
                for (int j = 0; j < Ch; j++)
                {
                    Trace t = TraceRay(i, j);
                    CastShadow(t);
                    RenderSmoke(t);
                    lbmp.SetPixel(i, j, t.Color);
                }
        }
        public Trace TraceRay(int x, int y)
        {
            x -= (Cw / 2);
            y = (Ch / 2) - y;
            Ray r = new Ray(new Vector3(x * Vw / Cw, y * Vh / Ch, d), new Vector3(0, 0, 0));
            return Composite.TraceRay(r, light);
        }
        public void CastShadow(Trace trace)
        {
            trace.IsShadowed = Composite.CastShadow(trace, light.Position);
            if (trace.IsShadowed)
                trace.Color = Color.FromArgb((int)(trace.Color.R * 0.3f), (int)(trace.Color.G * 0.3f), (int)(trace.Color.B * 0.3f));
        }
        public void RenderSmoke(Trace trace)
        {
            trace.Color = smoke.Intersection(new Vector3(0, 0, 0), trace.Point, trace.Color);
        }
        public void SetPixel(int i, int j, Color trace)
        {
            lbmp.SetPixel(i, j, trace);
        }
    }
}

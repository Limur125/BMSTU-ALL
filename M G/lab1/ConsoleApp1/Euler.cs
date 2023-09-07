using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace ConsoleApp1
{
    internal class Euler
    {
        static public List<Vector2> РешениеЯвное(Func<float, float, float> f, float x0, float y0, float xm, float h)
        {
            List<Vector2> res = new();
            Vector2 last = new(x0, y0);
            for (float x = x0; x <= xm; x += h)
            {
                float y = last.Y + h * f(last.X, last.Y);
                res.Add(new(last.X, last.Y));
                last = new(x, y);
            }
            return res;
        }
        static public List<Vector2> РешениеНеявное(Func<float, float, float> f, float x0, float y0, float xm, float h)
        {
            List<Vector2> res = new();
            Vector2 last = new(x0, y0);
            for (float x = x0; x <= xm; x += h)
            {
                Func<float, float> eq = y1 => y1 - h * f(x + h, y1) - last.Y;
                float y = EqSolve(eq, last.Y);
                res.Add(new(last.X, last.Y));
                last = new(x, y);
            }
            return res;
        }
        static public float EqSolve(Func<float, float> f, float x0)
        {
            float x = x0, df, h = 0.00001f;
            df = (f(x + h) - f(x)) / h;
            for (int i = 1; i <= 1000; i++)
                x = x - f(x) / df;
            return x;
        }
    }
}

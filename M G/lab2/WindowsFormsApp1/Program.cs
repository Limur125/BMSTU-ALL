using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Program
    {
        static double beta = 1;
        static double Tw = 2000;
        static double lp = 12;
        static double R = 0.35;
        static double Lk = 187e-6;
        static double Rk = 0.2500;
        static double Ck = 268e-6;
        static Func<double, double, double, double> dIdt = (t, U, I) => (U - (Rk + Rp(I)) * I) / Lk;
        static Func<double, double, double, double> dUdt = (t, U, I) => -I / Ck;
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f1 = new Form1(), f2 = new Form1(), f3 = new Form1();
            List<(double, double)> U, I, Rp = new List<(double, double)>(), IRp = new List<(double, double)>(), T0 = new List<(double, double)>();
            (I, U) = RungeKutta4(dIdt, dUdt, 0, 0.5, 1400, 800e-6, 1e-6);
            foreach (var u in U)
                f1.ChartAddPoint(1, u.Item1, u.Item2);
            foreach (var i in I)
            {
                f1.ChartAddPoint(0, i.Item1, i.Item2);
                Rp.Add((i.Item1, Program.Rp(i.Item2)));
                T0.Add((i.Item1, Program.T0(i.Item2)));
            }
            Rp[0] = (0, 0);
            for (int i = 0; i < I.Count; i++)
                IRp.Add((I[i].Item1, I[i].Item2 * Rp[i].Item2));
            foreach (var rp in Rp)
                f3.ChartAddPoint(2, rp.Item1, rp.Item2);
            foreach (var irp in IRp)
                f1.ChartAddPoint(3, irp.Item1, irp.Item2);
            foreach (var t0 in T0)
                f2.ChartAddPoint(4, t0.Item1, t0.Item2);

            Application.Run(f1);
            Application.Run(f2);
            Application.Run(f3);
            return 0;
        }
        static double Interpolation(double x0, double[] x1, double[] y1)
        {
            double[] x = new double[x1.Length], y = new double[y1.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = x1[i];
                y[i] = y1[i];
            }
            x0 = x0;
            double y0 = 0, l;
            for (int i = 0; i < x.Length; i++)
            {
                l = 1;
                for (int j = 0; j < x.Length; j++)
                    if (i != j)
                        l *= (x0 - x[j]) / (x[i] - x[j]);
                y0 += y[i] * l;
            }
            return y0;
        }

        static double T0(double I0)
        {
            double[] I = new double[] { 0.5, 1, 5, 10, 50, 200, 400, 800, 1200 };
            double[] T0 = new double[] { 6730, 6790, 7150, 7270, 8010, 9185, 10010, 11140, 12010 };
            return Interpolation(I0, I, T0);
        }
        static double M(double I0)
        {
            double[] I = new double[] { 0.5, 1, 5, 10, 50, 200, 400, 800, 1200 };
            double[] m0 = new double[] { 0.50, 0.55, 1.7, 3, 11, 32, 40, 41, 39 };
            return Interpolation(I0, I, m0);
        }
        static double Sigma(double z, double I)
        {
            double[] T = new double[] { 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
            double[] sig = new double[] { 0.031, 0.27, 2.05, 6.06, 12.0, 19.9, 29.6, 41.1, 54.1, 67.7, 81.5 };
            
            double t0 = T0(I);
            return z * Interpolation(t0 + (Tw - t0) * Math.Pow(z, M(I)), T, sig);
        }
        static double Integral(double a, double b, int n, Func<double, double, double> f, double i) 
        {
            double width = (b - a) / n;
            double integral = 0;
            for(int step = 0; step < n; step++) 
            {
                double x1 = a + step * width;
                double x2 = a + (step + 1) * width;
                integral += (x2 - x1) / 2 * (f(x1, i) + f(x2, i));
            }
            return integral;
        }
        static double Rp(double i)
        {
            return lp / (2 * Math.PI * R * R * Integral(0, 1, 30, Sigma, i));
        }
        static (List<(double, double)>, List<(double, double)>) RungeKutta1(Func<double, double, double, double> dIdt, Func<double, double, double, double> dUdt, double t0, double I0, double U0, double tm, double h)
        {
            List<(double, double)> Ires = new List<(double, double)>();
            (double, double) lastI = (t0, I0);
            List<(double, double)> Ures = new List<(double, double)>();
            (double, double) lastU = (t0, U0);
            for (double t = t0; t <= tm; t += h)
            {
                double I = lastI.Item2 + h * dIdt(lastI.Item1, lastU.Item2, lastI.Item2);
                double U = lastU.Item2 + h * dUdt(lastI.Item1, lastU.Item2, lastI.Item2);
                Ires.Add(lastI);
                Ures.Add(lastU);
                lastI = (t, I);
                lastU = (t, U);
            }
            return (Ires, Ures);
        }
        static (List<(double, double)>, List<(double, double)>) RungeKutta2(Func<double, double, double, double> dIdt, Func<double, double, double, double> dUdt, double t0, double I0, double U0, double tm, double h)
        {
            List<(double, double)> Ires = new List<(double, double)>();
            (double, double) lastI = (t0, I0);
            List<(double, double)> Ures = new List<(double, double)>();
            (double, double) lastU = (t0, U0);
            for (double t = t0; t <= tm; t += h)
            {
                double I1 = h * dIdt(lastI.Item1, lastU.Item2, lastI.Item2);
                double U1 = h * dUdt(lastI.Item1, lastU.Item2, lastI.Item2);
                double I2 = h * dIdt(lastI.Item1 + h / (2 * beta), lastU.Item2 + U1 / (2 * beta), lastI.Item2 + I1 / (2 * beta));
                double U2 = h * dUdt(lastI.Item1 + h/(2 * beta), lastU.Item2 + U1 / (2 * beta), lastI.Item2 + I1 / (2 * beta));
                double I = lastI.Item2 + ((1 - beta) * I1 + beta * I2);
                double U = lastU.Item2 + ((1 - beta) * U1 + beta * U2);
                Ires.Add(lastI);
                Ures.Add(lastU);
                lastI = (t, I);
                lastU = (t, U);
            }
            return (Ires, Ures);
        }
        static (List<(double, double)>, List<(double, double)>) RungeKutta4(Func<double, double, double, double> dIdt, Func<double, double, double, double> dUdt, double t0, double I0, double U0, double tm, double h)
        {
            List<(double, double)> Ires = new List<(double, double)>();
            (double, double) lastI = (t0, I0);
            List<(double, double)> Ures = new List<(double, double)>();
            (double, double) lastU = (t0, U0);
            for (double t = t0; t <= tm; t += h)
            {
                double I1 = h * dIdt(lastI.Item1, lastU.Item2, lastI.Item2);
                double U1 = h * dUdt(lastI.Item1, lastU.Item2, lastI.Item2);
                double I2 = h * dIdt(lastI.Item1 + h / 2, lastU.Item2 + U1 / 2, lastI.Item2 + I1 / 2);
                double U2 = h * dUdt(lastI.Item1 + h / 2, lastU.Item2 + U1 / 2, lastI.Item2 + I1 / 2); 
                double I3 = h * dIdt(lastI.Item1 + h / 2, lastU.Item2 + U2 / 2, lastI.Item2 + I2 / 2);
                double U3 = h * dUdt(lastI.Item1 + h / 2, lastU.Item2 + U2 / 2, lastI.Item2 + I2 / 2);
                double I4 = h * dIdt(lastI.Item1 + h, lastU.Item2 + U3, lastI.Item2 + I3);
                double U4 = h * dUdt(lastI.Item1 + h, lastU.Item2 + U3, lastI.Item2 + I3);
                double I = lastI.Item2 + (I1 + 2 * I2 + 2 * I3 + I4) / 6;
                double U = lastU.Item2 + (U1 + 2 * U2 + 2 * U3 + U4) / 6;
                Ires.Add(lastI);
                Ures.Add(lastU);
                lastI = (t, I);
                lastU = (t, U);
            }
            return (Ires, Ures);
        }

        static double[][] DivDiff(double[] x, double[] y, int node)
        {
            double[][] poly = new double[15][];
            int poly_n = 0;
            int i;
            for (i = 0; i < node; i++)
                poly[poly_n++] = new double[node + 1];
            for (i = 0; i < node; i++)
            {
                poly[i][0] = x[i];
                poly[i][1] = y[i];
            }
            i = 2;
            int new_node = node - 1;
            while (i < (node + 1))
            {
                int j = 0;
                while (j < new_node)
                {
                    poly[j][i] = (poly[j + 1][i - 1] - poly[j][i - 1]) / (poly[i - 1][0] - poly[0][0]);
                    j++;
                }
                i++;
                new_node--;
            }
            return poly;
        }

        static double Interpolation(double arg, double[] x1, double[] y1, int node)
        {
            double[][] poly = DivDiff(x1, y1, node);
            double arg_y = poly[0][1];
            int i = 2;
            while (i < node + 1)
            {
                int j = 0;
                double p = 1;
                while (j < i - 1)
                    p *= (arg - poly[j++][0]);
                arg_y += poly[0][i++] * p;
            }
            return Math.Exp(arg_y);
        }
    }

}

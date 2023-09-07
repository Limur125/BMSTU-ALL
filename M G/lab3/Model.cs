using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace lab3
{
    internal class Model
    {
        public double Np;
        public double r0;
        public double R;
        public double z0;
        public double zN;
        public double T0;
        public double sigma;
        public double F0;
        public double alpha;
        public double h;

        public Model()
        {
            z0 = r0 / R;
        }

        public Model(double np, double r0, double r, double zN, double t0, double sigma, double f0, double alpha, double h)
        {
            Np = np;
            this.r0 = r0;
            R = r;
            this.zN = zN;
            T0 = t0;
            this.sigma = sigma;
            F0 = f0;
            this.alpha = alpha;
            this.h = h;
            z0 = r0 / R;
        }

        public static double Labmda(double T)
        {
            //return 2.50e-2;
            double[] lambda = new double[] { 1.36e-2, 1.63e-2, 1.81e-2, 1.98e-2, 2.50e-2, 2.74e-2 };
            double[] t = new double[] {          300,     500,     800,    1100,     2000,   2400 };
            return Interpolation(T, t, lambda);
        }

        private static double K(double T)
        {
            double[] k = new double[] { 2.0e-2, 5.0e-2, 7.8e-2, 1.0e-1, 1.3e-1, 2.0e-1 };
            double[] t = new double[] { 293, 1278, 1528, 1677, 2000, 2400 };
            return Interpolation(T, t, k);
        }

        private double P(double T)
        {
            return 4 * Np * Np * sigma * K(T) * Math.Pow(T, 3);
        }

        private double F(double T)
        {
            return 4 * Np * Np * sigma * K(T) * Math.Pow(T0, 4);
        }

        private double V(double zn_1, double zn, double zn1)
        {
            return (Math.Pow((zn1 + zn) / 2, 2) - Math.Pow((zn + zn_1) / 2, 2)) / 2;
        }

        private double A(double yn_1, double yn, double zn_1, double zn)
        {
            return 1 / (R * R * h) * (zn_1 + zn) / 2 * (Labmda(yn_1) + Labmda(yn)) / 2;
        }

        private double B(double yn_1, double yn, double yn1, double zn_1, double zn, double zn1)
        {
            return A(yn_1, yn, zn_1, zn) + C(yn, yn1, zn, zn1) + (P(yn) * V(zn_1, zn, zn1));
        }

        private double C(double yn, double yn1, double zn, double zn1)
        {
            return 1 / (R * R * h) * (zn1 + zn) / 2 * (Labmda(yn1) + Labmda(yn)) / 2;
        }

        private double D(double yn, double zn_1, double zn, double zn1)
        {
            return F(yn) * V(zn_1, zn, zn1);
        }

        static int binary_search(double[] lst, double value)
        {
            int l = 0;
            int r = lst.Length - 1;

            while (l < r)
            {
                int mid = (l + r) / 2;
                if (lst[mid] < value)
                    l = mid + 1;
                else
                    r = mid;
            }
            return l;
        }
        private static double Interpolation(double x0, double[] x1, double[] y1)
        {
            if (x0 < x1[0])
                return y1[0];

            if (x0 > x1[x1.Length - 1])
                return y1[y1.Length - 1];

            int i = binary_search(x1, x0);

            if (x1[i] > x0)
                i--;

            if (1e-10 > Math.Abs(x1[i] - x0))
                return y1[i];

            return y1[i] + (y1[i + 1] - y1[i]) * (x0 - x1[i]) / (x1[i + 1] - x1[i]);


            /*
            double[] x = new double[x1.Length], y = new double[y1.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = x1[i];
                y[i] = y1[i];
            }
            double y0 = 0, l;
            for (int i = 0; i < x.Length; i++)
            {
                l = 1;
                for (int j = 0; j < x.Length; j++)
                    if (i != j)
                        l *= (x0 - x[j]) / (x[i] - x[j]);
                y0 += y[i] * l;
            }
            return y0;*/
        }

        public double[] RunMethod(double[] a, double[] b, double[] c, double[] d, double m0, double k0, double mn, double kn, double p0, double pn)
        {
            double[] y = new double[b.Length];
            double[] xi = new double[b.Length - 1];
            double[] eta = new double[b.Length - 1];
            xi[0] = -k0 / m0;
            eta[0] = p0 / m0;
            for (int i = 1; i < xi.Length; i++)
            {
                double tmp = b[i] - a[i] * xi[i - 1];
                xi[i] = c[i] / tmp;
                eta[i] = (a[i] * eta[i - 1] + d[i]) / tmp;
            }
            y[y.Length - 1] = (pn - mn * eta[eta.Length - 1]) / (kn + mn * xi[xi.Length - 1]);
            for (int i = y.Length - 1; i > 0; i--)
            {
                y[i - 1] = xi[i - 1] * y[i] + eta[i - 1];
            }
            return y;
        }

        private double P0(double z0, double z1, double y0, double y1)
        {
            double z12 = (z0 + z1) / 2;
            double f12 = (F(y0) + F(y1)) / 2;
            //return (z0 * F0 / R) + (h / 4 * ((F(y0) * z0) + (f12 * z12)));
            return F0;
        }

        private double K0(double z0, double z1, double y0, double y1)
        {
            double z12 = (z0 + z1) / 2;
            double x12 = (Labmda(y0) + Labmda(y1)) / 2;
            double p12 = (P(y0) + P(y1)) / 2;
            //return (-(z12 * x12) / (R * R * h)) + (h / 8 * p12 * z12);
            return -x12 / (R * h);
        }

        private double M0(double z0, double z1, double y0, double y1)
        {
            double z12 = (z0 + z1) / 2;
            double x12 = (Labmda(y0) + Labmda(y1)) / 2;
            double p12 = (P(y0) + P(y1)) / 2;
           // return (z12 * x12 / (R * R * h)) + (h / 8 * p12 * z12) + (h / 4 * P(y0) * z0);
            return x12 / (R * h);
        }

        private double PN(double zN_1, double zN, double yN_1, double yN)
        {
            double zN_12 = (zN_1 + zN) / 2;
            double fN_12 = (F(yN_1) + F(yN)) / 2;
            //return (-alpha * T0 * zN / R) + (h / 4 * ((F(yN) * zN) + (fN_12 * zN_12)));
            return alpha * T0;
        }

        private double KN(double zN_1, double zN, double yN_1, double yN)
        {
            double zN_12 = (zN + zN_1) / 2;
            double xN_12 = (Labmda(yN_1) + Labmda(yN)) / 2;
            double pN_12 = (P(yN) + P(yN_1)) / 2;
            //return (zN_12 * xN_12 / (R * R * h)) + (alpha * zN / R) + (h / 8 * pN_12 * zN_12) + (h / 4 * P(yN) * zN);
            return -xN_12 / (R * h) + alpha;
        }

        private double MN(double zN_1, double zN, double yN_1, double yN)
        {
            double zN_12 = (zN + zN_1) / 2;
            double xN_12 = (Labmda(yN) + Labmda(yN_1)) / 2;
            double pN_12 = (P(yN) + P(yN_1)) / 2;
            //return -((zN_12 * xN_12 / (R * R * h)) + (h / 8 * pN_12 * zN_12));
            return xN_12 / (R * h);
        }
        public (double[] x, double[] y) Run()
        {
            int n = ((int)((zN - z0) / h)) + 1;
            double[] y = new double[n], z = new double[n];
            for (int i = 0; i < n; i++)
            {
                z[i] = z0 + (i * h)/ R;
                y[i] = T0;
            }
            double[] erry = new double[n];
            double maxerr;
            int its = 0;
            do
            {
                double[] a = new double[n], b = new double[n], c = new double[n], d = new double[n];
                double k0, m0, p0, kn, pn, mn;
                for (int i = 1; i < n - 1; i++)
                {
                    a[i] = A(y[i - 1], y[i], z[i - 1], z[i]);
                    b[i] = B(y[i - 1], y[i], y[i + 1], z[i - 1], z[i], z[i + 1]);
                    c[i] = C(y[i], y[i + 1], z[i], z[i + 1]);
                    d[i] =D(y[i], z[i - 1], z[i], z[i + 1]);
                }
                k0 = K0(z[0], z[1], y[0], y[1]);
                m0 = M0(z[0], z[1], y[0], y[1]);
                p0 = P0(z[0], z[1], y[0], y[1]);
                kn = KN(z[n - 2], z[n - 1], y[n - 2], y[n - 1]);
                mn = MN(z[n - 2], z[n - 1], y[n - 2], y[n - 1]);
                pn = PN(z[n - 2], z[n - 1], y[n - 2], y[n - 1]);
                double[] newy = RunMethod(a, b, c, d, m0, k0, mn, kn, p0, pn);

                for (int i = 0; i < n; i++)
                    erry[i] = Math.Abs((newy[i] - y[i]) / newy[i]);
                y = newy;
                maxerr = erry.Max();
                its++;
            }
            while (maxerr > 1e-8 && its < 100);
            Console.WriteLine($"{its}");
            return (z, y);
        }
    }
}


using Microsoft.Win32.SafeHandles;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System;

using Double5Func = System.Func<double, double, double, double, double, double>;
using Double3Func = System.Func<double, double, double, double>;
using Double2Func = System.Func<double>;
using System.Data.SqlTypes;

namespace lab5
{
    internal class Model
    {
        public double a1, b1, c1, m1;
        public double beta, x0, z0, f0;
        public double hx, hz, tau;
        public double F0, T0;
        public double alpha2, alpha3, alpha4;
        public Model(double a1, double b1, double c1, double m1, double f0, double beta, 
            double x0, double z0, double hx, double hz, double F0, double T0, double alpha2,
            double alpha3, double alpha4)
        {
            this.a1 = a1;
            this.b1 = b1;
            this.c1 = c1;
            this.m1 = m1;
            this.beta = beta;
            this.x0 = x0;
            this.z0 = z0;
            this.f0 = f0;
            this.hx = hx;
            this.hz = hz;
            this.F0 = F0;
            this.T0 = T0;
            this.alpha2 = alpha2;
            this.alpha3 = alpha3;
            this.alpha4 = alpha4;
            this.tau = 1e-2;
        }

        public double Labmda(double T)
        {
            return 1;
            //return a1 * (b1 + c1 * Math.Pow(T, m1));
        }

        public double F(double x, double z)
        {
            //return 0;
            return f0 * Math.Exp(-beta * ((x - x0) * (x - x0) * (z - z0) * (z - z0)));
               // + f0 * Math.Exp(-beta * ((x - 3) * (x - 3) + (z - 3) * (z - 3)));
        }

        private double An(/*double yn_1m, double ynm*/)
        {
            double ln_12m = Labmda(1);
            return ln_12m / (hx * hx);
        }

        private double Bn(/*double yn_1m, double ynm, double yn1m*/)
        {
            return An() + Cn() + 2 / tau;
        }

        private double Cn(/*double ynm, double yn1m*/)
        {
            double ln12m = Labmda(1);
            return ln12m / (hx * hx);
        }

        private double Dn(double ynm_1, double ynm, double ynm1, double x, double z)
        {
            double lnm12 = (Labmda(ynm1) + Labmda(ynm)) / 2;
            double lnm_12 = (Labmda(ynm_1) + Labmda(ynm)) / 2;
            double dynm_12 = (ynm_1 - ynm) / (hz * hz);
            double dynm12 = (ynm - ynm1) / (hz * hz);
            return 2 * ynm / tau + lnm12 * (ynm_1 - 2*ynm + ynm1)/(hz * hz) + F(x, z);
           // return 2 * ynm / tau  - lnm_12 * dynm_12 + lnm12 * dynm12 + F(x, z);
        }

        //private double Dn0(double _, double yn0, double yn1, double x, double z)
        //{
        //    double ln12 = (Labmda(yn1) + Labmda(yn0)) / 2;
        //    double dyn12 = (yn0 - yn1) / (hz * hz);
        //    double Fn0 = alpha3 * (yn0 - T0) / hz;
        //    return 2 * yn0 / tau - Fn0 + ln12 * dyn12 + F(x, z);
        //}

        //private double DnM(double ynm_1, double ynm, double _, double x, double z)
        //{
        //    double lnm_12 = (Labmda(ynm_1) + Labmda(ynm)) / 2;
        //    double dynm_12 = (ynm_1 - ynm) / (hz * hz);
        //    double FnM = alpha4 * (ynm - T0) / hz;
        //    return 2 * ynm / tau - lnm_12 * dynm_12 - FnM + F(x, z);
        //}

        //private double P0n0(double _, double y0m, double y0m1, double x, double z)
        //{
        //    double l0m12 = (Labmda(y0m) + Labmda(y0m1)) / 2;
        //    double dy0m12 = (y0m - y0m1) / (hz * hz);
        //    double Fn0 = alpha3 * (y0m - T0) / hz;
        //    return l0m12 * dy0m12 - Fn0 + F0 / hx + 2 * y0m / tau + F(x, z);
        //}

        //private double P0nM(double y0m_1, double y0m, double _, double x, double z)
        //{
        //    double l0m_12 = (Labmda(y0m_1) + Labmda(y0m)) / 2;
        //    double dy0m_12 = (y0m_1 - y0m) / (hz * hz);
        //    double FnM = alpha4 * (y0m - T0) / hz;
        //    return -FnM - l0m_12 * dy0m_12 + F0 / hx + 2 * y0m / tau + F(x, z);
        //}

        //private double P0n(double y0m_1, double y0m, double y0m1, double x, double z)
        //{
        //    double l0m_12 = (Labmda(y0m_1) + Labmda(y0m)) / 2;
        //    double l0m12 = (Labmda(y0m) + Labmda(y0m1)) / 2;
        //    double dy0m12 = (y0m - y0m1) / (hz * hz);
        //    double dy0m_12 = (y0m_1 - y0m) / (hz * hz);
        //    return l0m12 * dy0m12 - l0m_12 * dy0m_12 + F0 / hx + 2 * y0m / tau + F(x, z);
        //}

        //private double K0n(double y0m, double y1m)
        //{
        //    double l12m = (Labmda(y0m) + Labmda(y1m)) / 2;
        //    return l12m / (hx * hx);
        //}

        //private double M0n(double y0m, double y1m)
        //{
        //    double l12m = (Labmda(y0m) + Labmda(y1m)) / 2;
        //    return 2 / tau - l12m / (hx * hx);
        //}

        //private double PNn(double yNm_1, double yNm, double yNm1, double x, double z)
        //{
        //    double lNm_12 = (Labmda(yNm_1) + Labmda(yNm)) / 2;
        //    double lNm12 = (Labmda(yNm) + Labmda(yNm1)) / 2;
        //    double dyNm12 = (yNm - yNm1) / (hz * hz);
        //    double dyNm_12 = (yNm_1 - yNm) / (hz * hz);
        //    return -lNm_12 * dyNm_12 + lNm12 * dyNm12 + alpha2 * T0 / hx + 2 * yNm / tau + F(x, z);
        //}

        //private double PNn0(double _, double yNm, double yNm1, double x, double z)
        //{
        //    double lNm12 = (Labmda(yNm1) + Labmda(yNm)) / 2;
        //    double dyNm12 = (yNm - yNm1) / (hz * hz);
        //    double Fn0 = alpha3 * (yNm - T0) / hz;
        //    return lNm12 * dyNm12 - Fn0 + alpha2 * T0 / hx + 2 * yNm / tau + F(x, z);
        //}

        //private double PNnM(double yNm_1, double yNm, double _, double x, double z)
        //{
        //    double lNm_12 = (Labmda(yNm_1) + Labmda(yNm)) / 2;
        //    double dyNm_12 = (yNm_1 - yNm) / (hz * hz);
        //    double FnM = alpha4 * (yNm - T0) / hz;
        //    return -lNm_12 * dyNm_12 - FnM + alpha2 * T0 / hx + 2 * yNm / tau + F(x, z);
        //}

        //private double KNn(double yN_1m, double yNm)
        //{
        //    double lN_12m = (Labmda(yN_1m) + Labmda(yNm)) / 2;
        //    return 2 / tau - lN_12m / (hx * hx) + alpha2 / hx;
        //}

        //private double MNn(double yN_1m, double yNm)
        //{
        //    double lN_12m = (Labmda(yN_1m) + Labmda(yNm)) / 2;
        //    return lN_12m / (hx * hx);
        //}

        private double Am(/*double ynm_1, double ynm*/)
        {
            double lnm_12 = Labmda(1);
            return lnm_12 / (hz * hz);
        }

        private double Bm(/*double ynm_1, double ynm, double ynm1*/)
        {
            return Am() + Cm() + 2 / tau;
        }

        private double Cm(/*double ynm, double ynm1*/)
        {
            double lnm12 = Labmda(1);
            return lnm12 / (hz * hz);
        }

        private double Dm(double yn_1m, double ynm, double yn1m, double x, double z)
        {
            double ln12m = (Labmda(yn1m) + Labmda(ynm)) / 2;
            double ln_12m = (Labmda(yn_1m) + Labmda(ynm)) / 2;
            double dyn_12m = (yn_1m - ynm) / (hx * hx);
            double dyn12m = (ynm - yn1m) / (hx * hx);
            return 2 * ynm / tau + ln12m * (yn_1m - 2 * ynm + yn1m) / (hx * hx) + F(x, z);
            //return 2 * ynm / tau - ln_12m * dyn_12m + ln12m * dyn12m + F(x, z);
        }
        //private double Dm0(double _, double ynm, double yn1m, double x, double z)
        //{
        //    double ln12m = (Labmda(yn1m) + Labmda(ynm)) / 2;
        //    double dyn12m = (ynm - yn1m) / (hx * hx);
        //    double Fm0 = F0 / hz;
        //    return 2 * ynm / tau + Fm0 + ln12m * dyn12m + F(x, z);
        //}

        //private double DmN(double yn_1m, double ynm, double _, double x, double z)
        //{
        //    double ln_12m = (Labmda(yn_1m) + Labmda(ynm)) / 2;
        //    double dyn_12m = (yn_1m - ynm) / (hx * hx);
        //    double FmN = alpha2 * (ynm - T0) / hx;
        //    return 2 * ynm / tau - ln_12m * dyn_12m - FmN + F(x, z);
        //}

        //private double P0m(double yn_10, double yn0, double yn10, double x, double z)
        //{
        //    double ln_120 = (Labmda(yn_10) + Labmda(yn0)) / 2;
        //    double ln120 = (Labmda(yn0) + Labmda(yn10)) / 2;
        //    double dyn_120 = (yn_10 - yn0) / (hx * hx);
        //    double dyn120 = (yn0 - yn10) / (hx * hx);
        //    return -ln_120 * dyn_120 + ln120 * dyn120 + alpha3 * T0 / hz + 2 * yn0 / tau + F(x, z);
        //}

        //private double P0m0(double _, double yn0, double yn10, double x, double z)
        //{
        //    double ln120 = (Labmda(yn0) + Labmda(yn10)) / 2;
        //    double dyn120 = (yn0 - yn10) / (hx * hx);
        //    double Fm0 = F0 / hx;
        //    return Fm0 + ln120 * dyn120 + alpha3 * T0 / hz + 2 * yn0 / tau + F(x, z);
        //}

        //private double P0mN(double yn_10, double yn0, double _, double x, double z)
        //{
        //    double ln_120 = (Labmda(yn_10) + Labmda(yn0)) / 2;
        //    double dyn_120 = (yn_10 - yn0) / (hx * hx);
        //    double FmN = alpha2 * (yn0 - T0) / hx;
        //    return -ln_120 * dyn_120 - FmN + alpha3 * T0 / hz + 2 * yn0 / tau + F(x, z);
        //}

        //private double K0m(double yn0, double yn1)
        //{
        //    double ln12 = (Labmda(yn0) + Labmda(yn1)) / 2;
        //    return ln12 / (hz * hz);
        //}

        //private double M0m(double yn0, double yn1)
        //{
        //    double ln12 = (Labmda(yn0) + Labmda(yn1)) / 2;
        //    return 2 / tau - ln12 / (hz * hz) + alpha3 / hz;
        //}

        //private double PNm(double yn_1M, double ynM, double yn1M, double x, double z)
        //{
        //    double ln_12M = (Labmda(yn_1M) + Labmda(ynM)) / 2;
        //    double ln12M = (Labmda(ynM) + Labmda(yn1M)) / 2;
        //    double dyn_12M = (yn_1M - ynM) / (hx * hx);
        //    double dyn12M = (ynM - yn1M) / (hx * hx);
        //    return -ln_12M * dyn_12M + ln12M * dyn12M + alpha4 * T0 / hz + 2 * ynM / tau + F(x, z);
        //}

        //private double PNm0(double _, double ynM, double yn1M, double x, double z)
        //{
        //    double ln12M = (Labmda(ynM) + Labmda(yn1M)) / 2;
        //    double dyn12M = (ynM - yn1M) / (hx * hx);
        //    double Fm0 = F0 / hx;
        //    return Fm0 + ln12M * dyn12M + alpha4 * T0 / hz + 2 * ynM / tau + F(x, z);
        //}

        //private double PNmN(double yn_1M, double ynM, double _, double x, double z)
        //{
        //    double ln_12M = (Labmda(yn_1M) + Labmda(ynM)) / 2;
        //    double dyn_12M = (yn_1M - ynM) / (hx * hx);
        //    double FmN = alpha2 * (ynM - T0) / hx;
        //    return -ln_12M * dyn_12M - FmN + alpha4 * T0 / hz + 2 * ynM / tau + F(x, z);
        //}

        //private double KNm(double ynM_1, double ynM)
        //{
        //    double lnM_12 = (Labmda(ynM_1) + Labmda(ynM)) / 2;
        //    return 2 / tau - lnM_12 / (hz * hz) + alpha4 / hz;
        //}

        //private double MNm(double ynM_1, double ynM)
        //{
        //    double lnM_12 = (Labmda(ynM_1) + Labmda(ynM)) / 2;
        //    return lnM_12 / (hz * hz);
        //}

        public double[] RunMethod(double[] a, double[] b, double[] c, double[] d)
        {
            double[] y = new double[b.Length];
            double[] xi = new double[b.Length - 1];
            double[] eta = new double[b.Length - 1];
           // xi[0] = -k0 / m0;// 0
           // eta[0] = p0 / m0;// u0
            xi[0] = 0;// 0
            eta[0] = T0;// u0
            for (int i = 1; i < xi.Length; i++)
            {
                double tmp = b[i] - a[i] * xi[i - 1];
                xi[i] = c[i] / tmp;
                eta[i] = (a[i] * eta[i - 1] + d[i]) / tmp;
            }
            //y[^1] = (pn - mn * eta[^1]) / (kn + mn * xi[^1]); //u0
            y[^1] = T0; //u0
            for (int i = y.Length - 1; i > 0; i--)
            {
                y[i - 1] = xi[i - 1] * y[i] + eta[i - 1];
            }
            return y;
        }

        public double[] NextLayer(double[] ym, double[] ym_1, double[] ym1, double[] x, double[] z,
            Double2Func A, Double2Func B, Double2Func C, Double5Func D)

        {
            int n = ym.Length;
            double[] y = new double[n];
            ym.CopyTo(y, 0);
            double[] erry = new double[n];
            double maxerr;
            int its = 0;
            //do
            //{
                double[] a = new double[n], b = new double[n], c = new double[n], d = new double[n];
                for (int i = 1; i < n - 1; i++)
                {
                    a[i] = A();
                    b[i] = B();
                    c[i] = C();
                    d[i] = D(ym_1[i], ym[i], ym1[i], x[i], z[i]);
                }
                double[] newy = RunMethod(a, b, c, d);

            //    for (int i = 0; i < n; i++)
            //        erry[i] = Math.Abs((newy[i] - y[i]) / newy[i]);
            //    y = newy;
            //    maxerr = erry.Max();
            //    its++;
            //}
            //while (maxerr > 1e-3 && its < 20); // bespolezno
            Console.WriteLine($"{its}");
            return newy;
        }

        private static double[][] Transpose(double[][] m)
        {
            double[][] mt = new double[m[0].Length][];
            for (int i = 0; i < mt.Length; i++)
                mt[i] = new double[m.Length];
            for (int i = 0; i < m.Length; i++)
                for (int j = 0; j < m[i].Length; j++)
                    mt[j][i] = m[i][j];
            return mt;
        }

        private static double[] Double2Arr(double a, int size)
        {
            double[] res = new double[size];
            for (int i = 0; i < size; i++) res[i] = a;
            return res;
        }

        public double[][] NextTime(double[] x, double[] z, double[][] y_)
        {
            int n = x.Length, m = z.Length;
            double[][] midy = new double[m][];
            midy[0] = new double[n];
            for (int i = 0; i < n;i++) 
                midy[0][i] = T0;
            for (int i = 1; i < m - 1; i++)
                midy[i] = NextLayer(y_[i], y_[i - 1], y_[i + 1], x, Double2Arr(z[i], m), An, Bn, Cn, Dn);
            midy[^1] = new double[n];
            for (int i = 0; i < n; i++)
                midy[m - 1][i] = T0;
            midy = Transpose(midy);
            double[][] newy = new double[n][];
            newy[0] = new double[m];
            for (int i = 0; i < m; i++)
                newy[0][i] = T0;
            for (int i = 1; i < n - 1; i++)
                newy[i] = NextLayer(midy[i], midy[i - 1], midy[i + 1], Double2Arr(x[i], n), z, Am, Bm, Cm, Dm);
            newy[^1] = new double[m];
            for (int i = 0; i < m; i++)
                newy[n - 1][i] = T0;
            return Transpose(newy);
        }
    }
}

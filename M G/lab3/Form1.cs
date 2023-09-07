using System;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Model m = new Model(np: 1.4, r0: 0.35, r: 0.5, zN: 1, t0: 300, sigma: 5.668e-12, f0: -10.0, alpha: -0.05, h: 3e-3);
            (double[] x, double[] y) = m.Run();
            for (int i = 0; i < x.Length; i++)
                chart1.Series[0].Points.AddXY(x[i], y[i]);
            //m.alpha = 0.;

            //(x, y) = m.Run();
            //for (int i = 0; i < x.Length; i++)
            //    chart1.Series[1].Points.AddXY(x[i], y[i]);
            System.Console.WriteLine(m.r0 * m.F0);
            System.Console.WriteLine(m.R * m.alpha * (y[y.Length - 1] - m.T0));
            double[] u = new double[x.Length];
            double c1 = m.F0 * m.r0 / Model.Labmda(1500);
            double c2 = m.T0 - Math.Log(m.R) * c1 - Model.Labmda(1500) * c1 / (m.alpha * m.R);
            for (int i = 0; i < x.Length; i++)
            {

                u[i] = -(Math.Log(x[i] * m.R) * c1 + c2);

                System.Console.WriteLine($"{y[i]} {u[i]}");
            }
        }
    }
}

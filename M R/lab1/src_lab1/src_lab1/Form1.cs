using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace src_lab1
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonEqual_Click(object sender, EventArgs e)
        {
            double a = (double)numericUpDownA.Value;
            double b = (double)numericUpDownB.Value;

            if (a >= b)
                MessageBox.Show("Ошибка", "Левая граница интервала (a) должна быть строго меньше правой (b)");

            EqualDistribution distr = new EqualDistribution(a, b);
            distr.buildPlots(chart1, chart2);
        }

        private void buttonPuasson_Click(object sender, EventArgs e)
        {
            int begin = (int)numericUpDownStart.Value;
            int end = (int)numericUpDownEnd.Value;
            double[] lambdas = new double[listBox1.Items.Count];
            double[] ps = new double[listBox2.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                lambdas[i] = (double)listBox1.Items[i];
                ps[i] = (double)listBox2.Items[i];
            }
            if (end <= begin)
                MessageBox.Show("Ошибка", "Левая граница интервала (a) должна быть строго меньше правой (b)");

            var distr = new HyperExpDistribution(lambdas, ps, 1000, begin, end);
            distr.buildPlots(chart1, chart2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double lambda = (double)numericUpDownLambda.Value;
            double p = (double)numericUpDown1.Value;
            listBox1.Items.Add(lambda);
            listBox2.Items.Add(p);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox2.SelectedIndex;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = listBox1.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            listBox1.Items?.RemoveAt(i);
            listBox2.Items?.RemoveAt(i);
        }
    }

    public class EqualDistribution
    {
        private double a;
        private double b;
        private double p;

        public EqualDistribution(double a, double b)
        {
            this.a = a;
            this.b = b;
            this.p = 1 / (b - a);

        }

        private  double f(double x)
        {
            if ((x < a) || (x > b))
                return 0;
            else
                return p;
        }

        private double F(double x)
        {
            if (x < a)
                return 0;
            else if (x < b)
                return (x - a) * p;
            else
                return 1;
        }
        private void prepareAxis(Chart chartDistr, Chart chartDens)
        {
            chartDistr.Series[0].Points.Clear();
            chartDistr.Series[0].ChartType = SeriesChartType.Line;
            chartDistr.Series[0].BorderWidth = 3;
            chartDistr.Titles.Clear();
            chartDistr.Titles.Add("Функция распределения (равномерное распределение)");
            Axis ax = new Axis();
            ax.Title = "x";
            chartDistr.ChartAreas[0].AxisX = ax;
            Axis ay = new Axis();
            ay.Title = "F(x)";
            chartDistr.ChartAreas[0].AxisY = ay;


            chartDens.Series[0].Points.Clear();
            chartDens.Series[0].ChartType = SeriesChartType.Line;
            chartDens.Series[0].BorderWidth = 3;
            chartDens.Titles.Clear();
            chartDens.Titles.Add("Функция плотности (равномерное распределение)");
            Axis ax2 = new Axis();
            ax2.Title = "x";
            chartDens.ChartAreas[0].AxisX = ax2;
            Axis ay2 = new Axis();
            ay2.Title = "f(x)";
            chartDens.ChartAreas[0].AxisY = ay2;
        }
        public void buildPlots(Chart chartDistr, Chart chartDens, double GraphStep = 1000)
        {
            var range = (b - a) * 2;
            var begin = (a + b - range) / 2;
            var end = (a + b + range) / 2;
            var step = range / GraphStep;

            prepareAxis(chartDistr, chartDens);

            for (double x = begin; x <= end; x += step)
            {
                chartDistr.Series[0].Points.AddXY(x, F(x));
                chartDens.Series[0].Points.AddXY(x, f(x));
            }
        }
    }

    public class HyperExpDistribution
    {
        private double[] lambdas;
        private double[] ps;
        private int n;
        private int begin, end;

        public HyperExpDistribution(double[] lambdas, double[] ps, int n, int begin, int end)
        {
            if (Math.Abs(ps.Sum() - 1) > 1e-3)
                throw new Exception();
            if (lambdas.Length != ps.Length)
                throw new Exception();
            for (int i = 0; i < lambdas.Length; i++)
                if (lambdas[i] * ps[i] < 0)
                    throw new Exception();
            this.lambdas = lambdas;
            this.n = n;
            this.ps = ps;
            this.begin = begin;
            this.end = end;
        }

        public double P(double x)
        {
            double res = 0;
            if (x < 0)
                return res;
            else
                for (int i = 0; i < lambdas.Length; i++)
                    res += lambdas[i] * Math.Exp(-lambdas[i] * x) * ps[i];
            return res;
        }

        public double F(double x)
        {
            if (x < 0)
                return 0;

            double res = 1;
            for (int i = 0; i < lambdas.Length; i++)
                res -= ps[i] * Math.Exp(-lambdas[i] * x);
            return res;
        }
   
        private void prepareAxis(Chart chartDistr, Chart chartDens)
        {
            chartDistr.Series[0].Points.Clear();
            chartDistr.Series[0].ChartType = SeriesChartType.Line;
            chartDistr.Series[0].BorderWidth = 3;
            chartDistr.Titles.Clear();
            chartDistr.Titles.Add("Функция распределения (Гиперэкспонециальное распределение)");
            Axis ax = new Axis();
            ax.Title = "x";
            chartDistr.ChartAreas[0].AxisX = ax;
            Axis ay = new Axis();
            ay.Title = "F(x)";
            chartDistr.ChartAreas[0].AxisY = ay;


            chartDens.Series[0].Points.Clear();
            chartDens.Series[0].ChartType = SeriesChartType.Line;
            chartDens.Series[0].BorderWidth = 3;
            chartDens.Titles.Clear();
            chartDens.Titles.Add("Функция плотности (Гиперэкспонециальное распределение)");
            Axis ax2 = new Axis();
            ax2.Title = "x";
            chartDens.ChartAreas[0].AxisX = ax2;
            Axis ay2 = new Axis();
            ay2.Title = "f(x)";
            chartDens.ChartAreas[0].AxisY = ay2;
        }
        public void buildPlots(Chart chartDistr, Chart chartDens)
        {
            double step = (end - begin) / ((double)n);

            prepareAxis(chartDistr, chartDens);

            for (double x = begin; x <= end; x += step)
            {
                
                chartDistr.Series[0].Points.AddXY(x, F(x));
                chartDens.Series[0].Points.AddXY(x, P(x));
            }
        }
    }
}

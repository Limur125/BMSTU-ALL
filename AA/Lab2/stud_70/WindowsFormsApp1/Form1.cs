using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public void ChartClear(int ser)
        {
            chart1.Series[ser].Points.Clear();
        }
        public void ChartAddPoint(int ser, double x, double y)
        {
            chart1.Series[ser].Points.AddXY(x, y);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}

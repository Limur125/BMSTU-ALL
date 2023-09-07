using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

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

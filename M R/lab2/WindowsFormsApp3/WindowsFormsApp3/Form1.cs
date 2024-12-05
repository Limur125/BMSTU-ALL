using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int Size = (int)sizeofmatrix.Value;
            dataGridView1.RowCount = Size;
            dataGridView1.ColumnCount = Size;
            dataGridView2.DefaultCellStyle.Format = "n5";

            string ind;
            for (int i = 0; i < Size; ++i)
            {
                ind = (i + 1).ToString();
                dataGridView1.Rows[i].HeaderCell.Value = ind;
                dataGridView1.Columns[i].HeaderCell.Value = ind;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Size = (int)sizeofmatrix.Value;
            dataGridView1.RowCount = Size;
            dataGridView1.ColumnCount = Size;
            string ind;
            for (int i = 0; i < Size; ++i)
            {
                ind = (i + 1).ToString();
                dataGridView1.Rows[i].HeaderCell.Value = ind;
                dataGridView1.Columns[i].HeaderCell.Value = ind;
            }
        }

        private void solve_button_Click(object sender, EventArgs e)
        {
            int Size = (int)sizeofmatrix.Value;
            double[,] lambdarray = new double[Size, Size];
            double[,] matrix = new double[Size + 1, Size + 1];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    lambdarray[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == j)
                    {
                        for (int k = 0; k < Size; k++)
                        {
                            if (k != i)
                            {
                                matrix[i,j] -= lambdarray[i,k];
                            }
                        }
                    }
                    else
                    {
                        matrix[i,j] += lambdarray[j,i];
                    }
                }
            }
            int minIndex = Size;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (lambdarray[i,j] != 0)
                    {
                        if (i < minIndex)
                        {
                            minIndex = i;
                        }
                        if (j < minIndex)
                        {
                            minIndex = j;
                        }
                    }
                }
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (lambdarray[i,j] != 0)
                    {
                        matrix[minIndex,i] = 1;
                        matrix[minIndex,j] = 1;
                    }
                }
            }

            matrix[minIndex,Size] = 1;
            
            var res = Solve.gauss(matrix, Size+1);

            dataGridView2.RowCount = Size;
            dataGridView2.ColumnCount = 2;
            dataGridView2.Columns[0].HeaderCell.Value = "Предельные вероятности";
            for (int i = 0; i < Size; i++)
            {
                dataGridView2.Rows[i].Cells[0].Value = res[i];
            }

            double[] start = new double[Size];
            start[0] = 1;
            for (int i = 1; i < Size; i++)
            {
                start[i] = 0;
            }

            var rtime = Solve.calculateTime(Size, res, start, lambdarray);

            dataGridView2.Columns[1].HeaderCell.Value = "Время стабилизации";
            for (int i = 0; i < Size; i++)
            {
                dataGridView2.Rows[i].Cells[1].Value = rtime[i];
            }
        }

        private void sizeofmatrix_ValueChanged(object sender, EventArgs e)
        {

        }

        //Предельная вероятность состояния Si имеет четкий смысл: она показывает 
        //среднее относительное время пребывания системы в этом состоянии
    }
}

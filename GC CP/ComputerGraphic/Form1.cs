using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
namespace ComputerGraphic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label6.Text = trackBar3.Value.ToString() + "%";
            label2.Text = trackBar2.Value.ToString() + " частиц в секунду";
            graphics = CreateGraphics();
            scene.AddObject(new Sphere(new(4, -2, 9), 2f, Color.Red, "sphere_1"));
            comboBox1.Items.Add("sphere_1");
            scene.AddObject(new Sphere(new(0, -1.9f, 4), 0.5f, Color.Blue, "sphere_2"));
            comboBox1.Items.Add("sphere_2");
            scene.AddObject(new Cube(new Vector3[]
            {
                new(-2, 0, 7),
                new(1, 0, 7),
                new(2, -0.5f, 9),
                new(0, 3, 8)
            },
            new int[][]
            {
                new[]{ 0, 3, 1 },
                new[]{ 1, 3, 2 },
                new[]{ 0, 1, 2 },
                new[]{ 0, 2, 3 }
            }, Color.Green, "cube_1"));
            comboBox1.Items.Add("cube_1");
            scene.AddObject(new Cube(new Vector3[]
            {
                new(-0.3f, 2.7f, 3.7f),
                new(0.3f, 2.7f, 3.7f),
                new(0, 2.7f, 4.3f),
                new(-0.3f, 3.9f, 3.7f),
                new(0.3f, 3.9f, 3.7f),
                new(0, 3.9f, 4.3f)
            },
            new int[][]
            {
                new[]{ 3, 5, 4 },
                new[]{ 0, 4, 3 },
                new[]{ 0, 1, 4 },
                new[]{ 1, 5, 4 },
                new[]{ 1, 2, 5 },
                new[]{ 0, 3, 5 },
                new[]{ 0, 5, 2 }
            }, Color.Green, "cube_2"));
            comboBox1.Items.Add("cube_2");
            Render();
        }

        private Scene scene = new();
        private Graphics graphics;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Render();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = trackBar3.Value.ToString() + "%";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar2.Value.ToString() + " частиц в секунду";
        }
        private void Render()
        {
            scene.Render();
            pictureBox1.Image = scene.Bmp;
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {

        }

        private void Key_Down(object sender, KeyPressEventArgs e)
        {

        }

        private void Key_Down(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    scene.Rotate(0.1f, 0, 0);
                    break;
                case Keys.Down:
                    scene.Rotate(-0.1f, 0, 0);
                    break;
                case Keys.Left:
                    scene.Rotate(0, 0.1f, 0);
                    break;
                case Keys.Right:
                    scene.Rotate(0, -0.1f, 0);
                    break;
                case Keys.W:
                    scene.Move(0, 0, -0.1f);
                    break;
                case Keys.S:
                    scene.Move(0, 0, 0.1f);
                    break;
                case Keys.A:
                    scene.Move(0.1f, 0, 0);
                    break;
                case Keys.D:
                    scene.Move(-0.1f, 0, 0);
                    break;
                default:
                    return;
            }
            Render();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var dres = colorDialog1.ShowDialog();
            if (dres == DialogResult.OK)
                DialogColorChange();
        }

        private void DialogColorChange()
        {
            button8.Text = "";
            button8.BackColor = colorDialog1.Color;
            scene.ParticleColor = colorDialog1.Color;
            Render();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Color pc = colorDialog1.Color == Color.Black 
                ? colorDialog1.Color 
                : Color.FromArgb(0xBBBBBB);
            scene.SimulateSmoke(pc, new(0, 0, decimal.ToInt32(numericUpDown11.Value)), decimal.ToSingle(numericUpDown10.Value), trackBar2.Value);
            Render();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //for (int time = 100; time <= 300; time += 100)
            //    for (int i = 10; i <= 50; i += 10)
            //    {
            //        Stopwatch sw = new();
            //        for (int j = 0; j < 10; j++)
            //        {
            //            sw.Start();
            //            scene.SimulateSmoke(Color.FromArgb(0xBBBBBB), new(0, 0, time), 0.15f, i);
            //            Render();
            //            sw.Stop();
            //            Console.WriteLine(sw.ElapsedMilliseconds);
            //            sw.Reset();
            //        }
            //        Console.WriteLine();
            //    }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = (string)comboBox1.SelectedItem;
            float dx, dy, dz;
            try
            {
                dx = Convert.ToSingle(textBox1.Text);
                dy = Convert.ToSingle(textBox2.Text);
                dz = Convert.ToSingle(textBox3.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            scene.Move(Convert.ToSingle(dx), Convert.ToSingle(dy), Convert.ToSingle(dz), name);
            Render();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string name = (string)comboBox1.SelectedItem;
            float ax, ay, az;
            try
            {
                ax = Convert.ToSingle(textBox4.Text);
                ay = Convert.ToSingle(textBox5.Text);
                az = Convert.ToSingle(textBox6.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            scene.Rotate(Convert.ToSingle(ax), Convert.ToSingle(ay), Convert.ToSingle(az), name);
            Render();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using Stream file = openFileDialog1.OpenFile();
            using StreamReader sr = new StreamReader(file);
            string? s = sr.ReadLine();
            string scol = s ?? "#00FFFFFF";
            TypeConverter cc = TypeDescriptor.GetConverter(typeof(Color));
            Color c = (Color)cc.ConvertFromString(scol) ;
            s = sr.ReadLine();
            int n = Convert.ToInt32(s);
            List<Vector3> vec = new List<Vector3>();
            for (int i = 0; i < n; i++)
            {
                s = sr.ReadLine();
                string ss = s ?? "";
                float[] ar = ss.Split(new[] { ' ' }).Select(x => float.Parse(x)).ToArray();
                vec.Add(new Vector3(ar[0], ar[1], ar[2]));
            }
            s = sr.ReadLine();
            n = Convert.ToInt32(s);
            List<int[]> l = new List<int[]>();
            for (int i = 0; i < n; i++)
            {
                s = sr.ReadLine();
                string ss = s ?? "";
                int[] ar = ss.Split(new[] { ' ' }).Select(x => int.Parse(x)).ToArray();
                l.Add(new[] { ar[0], ar[1], ar[2] });
            }
            SceneObject so = new Cube(vec.ToArray(), l.ToArray(), c, openFileDialog1.FileName);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            scene.SetLightIntensity(trackBar3.Value / 100f);
            Render();
        }
    }
}
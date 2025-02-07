using lab2;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Tree tree = new();
        —ходство∆ивотных сходство;
        public Form1()
        {
            InitData();
            сходство = new(tree);
            InitializeComponent();
            dataGridView1.Rows.Add(сходство.животные.Count);
            for (int i = 0; i < сходство.животные.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = сходство.животные[i].Name;
                dataGridView1.Rows[i].Cells[1].Value = "0";
            }
        }

        void InitData()
        {
            using StreamReader reader = new("../../../data.txt");
            string? petLine;
            while ((petLine = reader.ReadLine()) != null)
            {
                var petProps = petLine.Split(';', StringSplitOptions.TrimEntries);
                var pet = new Pet
                {
                    Name = petProps[0],
                    Vacinied = int.Parse(petProps[1]) == 1,
                    Age = uint.Parse(petProps[2]),
                    AvgAge = uint.Parse(petProps[3]),
                    Active = int.Parse(petProps[4]) == 1,
                    Color = ulong.Parse(petProps[5]),
                    Conditions = ulong.Parse(petProps[6]),
                    —ompatibility = ~ulong.Parse(petProps[7]),
                    Size = float.Parse(petProps[8]),
                };
                var type = tree.Types.Where(t => t.Name == petProps[9]).FirstOrDefault();
                if (type == null)
                {
                    type = new() { Name = petProps[9] };
                    tree.Types.Add(type);
                }
                var specie = type.Species.Where(s => s.Name == petProps[10]).FirstOrDefault();
                if (specie == null)
                {
                    specie = new() { Name = petProps[10] };
                    type.Species.Add(specie);
                }
                var breed = specie.Breeds.Where(b => b.Name == petProps[11]).FirstOrDefault();
                if (breed == null)
                {
                    breed = new() { Name = petProps[11] };
                    specie.Breeds.Add(breed);
                }
                pet.Type = type;
                pet.Specie = specie;
                pet.Breed = breed;
                breed.Pets.Add(pet);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                сходство.животные[i].Like = dataGridView1.Rows[i].Cells[1].Value as string ?? "0";
            }
            var res = сходство.Recommend();
            foreach (var pet in res)
            {
                listBox1.Items.Add(pet.Name);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pet = сходство.животные.Where(p => p.Name == listBox1.SelectedItem).First();
            new Form2(pet).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = "0";
            }
            listBox1.Items.Clear();
        }
    }
}

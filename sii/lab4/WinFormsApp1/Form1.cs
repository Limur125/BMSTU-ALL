using lab2;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Tree tree = new();
        public List<Pet> AllPets;
        public —ходство∆ивотных сходство;
        public List<Pet> Recomendations;
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
            AllPets = new(сходство.животные);
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

        public void UpdateData(List<Pet> newPets)
        {
            сходство = new(newPets);
            dataGridView1.Rows.Clear();
            if (newPets.Count == 0)
                return;
            dataGridView1.Rows.Add(newPets.Count);
            for (int i = 0; i < newPets.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = newPets[i].Name;
                dataGridView1.Rows[i].Cells[1].Value = "0";
            }
        }

        public void UpdateRecomendations(List<Pet> newPets)
        {
            listBox1.Items.Clear();
            
            foreach (var pet in newPets)
            {
                listBox1.Items.Add(pet.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                сходство.животные[i].Like = dataGridView1.Rows[i].Cells[1].Value as string ?? "0";
            }
            Recomendations = сходство.Recommend();
            foreach (var pet in Recomendations)
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
            dataGridView1.Rows.Clear();
            
            dataGridView1.Rows.Add(AllPets.Count);
            for (int i = 0; i < AllPets.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = AllPets[i].Name;
                dataGridView1.Rows[i].Cells[1].Value = "0";
            }
            listBox1.Items.Clear();
            сходство = new(AllPets);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form3(this).ShowDialog();
        }
        
        
    }
}

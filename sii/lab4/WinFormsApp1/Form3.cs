using lab2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form3 : Form
    {
        private Tree Tree;
        private Form1 mainForm;

        List<string> Color =
        [
            "черный",
            "белый",
            "оранжевый",
            "зеленый",
            "желтый",
            "коричневый",
            "серый",
            "красный",
            "cиний",
        ];

        List<string> Condition =
        [
            "нужен аквариум",
            "нужна вода 22-26 градусов",
            "подходят для рифов",
            "нужен закрытый аквариум",
            "нужен террариум",
            "ядовитые",
            "нужен переменный температурный режим",
            "нужно несколько особей",
            "нужны ветки",
            "нужен бассейн",
            "нужна клетка",
            "нужны приспособления для клетки",
            "нужно выпускать из клетки",
            "нужны игрушки",
            "нужно расчесывать раз в неделю",
            "нужно расчесывать каждый день",
            "нужно выгуливать",
            "нужно физические упражнения"
        ];

        public Form3(Form1 form1)
        {
            mainForm = form1;
            Tree = mainForm.tree;
            InitializeComponent();
            dataGridView2.Rows.Add(Condition.Count);
            for (int i = 0; i < Condition.Count; i++)
            {
                dataGridView2.Rows[i].Cells[0].Value = Condition[i];
                dataGridView2.Rows[i].Cells[1].Value = "0";
            }

            dataGridView1.Rows.Add(Color.Count);
            for (int i = 0; i < Color.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = Color[i];
                dataGridView1.Rows[i].Cells[1].Value = "0";
            }

            dataGridView5.Rows.Add(Tree.Types.Count);
            int k = 0;
            int j = 0;
            for (int i = 0; i < Tree.Types.Count; i++)
            {
                dataGridView5.Rows[i].Cells[0].Value = Tree.Types[i].Name;
                dataGridView5.Rows[i].Cells[1].Value = "0";
                dataGridView3.Rows.Add(Tree.Types[i].Species.Count);
                foreach (var specie in Tree.Types[i].Species)
                {
                    dataGridView3.Rows[j].Cells[0].Value = specie.Name;
                    dataGridView3.Rows[j].Cells[1].Value = "0";
                    dataGridView4.Rows.Add(specie.Breeds.Count);
                    foreach (var breed in specie.Breeds)
                    {
                        dataGridView4.Rows[k].Cells[0].Value = breed.Name;
                        dataGridView4.Rows[k].Cells[1].Value = "0";
                        k++;
                    }
                    j++;
                }
            }
            radioButton1.Checked = true;
            radioButton6.Checked = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public List<Pet> FilterData(List<Pet> pets)
        {
            return pets.Where(pet =>
            {
                var colorPos = GetFilter(dataGridView1, "+");
                var conditionPos = GetFilter(dataGridView2, "+");
                var typePos = GetFilter(dataGridView5, "+");
                var speciePos = GetFilter(dataGridView3, "+");
                var breedPos = GetFilter(dataGridView4, "+");
                if (!TagsContains(colorPos, pet.Color, Color)
                    || !TagsContains(conditionPos, pet.Conditions, Condition)
                    || !ClassContains(typePos, pet.Type.Name)
                    || !ClassContains(speciePos, pet.Specie.Name)
                    || !ClassContains(breedPos, pet.Breed.Name))
                {
                    return false;
                }
                var colorNeg = GetFilter(dataGridView1, "-");
                var conditionNeg = GetFilter(dataGridView2, "-");
                var typeNeg = GetFilter(dataGridView5, "-");
                var specieNeg = GetFilter(dataGridView3, "-");
                var breedNeg = GetFilter(dataGridView4, "-");
                if (TagsContainsNegative(colorNeg, pet.Color, Color)
                    || TagsContainsNegative(conditionNeg, pet.Conditions, Condition)
                    || ClassContainsNegative(typeNeg, pet.Type.Name)
                    || ClassContainsNegative(specieNeg, pet.Specie.Name)
                    || ClassContainsNegative(breedNeg, pet.Breed.Name))
                {
                    return false;
                }
                if (pet.Age < numericUpDown1.Value || pet.Age > numericUpDown2.Value)
                    return false;
                if (pet.AvgAge < numericUpDown4.Value || pet.AvgAge > numericUpDown3.Value)
                    return false;
                if (pet.Size < (float)numericUpDown6.Value || pet.Size > (float)numericUpDown5.Value)
                    return false;
                if (!radioButton1.Checked)
                {
                    if (!radioButton2.Checked && pet.Active)
                        return false;
                    if (radioButton2.Checked && !pet.Active)
                        return false;
                    if (!radioButton3.Checked && !pet.Active)
                        return false;
                    if (radioButton3.Checked && pet.Active)
                        return false;
                }

                if (!radioButton6.Checked)
                {
                    if (!radioButton5.Checked && pet.Vacinied)
                        return false;
                    if (radioButton5.Checked && !pet.Vacinied)
                        return false;
                    if (!radioButton4.Checked && !pet.Vacinied)
                        return false;
                    if (radioButton4.Checked && pet.Vacinied)
                        return false;
                }
                return true;
            }).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mainForm.сходство.животные != null)
            {
                var oldPets = mainForm.сходство.животные;
                var newPets = FilterData(oldPets);
                mainForm.UpdateData(newPets);
            }
            if (mainForm.Recomendations != null)
            { 
                var oldRecs = mainForm.Recomendations;
                var newRecs = FilterData(oldRecs);
                mainForm.UpdateRecomendations(newRecs);
            }
            Close();
        }

        bool ClassContainsNegative(List<string> classes, string petClass)
        {
            return classes.Contains(petClass);
        }

        bool TagsContainsNegative(List<string> tags, ulong pet, List<string> allTags)
        {
            List<string> petTags = [];
            int i = 0;
            foreach (var tag in allTags)
            {
                if (((pet >> i) & 1) == 1)
                {
                    petTags.Add(tag);
                }
                i++;
            }
            return tags.Any(petTags.Contains);
        }

        bool ClassContains(List<string> classes, string petClass)
        {
            List<string> petClasses = [petClass];
            return classes.All(petClasses.Contains);
        }

        bool TagsContains(List<string> tags, ulong pet, List<string> allTags)
        {
            List<string> petTags = [];
            int i = 0;
            foreach (var tag in allTags)
            {
                if (((pet >> i) & 1) == 1)
                {
                    petTags.Add(tag);
                }
                i++;
            }
            return tags.All(petTags.Contains);
        }

        List<string> GetFilter(DataGridView dgv, string selector)
        {
            List<string> res = [];
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[1].Value as string == selector)
                {
                    res.Add(row.Cells[0].Value as string);
                }
            }
            return res;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lab2;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(Pet pet)
        {
            InitializeComponent();
            NameL.Text = pet.Name;
            VaccineL.Text = pet.Vacinied ? "+" : "-";
            AgeL.Text = pet.Age.ToString();
            MaxAgeL.Text = pet.AvgAge.ToString();
            ActiveL.Text = pet.Active ? "+" : "-";
            ColorL.Text = "";
            for (ulong i = pet.Color, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    ColorL.Text += Color[(int)k] + " ";
                }
            }
            List<string> lines = [];
            for (ulong i = pet.Conditions, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    lines.Add(Condition[(int)k]);
                }
            }
            ConditionsL.Lines = [.. lines];
            TypeL.Text = pet.Type.Name;
            SpecieL.Text = pet.Specie.Name;
            BreedL.Text = pet.Breed.Name;
            SizeL.Text = pet.Size.ToString();
            if (pet.Type.Name == "Млекопитающие")
            {
                label2.Text = "Высота, см";
            }
            else
            {
                label2.Text = "Длина, см";
            }
        }

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
            "вода 22-26 градусов",
            "подходят для рифов",
            "обязательно закрытый аквариум",
            "нужен террариум",
            "ядовитые",
            "разный температурный режим по времени суток",
            "нужно несколько особей",
            "нужны ветки",
            "нужен бассейн",
            "необходима клетка",
            "необходимы приспособления для клетки (жердочки, поилка и пр.)",
            "нужно периодически выпускать из клетки",
            "нужны игрушки",
            "необходимо расчесывать раз в неделю",
            "необходимо расчесывать каждый день",
            "необходимо выгуливать",
            "необходимы физические упражнения"
        ];

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void TypeL_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

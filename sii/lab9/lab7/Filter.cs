using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lab7
{
    public class Filter
    {

        public List<string> Color =
        [
            "черные",
            "белые",
            "оранжевые",
            "зеленые",
            "желтые",
            "коричневые",
            "серые",
            "красные",
            "cиние",
        ];
        public List<string> Condition =
        [
            "нужен аквариум",
            "нужна теплая вода",
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

        public List<KeyValuePair<int, Func<Pet, bool, bool>>> Predicates { get; private set; } = [];

        public void ClearPredicates() => Predicates.Clear();
        public void SetConditionFilter(bool or, bool not, int condI)
        {
            ulong act = not ? 0u : 1u;
            if (or)
                Predicates.Add(new(1, (pet, res) => res || (((pet.Conditions >> condI) & 1) == act)));
            else
                Predicates.Add(new(1, (pet, res) => res && (((pet.Conditions >> condI) & 1) == act)));
        }
        public void SetColorFilter(bool or, bool not, int colI)
        {
            ulong act = not ? 0u : 1u;
            if (or)
                Predicates.Add(new(2, (pet, res) => res || (((pet.Color >> colI) & 1) == act)));
            else
                Predicates.Add(new(2, (pet, res) => res && (((pet.Color >> colI) & 1) == act)));
        }

        public void SetTypeFilter(bool or, bool not, Regex regex)
        {
            if (or)
                Predicates.Add(new(3, (pet, res) => res || (regex.IsMatch(pet.Type.Name.ToLower()) ^ not)));
            else
                Predicates.Add(new(3, (pet, res) => res && (regex.IsMatch(pet.Type.Name.ToLower()) ^ not)));
        }
        public void SetSpecieFilter(bool or, bool not, Regex regex)
        {
            if (or)
                Predicates.Add(new(4, (pet, res) => res || (regex.IsMatch(pet.Specie.Name.ToLower()) ^ not)));
            else
                Predicates.Add(new(4, (pet, res) => res && (regex.IsMatch(pet.Specie.Name.ToLower()) ^ not)));
        }

        public void SetBreedFilter(bool or, bool not, Regex regex)
        {
            if (or)
                Predicates.Add(new(5, (pet, res) => res || (regex.IsMatch(pet.Breed.Name.ToLower()) ^ not)));
            else
                Predicates.Add(new(5, (pet, res) => res && (regex.IsMatch(pet.Breed.Name.ToLower()) ^ not)));
        }

        public void SetSizeFilter(bool or, Predicate<float> predicate)
        {
            if (or)
                Predicates.Add(new(6, (pet, res) => res || predicate(pet.Size)));
            else
                Predicates.Add(new(6, (pet, res) => res && predicate(pet.Size)));
        }

        public void SetAgeFilter(bool or, Predicate<float> predicate)
        {
            if (or)
                Predicates.Add(new(7, (pet, res) => res || predicate(pet.Age)));
            else
                Predicates.Add(new(7, (pet, res) => res && predicate(pet.Age)));
        }

        public void SetAvgAgeFilter(bool or, Predicate<float> predicate)
        {
            if (or)
                Predicates.Add(new(8, (pet, res) => res || predicate(pet.AvgAge)));
            else
                Predicates.Add(new(8, (pet, res) => res && predicate(pet.AvgAge)));
        }
        public void SetActiveFilter(bool or, bool not)
        {
            if (or)
                Predicates.Add(new(9, (pet, res) => res || (pet.Active ^ not)));
            else
                Predicates.Add(new(9, (pet, res) => res && (pet.Active ^ not)));
        }
        public void SetVaccineFilter(bool or, bool not)
        {
            if (or)
                Predicates.Add(new(10, (pet, res) => res || (pet.Vacinied ^ not)));
            else
                Predicates.Add(new(10, (pet, res) => res && (pet.Vacinied ^ not)));
        }
        //        public List<Pet> FilterData(List<Pet> pets)
        //        {
        //            return pets.Where(pet =>
        //            {
        //                var colorPos = GetFilter(dataGridView1, "+");
        //                var conditionPos = GetFilter(dataGridView2, "+");
        //                var typePos = GetFilter(dataGridView5, "+");
        //                var speciePos = GetFilter(dataGridView3, "+");
        //                var breedPos = GetFilter(dataGridView4, "+");
        //                if (!TagsContains(colorPos, pet.Color, Color)
        //                    || !TagsContains(conditionPos, pet.Conditions, Condition)
        //                    || !ClassContains(typePos, pet.Type.Name)
        //                    || !ClassContains(speciePos, pet.Specie.Name)
        //                    || !ClassContains(breedPos, pet.Breed.Name))
        //                {
        //                    return false;
        //                }
        //                var colorNeg = GetFilter(dataGridView1, "-");
        //                var conditionNeg = GetFilter(dataGridView2, "-");
        //                var typeNeg = GetFilter(dataGridView5, "-");
        //                var specieNeg = GetFilter(dataGridView3, "-");
        //                var breedNeg = GetFilter(dataGridView4, "-");
        //                if (TagsContainsNegative(colorNeg, pet.Color, Color)
        //                    || TagsContainsNegative(conditionNeg, pet.Conditions, Condition)
        //                    || ClassContainsNegative(typeNeg, pet.Type.Name)
        //                    || ClassContainsNegative(specieNeg, pet.Specie.Name)
        //                    || ClassContainsNegative(breedNeg, pet.Breed.Name))
        //                {
        //                    return false;
        //                }
        //                if (pet.Age < numericUpDown1.Value || pet.Age > numericUpDown2.Value)
        //                    return false;
        //                if (pet.AvgAge < numericUpDown4.Value || pet.AvgAge > numericUpDown3.Value)
        //                    return false;
        //                if (pet.Size < (float)numericUpDown6.Value || pet.Size > (float)numericUpDown5.Value)
        //                    return false;
        //                if (!radioButton1.Checked)
        //                {
        //                    if (!radioButton2.Checked && pet.Active)
        //                        return false;
        //                    if (radioButton2.Checked && !pet.Active)
        //                        return false;
        //                    if (!radioButton3.Checked && !pet.Active)
        //                        return false;
        //                    if (radioButton3.Checked && pet.Active)
        //                        return false;
        //                }

        //                if (!radioButton6.Checked)
        //                {
        //                    if (!radioButton5.Checked && pet.Vacinied)
        //                        return false;
        //                    if (radioButton5.Checked && !pet.Vacinied)
        //                        return false;
        //                    if (!radioButton4.Checked && !pet.Vacinied)
        //                        return false;
        //                    if (radioButton4.Checked && pet.Vacinied)
        //                        return false;
        //                }
        //                return true;
        //            }).ToList();
        //        }

        //        private void button1_Click(object sender, EventArgs e)
        //        {
        //            if (mainForm.сходство.животные != null)
        //            {
        //                var oldPets = mainForm.сходство.животные;
        //                var newPets = FilterData(oldPets);
        //                mainForm.UpdateData(newPets);
        //            }
        //            if (mainForm.Recomendations != null)
        //            {
        //                var oldRecs = mainForm.Recomendations;
        //                var newRecs = FilterData(oldRecs);
        //                mainForm.UpdateRecomendations(newRecs);
        //            }
        //            Close();
        //        }

        //        bool ClassContainsNegative(List<string> classes, string petClass)
        //        {
        //            return classes.Contains(petClass);
        //        }

        //        bool TagsContainsNegative(List<string> tags, ulong pet, List<string> allTags)
        //        {
        //            List<string> petTags = [];
        //            int i = 0;
        //            foreach (var tag in allTags)
        //            {
        //                if (((pet >> i) & 1) == 1)
        //                {
        //                    petTags.Add(tag);
        //                }
        //                i++;
        //            }
        //            return tags.Any(petTags.Contains);
        //        }

        //        bool ClassContains(List<string> classes, string petClass)
        //        {
        //            List<string> petClasses = [petClass];
        //            return classes.All(petClasses.Contains);
        //        }

        //        bool TagsContains(List<string> tags, ulong pet, List<string> allTags)
        //        {
        //            List<string> petTags = [];
        //            int i = 0;
        //            foreach (var tag in allTags)
        //            {
        //                if (((pet >> i) & 1) == 1)
        //                {
        //                    petTags.Add(tag);
        //                }
        //                i++;
        //            }
        //            return tags.All(petTags.Contains);
        //        }

        //        List<string> GetFilter(DataGridView dgv, string selector)
        //        {
        //            List<string> res = [];
        //            foreach (DataGridViewRow row in dgv.Rows)
        //            {
        //                if (row.Cells[1].Value as string == selector)
        //                {
        //                    res.Add(row.Cells[0].Value as string);
        //                }
        //            }
        //            return res;
        //        }
    }
}

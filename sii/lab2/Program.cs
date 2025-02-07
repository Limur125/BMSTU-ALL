using System.IO;

namespace lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tree = new Tree();
            InitData(tree);
            СходствоЖивотных сходство = new(tree);
            float[][] a;
            int[][] b;
            PrintMatrix(сходство.TreeSimilarity);
            PrintMatrix(сходство.TagSimilarity);
            PrintMatrix(сходство.CosineSimilarity);
            PrintMatrix(сходство.Similarity);
        }
        static void InitData(Tree tree)
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
                    Сompatibility = ~ulong.Parse(petProps[7]),
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
        static void PrintMatrix(float[][] m)
        {
            for (int i = 0; i < m.Length; i++)
            {
                for(int j = 0; j < m[i].Length; j++)
                {
                    Console.Write($"\t{m[i][j]}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}

namespace lab7
{
    public class СходствоЖивотных
    {
        Tree tree;
        public List<Pet> animals { get; private set; }
        float[][] treeSimilarity;
        public float[][] TreeSimilarity
        {
            get
            {
                float[][] res = new float[animals.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[animals.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < animals.Count; i++)
                {
                    for (int j = 0; j < animals.Count; j++)
                    {
                        if (animals[i].Type.Name != animals[j].Type.Name)
                            res[i][j] += 1;
                        if (animals[i].Specie.Name != animals[j].Specie.Name)
                            res[i][j] += 1;
                        if (animals[i].Breed.Name != animals[j].Breed.Name)
                            res[i][j] += 1;
                        res[i][j] = 1 / res[i][j];
                    }
                }
                treeSimilarity = res;
                return res;
            }
            private set 
            { 
                treeSimilarity = value; 
            }
        }
        float[][] tagSimilarity;
        public float[][] TagSimilarity
        { 
            get
            {
                var res = new float[animals.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[animals.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < animals.Count; i++)
                {
                    for (int j = 0; j < animals.Count; j++)
                    {
                        ulong cond = ~(animals[i].Conditions ^ animals[j].Conditions);
                        int condK = 0;
                        for (; cond > 0; cond >>= 1) 
                            condK += (int)(cond & 1);

                        ulong color = ~(animals[i].Color ^ animals[j].Color);
                        int colorK = 0;
                        for (; color > 0; color >>= 1)
                            colorK += (int)(color & 1);
                        res[i][j] = colorK + condK;
                    }
                }
                tagSimilarity = res;
                return tagSimilarity;
            }
            private set
            {
                tagSimilarity = value;
            }
        }

        float[][] cosineSimilarity;
        public float[][] CosineSimilarity
        { 
            get
            {
                var res = new float[animals.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[animals.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < animals.Count; i++)
                {
                    for (int j = 0; j < animals.Count; j++)
                    {
                        float ijProd = VacineProd(animals[i], animals[j]) + AgeProd(animals[i], animals[j]) + ActiveProd(animals[i], animals[j]) + SizeProd(animals[i], animals[j]);
                        float iiProd = VacineProd(animals[i], animals[i]) + AgeProd(animals[i], animals[i]) + ActiveProd(animals[i], animals[i]) + SizeProd(animals[i], animals[i]);
                        float jjProd = VacineProd(animals[j], animals[j]) + AgeProd(animals[j], animals[j]) + ActiveProd(animals[j], animals[j]) + SizeProd(animals[j], animals[j]);
                        res[i][j] = ijProd / (float)(Math.Sqrt(iiProd * jjProd));
                    }
                }
                cosineSimilarity = res;
                return cosineSimilarity;
            }
            private set
            {
                cosineSimilarity = value;
            }
        }

        float[][] similarity;
        public float[][] Similarity
        {
            get
            {
                var res = new float[animals.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[animals.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < animals.Count; i++)
                {
                    for (int j = 0; j < animals.Count; j++)
                    {
                        res[i][j] = TreeSimilarity[i][j] * TagSimilarity[i][j] * CosineSimilarity[i][j];
                    }
                }
                similarity = res;
                return similarity;
            }
            private set
            {
                similarity = value;
            }
        }

        float VacineProd (Pet x, Pet y)
        {
            return (x.Vacinied ? 1 : 0) * (y.Vacinied ? 1 : 0);
        }
        float AgeProd(Pet x, Pet y)
        {
            return ((float)x.Age / x.AvgAge) * ((float)y.Age / y.AvgAge);
        }
        float ActiveProd(Pet x, Pet y)
        {
            return (x.Active ? 1 : 0) * (y.Active ? 1 : 0);
        }
        float SizeProd(Pet x, Pet y)
        {
            return x.Size * y.Size;
        }

        public СходствоЖивотных(List<Pet> pets)
        {
            foreach (var pet in pets)
            {
                tree = new Tree();
                var type = tree.Types.Where(t => t.Name == pet.Type.Name).FirstOrDefault();
                if (type == null)
                {
                    type = new() { Name = pet.Type.Name };
                    tree.Types.Add(type);
                }
                var specie = type.Species.Where(s => s.Name == pet.Specie.Name).FirstOrDefault();
                if (specie == null)
                {
                    specie = new() { Name = pet.Specie.Name };
                    type.Species.Add(specie);
                }
                var breed = specie.Breeds.Where(b => b.Name == pet.Breed.Name).FirstOrDefault();
                if (breed == null)
                {
                    breed = new() { Name = pet.Breed.Name };
                    specie.Breeds.Add(breed);
                }
                pet.Type = type;
                pet.Specie = specie;
                pet.Breed = breed;
                breed.Pets.Add(pet);
            }
            animals = pets;
        }

        public СходствоЖивотных(Tree деревоЖивотных)
        {
            tree = деревоЖивотных;
            animals = [];
            foreach (var type in tree.Types)
                foreach(var specie in type.Species)
                    foreach(var breed in specie.Breeds)
                        foreach(var pet in breed.Pets)
                            animals.Add(pet);
        }

        public List<Pet> Recommend()
        {
            Dictionary<Pet, float> res = [];
            List<Pet> liked = [];
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals[i].Like == "+")
                {
                    var sim = new List<float>(Similarity[i]);
                    var petsCopy = new List<Pet>(animals);
                    for (int j = 0; j < 10 && sim.Count > 0; j++)
                    {
                        int ind = sim.IndexOf(sim.Max());
                        if (!res.TryAdd(petsCopy[ind], sim[ind]))
                        {
                            res[petsCopy[ind]] += sim[ind];
                        }
                        petsCopy.RemoveAt(ind);
                        sim.RemoveAt(ind);
                    }
                    liked.Add(animals[i]);
                }
                else if (animals[i].Like == "-")
                {
                    var sim = new List<float>(Similarity[i]);
                    var petsCopy = new List<Pet>(animals);
                    for (int j = 0; j < 3 && sim.Count > 0; j++)
                    {
                        int ind = sim.IndexOf(sim.Max());
                        if (!res.TryAdd(petsCopy[ind], -sim[ind]))
                        {
                            res[petsCopy[ind]] -= sim[ind];
                        }
                        petsCopy.RemoveAt(ind);
                        sim.RemoveAt(ind);
                    }
                    for (int j = 0; j < 10 && sim.Count > 0; j++)
                    {
                        int ind = sim.IndexOf(sim.Min());
                        if (!res.TryAdd(petsCopy[ind], sim[ind]))
                        {
                            res[petsCopy[ind]] += sim[ind];
                        }
                        petsCopy.RemoveAt(ind);
                        sim.RemoveAt(ind);
                    }
                    liked.Add(animals[i]);
                }
            }
            foreach (var pet in liked)
            {
                res.Remove(pet);
            }
            var ordered = res.Where(x => x.Value > 0).OrderByDescending(x => x.Value).Take(15);
            List<Pet> resPets = [];
            foreach (var pet in ordered)
            {
                resPets.Add(pet.Key);
            }
            return resPets;
        }
    }
}

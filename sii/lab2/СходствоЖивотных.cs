using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class СходствоЖивотных
    {
        Tree дерево;
        List<Pet> животные;
        float[][] treeSimilarity;
        public float[][] TreeSimilarity
        {
            get
            {
                float[][] res = new float[животные.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[животные.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < животные.Count; i++)
                {
                    for (int j = 0; j < животные.Count; j++)
                    {
                        if (животные[i].Type.Name != животные[j].Type.Name)
                            res[i][j] += 1;
                        if (животные[i].Specie.Name != животные[j].Specie.Name)
                            res[i][j] += 1;
                        if (животные[i].Breed.Name != животные[j].Breed.Name)
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
                var res = new float[животные.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[животные.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < животные.Count; i++)
                {
                    for (int j = 0; j < животные.Count; j++)
                    {
                        ulong cond = ~(животные[i].Conditions ^ животные[j].Conditions);
                        int condK = 0;
                        for (; cond > 0; cond >>= 1) 
                            condK += (int)(cond & 1);

                        ulong color = ~(животные[i].Color ^ животные[j].Color);
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
                var res = new float[животные.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[животные.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < животные.Count; i++)
                {
                    for (int j = 0; j < животные.Count; j++)
                    {
                        float ijProd = VacineProd(животные[i], животные[j]) + AgeProd(животные[i], животные[j]) + ActiveProd(животные[i], животные[j]) + SizeProd(животные[i], животные[j]);
                        float iiProd = VacineProd(животные[i], животные[i]) + AgeProd(животные[i], животные[i]) + ActiveProd(животные[i], животные[i]) + SizeProd(животные[i], животные[i]);
                        float jjProd = VacineProd(животные[j], животные[j]) + AgeProd(животные[j], животные[j]) + ActiveProd(животные[j], животные[j]) + SizeProd(животные[j], животные[j]);
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
                var res = new float[животные.Count][];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new float[животные.Count];
                    res[i].Initialize();
                }
                for (int i = 0; i < животные.Count; i++)
                {
                    for (int j = 0; j < животные.Count; j++)
                    {
                        res[i][j] = treeSimilarity[i][j] * tagSimilarity[i][j] * cosineSimilarity[i][j];
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

        public СходствоЖивотных(Tree деревоЖивотных)
        {
            дерево = деревоЖивотных;
            животные = [];
            foreach (var type in дерево.Types)
                foreach(var specie in type.Species)
                    foreach(var breed in specie.Breeds)
                        foreach(var pet in breed.Pets)
                            животные.Add(pet);
        }
    }
}

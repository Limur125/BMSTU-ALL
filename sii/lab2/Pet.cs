using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Pet
    {
        public string Name { get; set; }
        public bool Vacinied { get; set; }
        public uint Age { get; set; } 
        public uint AvgAge { get; set; }
        public bool Active { get; set; }
        public ulong Conditions { get; set; }
        public ulong Color { get; set; }
        public ulong Сompatibility { get; set; }
        public float Size { get; set; }
        public Specie Specie { get; set; }
        public Breed Breed { get; set; }
        public Type Type { get; set; }
    }
    enum Color : ulong
    {
        Black = 1 << 0,
        White = 1 << 1,
        Orange = 1 << 2,
        Green = 1 << 3,
        Yellow = 1 << 4,
        Brown = 1 << 5,
        Gray = 1 << 6,
        Red = 1 << 7,
    }

    public class Breed
    {
        public List<Pet> Pets = new();
        public string Name { get; set; }
    }

    public class Specie
    {
        public List<Breed> Breeds = new();
        public string Name { get; set; }
    }

    public class Type
    {
        public List<Specie> Species = new();
        public string Name { get; set; }
    }


    public class Tree
    {
        public List<Type> Types { get; } = new();
    }
}


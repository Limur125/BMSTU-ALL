using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab7
{
    public class Pet
    {
        public string Name { get; set; } = "";
        public bool Vacinied { get; set; } = false;
        public uint Age { get; set; } = 0;
        public uint AvgAge { get; set; } = 0; 
        public bool Active { get; set; } = false;
        public ulong Conditions { get; set; } = 0;
        public ulong Color { get; set; } = 0;
        public ulong Сompatibility { get; set; } = 0;
        public float Size { get; set; } = 0;
        public Specie Specie { get; set; } = new Specie();
        public Breed Breed { get; set; } = new Breed();
        public Type Type { get; set; } = new Type();
        public string Like { get; set; } = "0";
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


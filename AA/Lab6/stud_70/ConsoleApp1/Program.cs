using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Lab6
{
    class Program
    {
        static Map[] maps = new Map[3];
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Write("Меню:\n" +
                    "\t 0. Выход.\n" +
                    "\t 1. Тестирование работы муравьиного алгоритма.\n" +
                    "\t 2. Параметризация.\n" +
                    "\t 3. Сравнение алгоритмов ПП и муравьиного.\n" +
                    "\t 4. Демонстрация работы муравьиного алгоритма.\n" +
                    "\t Выбор: ");
                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    choice = -1;
                }
                if (choice == 0)
                    exit = true;
                if (choice == 1)
                {
                    Console.WriteLine("TEST");
                    Map test1 = new Map(new int[4, 4] { { 0, 1, 10, 7 }, { 1, 0, 1, 2 }, { 10, 1, 0, 1 }, { 7, 2, 1, 0 } }, 4);
                    Path bf1 = BruteForce.GetRoute(test1);
                    Path res1 = AntAlgorithm.GetRoute(test1, 30, 0.5, 0.5, bf1.N, 0.1);
                    Console.Write(res1.N + "\t");
                    res1.Print();
                    Console.Write(bf1.N + "\t");
                    bf1.Print();
                    Console.WriteLine();
                    Map test2 = new Map(new int[4, 4] { { 0, 7, 5, 7 }, { 7, 0, 1, 2 }, { 5, 1, 0, 1 }, { 7, 2, 1, 0 } }, 4);
                    Path bf2 = BruteForce.GetRoute(test2);
                    Path res2 = AntAlgorithm.GetRoute(test2, 3, 0.9, 0.1, bf2.N, 0.1);
                    Console.Write(res2.N + "\t");
                    res2.Print();
                    Console.Write(bf2.N + "\t");
                    bf2.Print();
                    Console.WriteLine();
                    Map test3 = new Map(new int[,] { { 0, 3, 5 }, { 3, 0, 1 }, { 5, 1, 0 } }, 3);
                    Path bf3 = BruteForce.GetRoute(test3);
                    Path res3 = AntAlgorithm.GetRoute(test3, 3, 0.9, 0.1, bf3.N, 0.1);
                    Console.Write(res3.N + "\t");
                    res3.Print();
                    Console.Write(bf3.N + "\t");
                    bf3.Print();
                }
                else if (choice == 2)
                {
                    Console.WriteLine("TASK");
                    CollectData();
                    double[] alpha = new double[] { .1, .25, .5, .75, .9 };
                    int[] lifeTime = new int[] { 100, 200, 500, 1000, 2000 };
                    foreach (int lt in lifeTime)
                        foreach (double a in alpha)
                            foreach (double r in alpha)
                            {
                                List<int> sigma = new List<int>();
                                for (int j = 0; j < 10; j++)
                                {
                                    sigma.Add(AntAlgorithm.GetRoute(maps[0], lt, a, 1 - a, maps[0].BestDistance, r).N - maps[0].BestDistance);
                                    sigma.Add(AntAlgorithm.GetRoute(maps[1], lt, a, 1 - a, maps[1].BestDistance, r).N - maps[1].BestDistance);
                                    sigma.Add(AntAlgorithm.GetRoute(maps[2], lt, a, 1 - a, maps[2].BestDistance, r).N - maps[2].BestDistance);
                                }
                                sigma.Sort();

                                Console.WriteLine($"{lt}\t\t{a}\t\t{r}\t\t{sigma[sigma.Count - 1]}\t\t{sigma[sigma.Count / 2]}");
                            }
                }
                else if(choice == 3)
                {
                    double count = 20;
                    int[] ns = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                    Process process = Process.GetCurrentProcess();
                    foreach (int n in ns)
                    {
                        double timeResBF = 0;
                        double timeResAA = 0;
                        
                        for (int i = 0; i < count; i++)
                        {
                            Map m = new Map(Generator(n), n);
                            TimeSpan start = process.TotalProcessorTime;
                            Path path = BruteForce.GetRoute(m);
                            timeResBF += (process.TotalProcessorTime - start).TotalMilliseconds;
                            start = process.TotalProcessorTime;
                            AntAlgorithm.GetRoute(m, 500, 0.5, 0.5, path.N, 0.5);
                            timeResAA += (process.TotalProcessorTime - start).TotalMilliseconds;
                        }
                        Console.WriteLine($"{n}\t{timeResAA / count, 10} {timeResBF / count, 10}");
                    }

                }
                else if (choice == 4)
                {
                    Console.Write("Введите имя файла с матрицей смежности: ");
                    string fileName = Console.ReadLine();
                    using (StreamReader streamReader = new StreamReader(File.OpenRead(fileName)))
                    {
                        int n = int.Parse(streamReader.ReadLine());
                        int[,] m = new int[n, n];
                        for (int i = 0; i < n; i++)
                        {
                            int[] ar = streamReader.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                            if (ar.Length != n)
                                throw new Exception("ERROR N");
                            for (int j = 0; j < n; j++)
                                m[i, j] = ar[j];
                        }
                        Map map = new Map(m, n);
                        Path bf = BruteForce.GetRoute(map);
                        Console.Write("Полный перебор\t\t" + bf.N + "\t");
                        bf.Print();
                        Console.WriteLine();
                        Path aa = AntAlgorithm.GetRoute(map, 500, 0.5, 0.5, bf.N, 0.5);
                        Console.Write("Муравьиный алгоритм\t" + aa.N + "\t");
                        aa.Print();
                        //int bst = int.Parse(streamReader.ReadLine());
                        //maps[0] = new Map(m, n, bst);
                    }
                }
            }
        }
        static void CollectData()
        {
            using (StreamReader streamReader = new StreamReader(File.OpenRead(@"..\..\1.txt")))
            {
                int n = int.Parse(streamReader.ReadLine());
                int[,] m = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    int[] ar = streamReader.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                    if (ar.Length != n)
                        throw new Exception("ERROR 1N");
                    for (int j = 0; j < n; j++)
                        m[i, j] = ar[j];
                }
                //Path bf = BruteForce.GetRoute(new Map(m, n));
                //Console.Write(bf.N + "\t");
                //bf.Print();
                int bst = int.Parse(streamReader.ReadLine());
                maps[0] = new Map(m, n, bst);
            }
            using (StreamReader streamReader = new StreamReader(File.OpenRead(@"..\..\2.txt")))
            {
                int n = int.Parse(streamReader.ReadLine());
                int[,] m = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    int[] ar = streamReader.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                    if (ar.Length != n)
                        throw new Exception("ERROR 2N");
                    for (int j = 0; j < n; j++)
                        m[i, j] = ar[j];
                }
                //Path bf = BruteForce.GetRoute(new Map(m, n));
                //Console.Write(bf.N + "\t");
                //bf.Print();
                int bst = int.Parse(streamReader.ReadLine());
                maps[1] = new Map(m, n, bst);
            }
            using (StreamReader streamReader = new StreamReader(File.OpenRead(@"..\..\3.txt")))
            {
                int n = int.Parse(streamReader.ReadLine());
                int[,] m = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    int[] ar = streamReader.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                    if (ar.Length != n)
                        throw new Exception("ERROR 3N");
                    for (int j = 0; j < n; j++)
                        m[i, j] = ar[j];
                }
                //Path bf = BruteForce.GetRoute(new Map(m, n));
                //Console.Write(bf.N + "\t");
                //bf.Print();
                int bst = int.Parse(streamReader.ReadLine());
                maps[2] = new Map(m, n, bst);
            }
        }
        static int[,] Generator(int len)
        {
            int[,] res = new int[len, len];
            Random rnd = new Random();
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                    res[i, j] = rnd.Next(2900) + 100;
            return res;
        }
    }
}

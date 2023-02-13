using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Program
    {
        static Form1 f;
        readonly static int count = 100;
        delegate int[] ArrayGenerator(int n);
        readonly static int[] arrayLengths = {1, 5, 10, 50, 100, 500, 1000, 2000, 2500};
        public const string clock_func = @"C:\zolot\AA\Lab3\stud_70\WindowsFormsApp1\Project1.dll";
        [DllImport(clock_func, CallingConvention = CallingConvention.Cdecl)]
        public static extern double mclock();
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f = new Form1();
            bool exit = false;
            while (!exit)
            {
                f = new Form1();
                Console.Write("Меню:" +
                    "0.Выход\n" +
                    "\t 1. Сортировка пузырьком.\n" +
                    "\t 2. Быстрая сортировка\n" +
                    "\t 3. Сортировка расческой\n" +
                    "\t 4. Замер времени на отсортированных массивах (длина от 1 до 2500)\n" +
                    "\t 5. Замер времени на обратно отсортированных массивах (длина от 1 до 2500)\n" +
                    "\t 6. Замер времени на случайных массивах (длина от 1 до 2500)\n" +
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
                else if (choice > 0 && choice < 4)
                {
                    int[] ar;
                    try
                    {
                        Console.Write("Введите элементы массива (целые): ");
                        ar = Console.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                    }
                    catch (InvalidCastException e)
                    {
                        e.ToString();
                        continue;
                    }
                    switch (choice)
                    {
                        case 1:
                            CalculateAlgo(ar, new BubbleSort<int>((int a1, int a2) => a1 - a2));
                            break;
                        case 2:
                            CalculateAlgo(ar, new QuickSort<int>((int a1, int a2) => a1 - a2));
                            break;
                        case 3:
                            CalculateAlgo(ar, new CombSort<int>((int a1, int a2) => a1 - a2));
                            break;
                    }
                }
                else if (choice > 3 && choice < 7)
                {
                    ArrayGenerator ar_gen = SortedGen;
                    f.ChartClear(0);
                    f.ChartClear(1);
                    f.ChartClear(2);
                    Console.Write("\t\t");
                    foreach (int len in arrayLengths)
                        Console.Write($"{len, 13}");
                    switch (choice)
                    {
                        case 4:
                            ar_gen = SortedGen;
                            break;
                        case 5:
                            ar_gen = ReverseGen;
                            break;
                        case 6:
                            ar_gen = RandomGen;
                            break;
                    }
                    Console.Write("\nBubbleSort\t");
                    TimeAnalysis(new BubbleSort<int>((int a1, int a2) => a1 - a2), ar_gen, 0);
                    Console.Write("\nQuickSort\t");
                    TimeAnalysis(new QuickSort<int>((int a1, int a2) => a1 - a2), ar_gen, 1);
                    Console.Write("\nCombSort\t");
                    TimeAnalysis(new CombSort<int>((int a1, int a2) => a1 - a2), ar_gen, 2);
                    Console.WriteLine();
                    Application.Run(f);
                }
            }
            return 0;
        }
        static void CalculateAlgo(int[] ar, BaseSort<int> algo)
        {
            algo.Sort(ar);
            Console.Write("\nРезультат:");
            foreach (var i in ar)
                Console.Write(i + " ");
            Console.WriteLine();
        }

        static void TimeAnalysis(BaseSort<int> a, ArrayGenerator gen, int ser)
        {
            foreach (int len in arrayLengths)
            {
                int[] ar1 = gen(len);
                var ar = new int[len];
                
                var s = mclock();
                for (int i = 0; i < count; i++)
                {
                    ar1.CopyTo(ar, 0);
                    a.Sort(ar);
                }
                var e = mclock();
                var res = (e - s) / count;
                f.ChartAddPoint(ser, len, res);
                Console.Write($"{res, 13}");
            }
        }

        static int[] SortedGen(int len)
        {
            int[] ar = new int[len];
            for (int i = 0; i < len; i++)
                ar[i] = i;
            return ar;
        }

        static int[] ReverseGen(int len)
        {
            int[] ar = new int[len];
            for (int i = 0; i < len; i++)
                ar[i] = len - i;
            return ar;
        }

        static int[] RandomGen(int len)
        {
            int[] ar = new int[len];
            Random r = new Random();
            for (int i = 0; i < len; i++)
                ar[i] = r.Next();
            return ar;
        }

    }

}

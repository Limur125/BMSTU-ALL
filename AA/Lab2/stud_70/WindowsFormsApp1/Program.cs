using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;

namespace WindowsFormsApp1
{
    public partial class Program
    {
        static Form1 f;
        readonly static int count = 50;
        readonly static int[] arrayLengths = new int[10];
        public const string clock_func = @"C:\zolot\AA\Lab2\stud_70\WindowsFormsApp1\Project1.dll";
        [DllImport(clock_func, CallingConvention = CallingConvention.Cdecl)]
        public static extern double mclock();
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool exit = false;
            while (!exit)
            {
                f = new Form1();
                Console.Write("Меню:" +
                    "0.Выход\n" +
                    "\t 1. Классическое умножение.\n" +
                    "\t 2. Алгоритм уножения по Винограду.\n" +
                    "\t 3. Оптимизированный алгоритм умножения по Винограду.\n" +
                    "\t 4. Замер времени умножения квадратных матриц (размерности от 100 до 1000 с шагом 100)\n" +
                    "\t 5. Замер времени умножения квадратных матриц (размерности от 101 до 1001 с шагом 100)\n" +
                    "\t Выбор: ");
                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    choice = -1;
                }
                if (choice == 0)
                    exit = true;
                else if (choice > 0 && choice < 4)
                {
                    int m, n, q;
                    int[][] A, B;
                    try
                    {
                        Console.Write("Введите размеры первой матрицы: ");
                        int[] ar;
                        ar = Console.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                        if (ar.Length != 2)
                            throw new Exception("Неверное количество размеров");
                        m = ar[0];
                        n = ar[1];

                        Console.Write("Введите размеры второй матрицы: ");
                        ar = Console.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                        if (ar.Length != 2)
                            throw new Exception("Неверное количество размеров");
                        if (ar[0] != n)
                            throw new Exception("Размеры матриц не совпадают");
                        q = ar[1];

                        Console.WriteLine("Введите элементы первой матрицы");
                        A = new int[m][];
                        for (int i = 0; i < m; i++)
                        {
                            A[i] = Console.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                            if (A[i].Length != n)
                                throw new Exception("Неверное количество элементов в строке");
                        }
                        Console.WriteLine("Введите элементы второй матрицы");
                        B = new int[n][];
                        for (int i = 0; i < n; i++)
                        {
                            B[i] = Console.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                            if (B[i].Length != q)
                                throw new Exception("Неверное количество элементов в строке");
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                    switch (choice)
                    {
                        case 1:
                            Demonstrate(new Classic(), A, B);
                            break;
                        case 2:
                            Demonstrate(new Vinograd(), A, B);
                            break;
                        case 3:
                            Demonstrate(new OptimizedVinograd(), A, B);
                            break;
                    }
                }
                else if (choice == 4 || choice == 5)
                {
                    if (choice == 4)
                        arrayLengths[0] = 100;
                    else
                        arrayLengths[0] = 101;
                    for (int i = 1; i < arrayLengths.Length; i++)
                        arrayLengths[i] = arrayLengths[i - 1] + 100;
                    f.ChartClear(0);
                    f.ChartClear(1);
                    f.ChartClear(2);
                    Console.Write("\t\t");
                    foreach (int len in arrayLengths)
                        Console.Write($"{len,13}");
                    Console.Write("\nClassic\t\t");
                    TimeAnalysis(new Classic(), 0);
                    Console.Write("\nVinograd\t");
                    TimeAnalysis(new Vinograd(), 1);
                    Console.Write("\nOptVinograd\t");
                    TimeAnalysis(new OptimizedVinograd(), 2);
                    Console.WriteLine();
                    Application.Run(f);
                }
            }
            return 0;
        }
        static void Demonstrate(BaseAlgo a, int[][] A, int[][] B)
        {
            int[][] C = a.Multiply(A, B);
            Console.WriteLine("Результат:");
            foreach (var line in C)
            {
                foreach (var el in line)
                    Console.Write($"{el} ");
                Console.WriteLine();
            }
        }
        static void TimeAnalysis(BaseAlgo a, int ser)
        {
            foreach (int len in arrayLengths)
            {
                int[][] A = Generator(len);
                int[][] B = Generator(len);

                var s = mclock();
                for (int i = 0; i < count; i++)
                {
                    a.Multiply(A, B);
                }
                var e = mclock();
                var res = (e - s) / count;
                f.ChartAddPoint(ser, len, res);
                Console.Write($"{res,13}");
            }
        }
        static int[][] Generator(int len)
        {
            int[][] res = new int[len][];
            Random rnd = new Random();
            for (int i = 0; i < len; i++)
            {
                res[i] = new int[len];
                for (int j = 0; j < len; j++)
                    res[i][j] = rnd.Next(1000);
            }
            return res;
        }
    }
}

using ConsoleApp1;
using System.Diagnostics;
using System.Runtime.InteropServices;

internal class Program
{
    readonly static int count = 100;
    static Process ps = Process.GetCurrentProcess();
    public const string clock_func = @"C:\zolot\AA\Lab1\stud_70\ConsoleApp1\Project1.dll";
    [DllImport(clock_func, CallingConvention = CallingConvention.Cdecl)]
    public static extern double mclock();
    [DllImport(clock_func, CallingConvention = CallingConvention.Cdecl)]
    public static extern double getCPUTime();

    private static int Main(string[] args)
    {
        Console.WriteLine(ps.ProcessName);
        bool exit = false;
        while (!exit)
        {
            Console.Write("Меню:" +
                "0.Выход\n" +
                "\t 1. Расстояние Левенштейна нерекурсивно\n" +
                "\t 2. Расстояние Дамерау-Левенштейна матрица\n" +
                "\t 3. Расстояние Дамерау-Левенштейна рекурсивно (кеш)\n" +
                "\t 4. Расстояние Дамерау-Левенштейна рекурсивно\n" +
                "\t 5. Замер времени (длина слов от 1 до 10)\n" +
                "\t Выбор: ");
            int choice;
            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (InvalidCastException e)
            {
                e.ToString();
                return 1;
            }
            if (choice == 0)
                exit = true;
            else if (choice > 0 && choice < 3)
            {
                int a;
                switch (choice)
                {
                    case 1:
                        a = CalculateAlgo(new LevensteinAlgo());
                        Console.WriteLine($"рассчитанное по алгоритму Левенштейна равно {a}");
                        break;
                    case 2:
                        a = CalculateAlgo(new DamLevAlgo());
                        Console.WriteLine($"рассчитанное по алгоритму Дамерау-Левенштейна равно {a}");
                        break;
                }
            }
            else if (choice > 2 && choice < 5)
            {
                int a;
                switch (choice)
                {
                    case 3:
                        a = CalculateRecAlgo(new RecDamLevAlgo());
                        Console.WriteLine($"рассчитанное по рекусрсивному алгоритму Дамерау-Левенштейна равно {a}");
                        break;
                    case 4:
                        a = CalculateRecAlgo(new RecCacheDamLevAlgo());
                        Console.WriteLine($"рассчитанное по рекусрсивному алгоритму с кэшем Дамерау-Левенштейна равно {a}");
                        break;
                }
            }
            else if (choice == 5)
            {
                for (int i = 0; i <= 100; i += 10)
                {
                    Console.WriteLine($"Длина строки {i}");
                    Console.WriteLine("Среднее время работы алгоритма Левенштейна: " +
                        $"{ TimeAnalysis((a, b) => new LevensteinAlgo().DoAlgorithm(a, b), i), 13}");
                    Console.WriteLine("Среднее время работы алгоритма Дамерау-Левенштейна: " +
                        $"{TimeAnalysis((a, b) => new DamLevAlgo().DoAlgorithm(a, b), i),13}");
                    Console.WriteLine("Среднее время работы рекурсивного алгоритма Дамерау-Левенштейна с кэшем: " +
                        TimeAnalysis((a, b) => new RecCacheDamLevAlgo().DoAlgorithm(a, b), i).ToString());
                }
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Длина строки {i}");
                    Console.WriteLine("Среднее время работы рекусрсивного алгоритма Дамерау-Левенштейна: " +
                        TimeAnalysis((a, b) => new RecDamLevAlgo().DoAlgorithm(a, b), i).ToString());
                }
            }
        }

        return 0;
    }
    static string RandomString(int len)
    {
        string s = "";
        Random r = new();
        for (; len > 0; len--)
            s += Convert.ToChar(r.Next('a', 'z'));
        return s;
    }

    static double TimeAnalysis(Action<string, string> act, int strLen)
    {
        var s1 = RandomString(strLen);
        var s2 = RandomString(strLen);
        var s = mclock();
        for (int i = 0; i < count; i++)
        {
            act(s1, s2);
        }
        var e = mclock();
        var res = (e - s) / count;
        return res;
    }

    static int CalculateRecAlgo(BaseRecurAlgo alg)
    {
        Console.Write("Введите первую строку:");
        string fs = Console.ReadLine() ?? "";
        Console.Write("Введите вторую строку:");
        string ss = Console.ReadLine() ?? "";
        int m = alg.DoAlgorithm(fs, ss);
        Console.Write($"Редакционное расстояние между строками {ss} и {fs} ");
        return m;
    }

    static int CalculateAlgo(BaseAlgo alg)
    {
        Console.Write("Введите первую строку:");
        string fs = Console.ReadLine() ?? "";
        Console.Write("Введите вторую строку:");
        string ss = Console.ReadLine() ?? "";
        int[,] m = alg.DoAlgorithm(fs, ss);
        Console.Write("\t\t0");
        foreach (var c in fs)
            Console.Write($"\t{c}");
        Console.WriteLine();
        string nss = "0" + ss;
        for (int i = 0; i < nss.Length; i++)
        {
            Console.Write($"\t{nss[i]}");
            for (int j = 0; j < fs.Length + 1; j++)
                Console.Write($"\t{m[i, j]}");
            Console.WriteLine();
        }
        Console.Write($"Редакционное расстояние между строками {ss} и {fs} ");
        return m[ss.Length, fs.Length];
    }

}
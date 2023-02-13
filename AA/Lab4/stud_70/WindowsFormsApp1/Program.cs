using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComputerGraphic;
using System.Drawing;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        static Form1 app;
        static Scene scene;
        static readonly int count = 5;
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            scene = new Scene();
            bool exit = false;
            while (!exit)
            {
                app = new Form1();
                Console.Write("Меню:\n" +
                    "\t 0.Выход\n" +
                    "\t 1. Демонстрация работы алгоритма.\n" +
                    "\t 2. Сравнение времени последовательного выполнения и однопоточного.\n" +
                    "\t 3. Замер времени на разном количестве потоков. \n" +
                    "\t Выбор: ");
                int choice;
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    choice = -1;
                }
                if (choice == 0)
                    exit = true;
                else if (choice == 1)
                {
                    Console.WriteLine("Введите количество частиц в секунду, размер частиц, время симуляции.");
                    int[] ar;
                    try
                    {
                        ar = Console.ReadLine().Split(new[] { ' ', '\n' }).Select(x => int.Parse(x)).ToArray();
                    }
                    catch (InvalidCastException e)
                    {
                        e.ToString();
                        continue;
                    }
                    if (ar.Length != 3)
                        continue;
                    scene.SimulateSmoke(Color.Gray, new TimeSpan(0, 0, ar[2]), ar[1] / 100.0f, ar[0]);
                    scene.Render(4);
                    app.Render(scene.Bmp);
                    Application.Run(app);
                }
                else if (choice == 2)
                {
                    Scene localScene = new Scene();
                    Console.WriteLine("Количество дополнительных сфер \t Количество потоков \t Время выполнения");
                    for (int k = 0; k < 10; k++)
                    {

                        Stopwatch sw = new Stopwatch();
                        for (int j = 0; j < count; j++)
                        {
                            sw.Start();
                            localScene.RenderFollow();
                            sw.Stop();
                        }
                        Console.WriteLine($"{k * 100} \t\t\t\t Последовательное выполнение \t {sw.ElapsedMilliseconds / (double)count}");
                        sw.Reset();
                        for (int j = 0; j < count; j++)
                        {
                            sw.Start();
                            localScene.RenderSingle();
                            sw.Stop();
                        }
                        Console.WriteLine($"{k * 100} \t\t\t\t Однопоточное выполнение \t {sw.ElapsedMilliseconds / (double)count}");

                        localScene.AddSphere(100);
                    }
                }
                else if (choice == 3)
                {
                    Scene localScene = new Scene();
                    Console.WriteLine("Количество дополнительных сфер \t Количество потоков \t Время выполнения");
                    for (int k = 0; k < 10; k++)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Stopwatch sw = new Stopwatch();
                            for (int j = 0; j < count; j++)
                            {
                                sw.Start();
                                localScene.Render(i);
                                sw.Stop();
                            }
                            Console.WriteLine($"{k * 100} \t\t\t\t {3 * (int)Math.Pow(2, i + 1)} \t\t\t {sw.ElapsedMilliseconds / (double)count}");
                        }
                        localScene.AddSphere(100);
                    }
                }
            }
        }
    }
}

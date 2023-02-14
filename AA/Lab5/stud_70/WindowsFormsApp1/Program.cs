using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComputerGraphic;
using System.Drawing;
using System.IO;
using System.Reflection;

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
            
            scene = new Scene(640, 720);
            bool exit = false;
            while (!exit)
            {
                app = new Form1();
                Console.Write("Меню:\n" +
                    "\t 0. Выход.\n" +
                    "\t 1. Демонстрация работы алгоритма.\n" +
                    "\t 2. Сравнение времени последовательного выполнения и конвейерной при разном количестве заявок.\n" +
                    "\t 3. Демонстрация конвеерной обработки. \n" +
                    "\t 4. Сравнение времени последовательного выполнения и конвейерной при разном количестве объектов.\n" +
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
                    List<Query> res = scene.RenderConveyor(640, 720);
                    app.Render(scene.Bmp);
                    Application.Run(app);
                }
                else if (choice == 2)
                {
                    Scene ls = new Scene(720, 640);
                    ls.AddSphere(400);
                    ls.SimulateSmoke(Color.Gray, new TimeSpan(0, 0, 60), 20 / 100.0f, 40);
                    int[] xar = new int[] { 240, 480, 720 };
                    int[] yar = new int[] { 160, 320, 480, 640 };
                    foreach (int x in xar)
                        foreach (int y in yar)
                        {
                            Stopwatch sw = new Stopwatch();
                            for (int j = 0; j < count; j++)
                            {
                                sw.Start();
                                ls.RenderFollow(x, y);
                                sw.Stop();
                            }
                            Console.Write($"{x * y} \t\t\t\t {sw.ElapsedMilliseconds / (double)count}");
                            sw.Reset();
                            for (int j = 0; j < count; j++)
                            {
                                sw.Start();
                                ls.RenderConveyor(x, y);
                                sw.Stop();
                            }
                            Console.WriteLine($"\t\t {sw.ElapsedMilliseconds / (double)count}");
                        }
                }
                else if (choice == 3)
                {
                    Scene localScene = new Scene(7, 7);
                    localScene.AddSphere(300);
                    localScene.SimulateSmoke(Color.Gray, new TimeSpan(0, 0, 60), 20 / 100.0f, 40);
                    List<Query> res = localScene.RenderConveyor(7, 7);
                    List<TimeStamp> times = new List<TimeStamp>();
                    FileStream f = File.Open(@"C:\zolot\AA\Lab5\stud_70\result.txt", FileMode.Truncate);
                    using (StreamWriter sw = new StreamWriter(f))
                    {
                        foreach(Query q in res)
                            foreach (TimeStamp stamp in q.times)
                                times.Add(stamp);
                        var sorted = from time in times
                                     orderby time.ts ascending
                                     select time;
                        foreach (TimeStamp stamp in sorted)
                            sw.WriteLine(stamp.message + stamp.ts);
                    }
                }
                else if (choice == 4)
                {
                    Scene ls = new Scene(50, 50);
                    ls.SimulateSmoke(Color.Gray, new TimeSpan(0, 0, 50), 20 / 100.0f, 40);
                    double count = 5.0;
                    for(int i = 0; i < 10; i++)
                    {
                        Stopwatch sw = new Stopwatch();
                        for (int j = 0; j < count; j++)
                        {
                            sw.Start();
                            ls.RenderFollow(50, 50);
                            sw.Stop();
                        }
                        Console.Write($"{i * 100} \t\t\t\t {sw.ElapsedMilliseconds / count}");
                        sw.Reset();
                        for (int j = 0; j < count; j++)
                        {
                            sw.Start();
                            ls.RenderConveyor(50, 50);
                            sw.Stop();
                        }
                        Console.WriteLine($"\t\t {sw.ElapsedMilliseconds / count}");
                        ls.AddSphere(100);
                    }
                }
            }
        }
    }
}

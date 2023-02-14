using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataBase db = new DataBase();
            int choice = 0;
            do
            {
                Console.Write("Меню.\n" +
                    "\t0. Выход.\n" +
                    "\t1. Cкалярный запрос.\n" +
                    "\t2. Запрос с несколькими соединениями.\n" +
                    "\t3. Запрос с ОТВ и оконными функциями.\n" +
                    "\t4. Запрос к метаданным.\n" +
                    "\t5. Вызов скалярной функции из Л/Р 3.\n" +
                    "\t6. Вызов табличной функции из Л/Р 3.\n" +
                    "\t7. Вызов хранимой процедуры из Л/Р 3.\n" +
                    "\t8. Вызов системной функции.\n" +
                    "\t9. Создание таблицы в базе данных.\n" +
                    "\t10. Вставка данных в созданную таблицу из п.9.\n" +
                    "Выбор: ");
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n");
                    continue;
                }
                switch (choice)
                {
                    case 0:
                        choice = 0;
                        break;
                    case 1:
                        db.ScalarQuery();
                        break;
                    case 2:
                        db.JoinQuery();
                        break;
                    case 3:
                        db.CTEWindowQuery();
                        break;
                    case 4:
                        db.MetaQuery();
                        break;
                    case 5:
                        db.ScalarFunction();
                        break;
                    case 6:
                        db.TableFunction();
                        break;
                    case 7:
                        db.Procedure();
                        break;
                    case 8:
                        db.SystemFunction();
                        break;
                    case 9:
                        db.CreateTable();
                        break;
                    case 10:
                        db.ВставкаДанных();
                        break;
                    default:
                        choice = -1;
                        break;
                }
            }
            while (choice != 0);
        }
    }
}

using System.Collections.Generic;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> dictionary = Load(), res = new();
            while (true)
            {
                Console.WriteLine("Введите вопрос:");

                string question = Console.ReadLine();
                int category = IsQuestionValid(question);
                switch (category)
                {
                    case -1:
                        Console.WriteLine("Вопрос не распознан.");
                        break;
                    case 0:
                        Console.WriteLine("Вопрос не соответствует тематике.");
                        break;
                    case 1:
                        res = Find(dictionary, 0, 5000);
                        break;
                    case 2:
                        res = Find(dictionary, 5000, 10000);
                        break;
                    case 3:
                        res = Find(dictionary, 10000, 30000);
                        break;
                    case 4:
                        res = Find(dictionary, 30000, 66667); 
                        break;
                    case 5:
                        res = Find(dictionary, 66667, 100000); 
                        break;
                    case 6:
                        res = Find(dictionary, 100000, 320000); 
                        break;
                    case 7:
                        res = Find(dictionary, 320000, 750000); 
                        break;
                    case 8:
                        res = Find(dictionary, 750000, 10000000); 
                        break;
                    case 9:
                        res = Find(dictionary, 10000000, 100000000); 
                        break;
                    case 10:
                        res = Find(dictionary, 100000000, 200000000); 
                        break;
                }
                if (category > 0)
                {
                    Console.WriteLine($"Найдено {res.Count} результатов"); 
                    foreach (var item in res)
                        Console.WriteLine($"{item.Key, 100}{item.Value, 10}");
                }
            }
        }
        static int IsQuestionValid(string question)
        {
            SubstringFinder sf = new SubstringFinder();
            int pos;
            if ((pos = sf.FindSubstring(question.ToLowerInvariant(), "найди")) != -1) ;
            else if ((pos = sf.FindSubstring(question.ToLowerInvariant(), "какие")) != -1) ;
            else if ((pos = sf.FindSubstring(question.ToLowerInvariant(), "выбери")) != -1) ;
            else if ((pos = sf.FindSubstring(question.ToLowerInvariant(), "покажи")) != -1) ;
            else return -1;
            int ppos = pos;
            if ((pos = sf.FindSubstring(question, "видео")) != -1) { if (ppos > pos) return -1; }
            else if ((pos = sf.FindSubstring(question, "видосы")) != -1) { if (ppos > pos) return -1; }
            else return 0;
            ppos = pos;
            int[] cat = Category(question);
            int cpos = cat[0], cc = cat[1];
            if (cpos == -1 || cpos < ppos)
                return -1;
            if ((pos = sf.FindSubstring(question, "популярност")) != -1) { if (ppos > pos || cpos > pos) return -1; }
            else if ((pos = sf.FindSubstring(question, "просмотр")) != -1) { if (ppos > pos || cpos > pos) return -1; }
            else if ((pos = sf.FindSubstring(question, "посмотрел")) != -1)
            {
                if (ppos > pos || cpos < pos) return -1;
                ppos = pos;
                if ((pos = sf.FindSubstring(question, "количество людей")) != -1) { if (ppos > pos || pos < cpos) return -1; }
                else if ((pos = sf.FindSubstring(question, "количество человек")) != -1) { if (ppos > pos || cpos > pos) return -1; }
            }
            else return 0;
            return cc;
        }
        static int[] Category(string question)
        {
            SubstringFinder sf = new();
            int[] res = new int[] { -1, 0 };
            if ((res[0] = sf.FindSubstring(question, "не очень маленьк")) > 0)
                res[1] = 2;
            else if ((res[0] = sf.FindSubstring(question, "очень маленьк")) > 0)
                res[1] = 1;
            else if ((res[0] = sf.FindSubstring(question, "немаленьк")) > 0) 
                res[1] = 6;
            else if ((res[0] = sf.FindSubstring(question, "маленьк")) > 0) 
                res[1] = 4;
            else if ((res[0] = sf.FindSubstring(question, "средн")) > 0)
                res[1] = 5;
            else if ((res[0] = sf.FindSubstring(question, "не очень больш")) > 0) 
                res[1] = 7;
            else if ((res[0] = sf.FindSubstring(question, "очень больш")) > 0)
                res[1] = 9;
            else if ((res[0] = sf.FindSubstring(question, "небольш")) > 0) 
                res[1] = 3;
            else if ((res[0] = sf.FindSubstring(question, "невероятно больш")) > 0)
                res[1] = 10;
            else if ((res[0] = sf.FindSubstring(question, "больш")) > 0) 
                res[1] = 8;
            return res;
        }

        static Dictionary<string, int> Load()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            using StreamReader streamReader = new(File.OpenRead(@"..\..\..\..\USvideos.csv"));
            string line;
            streamReader.ReadLine();
            while ((line = streamReader.ReadLine()) != null)
            {
                string[] kv = line.Split(';');
                string kt = string.Join(" ", kv);
                string k = kt.Substring(0, kt.Length - kv[^1].Length);
                res.Add(k, Convert.ToInt32(kv[^1]));
            }
            return res;
        }
        static Dictionary<string, int> Find(Dictionary<string, int> src, int l, int u)
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach(KeyValuePair<string, int> kv in src)
                if (kv.Value > l && kv.Value < u)
                    res.Add(kv.Key, kv.Value);
            //var d = from kv in src where kv.Value > l && kv.Value < u select kv;
            return res;
        }
    }

    class SubstringFinder
    {
        public int FindSubstring(string source, string sub)
        {
            int res = -1;
            if (source.Length == 0 || sub.Length == 0)
                return res;
            for (int i = 0; i < source.Length - sub.Length + 1; i++)
                for (int j = 0; j < sub.Length; j++)
                    if (sub[j] != source[i + j])
                        break;
                    else if (j == sub.Length - 1)
                    {
                        res = i;
                        break;
                    }
            return res;
        }
    }
}

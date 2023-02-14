using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab6
{
    static class BruteForce
    {
        public static Path GetRoute(Map map)
        {
            Path shortest = new Path(null, map, int.MaxValue);
            List<int> a = new List<int>();
            for (int i = 0; i < map.N; i++)
                a.Add(i);
            foreach (List<int> cur in GetAllRoutes(a, new List<int>()))
            {
                Path check = new Path(cur, map, -1);
                check.GetDistance();
                if (shortest.N > check.N)
                    shortest = check;
            }
            return shortest;
        }

        private static IEnumerable<List<int>> GetAllRoutes(List<int> arg, List<int> awithout)
        {
            if (arg.Count == 1)
            {
                var result = new List<List<int>> { new List<int>() };
                result[0].Add(arg[0]);
                return result;
            }
            else
            {
                var result = new List<List<int>>();

                foreach (var first in arg)
                {
                    var others0 = new List<int>(arg.Except(new int[1] { first }));
                    awithout.Add(first);
                    var others = new List<int>(others0.Except(awithout));

                    var combinations = GetAllRoutes(others, awithout);
                    awithout.Remove(first);

                    foreach (var tail in combinations)
                    {
                        tail.Insert(0, first);
                        result.Add(tail);
                    }
                }
                return result;
            }
        }
        private static void Print(List<int> cur)
        {
            foreach (int num in cur)
            {
                Console.Write(num);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Lab6
{
    class Map
    {
        private int count;
        private int[,] dist;
        private int best;

        public Map(int n, int rand)
        {
            Random r = new Random();
            count = n;
            dist = new int[count, count];
            for (int i = 0; i < count; i++)
            {
                dist[i, i] = -1;
                for (int j = i + 1; j < count; j++)
                {
                    int tmp = 0;
                    while (tmp == 0)
                    {
                        tmp = r.Next(rand);
                    }
                    dist[i, j] = dist[j, i] = tmp;
                }
            }
        }
        public Map(int[,] matr, int n)
        {
            dist = matr;
            count = n;
        }
        public Map(int[,] matr, int n, int best)
        {
            dist = matr;
            count = n;
            this.best = best;
        }

        public int this[int i, int j]
        {
            get { return dist[i, j]; }
            set { dist[i, j] = value; }
        }
        public int N
        {
            get { return count; }
            set { count = value; }
        }
        public int BestDistance
        {
            get { return best; }
            private set { best = value; }
        }
        public int Sum()
        {
            int sum = 0;
            for (int i = 0; i < count; i++)
                for (int j = 0; j < count; j++)
                    sum += dist[i, j];
            return sum;
        }
        public void Print()
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    Console.Write(dist[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}

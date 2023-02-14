using System;
using System.Collections.Generic;
using System.Text;

namespace Lab6
{
    class Path
    {
        private List<int> path;
        private int dist;
        private Map map;

        public Path(List<int> path, Map map, int dist = -1)
        {
            this.path = path;
            this.dist = dist;
            this.map = map;
        }
        public List<int> Way 
        {
            get { return path; }
            private set { path = value; }
        }
        public Path(Map map, int town)
        {
            path = new List<int>();
            path.Add(town);
            this.map = map;
            GetDistance();
        }

        public void AddTown(int town)
        {
            dist += map[path[path.Count - 1], town];
            path.Add(town);
        }
        public int LastTown()
        {
            return path[path.Count - 1];
        }
        public bool CheckTown(int town)
        {
            foreach (int num in path)
                if (num == town)
                    return true;
            return false;
        }

        public int this[int i]
        {
            get { return path[i]; }
        }

        public int N
        {
            get { return dist; }
            set { dist = value; }
        }

        public void GetDistance()
        {
            int distance = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                distance += map[path[i], path[i + 1]];
            }
            this.dist = distance;
        }
        public void Print()
        {
            foreach (int num in path)
            {
                Console.Write(num);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}

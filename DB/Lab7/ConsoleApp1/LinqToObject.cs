using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    internal class LinqToObject
    {
        public void LinkToObject()
        {
            GetOneGame();
            GetOldGame();
            GetOrderedByDeveloper();
            GetCountByYear();
            GetFirstDevelopedGame();
        }
        static void GetOneGame()
        {
            Console.WriteLine("\n\n\n1.");
            var pass = from g in VideoGamesTable.GetVideoGames()
                       where g.Title == "Angry Birds Rio"
                       select g;

            foreach (var p in pass)
            {
                Console.WriteLine(p);
            }
        }

        static void GetOldGame()
        {
            Console.WriteLine("\n\n\n2.");
            var pass = from g in VideoGamesTable.GetVideoGames()
                       where g.Release < 2000
                       select g;

            foreach (var p in pass)
            {
                Console.WriteLine(p);
            }
        }

        static void GetOrderedByDeveloper()
        {
            Console.WriteLine("\n\n\n3.");
            var pass = from g in VideoGamesTable.GetVideoGames()
                       orderby g.Developer ascending
                       select g;

            foreach (var p in pass)
            {
                Console.WriteLine(p);
            }
        }

        static void GetCountByYear()
        {
            Console.WriteLine("\n\n\n4.");
            var pass = from g in VideoGamesTable.GetVideoGames()
                       group g by g.Release into rel
                       orderby rel.Key ascending
                       select new { Year = rel.Key, Count = rel.Count() };

            foreach (var p in pass)
            {
                Console.WriteLine(p);
            }
        }

        static void GetFirstDevelopedGame()
        {
            Console.WriteLine("\n\n\n5.");
            var pass = from g in VideoGamesTable.GetVideoGames()
                       group g by g.Developer into rel
                       orderby rel.Key ascending
                       select new { Developer = rel.Key, Count = rel.Min(g => g.Release) };

            foreach (var p in pass)
            {
                Console.WriteLine(p);
            }
        }
    }
    class VideoGamesTable
    {
        private static List<VideoGame> games;

        public static IList<VideoGame> GetVideoGames()
        {
            if (games == null)
            {
                games = new List<VideoGame>();
                string[] lines = File.ReadAllLines(@"C:\zolot\DB\Lab7\ConsoleApp1\data.csv");
                foreach(var line in lines)
                {
                    string[] w = line.Split(new char[] { ';' });
                    if (w.Length != 5)
                    {
                        Console.WriteLine("Read Error");
                        return null;
                    }
                    int release = Convert.ToInt32(w[1]);

                    VideoGame gc = new VideoGame(w[0], release, w[2], w[3], w[4]);
                    games.Add(gc);
                }
            }
            return games;
        }
    }

    class VideoGame
    {
        public VideoGame(string title, int release, string developer, string publisher, string genre)
        {
            Title = title;
            Release = release;
            Developer = developer;
            Publisher = publisher;
            Genre = genre;
        }

        public string Title { get; set; }
        public int Release { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }

        public override string ToString() 
        {
            return string.Format($"{Title} {Release} {Developer} {Publisher} {Genre}");
        }

    }
}


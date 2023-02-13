using System.Diagnostics;
namespace ConsoleApp1
{
    internal class DamLevAlgo : BaseAlgo
    {
        
        public override int[,] DoAlgorithm(string f_str, string s_str)
        {
            Process ps = Process.GetCurrentProcess();
            int[,] matrix = new int[s_str.Length + 1, f_str.Length + 1];
            for (int i = 0; i < f_str.Length + 1; i++)
                matrix[0, i] = i;
            for (int i = 0; i < s_str.Length + 1; i++)
                matrix[i, 0] = i;
            for (int i = 1; i < s_str.Length + 1; i++)
                for (int j = 1; j < f_str.Length + 1; j++)
                {
                    int t1 = matrix[i, j - 1] + 1;
                    int t2 = matrix[i - 1, j] + 1;
                    int t3 = matrix[i - 1, j - 1] + (s_str[i - 1] == f_str[j - 1] ? 0 : 1);
                    int t4 = int.MaxValue;
                    if (i > 1 && j > 1 && s_str[i - 1] == f_str[j - 2] && s_str[i - 2] == f_str[j - 1])
                        t4 = matrix[i - 2, j - 2] + 1;
                    matrix[i, j] = Math.Min(Math.Min(t1, t4), Math.Min(t2, t3));
                }
            return matrix;
        }
    }
}

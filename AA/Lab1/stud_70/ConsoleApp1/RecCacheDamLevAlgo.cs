namespace ConsoleApp1
{
    internal class RecCacheDamLevAlgo : BaseRecurAlgo
    {
        public override int DoAlgorithm(string f_str, string s_str)
        {
            int n = f_str.Length, m = s_str.Length;
            int[,] matrix = new int[f_str.Length + 1, s_str.Length + 1];

            static int recursive(string f_str, string s_str, int n, int m, int[,] matrix)
            {
                if (matrix[n, m] != -1)
                    return matrix[n, m];

                if (n == 0)
                {
                    matrix[n, m] = m;
                    return matrix[n, m];
                }

                if (n > 0 && m == 0)
                {
                    matrix[n, m] = n;
                    return matrix[n, m];
                }
                int delete = recursive(f_str, s_str, n - 1, m, matrix) + 1;
                int add = recursive(f_str, s_str, n, m - 1, matrix) + 1;
                int change = recursive(f_str, s_str, n - 1, m - 1, matrix) + (s_str[m - 1] == f_str[n - 1] ? 0 : 1);
                int xch = int.MaxValue;
                if (m > 1 && n > 1 && s_str[m - 1] == f_str[n - 2] && s_str[m - 2] == f_str[n - 1])
                    xch = recursive(f_str, s_str, n - 2, m - 2, matrix) + 1;

                matrix[n, m] = Math.Min(Math.Min(add, xch), Math.Min(delete, change));

                return matrix[n, m];
            }

            for (int i = 0; i < n + 1; i++)
                for (int j = 0; j < m + 1; j++)
                    matrix[i, j] = -1;

            recursive(f_str, s_str, n, m, matrix);

            return matrix[n, m];

        }
    }
}

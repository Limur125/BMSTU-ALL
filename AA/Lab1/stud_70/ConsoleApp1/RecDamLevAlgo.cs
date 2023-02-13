namespace ConsoleApp1
{
    internal class RecDamLevAlgo : BaseRecurAlgo
    {
        public override int DoAlgorithm(string f_str, string s_str)
        {
            int n = f_str.Length, m = s_str.Length;
            if (n == 0 || m == 0)
                return Math.Abs(n - m);
            int t1 = DoAlgorithm(f_str[..(n - 1)], s_str[..m]) + 1;
            int t2 = DoAlgorithm(f_str[..n], s_str[..(m - 1)]) + 1;
            int t3 = DoAlgorithm(f_str[..(n - 1)], s_str[..(m - 1)]) + (s_str[^1] == f_str[^1] ? 0 : 1);
            int t4 = int.MaxValue;
            if (m > 1 && n > 1 && s_str[^1] == f_str[^2] && s_str[^2] == f_str[^1])
                t4 = DoAlgorithm(f_str[..(n - 2)], s_str[..(m - 2)]) + 1;
            return Math.Min(Math.Min(t1, t4), Math.Min(t2, t3));
        }
    }
}

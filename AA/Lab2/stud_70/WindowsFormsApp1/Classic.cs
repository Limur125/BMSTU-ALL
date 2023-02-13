namespace WindowsFormsApp1
{
    internal class Classic : BaseAlgo
    {
        public override int[][] Multiply(int[][] A, int[][] B)
        {
            int Ar = A.Length;
            int Br = B.Length;

            if (Ar == 0 || Br == 0)
                return null;

            int Ac = A[0].Length;
            int Bc = B[0].Length;

            if (Ac != Br)
                return null;

            int[][] C = new int[Ar][];
            for (int i = 0; i < Ar; i++)
                C[i] = new int[Bc];

            for (int i = 0; i < Ar; i++)
                for (int j = 0; j < Bc; j++)
                    for (int k = 0; k < Ac; k++)
                        C[i][j] += A[i][k] * B[k][j];

            return C;
        }
    }
}

namespace WindowsFormsApp1
{
    internal class OptimizedVinograd : BaseAlgo
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

            int[] mulH = new int[Ar];
            int[] mulV = new int[Bc];

            int[][] C = new int[Ar][];
            for (int i = 0; i < Ar; i++)
                C[i] = new int[Bc];

            int Ac2 = Ac >> 1;
            for (int i = 0; i < Ar; i++)
                for (int j = 0; j < Ac2; j++)
                {
                    int j2 = j << 1;
                    mulH[i] += A[i][j2] * A[i][j2 + 1];
                }

            for (int i = 0; i < Bc; i++)
                for (int j = 0; j < Ac2; j++)
                {
                    int j2 = j << 1;
                    mulV[i] += B[j2][i] * B[j2 + 1][i];
                }

            for (int i = 0; i < Ar; i++)
                for (int j = 0; j < Bc; j++)
                {
                    int buf = -mulH[i] - mulV[j];
                    for (int k = 0; k < Ac2; k++)
                    {
                        int k2 = k << 1;
                        int k21 = k2 + 1;
                        buf += (A[i][k21] + B[k2][j]) * (A[i][k2] + B[k21][j]);
                    }
                    C[i][j] = buf;
                }


            if (Ac % 2 == 1)
                for (int i = 0; i < Ar; i++)
                    for (int j = 0; j < Bc; j++)
                    {
                        int Ac1 = Ac - 1;
                        C[i][j] += A[i][Ac1] * B[Ac1][j];
                    }

            return C;
        }
    }
}

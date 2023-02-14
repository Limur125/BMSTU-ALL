namespace WindowsFormsApp1
{
    internal class Vinograd : BaseAlgo
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

            for (int i = 0; i < Ar; i++)
                for (int j = 0; j < Ac / 2; j++)
                    mulH[i] = mulH[i] + A[i][j * 2] * A[i][j * 2 + 1];

            for (int i = 0; i < Bc; i++)
                for (int j = 0; j < Br / 2; j++)
                    mulV[i] = mulV[i] + B[j * 2][i] * B[j * 2 + 1][i];

            for (int i = 0; i < Ar; i++)
                for (int j = 0; j < Bc; j++)
                {
                    C[i][j] = -mulH[i] - mulV[j];
                    for (int k = 0; k < Ac / 2; k++)
                        C[i][j] = C[i][j] + (A[i][2 * k + 1] + B[2 * k][j]) * (A[i][2 * k] + B[2 * k + 1][j]);
                }

            if (Ac % 2 == 1)
                for (int i = 0; i < Ar; i++)
                    for (int j = 0; j < Bc; j++)
                        C[i][j] = C[i][j] + A[i][Ac - 1] * B[Ac - 1][j];

            return C;
        }
    }
}

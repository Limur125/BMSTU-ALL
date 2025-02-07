using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WindowsFormsApp3
{
    public class Solve
    {
        private static double delta = 1e-4;
        private static double eps = 1e-4;
        public static double[] gauss(double[,] matrix, int stateCount)
        {
            double[] result = new double[stateCount];

            for (int i = 0; i < stateCount; i++)
            {
                double max = Math.Abs(matrix[i, i]);
                int index = i;
                for (int j = i + 1; j < stateCount; j++)
                {
                    if (Math.Abs(matrix[j, i]) > max)
                    {
                        max = Math.Abs(matrix[j, i]);
                        index = j;
                    }
                }
                if (max == 0)
                {
                    continue;
                }
                for (int j = 0; j < stateCount; j++)
                {
                    double tmp = matrix[i, j];
                    matrix[i, j] = matrix[index, j];
                    matrix[index, j] = tmp;
                }
                for (int j = 0; j < stateCount; j++)
                {
                    double tmp = matrix[j, i];
                    if (Math.Abs(tmp) == 0)
                    {
                        continue;
                    }
                    for (int k = 0; k < stateCount; k++)
                    {
                        matrix[j, k] /= tmp;
                    }
                    if (j == i)
                    {
                        continue;
                    }
                    for (int k = 0; k < stateCount; k++)
                    {
                        matrix[j, k] -= matrix[i, k];
                    }
                }
            }

            for (int i = 0; i < stateCount; i++)
            {
                double tmp = matrix[i, i];
                if (matrix[i, i] != 0)
                {
                    for (int j = 0; j < stateCount; j++)
                    {
                        matrix[i, j] = matrix[i, j] / tmp;
                    }
                }
            }

            for (int i = stateCount - 1; i >= 0; --i)
            {
                result[i] = matrix[i, stateCount - 1];
                for (int j = 0; j < i; j++)
                {
                    matrix[j, stateCount - 1] -= matrix[j, i] * result[i];
                }
            }

            return result;
        }

        private static ArrayList dp(ArrayList ps, int n, double[,] lambdas)
        {
            ArrayList result = new ArrayList();
            for (int i = 0; i < n; i++)
            {
                double current = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == i)
                    {
                        double outt = 0;
                        for (int k = 0; k < n; k++)
                        {
                            if (k != i)
                            {
                                outt -= lambdas[i, k];
                            }
                        }
                        outt *= Convert.ToDouble(ps[j]);
                        current += outt;
                    }
                    else
                    {
                        current += lambdas[j, i] * Convert.ToDouble(ps[j]);
                    }
                }
                result.Add(current * delta);
            }
            return result;
        }

        public static double[] calculateTime(int stateCount, double[] marginalProbs, double[] startProbs, double[,] lambdas)
        {
            bool[] ready = new bool[stateCount];
            bool isEndOfCycle = false;
            for (int i = 0; i < stateCount; i++)
            {
                ready[i] = false;
            }
            double[] times = new double[stateCount];
            for (int i = 0; i < stateCount; i++)
            {
                times[i] = 0;
            }
            double curTime = 0.0;
            ArrayList current = new ArrayList(startProbs);
            ArrayList prev = new ArrayList(marginalProbs);
            int count = 0;
            while (!isEndOfCycle)
            {
                count++;
                isEndOfCycle = true;
                ArrayList curDps = dp(current, stateCount, lambdas);

                for (int i = 0; i < stateCount; i++)
                {
                    if (Math.Abs(marginalProbs[i] - Convert.ToDouble(current[i])) <= eps && Convert.ToDouble(curDps[i]) <= eps)
                    {
                        if (!ready[i])
                        {
                            times[i] = curTime;
                        }
                        ready[i] = true;
                    }
                    else
                    {
                        isEndOfCycle = false;
                    }
                    prev[i] = current[i];
                    current[i] = Convert.ToDouble(current[i]) + Convert.ToDouble(curDps[i]);

                }
                curTime += delta;
            }
            return times;
        }
    }
}

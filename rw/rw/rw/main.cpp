#include <vector>

#include "Lamp.hpp"
#include "Cylinder.hpp"
#include "Reader.hpp"
#include "PhysicsFunctions.h"
#include "CudaRayTracing.cuh"
double TInter(double z0, double z1, double* zArr, double* tzArr, int zN)
{
	int zI1 = 1;
	for (; zI1 < zN && zArr[zI1] < z0; zI1++);
	int zI2 = zI1;
	for (; zI2 < zN && zArr[zI2] < z1; zI2++);
	double sum = 0;
	if (zI1 != zI2)
	{
		for (int i = zI1 + 1; i <= zI2; i++)
		{
			sum += (tzArr[i] + tzArr[i - 1]) / 2 * (zArr[i] - zArr[i - 1]);
		}
		double t = sum / (zArr[zI2] - zArr[zI1]);
		return t;
	}
	return tzArr[zI1];
}


int main(void)
{
	int N = 80;
	double R = 0.35;
	Reader reader("data.txt");
	if (!reader.Read())
		return 1;
	if (!reader.ReadTemp("temp.txt"))
		return 1;
	Cylinder *cylinders = new Cylinder[N];
	for (int i = 0; i < N; i++)
	{
		cylinders[N - i - 1] = { R / N * (i + 1), TInter(1.0 / N * i, 1.0 / N * (i + 1), reader.zArr, reader.tzArr, reader.zN) /*Tavg(R / N * i, R / N * (i + 1), R)*/};
	}
	Lamp lamp(R, cylinders, N,
		reader.TArr, reader.TN,
		reader.nuArr, reader.nuN,
		reader.kMatr);
	int n = 20, m = 10;
	double *res = new double[n * m + 1];

	//for (int i = 0; i < lamp.nuN - 1; i++)
	//{
	//	for (int j = 0; j < lamp.TN; j++)
	//	{
	//		printf("%.3e   ", lamp.kMatr[j][i]);
	//	}
	//	printf("\n");
	//}

	GetInitialIntensity(lamp, res, n, m);
}


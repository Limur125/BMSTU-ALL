#include "CudaRayTracing.cuh"
#include <exception>

#define PI (4 * atan(1))
#define PI2 (acos(0))
#define REFLECT_COEF 0.9

void RayIntensity(Ray* rays, int raysN, Cylinder* cyls, int cylsN, double* intens,
	double* TArr, double* nuArr, int TN, int nuN, double** kMatr);
__host__ __device__ double Inup(double nu, double T);
__host__ __device__ double Inu(double InuPrev, double nu, double dnu, double h, double k, double T);
__host__ __device__ double k(double T, int nu, double** kMatr, double* TArr, double* nuArr, int TN, int nuN);
__host__ __device__ double InuAbsorb(double InuPrev, double h, double k);


__device__ __host__ PairDouble GetIntersections(Ray ray, Cylinder cyl)
{
	double ac = cyl.R0, bc = cyl.R0;
	double y0 = ray.Start.Y, z0 = ray.Start.Z;
	double yd = ray.Direction.Y, zd = ray.Direction.Z;
	double b = (bc * bc * z0 * zd + ac * ac * y0 * yd);
	double a = (bc * bc * zd * zd + ac * ac * yd * yd);
	double c = (bc * bc * z0 * z0 + ac * ac * y0 * y0 - ac * ac * bc * bc);
	double d = b * b - a * c;
	double t1, t2;
	t1 = (-b + sqrt(d)) / a;
	t2 = (-b - sqrt(d)) / a;

	return { t1, t2 };
}

void RayIntensity(Ray* rays, int raysN, Cylinder* cyls, int cylsN, double* intens,
	double* TArr, double* nuArr, int TN, int nuN, double** kMatr)
{
	for (int nuId = 0; nuId < nuN - 1; nuId++)
	{
		for (int rId = 0; rId < raysN; rId++)
		{
			Ray ray = rays[rId];
			PairDouble inters = GetIntersections(ray, cyls[0]);
			double newStart = abs(inters.value1) > abs(inters.value2) ? inters.value1 : inters.value2;
			Ray newRay(ray.Point(newStart), ray.Direction);
			double dnu = nuArr[nuId + 1] - nuArr[nuId];
			double nu = (nuArr[nuId + 1] + nuArr[nuId]) / 2;
			double intensPrev = 0;
			int curCylI = 0;
			for (int i = 0; i < 2 * cylsN - 1; i++)
			{
				int nextCylI = (i + 1) < cylsN ? (i + 1) : cylsN - ((i + 1) % cylsN) - 1;

				inters = GetIntersections(newRay, cyls[nextCylI]);
				if (isnan(inters.value1) || isnan(inters.value2))
					continue;
				if (inters.value1 > 0 && inters.value2 > 0)
					newStart = inters.value1 < inters.value2 ? inters.value1 : inters.value2;
				else if (inters.value2 < 1e-12 && inters.value1 > 0)
					newStart = inters.value1;
				else if (inters.value2 > 0 && inters.value1 < 1e-12)
					newStart = inters.value2;
				else
					throw std::exception("Blb yf[eq");
				Cylinder curCyl = cyls[curCylI < nextCylI ? curCylI : nextCylI];
				double fk = k(curCyl.T, nuId, kMatr, TArr, nuArr, TN, nuN);
				double len = newRay.Start.Length(newRay.Point(newStart));
				double intens = Inu(intensPrev, nu, dnu, len, fk, curCyl.T);
				intensPrev = intens;
				curCylI = nextCylI;
				newRay = { newRay.Point(newStart), newRay.Direction };
			}
			intens[raysN * nuId + rId] = intensPrev;
		}
	}
}
		
void RayIntensityAbsorb(Ray * rays, int raysN, Cylinder * cyls, int cylsN, double* intens,
	double* TArr, double* nuArr, int TN, int nuN, double** kMatr, double* cylsEnergy)
{
	for (int nuId = 0; nuId < nuN - 1; nuId++)
	{
		for (int rId = 0; rId < raysN; rId++)
		{
			double intensPrev = intens[raysN * nuId + rId];
			Ray newRay = rays[rId];

			while (intensPrev > intens[raysN * nuId + rId] * 0.01)
			{
				PairDouble inters;
				int curCylI = 0;
				double newStart = 0;
				newRay.Direction = (-newRay.Direction).Reflect((-newRay.Start).Normalize());
				cylsEnergy[(cylsN * (nuN - 1) + nuId) * raysN + rId] += intensPrev * (1 - REFLECT_COEF);
				intensPrev *= REFLECT_COEF;
				for (int i = 0; i < 2 * cylsN - 1 && intensPrev > intens[raysN * nuId + rId] * 0.01; i++)
				{
					int nextCylI = (i + 1) < cylsN ? (i + 1) : cylsN - ((i + 1) % cylsN) - 1;

					inters = GetIntersections(newRay, cyls[nextCylI]);
					if (isnan(inters.value1) || isnan(inters.value2))
						continue;
					if (inters.value1 > 1e-12 && inters.value2 > 1e-12)
						newStart = inters.value1 < inters.value2 ? inters.value1 : inters.value2;
					else if (inters.value2 <= 1e-12 && inters.value1 > 1e-12)
						newStart = inters.value1;
					else if (inters.value2 > 1e-12 && inters.value1 <= 1e-12)
						newStart = inters.value2;
					else
						throw new std::exception("das");
					int ccI = curCylI < nextCylI ? curCylI : nextCylI;
					Cylinder curCyl = cyls[ccI];
					double fk = k(curCyl.T, nuId, kMatr, TArr, nuArr, TN, nuN);
					double len = newRay.Start.Length(newRay.Point(newStart));
					double inten = InuAbsorb(intensPrev, len, fk);
					cylsEnergy[(ccI * (nuN - 1) + nuId) * raysN + rId] += (intensPrev - inten);
					intensPrev = inten;
					curCylI = nextCylI;
					newRay = { newRay.Point(newStart), newRay.Direction };
					if (intensPrev < intens[raysN * nuId + rId] * 0.01)
					{
						if (i + 1 < 2 * cylsN - 1)
						{
							cylsEnergy[(nextCylI * (nuN - 1) + nuId) * raysN + rId] += intensPrev;
						}
						else
						{
							cylsEnergy[(curCylI * (nuN - 1) + nuId) * raysN + rId] += intensPrev;
						}
					}
				}
			}
		}
	}
}

__global__ void RayIntensityKernel(Ray* rays, Cylinder* cyls, int cylsN, double* intens,
	double* TArr, double* nuArr, int TN, int nuN, double** kMatr)
{
	int rId = threadIdx.x;
	int nuId = blockIdx.x;
	int raysN = blockDim.x;
	Ray ray = rays[rId];
	PairDouble inters = GetIntersections(ray, cyls[0]);
	double newStart = abs(inters.value1) > abs(inters.value2) ? inters.value1 : inters.value2;
	Ray newRay(ray.Point(newStart), ray.Direction);
	double dnu = nuArr[nuId + 1] - nuArr[nuId];
	double nu = (nuArr[nuId + 1] + nuArr[nuId]) / 2;
	double intensPrev = 0;
	int curCylI = 0;
	for (int i = 0; i < 2 * cylsN - 1; i++)
	{
		int nextCylI = (i + 1) < cylsN ? (i + 1) : cylsN - ((i + 1) % cylsN) - 1;

		inters = GetIntersections(newRay, cyls[nextCylI]);
		if (isnan(inters.value1) || isnan(inters.value2))
			continue;
		if (inters.value1 > 0 && inters.value2 > 0)
			newStart = inters.value1 < inters.value2 ? inters.value1 : inters.value2;
		else if (inters.value2 < 1e-18 && inters.value1 > 0)
			newStart = inters.value1;
		else if (inters.value2 > 0 && inters.value1 < 1e-18)
			newStart = inters.value2;
		else
			return;
		Cylinder curCyl = cyls[curCylI < nextCylI ? curCylI : nextCylI];
		double fk = k(curCyl.T, nuId, kMatr, TArr, nuArr, TN, nuN);
		double len = newRay.Start.Length(newRay.Point(newStart));
		double intens = Inu(intensPrev, nu, dnu, len, fk, curCyl.T);
		intensPrev = intens;
		curCylI = nextCylI;
		newRay = { newRay.Point(newStart), newRay.Direction };
	}
	intens[raysN * nuId + rId] = intensPrev;
}

__host__ __device__ double Inup(double nu, double T)
{
	double coef = 4.8e4 * nu / T;
	double e = exp(coef) - 1;
	return 1.47e6 * nu * nu * nu / e;
}

__host__ __device__ double Inu(double InuPrev, double nu, double dnu, double h, double k, double T)
{
	double e = exp(-k * h);
	return InuPrev * e + Inup(nu, T) * dnu * (1 - e);
}

__host__ __device__ double InuAbsorb(double InuPrev, double h, double k)
{
	double e = exp(-k * h);
	return InuPrev * e;
}

__host__ __device__ int binarySearch(double* arr, int low, int high, double x) {
	while (low < high) 
	{
		int mid = low + (high - low) / 2;
		if (arr[mid] < x) 
			low = mid + 1;
		else 
			high = mid - 1;
	}
	return low;
}

__host__ __device__ double k(double T, int nuId, double** kMatr, double* TArr, double* nuArr, int TN, int nuN)
{
	int TI = binarySearch(TArr, 0, TN, T);
	double kPrev = log(kMatr[TI][nuId]), kNext = log(kMatr[TI + 1][nuId]);
	double TPrev = log(TArr[TI]), TNext = log(TArr[TI + 1]);
	double kavg = (kPrev + (kNext - kPrev) / (TNext - TPrev) * (log(T) - TPrev));
	return exp(kavg);
}

cudaError_t GetInitialIntensity(Lamp lamp, double* res, int n, int m)
{
	cudaError_t cudaStatus;
	long raysN = n * m + 1;
	Ray* dev_rays;
	Ray* host_rays = new Ray[raysN];
	double* dev_intens;
	double* host_intens = new double[(lamp.nuN - 1) * raysN];
	double* devCylEnergy;
	double* hostCylEnergy = new double[(lamp.cylsN + 1) * (lamp.nuN - 1) * raysN] {0};
	Cylinder* dev_cyls;
	double** dev_kMatr, * dev_TArr, * dev_nuArr;
	Vector3 direction, start;

	cudaStatus = cudaSetDevice(0);
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaSetDevice failed!  Do you have a CUDA-capable GPU installed?");
		goto Error;
	}

#pragma region DevLightRay

	cudaStatus = cudaMalloc(&dev_rays, raysN * sizeof(Ray));
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMalloc failed dev_rays!");
		goto Error;
	}

	for (int j = 1; j <= m; j++)
	{
		for (int i = 0; i < n; i++)
		{
			start = Vector3(0, 0, lamp.R);
			direction = Vector3(PI2 / (m + 1) * j, 2 * PI / n * i);
			Ray ray(start, direction);
			host_rays[i * m + j - 1] = ray;
		}
	}

	start = Vector3(0, 0, lamp.R);
	direction = Vector3(0, 0, 1);
	Ray ray(start, direction);
	host_rays[raysN - 1] = ray;

	cudaStatus = cudaMalloc(&dev_rays, raysN * sizeof(Ray));
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMemcpy failed! ray start");
		goto Error;
	}

	cudaStatus = cudaMemcpy(dev_rays, host_rays, sizeof(Ray) * raysN, cudaMemcpyHostToDevice);
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMemcpy failed! ray start");
		goto Error;
	}
#pragma endregion

#pragma region DevIntense
	cudaStatus = cudaMalloc(&dev_intens, sizeof(double) * (lamp.nuN - 1) * raysN);
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMalloc failed dev_intens!");
		goto Error;
	}
#pragma endregion

	cudaStatus = cudaMalloc(&devCylEnergy, sizeof(double) * (lamp.cylsN + 1) * (lamp.nuN - 1) * raysN);
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMalloc failed dev_intens!");
		goto Error;
	}

#pragma region DevCylinders
	cudaStatus = cudaMalloc(&dev_cyls, lamp.cylsN * sizeof(Cylinder));
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMalloc failed dev_cyls!");
		goto Error;
	}

	cudaStatus = cudaMemcpy(dev_cyls, lamp.Cylinders, lamp.cylsN * sizeof(Cylinder), cudaMemcpyHostToDevice);
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMemcpy failed!");
		goto Error;
	}
#pragma endregion

#pragma region DevKMatr
	cudaStatus = cudaMalloc(&dev_kMatr, lamp.TN * sizeof(double*));
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMalloc failed dev_kMatr!");
		goto Error;
	}

	for (int i = 0; i < lamp.TN; i++)
	{
		double* dev_kMatrRow;
		cudaStatus = cudaMalloc(&dev_kMatrRow, (lamp.nuN - 1) * sizeof(double));
		if (cudaStatus != cudaSuccess)
		{
			printf("cudaMalloc failed dev_kMatr %d!", i);
			goto Error;
		}
		cudaStatus = cudaMemcpy(dev_kMatrRow, lamp.kMatr[i], sizeof(double) * (lamp.nuN - 1), cudaMemcpyHostToDevice);
		if (cudaStatus != cudaSuccess)
		{
			printf("cudaMemcpy failed! dev_kMatr %d", i);
			goto Error;
		}

		cudaStatus = cudaMemcpy(dev_kMatr + i, &dev_kMatrRow, sizeof(double*), cudaMemcpyHostToDevice);
		if (cudaStatus != cudaSuccess)
		{
			printf("cudaMemcpy failed! dev_kMatrRow %d", i);
			goto Error;
		}
	}
#pragma endregion

#pragma region DevTArr
	cudaStatus = cudaMalloc(&dev_TArr, lamp.TN * sizeof(double));
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMalloc failed dev_TArr!");
		goto Error;
	}
	cudaStatus = cudaMemcpy(dev_TArr, lamp.TArr, sizeof(double) * lamp.TN, cudaMemcpyHostToDevice);
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMemcpy failed! dev_TArr");
		goto Error;
	}
#pragma endregion

#pragma region DevNuArr
	cudaStatus = cudaMalloc(&dev_nuArr, lamp.nuN * sizeof(double));
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMalloc failed dev_nuArr!");
		goto Error;
	}

	cudaStatus = cudaMemcpy(dev_nuArr, lamp.nuArr, sizeof(double) * lamp.nuN, cudaMemcpyHostToDevice);
	if (cudaStatus != cudaSuccess)
	{
		printf("cudaMemcpy failed! dev_nuArr");
		goto Error;
	}
#pragma endregion

	RayIntensity(host_rays, raysN, lamp.Cylinders, lamp.cylsN, host_intens,
		lamp.TArr, lamp.nuArr, lamp.TN, lamp.nuN, lamp.kMatr);

	double dtetha = PI2 / (m + 1);
	double dphi = 2 * PI / n;
	for (int l = 0; l < lamp.nuN - 1; l++)
	{
		double sum = 0;
		for (int r = 0; r < raysN; r++)
		{
			double x = host_rays[r].Direction.X;
			double y = host_rays[r].Direction.Y;
			double z = host_rays[r].Direction.Z;
			double tetha = atan(sqrt(x * x + y * y) / z);
			host_intens[l * raysN + r] *= dphi * dtetha * sin(tetha) * cos(tetha);
			//printf("%.3g ", host_intens[l * raysN + r]);
		}
		//printf("\n");
	}

	printf("\n");
	RayIntensityAbsorb(host_rays, raysN, lamp.Cylinders, lamp.cylsN, host_intens,
		lamp.TArr, lamp.nuArr, lamp.TN, lamp.nuN, lamp.kMatr, hostCylEnergy);
	double prevR = 0;
	for (int l = 0; l < lamp.nuN - 1; l++)
	{
		double nuAvg = (lamp.nuArr[l] + lamp.nuArr[l + 1]) / 2;
		
		for (int i = lamp.cylsN - 1; i >= 0; i--)
		{
			double K = k(lamp.Cylinders[i].T, l, lamp.kMatr, lamp.TArr, lamp.nuArr, lamp.TN, lamp.nuN);
			double c = 3e8;
			double q3 = 0;
			for (int r = 0; r < raysN; r++)
			{
				q3 += hostCylEnergy[(i * (lamp.nuN - 1) + l) * raysN + r];
			}
			double qp = q3 * 2 * lamp.R / (lamp.Cylinders[i].R0 * lamp.Cylinders[i].R0 - prevR * prevR);
			prevR = lamp.Cylinders[i].R0;
			double Fl = Inup(nuAvg, lamp.Cylinders[i].T) * 4 * PI * K - qp;
			printf("%.3g ", Fl);
		}
		printf("\n");
	}


	//for (int l = 0; l < lamp.nuN - 1; l++)
	//{
	//	for (int i = 0; i < lamp.cylsN + 1; i++)
	//	{
	//		double sum = 0;
	//		for (int r = 0; r < raysN; r++)
	//		{
	//			sum += hostCylEnergy[(i * (lamp.nuN - 1) + l) * raysN + r];
	//		}
	//		printf("%.3g ", sum);
	//	}
	//	printf("\n");
	//}
	/*RayIntensityKernel<<<lamp.nuN - 1, raysN>>>(dev_rays, dev_cyls, lamp.cylsN, dev_intens,
		dev_TArr, dev_nuArr, lamp.TN, lamp.nuN, dev_kMatr);*/

	//Check for any errors launching the kernel
	cudaStatus = cudaGetLastError();
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "addKernel launch failed: %s\n", cudaGetErrorString(cudaStatus));
		goto Error;
	}

	// cudaDeviceSynchronize waits for the kernel to finish, and returns
	// any errors encountered during the launch.
	cudaStatus = cudaDeviceSynchronize();
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaDeviceSynchronize returned error code %d after launching RayIntensityKernel!\n%s\n", cudaStatus, cudaGetErrorString(cudaStatus));
		goto Error;
	}
	//for (int j = 0; j < lamp.nuN; j++)
	//{
	//	printf("%.4e\n", lamp.nuArr[j]);
	//	for (int i = 0; i < n * m + 1; i++)
	//		printf("%g ", host_intens[j * (n * m + 1) + i]);
	//	printf("\n");
	//}
	// Copy output vector from GPU buffer to host memory.
	//cudaStatus = cudaMemcpy(host_intens, dev_intens, sizeof(double) * (lamp.nuN - 1) * raysN, cudaMemcpyDeviceToHost);
	//if (cudaStatus != cudaSuccess) {
	//	fprintf(stderr, "cudaMemcpy failed!");
	//	goto Error;
	//}

Error:
	return cudaStatus;
}


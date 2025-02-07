#pragma once

#include "cuda_runtime.h"

class Cylinder
{
public:
	double R0;
	double T;
	__host__ __device__ Cylinder() { }
	__host__ __device__ Cylinder(double R0, double T) : R0(R0), T(T) { }

private:

};

#pragma once

#include "cuda_runtime.h"

class PairDouble
{
public:
	int count;
	double value1;
	double value2;
	__host__ __device__ PairDouble(double value1): value1(value1), count(1) {}
	__host__ __device__ PairDouble() : count(0) {}
	__host__ __device__ PairDouble(double value1, double value2) : value1(value1), value2(value2), count(2) {}
private:

};
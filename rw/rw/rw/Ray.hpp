#pragma once
#include "cuda_runtime.h"
#include "Vector3.hpp"

class Ray
{
public:

	Vector3 Start;
	Vector3 Direction;

	__host__ __device__ Ray() { }
	__host__ __device__ Ray(Vector3 start, Vector3 dir)
	{
		Start = start;
		Direction = dir;
	}

	__host__ __device__ Vector3 Point(double t)
	{
		return Start + Direction * t;
	}
private:

};

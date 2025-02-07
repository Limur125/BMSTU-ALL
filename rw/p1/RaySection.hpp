#pragma once
#include "cuda_runtime.h"
#include "Cylinder.hpp"
#include "Ray.hpp"

class RaySection
{
public:
	Cylinder cylinder;
	double tStart, tEnd;
	Ray ray;
	double length;
	__host__ __device__ RaySection() { tStart = 0; tEnd = 0; length = 0; }
	__host__ __device__ RaySection(Ray ray, double tStart, double tEnd, Cylinder cylinder) :
		ray(ray), tStart(tStart), tEnd(tEnd), cylinder(cylinder)
	{
		length = ray.Point(tStart).Length(ray.Point(tEnd));
	}

private:

};
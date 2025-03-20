#pragma once

#include "cuda_runtime.h"
struct Vector3
{
	double X;
	double Y;
	double Z;

	__host__ __device__ Vector3()
	{
		X = 0;
		Y = 0;
		Z = 0;
	}

	__host__ __device__ Vector3(double tetha, double phi)
	{
		X = cos(tetha) * cos(phi);
		Y = cos(tetha) * sin(phi);
		Z = sin(tetha);
	}

	__host__ __device__ Vector3(double x, double y, double z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	__host__ __device__ double Length() const
	{
		return sqrt(X * X + Y * Y + Z * Z);
	}

	__host__ __device__ double Length(Vector3 v1) const
	{
		double xdif = X - v1.X, ydif = Y - v1.Y, zdif = Z - v1.Z;
		return sqrt(xdif * xdif + ydif * ydif + zdif * zdif);
	}
	
	__host__ __device__ Vector3 Normalize()
	{
		return (*this) * (1.0 / this->Length());
	}

	__host__ __device__ Vector3 operator +(Vector3 v1)
	{
		double xres = X + v1.X, yres = Y + v1.Y, zres = Z + v1.Z;
		Vector3 res(xres, yres, zres);
		return res;
	}

	__host__ __device__ Vector3 operator -(Vector3 v1)
	{
		return *this + (-v1);
	}

	__host__ __device__ Vector3 operator -()
	{
		return (*this * -1);
	}

	__host__ __device__ Vector3 operator *(double k)
	{
		double xres = X * k, yres = Y * k, zres = Z * k;
		Vector3 res(xres, yres, zres);
		return res;
	}

	__host__ __device__ double operator *(Vector3 v)
	{
		return X * v.X + Y * v.Y + Z * v.Z;
	}

	__host__ __device__ Vector3 Reflect(Vector3 n)
	{
		double dProd = (*this) * n;
		return n * 2 * dProd - (*this);
	}
};
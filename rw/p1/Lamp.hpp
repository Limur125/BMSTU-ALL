#pragma once

#include <vector>

#include "Cylinder.hpp"


class Lamp
{
public:
	Lamp(double r, Cylinder* cylinders, int cylsN,
		double* TArr, int TN,
		double*nuArr, int nuN,
		double** kMatr) : R(r), Cylinders(cylinders), cylsN(cylsN), kMatr(kMatr),
		TArr(TArr), TN(TN),
		nuArr(nuArr), nuN(nuN)
	{ }

	double R;
	double** kMatr;
	double* TArr, * nuArr;
	int TN, nuN, cylsN;
	Cylinder* Cylinders;
private:

};
#include "PhysicsFunctions.h"

double Tavg(double r0, double r1, double R)
{
	const float T0 = 8500;
	const float m = 6;
	const float Tw = 2000;
	float h = r1 - r0;
	float r_avg = (r0 + r1) / 2;
	return T0 + (Tw - T0) * (pow(r1, m + 2) - pow(r0, m + 2)) / (h * r_avg * (m + 2) * pow(R, m));
}
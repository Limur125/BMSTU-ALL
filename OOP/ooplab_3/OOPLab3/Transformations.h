#ifndef TRANSFORMATIONS_H
#define TRANSFORMATIONS_H

class MoveT
{
	double dx;
	double dy;
	double dz;
public:
	MoveT(double dx, double dy, double dz) : dx(dx), dy(dy), dz(dz) {}
	double getdx() const { return dx; }
	double getdy() const { return dy; }
	double getdz() const { return dz; }
};

class ScaleT
{
	double kx;
	double ky;
	double kz;
public:
	ScaleT(double kx, double ky, double kz) : kx(kx), ky(ky), kz(kz) {}
	double getkx() const { return kx; }
	double getky() const { return ky; }
	double getkz() const { return kz; }
};

class RotateT
{
	double ax;
	double ay;
	double az;
public:
	RotateT(double ax, double ay, double az) : ax(ax), ay(ay), az(az) {}
	double getax() const { return ax; }
	double getay() const { return ay; }
	double getaz() const { return az; }
};

#endif // TRANSFORMATIONS_H

#ifndef POINT_H
#define POINT_H

#include <iostream>
#include <cmath>
#include "Transformations.h"

using namespace std;

class Point
{
    double x;
    double y;
    double z;
    Point move(const MoveT& mt);
    Point scale(const ScaleT& st, const Point &center);
    Point rotate(const RotateT rt, const Point& center);
    void _rotate(double &x, double &y, double a);
public:
    Point() = default;
    Point(double x, double y, double z) : x(x), y(y), z(z) {}
    Point(const Point& p) { *this = p; }
    Point(const Point&& p) noexcept { *this = p; }
    Point& operator=(const Point& p) { x = p.x, y = p.y, z = p.z; return *this; }
    Point& operator=(const Point&& p) noexcept { x = p.x, y = p.y, z = p.z; return *this; }
    Point transform(const MoveT& mt, const ScaleT& st, const RotateT& rt, const Point& center);
    friend istream& operator>>(istream& is, Point& point);
    double getx() const { return x; }
    double gety() const { return y; }
    double getz() const { return z; }
};

#endif // POINT_H

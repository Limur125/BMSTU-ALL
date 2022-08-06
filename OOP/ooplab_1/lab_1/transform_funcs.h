#ifndef TRANSFORM_FUNCS_H
#define TRANSFORM_FUNCS_H

#include <cmath>
#include "point.h"

struct move_t
{
    double dx;
    double dy;
    double dz;
};

struct rotate_t
{
    double ax;
    double ay;
    double az;
};

struct scale_t
{
    double kx;
    double ky;
    double kz;
};

ret_code points_move(points_t &points, const move_t m);
ret_code points_rotate(points_t &points, const point_t center, const rotate_t r);
ret_code points_scale(points_t &points, const point_t center, const scale_t s);
point_t point_move(const point_t point, const move_t m);
point_t point_rotate(const point_t point, const point_t center, const rotate_t r);
point_t point_scale(const point_t point, const point_t center, const scale_t s);


#endif // TRANSFORM_FUNCS_H

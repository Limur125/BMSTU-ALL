#ifndef POINT_H
#define POINT_H

#include <iostream>
#include <cstdlib>
#include "return_codes.h"


struct point_t
{
    double x;
    double y;
    double z;
};

struct points_t
{
    int count;
    point_t *points;
};

ret_code allocate_points(points_t &points);
ret_code read_point(point_t &point, FILE *f);
void points_init(points_t &points);
void points_free(points_t &points);
ret_code load_points(points_t &points, FILE *f);
void point_init(point_t &point);


#endif // POINT_H

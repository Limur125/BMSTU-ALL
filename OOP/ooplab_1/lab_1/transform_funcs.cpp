#include "transform_funcs.h"

static rotate_t to_radians(rotate_t r)
{
    r.ax = r.ax * M_PI / 180;
    r.ay = r.ay * M_PI / 180;
    r.az = r.az * M_PI / 180;
    return r;
}

ret_code points_move(points_t &points, const move_t m)
{
    for (int i = 0; i < points.count; i++)
        points.points[i] = point_move(points.points[i], m);
    return OK;
}

ret_code points_rotate(points_t &points, const point_t center, const rotate_t r)
{
    for (int i = 0; i < points.count; i++)
        points.points[i] = point_rotate(points.points[i], center, to_radians(r));
    return OK;
}

ret_code points_scale(points_t &points, const point_t center, const scale_t s)
{
    for (int i = 0; i < points.count; i++)
        points.points[i] = point_scale(points.points[i], center, s);
    return OK;
}

point_t point_move(const point_t point, const move_t m)
{
    point_t point_c = point;

    point_c.x += m.dx;
    point_c.y += m.dy;
    point_c.z += m.dz;

    return point_c;
}

void rotate(double &x, double &y, double a)
{
    double tmp_x = x;
    double tmp_y = y;
    tmp_x = x * cos(a) + y * sin(a);
    tmp_y = -x * sin(a) + y * cos(a);
    x = tmp_x;
    y = tmp_y;
}

point_t rotate_rel(point_t point, rotate_t r)
{
    rotate(point.y, point.z, r.ax);
    rotate(point.x, point.z, r.ay);
    rotate(point.x, point.y, r.az);
    return point;
}

move_t move_init(double dx, double dy, double dz)
{
    move_t m = {dx, dy, dz};
    return m;
}


point_t point_rotate(const point_t point, const point_t center, const rotate_t r)
{
    point_t point_c = point; 

    move_t m_neg = move_init(-center.x, -center.y, -center.z);
    point_c = point_move(point_c, m_neg);

    point_c = rotate_rel(point_c, r);

    move_t m_pos = move_init(center.x, center.y, center.z);
    point_c = point_move(point_c, m_pos);

    return point_c;
}

point_t point_scale(const point_t point, const point_t center, const scale_t s)
{
    point_t point_c = point; 

    point_c.x = point_c.x * s.kx + (1 - s.kx) * center.x;
    point_c.y = point_c.y * s.ky + (1 - s.ky) * center.y;
    point_c.z = point_c.z * s.kz + (1 - s.kz) * center.z;

    return point_c;
}
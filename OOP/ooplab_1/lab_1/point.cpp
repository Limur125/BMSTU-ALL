#include "point.h"

static ret_code read_count(points_t &points, FILE *f)
{
    if (f == NULL)
        return FILE_OPEN_ERROR;

    if (fscanf(f ,"%d", &points.count) != 1)
        return FILE_FORMAT_ERROR;

    if (points.count <= 0)
        return NO_POINTS;

    return OK;
}

ret_code allocate_points(points_t &points)
{
    if (points.count <= 0)
        return NO_POINTS;

    points.points = (point_t *) malloc(points.count * sizeof(point_t));

    if (points.points == NULL)
        return MEMORY_ERROR;

    return OK;
}

ret_code read_point(point_t &point, FILE *f)
{
    if (f == NULL)
        return FILE_OPEN_ERROR;

    if (fscanf(f, "%lf%lf%lf", &point.x, &point.y, &point.z) != 3)
        return FILE_FORMAT_ERROR;
        
    return OK; 
}

void points_init(points_t &points)
{
    points.points = NULL;
    points.count = 0;
}

void point_init(point_t &point)
{
    point.x = 0;
    point.y = 0;
    point.z = 0;
}

void points_free(points_t &points)
{
    free(points.points);
    points_init(points);
}

ret_code read_points(points_t &points, FILE *f)
{
    ret_code rc = OK;

    for (int i = 0; rc == OK && i < points.count; i++)
        rc = read_point(points.points[i], f);

    return rc;
}

ret_code load_points(points_t &points, FILE *f)
{
    if (f == NULL)
        return FILE_OPEN_ERROR;

    ret_code rc = read_count(points, f);

    if (rc != OK)
        return rc;
    
    rc = allocate_points(points);

    if (rc != OK)
        return rc;

    if (rc == OK)
        rc = read_points(points, f);

    if (rc != OK)
        points_free(points);

    return rc;
}

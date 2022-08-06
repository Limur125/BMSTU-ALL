#include "render_funcs.h"

line_t create_line(const points_t points, const link_t links)
{
    line_t line;
    point_t &point1 = points.points[links.p1];
    point_t &point2 = points.points[links.p2];
    line.p1 = point1;
    line.p2 = point2;
    return line;
}

void render_lines(canvas_t canv, points_t points, links_t links)
{
    for (int i = 0; i < links.count; ++i)
    {
        line_t line = create_line(points, links.links[i]);
        my_addline(canv, line.p1, line.p2);
    }
}

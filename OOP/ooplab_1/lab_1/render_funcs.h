#ifndef RENDER_FUNCS_H
#define RENDER_FUNCS_H

#include "canvas_funcs.h"
#include "return_codes.h"
#include "link.h"

struct line_t
{
    point_t p1;
    point_t p2;
};

void render_lines(canvas_t canv, points_t points, links_t links);

#endif // RENDER_FUNCS_H

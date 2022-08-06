#include "canvas_funcs.h"

void my_addline(canvas_t &canv, point_t p1, point_t p2)
{
    double canvas_zero_x = canv.w / 2;
    double canvas_zero_y = canv.h / 2;

    p1.x += canvas_zero_x;
    p1.y += canvas_zero_y;

    p2.x += canvas_zero_x;
    p2.y += canvas_zero_y;

    canv.scene->addLine(p1.x, p1.y, p2.x, p2.y);
}


void my_clear(canvas_t &canv)
{
    canv.scene->clear();
}

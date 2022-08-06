#ifndef CANVAS_FUNCS_H
#define CANVAS_FUNCS_H

#include <QGraphicsView>
#include "point.h"

struct canvas_t
{
    QGraphicsScene* scene;
    int w;
    int h;
};

void my_addline(canvas_t &canv, point_t p1, point_t p2);
void my_clear(canvas_t &canv);

#endif // CANVAS_FUNCS_H

#ifndef PICTURE_H
#define PICTURE_H

#include <iostream>
#include <cstdlib>
#include "canvas_funcs.h"
#include "render_funcs.h"
#include "return_codes.h"
#include "transform_funcs.h"
#include "link.h"
#include "point.h"

struct picture_t
{
    links_t links;
    points_t points;
    point_t center;
};

picture_t &picture_init();
void picture_free(picture_t &pic);
ret_code picture_load(picture_t &pic, const char filename[]);
ret_code picture_render(canvas_t &canv, const picture_t pic);
ret_code picture_move(picture_t &pic, const move_t move);
ret_code picture_rotate(picture_t &pic, const rotate_t rot);
ret_code picture_scale(picture_t &pic, const scale_t scale);

#endif // PICTURE_H

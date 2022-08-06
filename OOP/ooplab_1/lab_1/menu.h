#ifndef MENU_H
#define MENU_H

#include "canvas_funcs.h"
#include "render_funcs.h"
#include "picture.h"
#include "return_codes.h"

enum task_en
{
    START,
    LOAD,
    RENDER,
    MOVE,
    ROTATE,
    SCALE,
    FINISH
};

struct menu_t 
{
    task_en task;
    union
    {
        const char *filename;
        canvas_t render_field;
        move_t move_field;
        scale_t scale_field;
        rotate_t rotate_field;
    };
};
ret_code menu(menu_t &choose);

#endif // MENU_H

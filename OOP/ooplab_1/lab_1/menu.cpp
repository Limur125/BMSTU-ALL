#include "menu.h"

ret_code menu(menu_t &choose)
{
    ret_code rc = OK;
    static picture_t pic = picture_init();
    switch (choose.task)
    {
        case START:
            rc = OK;
            break;
        case LOAD:
            rc = picture_load(pic, choose.filename);
            break;
        case RENDER:
            rc = picture_render(choose.render_field, pic);
            break;
        case MOVE:
            rc = picture_move(pic, choose.move_field);
            break;
        case ROTATE:
            rc = picture_rotate(pic, choose.rotate_field);
            break;
        case SCALE:
            rc = picture_scale(pic, choose.scale_field);
            break;
        case FINISH:
            rc = OK;
            picture_free(pic);
            break;
        default:
            rc = UNKNOWN_COMMAND;
            break;
    }
    return rc;
}

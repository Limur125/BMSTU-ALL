#include "picture.h"

picture_t &picture_init()
{
    static picture_t pic;

    points_init(pic.points);
    links_init(pic.links);
    point_init(pic.center);

    return pic;
}

void picture_free(picture_t &pic)
{
    links_free(pic.links);
    points_free(pic.points);
}

ret_code check_link(const link_t link, const int points_count)
{
    if (link.p1 < points_count && link.p2 < points_count)
        return OK;

    return FILE_FORMAT_ERROR;
}

ret_code check_links(const links_t links, const points_t points)
{
    ret_code rc = OK;

    for (int i = 0; rc == OK && i < links.count; i++)
        rc = check_link(links.links[i], points.count);

    return rc;
}

ret_code is_pic_valid(const picture_t pic)
{
    return check_links(pic.links, pic.points);
}

ret_code picture_read(picture_t &pic, FILE *f)
{
    if (f == NULL)
        return FILE_OPEN_ERROR;
        
    ret_code rc = load_points(pic.points, f);

    if (rc == OK)
    {
        rc =  load_links(pic.links, f);
        if (rc != OK)
            points_free(pic.points);
    }
    return rc;
}

ret_code picture_load(picture_t &pic, const char filename[])
{
    FILE *f = fopen(filename, "r");

    if (f == NULL)
        return FILE_OPEN_ERROR;

    ret_code rc = OK;
    picture_t tmp_pic;

    rc = picture_read(tmp_pic, f);
    
    fclose(f);

    if (rc == OK)
    {
        rc = is_pic_valid(tmp_pic);
        if (rc != OK)
            picture_free(tmp_pic);
    }
    
    if (rc == OK)
    {
        picture_free(pic);
        pic = tmp_pic;
    }

    return rc;
}

ret_code picture_render(canvas_t &canv, const picture_t pic)
{
    my_clear(canv);
    render_lines(canv, pic.points, pic.links);
    return OK;
}

ret_code picture_move(picture_t &pic, const move_t move)
{
    points_move(pic.points, move);
    pic.center = point_move(pic.center, move);
    return OK;
}

ret_code picture_rotate(picture_t &pic, const rotate_t rot)
{
    points_rotate(pic.points, pic.center, rot);
    return OK;
}

ret_code picture_scale(picture_t &pic, const scale_t scale)
{
    points_scale(pic.points, pic.center, scale);
    return OK;
}

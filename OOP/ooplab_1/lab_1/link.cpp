#include "link.h"

static ret_code read_count(links_t &links, FILE *f)
{
    if (f == NULL)
        return FILE_OPEN_ERROR;

    if (fscanf(f ,"%d", &links.count) != 1)
        return FILE_FORMAT_ERROR;

    if (links.count <= 0)
        return NO_LINKS;

    return OK;
}

ret_code allocate_links(links_t &links)
{
    if (links.count <= 0)
        return NO_LINKS;

    links.links = (link_t *) malloc(links.count * sizeof(link_t));

    if (links.links == NULL)
        return MEMORY_ERROR;

    return OK;
}

ret_code read_link(link_t &link, FILE *f)
{
    if (f == NULL)
        return FILE_OPEN_ERROR;

    if (fscanf(f, "%d%d", &link.p1, &link.p2) != 2)
        return FILE_FORMAT_ERROR;

    if (link.p1 < 0 || link.p2 < 0)
        return FILE_FORMAT_ERROR;

    return OK; 
}

void links_init(links_t &links)
{
    links.count = 0;
    links.links = NULL;
}

void links_free(links_t &links)
{
    free(links.links);
    links_init(links);
}

ret_code read_links(links_t links, FILE *f)
{
    ret_code rc = OK;

    for (int i = 0; rc == OK && i < links.count; i++)
        rc = read_link(links.links[i], f);
    
    return rc;
}

ret_code load_links(links_t &links, FILE *f)
{
    if (f == NULL)
        return FILE_OPEN_ERROR;

    ret_code rc = read_count(links, f);

    if (rc != OK)
        return rc;
    
    rc = allocate_links(links);

    if (rc != OK)
        return rc;

    if (rc == OK)
        rc = read_links(links, f);

    if (rc != OK)
        links_free(links);

    return rc;
}

#ifndef LINK_H
#define LINK_H

#include <iostream>
#include <cstdlib>
#include "return_codes.h"

struct link_t
{
    int p1;
    int p2;
};

struct links_t
{
    int count;
    link_t *links;
};

void links_init(links_t &links);
void links_free(links_t &links);
ret_code load_links(links_t &links, FILE *f);
ret_code allocate_links(links_t &links);
ret_code read_link(link_t &link, FILE *f);

#endif // LINK_H

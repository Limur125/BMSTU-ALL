#include "Link.h"

Link& Link::operator=(const Link& link)
{
    begin = link.begin;
    end = link.end;

    return *this;
}

istream& operator>>(std::istream& is, Link& link)
{
    size_t p1, p2;
    is >> p1 >> p2;
    link.begin = p1;
    link.end = p2;

    return is;
}
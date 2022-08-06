#ifndef LINK_H
#define LINK_H

#include <iostream>
#include <cstdlib>

using namespace std;

class Link
{
    size_t begin;
    size_t end;
public:
    Link() : begin(0), end(0) {}
    Link(const size_t begin, const size_t end) : begin(begin), end(end) {}
    explicit Link(const Link& link) : begin(link.begin), end(link.end) {}
    ~Link() = default;
    Link& operator=(const Link& link);
    size_t get_begin() { return begin; }
    size_t get_end() { return end; }
    friend istream& operator>>(std::istream& is, Link& link);
};

#endif // LINK_H

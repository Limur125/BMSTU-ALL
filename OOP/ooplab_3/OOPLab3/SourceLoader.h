#ifndef SOURCELOADER_H
#define SOURCELOADER_H

#include <string>
#include <vector>
#include "Point.h"
#include "Link.h"

using namespace std;

class SourceLoader
{
public:
    SourceLoader() = default;
    ~SourceLoader() noexcept = default;

    virtual void open(string source_name) = 0;
    virtual bool is_open() const = 0;
    virtual void close() = 0;
    virtual vector<Point> read_points() = 0;
    virtual vector<Link> read_links() = 0;
};

#endif // SOURCELOADER_H

#ifndef DRAWER_H
#define DRAWER_H

#include <memory>
#include <QGraphicsScene>
#include "Point.h"

using namespace std;

class BaseDrawer
{
public:
    BaseDrawer() = default;
    ~BaseDrawer() = default;

    virtual void clear() = 0;
    virtual void create_line(const Point &point1, const Point &point2) = 0;
};

class QtDrawer : public BaseDrawer
{
    shared_ptr<QGraphicsScene> scene;
public:
	QtDrawer() = default;
    explicit QtDrawer(shared_ptr<QGraphicsScene> s) : scene(s) {}
    ~QtDrawer() = default;

    virtual void clear();
    virtual void create_line(const Point& point1, const Point& point2);
};

#endif // DRAWER_H

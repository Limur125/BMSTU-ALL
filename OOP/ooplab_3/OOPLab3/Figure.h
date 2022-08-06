#ifndef FIGURE_H
#define FIGURE_H

#include <vector>

#include "BaseObject.h"
#include "BaseVisitor.h"
#include "Point.h"
#include "Link.h"

using namespace std;

class VisibleObject;

class FigureData
{
    vector<Point> points;
    vector<Link> links;
    Point center;
public:
    FigureData() = default;
    FigureData(vector<Point>& points, vector<Link>& links, Point& center) { this->points = points, this->links = links, this->center = center; }
    FigureData(vector<Point>& points, vector<Link>& links) { this->points = points, this->links = links; }
    FigureData& operator=(const shared_ptr<FigureData> d) { points = d->points, links = d->links; return *this; };
    const vector<Point>& get_points() const { return points; }
    const vector<Link>& get_links() const { return links; }
    const Point get_center() const { return center; }
    void add(const Point& p) { points.push_back(p); }
    void add(const Link& l) { links.push_back(l); }
    void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center);
    void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt);
};

class BaseFigure : public VisibleObject 
{
public:
    virtual const Point get_center() = 0;
};

class Figure: public BaseFigure
{
    friend Visitor;
    shared_ptr<FigureData> fd;
public:
    Figure() : fd(make_shared<FigureData>()) {}
    explicit Figure(const shared_ptr<FigureData> fd) : fd(fd) {}
    ~Figure() = default;
    virtual const Point get_center() override { return fd->get_center(); }
    void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt) override;
    void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center) override;
    void accept(shared_ptr<BaseVisitor> visitor) override { visitor->visit(*this); }
};

#endif // FIGURE_H

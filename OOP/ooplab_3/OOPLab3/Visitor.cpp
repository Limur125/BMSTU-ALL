#include "Visitor.h"

Point Visitor::project(Point p)
{
    shared_ptr<CameraData> cd = camera->cd;
    Point cam_pos = cd->get_pos();
    MoveT mt(-cam_pos.getx(), -cam_pos.gety(), -cam_pos.getz());
    RotateT rt(-cd->get_xdir(), -cd->get_ydir(), -cd->get_zdir());
    ScaleT st(1, 1, 1);
    Point new_p = p.transform(mt, st, rt, cam_pos);
    return new_p;
}

void Visitor::visit(Figure& figure)
{
    if (!canvas)
        throw CanvasException(__FILE__, typeid(*this).name(), __LINE__);

    if (!camera)
        throw CameraException(__FILE__, typeid(*this).name(), __LINE__);
    shared_ptr<FigureData> fd = figure.fd;
    vector<Point> points = fd->get_points();
    vector<Link> links = fd->get_links();

    for (auto& link : links)
    {
        Point p1 = project(points[link.get_begin()]);
        Point p2 = project(points[link.get_end()]);
        canvas->create_line(p1, p2);
    }
}

void Visitor::visit(Camera& camera)
{

}

void Visitor::visit(Composite& composite)
{

}

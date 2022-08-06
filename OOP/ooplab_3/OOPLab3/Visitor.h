#ifndef VISITOR_H
#define VISITOR_H

#include <memory>
#include "Drawer.h"
#include "BaseVisitor.h"
#include "Camera.h"
#include "Figure.h"
#include "Composite.h"
#include "Exceptions.h"

using namespace std;

class Visitor: public BaseVisitor
{
    shared_ptr<BaseDrawer> canvas;
    shared_ptr<Camera> camera;
    Point project(Point p);
public:
    Visitor() = default;
    Visitor(shared_ptr<BaseDrawer> canvas, shared_ptr<Camera> camera) : canvas(canvas), camera(camera) {}
    ~Visitor() = default;

    void visit(Camera& camera) override;
    void visit(Figure& figure) override;
    void visit(Composite& composite) override;
};

#endif // VISITOR_H

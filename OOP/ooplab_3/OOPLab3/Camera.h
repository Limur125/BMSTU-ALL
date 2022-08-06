#ifndef CAMERA_H
#define CAMERA_H

#include "BaseObject.h"
#include "Point.h"
#include "Transformations.h"
#include "BaseVisitor.h"

class InvisibleObject;

class CameraData
{
    Point position;
    double ax, ay, az;
    void move(const MoveT& mt);
    void rotate(const RotateT& rt);
public:
    CameraData() = default;
    CameraData(const Point& position, double ax, double ay, double az) : position(position), ax(ax), ay(ay), az(az) {}
    CameraData(const shared_ptr<CameraData> d) { position = Point(d->position.getx(), d->position.gety(), d->position.getz()), ax = d->ax, ay = d->ay, az = d->az; }
    double get_xdir() const { return ax; }
    double get_ydir() const { return ay; }
    double get_zdir() const { return az; }
    Point get_pos() const { return position; }
    void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center);
};

class Camera: public InvisibleObject
{
    friend Visitor;
    shared_ptr<CameraData> cd;
public:
    Camera() : cd(make_shared<CameraData>()) {}
    explicit Camera(const shared_ptr<CameraData> cd) : cd(cd) {}
    explicit Camera(const shared_ptr<Camera> c) { cd = make_shared<CameraData>(c->cd->get_pos(), c->cd->get_xdir(), c->cd->get_ydir(), c->cd->get_zdir()); }
    ~Camera() = default;
    virtual const Point get_center() override { return cd->get_pos(); }
    void transformation(const MoveT & mt, const ScaleT & st, const RotateT & rt);
    void transformation(const MoveT & mt, const ScaleT & st, const RotateT & rt, Point & center);
    void accept(shared_ptr<BaseVisitor> visitor) { visitor->visit(*this); }
};

#endif // CAMERA_H

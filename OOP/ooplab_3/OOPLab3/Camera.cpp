#include "Camera.h"

void CameraData::move(const MoveT& mt)
{
    RotateT rt(0, 0, 0);
    ScaleT st(1, 1, 1);
    Point p;
    position = position.transform(mt, st, rt, p);
}

void CameraData::rotate(const RotateT& rt)
{
    ax += rt.getax();
    ay += rt.getay();
    az += rt.getaz();
}

void Camera::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt)
{
    Point p(0, 0, 0);
    cd->transformation(mt, st, rt, p);
}
void Camera::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center)
{
    cd->transformation(mt, st, rt, center);
}

void CameraData::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center)
{
    move(mt);
    rotate(rt);
}



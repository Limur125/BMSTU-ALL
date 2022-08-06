#ifndef BASEACTION_H
#define BASEACTION_H

#include <memory>
#include <string>
#include <cstdlib>

#include "Drawer.h"
#include "LoadManager.h"
#include "SceneManager.h"
#include "DrawManager.h"

using namespace std;

class BaseAction
{
public:
    BaseAction() = default;
    ~BaseAction() = default;

    virtual void execute() = 0;
};

class BaseFigureAction : public BaseAction
{
public:
	BaseFigureAction() = default;
    ~BaseFigureAction() = default;
    virtual void execute() = 0;
};

class BaseCameraAction : public BaseAction
{
public:
    BaseCameraAction() = default;
    ~BaseCameraAction() = default;
    virtual void execute() = 0;
};

class BaseSceneAction : public BaseAction
{

public:
    BaseSceneAction() = default;
    ~BaseSceneAction() = default;
    virtual void execute() = 0;
};

class LoadFigure : public BaseFigureAction
{
    string filename;
    size_t index;
public:
    LoadFigure(string filename, size_t ind) : filename(filename), index(ind) {}
    ~LoadFigure() = default;
    virtual void execute() override;
};

class DeleteFigure : public BaseFigureAction
{
    size_t obj_index;
public:
    DeleteFigure(size_t ind) : obj_index(ind) {}
    ~DeleteFigure() = default;
    virtual void execute() override;
};

class MoveFigure : public BaseFigureAction
{
    size_t obj_index;
    double dx, dy, dz;
public:
    MoveFigure(size_t ind, double dx, double dy, double dz) : obj_index(ind), dx(dx), dy(dy), dz(dz) {}
    ~MoveFigure() = default;
    virtual void execute() override;
};

class RotateFigure : public BaseFigureAction
{
    size_t obj_index;
    double ax, ay, az;
public:
    RotateFigure(size_t ind, double ax, double ay, double az) : obj_index(ind), ax(ax), ay(ay), az(az) {}
    ~RotateFigure() = default;
    virtual void execute() override;
};

class ScaleFigure : public BaseFigureAction
{
    size_t obj_index;
    double kx, ky, kz;
public:
    ScaleFigure(size_t ind, double kx, double ky, double kz) : obj_index(ind), kx(kx), ky(ky), kz(kz) {}
    ~ScaleFigure() = default;
    virtual void execute() override;
};

class AddCamera : public BaseCameraAction
{
    size_t index;
public:
    AddCamera(size_t ind) : index(ind) {}
    ~AddCamera() = default;
    virtual void execute() override;
};

class DeleteCamera : public BaseCameraAction
{
    size_t cam_index;
public:
    DeleteCamera(size_t cam_index) : cam_index(cam_index) {}
    ~DeleteCamera() = default;
    virtual void execute() override;
};

class SetCamera : public BaseCameraAction
{
    size_t cam_index;
public:
    SetCamera(size_t cam_index) : cam_index(cam_index) {}
    ~SetCamera() = default;
    virtual void execute() override;
};

class MoveCamera : public BaseCameraAction
{
    size_t cam_index;
    double dx, dy, dz;
public:
    MoveCamera(size_t cam_index, double dx, double dy, double dz) : cam_index(cam_index), dx(dx), dy(dy), dz(dz) {}
    ~MoveCamera() = default;
    virtual void execute() override;
};

class RotateCamera : public BaseCameraAction
{
    size_t cam_index;
    double ax, ay, az;
public:
    RotateCamera(size_t cam_index, double ax, double ay, double az) : cam_index(cam_index), ax(ax), ay(ay), az(az)  {}
    ~RotateCamera() = default;
    virtual void execute() override;
};

class RenderScene : public BaseSceneAction
{
    shared_ptr<BaseDrawer> canvas;
public:
    RenderScene(shared_ptr<BaseDrawer> canvas) : canvas(canvas) {}
    ~RenderScene() = default;
    virtual void execute() override;
};

class MoveScene : public BaseSceneAction
{
    double dx, dy, dz;
public:
    MoveScene(double dx, double dy, double dz) : dx(dx), dy(dy), dz(dz) {}
    ~MoveScene() = default;
    virtual void execute() override;
};

class RotateScene : public BaseSceneAction
{
    double ax, ay, az;
public:
    RotateScene(double ax, double ay, double az) : ax(ax), ay(ay), az(az) {}
    ~RotateScene() = default;
    virtual void execute() override;
};

class ScaleScene : public BaseSceneAction
{
    double kx, ky, kz;
public:
    ScaleScene(double kx, double ky, double kz) : kx(kx), ky(ky), kz(kz) {}
    ~ScaleScene() = default;
    virtual void execute() override;
};

#endif // BASEACTION_H

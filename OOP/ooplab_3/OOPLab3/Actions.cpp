#include "Actions.h"

void LoadFigure::execute()
{
	shared_ptr<Figure> fig = LoadManagerSingleton::instance().load_figure(filename);
    SceneManagerSingleton::instance().get_scene()->add_object(fig, index);
}

void DeleteFigure::execute()
{
	ObjectIterator it = SceneManagerSingleton::instance().get_scene()->get_object(this->obj_index);
	SceneManagerSingleton::instance().get_scene()->remove_object(it);
}

void MoveFigure::execute()
{
	ObjectIterator it = SceneManagerSingleton::instance().get_scene()->get_object(this->obj_index);
	MoveT mt(dx, dy, dz);
	RotateT rt(0, 0, 0);
	ScaleT st(1, 1, 1);
	shared_ptr<BaseObject> obj = *it;
	obj->transformation(mt, st, rt);
}

void RotateFigure::execute()
{
	ObjectIterator it = SceneManagerSingleton::instance().get_scene()->get_object(this->obj_index);
	MoveT mt(0, 0, 0);
	RotateT rt(ax, ay, az);
	ScaleT st(1, 1, 1);
	shared_ptr<BaseObject> obj = *it;
	obj->transformation(mt, st, rt);
}

void ScaleFigure::execute()
{
	ObjectIterator it = SceneManagerSingleton::instance().get_scene()->get_object(this->obj_index);
	MoveT mt(0, 0, 0);
	RotateT rt(0, 0, 0);
	ScaleT st(kx, ky, kz);
	shared_ptr<BaseObject> obj = *it;
	obj->transformation(mt, st, rt);
}

void AddCamera::execute()
{
	shared_ptr<Camera> new_cam = SceneManagerSingleton::instance().get_new_camera();
    SceneManagerSingleton::instance().get_scene()->add_object(new_cam, index);
}

void DeleteCamera::execute()
{
	ObjectIterator it = SceneManagerSingleton::instance().get_scene()->get_object(cam_index);
	SceneManagerSingleton::instance().get_scene()->remove_object(it);
}

void SetCamera::execute()
{
	SceneManagerSingleton::instance().set_camera(cam_index);
}

void MoveCamera::execute()
{
	shared_ptr<Camera> cam = SceneManagerSingleton::instance().get_camera();
	MoveT mt(dx, dy, dz);
	RotateT rt(0, 0, 0);
	ScaleT st(1, 1, 1);
	cam->transformation(mt, st, rt);
}

void RotateCamera::execute()
{
	shared_ptr<Camera> cam = SceneManagerSingleton::instance().get_camera();
	MoveT mt(0, 0, 0);
	RotateT rt(ax, ay, az);
	ScaleT st(1, 1, 1);
	cam->transformation(mt, st, rt);
}

void RenderScene::execute()
{
	DrawManagerSingleton::instance().set_scene(SceneManagerSingleton::instance().get_scene());
	shared_ptr<Camera> cam = SceneManagerSingleton::instance().get_camera();
    if (!cam)
        return;
	DrawManagerSingleton::instance().draw(cam, canvas);
}

void MoveScene::execute()
{
	shared_ptr<BaseObject> com = SceneManagerSingleton::instance().get_scene()->get_objects();
	Point p = SceneManagerSingleton::instance().get_scene()->get_center();
	MoveT mt(dx, dy, dz);
	RotateT rt(0, 0, 0);
	ScaleT st(1, 1, 1);
	com->transformation(mt, st, rt, p);
}

void RotateScene::execute()
{
	shared_ptr<BaseObject> com = SceneManagerSingleton::instance().get_scene()->get_objects();
	Point p = SceneManagerSingleton::instance().get_scene()->get_center();
	MoveT mt(0, 0, 0);
	RotateT rt(ax, ay, az);
	ScaleT st(1, 1, 1);
	com->transformation(mt, st, rt, p);
}

void ScaleScene::execute()
{
	shared_ptr<BaseObject> com = SceneManagerSingleton::instance().get_scene()->get_objects();
	Point p = SceneManagerSingleton::instance().get_scene()->get_center();
	MoveT mt(0, 0, 0);
	RotateT rt(0, 0, 0);
	ScaleT st(kx, ky, kz);
	com->transformation(mt, st, rt, p);
}

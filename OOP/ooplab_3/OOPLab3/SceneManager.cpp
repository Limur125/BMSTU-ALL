#include "SceneManager.h"

shared_ptr<Scene> SceneManager::get_scene()
{
    if (!scene)
        set_scene(make_shared<Scene>());
    return scene;
}

void SceneManager::set_scene(shared_ptr<Scene> scene)
{
    this->scene = scene;
}

void SceneManager::set_camera(int index)
{
    ObjectIterator it = scene->get_object(index);
    set_camera(it);
}

void SceneManager::set_camera(ObjectIterator& it)
{
    camera = dynamic_pointer_cast<Camera>(*it);
}

shared_ptr<Camera> SceneManager::get_camera()
{
    if (camera.expired())
        return nullptr;
    return camera.lock();
}

shared_ptr<Camera> SceneManager::get_new_camera()
{
    shared_ptr<Camera> cam = get_camera();
    if (!cam)
        cam = make_shared<Camera>(make_shared<CameraData>(Point(20, 0, 0), 0, 0, 0));
    shared_ptr<Camera> new_cam = make_shared<Camera>(cam);
    return new_cam;
}

SceneManager& SceneManagerSingleton::instance()
{
    static unique_ptr<SceneManager> manager = make_unique<SceneManager>();
    return *manager;
}

#ifndef SCENEMANAGER_H
#define SCENEMANAGER_H

#include <memory>
#include "BaseManager.h"
#include "Scene.h"
#include "Camera.h"

using namespace std;

class SceneManager : public BaseManager
{
    shared_ptr<Scene> scene;
    weak_ptr<Camera> camera;
public:
    shared_ptr<Scene> get_scene();
    void set_scene(shared_ptr<Scene> scene);
    shared_ptr<Camera> get_new_camera();
    shared_ptr<Camera> get_camera();
    void set_camera(int index);
    void set_camera(ObjectIterator& it);
};

class SceneManagerSingleton
{
public:
    SceneManagerSingleton() = delete;
    static SceneManager& instance();
};

#endif // SCENEMANAGER_H

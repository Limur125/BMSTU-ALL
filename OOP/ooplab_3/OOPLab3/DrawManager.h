#ifndef DRAWMANAGER_H
#define DRAWMANAGER_H

#include <memory>

#include "BaseManager.h"
#include "Drawer.h"
#include "Visitor.h"
#include "Camera.h"
#include "Scene.h"

using namespace std;

class DrawManager : public BaseManager
{
    shared_ptr<Scene> scene;
public:
    void set_scene(shared_ptr<Scene> scene) { this->scene = scene; }
    void draw(shared_ptr<Camera> camera, shared_ptr<BaseDrawer> canvas);
};

class DrawManagerSingleton
{
public:
    DrawManagerSingleton() = delete;
    static DrawManager& instance();
};


#endif // DRAWMANAGER_H

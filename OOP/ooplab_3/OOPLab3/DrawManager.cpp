#include "DrawManager.h"

DrawManager& DrawManagerSingleton::instance()
{
    static unique_ptr<DrawManager> manager = make_unique<DrawManager>();
    return *manager;
}

void DrawManager::draw(shared_ptr<Camera> camera, shared_ptr<BaseDrawer> canvas)
{
    shared_ptr<BaseVisitor> visitor = make_shared<Visitor>(canvas, camera);
    scene->accept(visitor);
}

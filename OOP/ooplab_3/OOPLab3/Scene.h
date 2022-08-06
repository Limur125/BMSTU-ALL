#ifndef SCENE_H
#define SCENE_H

#include <cstdlib>
#include <memory>

#include "BaseObject.h"
#include "Composite.h"

using namespace std;

class Scene
{
public:
    Scene() { objects = make_shared<Composite>(); }
    ~Scene() = default;
    shared_ptr<Composite> get_objects() { return objects; }
    Point get_center() { return objects->get_center(); }

    void accept(shared_ptr<BaseVisitor> visitor);

    void add_object(shared_ptr<BaseObject> obj, size_t index);
    void remove_object(ObjectIterator& it);

    ObjectIterator get_object(int id) const;

    ObjectIterator begin();
    ObjectIterator end();

    Scene& operator=(const Scene& scene);

private:
    shared_ptr<Composite> objects;
};

#endif // SCENE_H

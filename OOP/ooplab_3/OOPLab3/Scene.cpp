#include "Scene.h"

void Scene::add_object(shared_ptr<BaseObject> obj, size_t index)
{
    objects->add(obj, index);
};

void Scene::accept(shared_ptr<BaseVisitor> visitor)
{
    objects->accept(visitor);
}

ObjectIterator Scene::begin()
{
    return objects->begin();
}
ObjectIterator Scene::end()
{
    return objects->end();
}

Scene& Scene::operator=(const Scene& scene)
{
    objects = scene.objects;
    return (*this);
};

void Scene::remove_object(ObjectIterator& it)
{
    objects->remove(it);
}

ObjectIterator Scene::get_object(int id) const
{
    int id_tmp = 0;
    for (const auto& elem : *objects)
    {
        if (elem->get_id() == id)
            return objects->begin() + id_tmp;
        id_tmp++;
    }
    return objects->end() + 1;
};

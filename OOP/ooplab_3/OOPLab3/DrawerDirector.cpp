#include "DrawerDirector.h"

shared_ptr<BaseDrawer> DrawerDirector::get_drawer(shared_ptr<AbstractFactory>& f)
{
    f = make_shared<QtFactory>(scene);
    auto drawer = f->create();
    return drawer;
}
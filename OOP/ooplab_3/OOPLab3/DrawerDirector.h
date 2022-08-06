#ifndef DRAWERDIRECTOR_H
#define DRAWERDIRECTOR_H

#include <memory>
#include <QGraphicsScene>
#include "AbstractFactory.h"

using namespace std;

class DrawerDirector
{
    shared_ptr<QGraphicsScene> scene;
public:
    void set_scene(shared_ptr<QGraphicsScene> sc) { scene = sc; }
    shared_ptr<BaseDrawer> get_drawer(shared_ptr<AbstractFactory>& f);
};

#endif // DRAWERDIRECTOR_H

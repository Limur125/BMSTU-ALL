#ifndef ABSTRACTFACTORY_H
#define ABSTRACTFACTORY_H

#include <memory>
#include <QGraphicsScene>
#include "Drawer.h"

using namespace std;

class AbstractFactory
{
public:
    AbstractFactory() = default;
    ~AbstractFactory() = default;

    virtual shared_ptr<BaseDrawer> create() = 0;
};

class QtFactory : public AbstractFactory
{
    shared_ptr<QGraphicsScene> scene;
public:
    explicit QtFactory(shared_ptr<QGraphicsScene> s) : scene(s) {}
    ~QtFactory() = default;

    virtual shared_ptr<BaseDrawer> create() override;
};

#endif // ABSTRACTFACTORY_H

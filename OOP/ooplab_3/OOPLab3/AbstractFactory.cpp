#include "AbstractFactory.h"

shared_ptr<BaseDrawer> QtFactory::create()
{
    shared_ptr<BaseDrawer> p = make_shared<QtDrawer>(this->scene);
    return p;
}

#ifndef FACADE_H
#define FACADE_H

#include <memory>
#include "Actions.h"

using namespace std;

class Facade
{
public:
    Facade() = default;
    void execute(const shared_ptr<BaseAction> &act);
};

#endif // FACADE_H

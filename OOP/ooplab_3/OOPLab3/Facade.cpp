#include "Facade.h"

void Facade::execute(const shared_ptr<BaseAction> &act)
{
	act->execute();
}

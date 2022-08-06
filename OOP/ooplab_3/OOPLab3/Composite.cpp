#include "Composite.h"

bool Composite::is_composite()
{
	return true;
}

bool Composite::add(shared_ptr<BaseObject> &obj, size_t index)
{
    obj->set_id(index);
	objects.push_back(obj);
	return true;
}

bool Composite::remove(ObjectIterator& iter)
{
	objects.erase(iter);
	return true;
}

ObjectIterator Composite::begin()
{
	return objects.begin();
}

ObjectIterator Composite::end()
{
	return objects.end();
}

void Composite::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center)
{
	for (auto& elem : objects)
		if (elem->is_visible())
			elem->transformation(mt, st, rt, center);
}

void Composite::transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt)
{
	for (auto& elem : objects)
		if (elem->is_visible())
			elem->transformation(mt, st, rt);
}

void Composite::accept(shared_ptr<BaseVisitor> visitor)
{
	for (auto& elem : objects)
		elem->accept(visitor);
}

#ifndef BASEOBJECT_H
#define BASEOBJECT_H

#include <memory>
#include <vector>
#include "Transformations.h"
#include "BaseVisitor.h"
#include "Point.h"

using namespace std;

class Visitor;
class BaseObject;

using ObjectIterator = vector<shared_ptr<BaseObject>>::const_iterator;


class BaseObject
{
protected:
    int id = 0;
public:
    BaseObject() = default;
    ~BaseObject() = default;

    virtual bool is_visible() const = 0;
    virtual bool is_composite() { return false; }
    virtual bool add(shared_ptr<BaseObject>& obj, size_t index) { return false; };
    virtual bool remove(ObjectIterator& iter) { return false; };

    virtual const Point get_center() = 0;
    virtual void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt) = 0;
    virtual void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center) = 0;
    virtual void accept(shared_ptr<BaseVisitor> visitor) = 0;

    virtual ObjectIterator begin() { return ObjectIterator(); };
    virtual ObjectIterator end() { return ObjectIterator(); };

    void set_id(int new_id) { id = new_id; };
    int get_id() { return id; }
};

class VisibleObject : public BaseObject
{
public:
    explicit VisibleObject() = default;
    ~VisibleObject() = default;

    bool is_visible() const { return true; };
};

class InvisibleObject : public BaseObject
{
public:
    explicit InvisibleObject() = default;
    ~InvisibleObject() = default;

    bool is_visible() const { return false; };
};

#endif // BASEOBJECT_H

#ifndef COMPOSITE_H
#define COMPOSITE_H

#include <vector>
#include "BaseObject.h"
#include "Transformations.h"
#include "BaseVisitor.h"

using namespace std;

class Composite : public BaseObject
{
    vector<shared_ptr<BaseObject>> objects;
    Point composite_center;
public:
    Composite() = default;
    Composite(Point center) { composite_center = center; }
    ~Composite() = default;

    virtual const Point get_center() override { return composite_center; }
    virtual bool is_visible() const { return false; }
    virtual bool is_composite();
    virtual bool add(shared_ptr<BaseObject> &obj, size_t index) override;
    virtual bool remove(ObjectIterator& iter) override;

    virtual ObjectIterator begin() override;
    virtual ObjectIterator end() override;

    virtual void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt, Point& center) override;
    virtual void transformation(const MoveT& mt, const ScaleT& st, const RotateT& rt) override;

    virtual void accept(shared_ptr<BaseVisitor> visitor) override;
};

#endif // COMPOSITE_H

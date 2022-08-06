#ifndef BASEVISITOR_H
#define BASEVISITOR_H

class Figure;
class Camera;
class Composite;

class BaseVisitor
{
public:
    BaseVisitor() = default;
    virtual ~BaseVisitor() = default;
    virtual void visit(Camera& camera) = 0;
    virtual void visit(Figure& figure) = 0;
    virtual void visit(Composite& composite) = 0;
};


#endif // BASEVISITOR_H

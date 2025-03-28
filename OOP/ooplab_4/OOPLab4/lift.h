#ifndef LIFT_H
#define LIFT_H

#include <QObject>

#include "liftcabin.h"
#include "liftcontroller.h"
#include "doors.h"

class Lift : public QObject
{
    Q_OBJECT
public:
    explicit Lift(QObject *parent = nullptr);
    ~Lift() = default;

    QWidget *widget();

private:
    Controller controller;
    Cabin cabin;
};

#endif // LIFT_H

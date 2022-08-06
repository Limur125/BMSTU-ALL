#include "lift.h"

Lift::Lift(QObject *parent) : QObject(parent)
{
    QObject::connect(
        &this->controller, &Controller::controller_ready_to_move,
        &this->cabin, &Cabin::moving);
    QObject::connect(
        &this->cabin, &Cabin::cabin_moving,
        &this->controller, &Controller::moving);
    QObject::connect(
        &this->controller, &Controller::controller_reached_target,
        &this->cabin, &Cabin::wait);
    QObject::connect(
        &this->cabin, &Cabin::cabin_ready_to_move,
        &this->controller, &Controller::free);
}

QWidget *Lift::widget()
{
    return &this->controller;
}

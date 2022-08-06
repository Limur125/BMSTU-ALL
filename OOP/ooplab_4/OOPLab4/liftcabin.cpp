#include "liftcabin.h"

Cabin::Cabin()
{
    connect(&this->doors, &Doors::doors_closed, this, &Cabin::ready_to_move);
    connect(this, &Cabin::cabin_waiting, &this->doors, &Doors::opening);
}

void Cabin::ready_to_move()
{
    if (this->status == WAITING)
    {
        this->status = READY_TO_MOVE;
        emit cabin_ready_to_move();
    }
}

void Cabin::moving()
{
    if (this->status == READY_TO_MOVE)
    {
        this->status = MOVING;
        emit cabin_moving();
    }
}

void Cabin::wait()
{
    if (this->status == MOVING || this->status == READY_TO_MOVE)
    {
        this->status = WAITING;
        emit cabin_waiting();
    }
}


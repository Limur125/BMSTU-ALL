#ifndef LIFTCABIN_H
#define LIFTCABIN_H

#include <QObject>
#include "doors.h"

class Cabin : public QObject
{
    Q_OBJECT
public:
    enum Status
    {
        WAITING,
        READY_TO_MOVE,
        MOVING,
    };

    Cabin();

signals:
    void cabin_ready_to_move();
    void cabin_moving();
    void cabin_waiting();

public slots:
    void moving();
    void wait();
    void ready_to_move();

private:
    Doors doors;
    Status status = READY_TO_MOVE;
};

#endif // LIFTCABIN_H

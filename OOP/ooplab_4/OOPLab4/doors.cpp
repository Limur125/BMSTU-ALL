#include "doors.h"
#include "qdebug.h"

Doors::Doors()
{
    this->opening_timer.setSingleShot(true);
    this->opening_timer.setInterval(DOORS_OPEN_TIME);
    this->wait_timer.setSingleShot(true);
    this->wait_timer.setInterval(DOORS_WAIT_TIME);
    this->closing_timer.setSingleShot(false);
    this->closing_timer.setInterval(DOORS_CLOSE_TIME);
    connect(&this->opening_timer, &QTimer::timeout, this, &Doors::open);
    connect(&this->wait_timer, &QTimer::timeout, this, &Doors::closing);
    connect(&this->closing_timer, &QTimer::timeout, this, &Doors::close);
}

void Doors::opening()
{
    if (this->status == CLOSED || this->status == CLOSING)
    {
        if (this->status == CLOSED)
        {
            status = OPENING;
            this->opening_timer.start();
        }
        else
        {
            status = OPENING;
            auto elapsedTime = closing_timer.remainingTime();
            closing_timer.stop();
            opening_timer.start(DOORS_CLOSE_TIME - elapsedTime);
        }

        qDebug("Двери открываются.");
    }
}

void Doors::open()
{
    if (this->status == OPENING)
    {
        this->status = OPEN;
        this->wait_timer.start();
        qDebug("Двери открыты.");
        emit doors_opened();
    }
}

void Doors::closing()
{
    if (this->status == OPEN)
    {
        this->status = CLOSING;
        this->closing_timer.start();
        qDebug("Двери закрываются.");
    }
}

void Doors::close()
{
    if (this->status == CLOSING)
    {
        this->status = CLOSED;
        qDebug("Двери закрыты.");
        emit doors_closed();
    }
}

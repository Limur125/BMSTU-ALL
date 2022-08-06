#ifndef DOORS_H
#define DOORS_H

#include <QObject>

#include "define.h"

class Doors : public QObject
{
    friend class Cabin;

    Q_OBJECT
public:
    enum Status
    {
        CLOSED,
        OPENING,
        OPEN,
        CLOSING
    };
    Doors();

signals:

    void doors_opened();
    void doors_closed();

private slots:
    void open();
    void close();
    void closing();

public slots:
    void opening();

private:
    QTimer opening_timer, closing_timer, wait_timer;
    Status status = CLOSED;
};

#endif // DOORS_H

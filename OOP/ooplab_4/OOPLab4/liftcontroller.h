#ifndef LIFTCONTROLLER_H
#define LIFTCONTROLLER_H

#include <QVBoxLayout>
#include <QWidget>
#include <QDebug>
#include <memory>
#include <vector>

#include "define.h"
#include "button.h"
#include "liftcabin.h"
#include "doors.h"

using namespace std;

class Controller : public QWidget
{
    Q_OBJECT
public:
    enum Status
    {
        FREE,
        ADD_TARGET,
        BUSY
    };

    Controller(QWidget *parent = nullptr);

signals:
    void controller_free();
    void controller_ready_to_move();
    void controller_reached_target(const int floor);

public slots:
    void add_target(const int floor);
    void moving();
    void free();

private:
    void find_target();
    Status status = FREE;
    vector<shared_ptr<Button>> button_arr;
    unique_ptr<QVBoxLayout> layout;
    vector<int> targets;
    int current_target = 1;
    int current_floor = 1;
    Direction direction = UP;
    QTimer floor_timer;
};

#endif // LIFTCONTROLLER_H

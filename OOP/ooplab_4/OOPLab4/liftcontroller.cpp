#include "liftcontroller.h"

Controller::Controller(QWidget *parent) : QWidget(parent)
{
    this->floor_timer.setInterval(FLOOR_PASS_TIME);
    QObject::connect(&this->floor_timer, &QTimer::timeout, this, &Controller::moving);

    this->layout = make_unique<QVBoxLayout>();
    this->setLayout(this->layout.get());

    for (int i = 0; i < ALL_FLOORS; ++i)
    {
        shared_ptr<Button> new_button = make_shared<Button>();

        new_button->set_floor(ALL_FLOORS - i);
        new_button->setText(QString::number(ALL_FLOORS - i));

        this->button_arr.insert(this->button_arr.begin(), new_button);
        layout->addWidget(dynamic_cast<QPushButton*>(new_button.get()));

        QObject::connect(new_button.get(), &Button::press_signal,
                         this, &Controller::add_target);
    }
}

void Controller::add_target(int floor)
{
    this->targets.push_back(floor);
    if (status == BUSY)
        return;

    this->status = ADD_TARGET;
    find_target();

    if (this->current_floor == this->current_target)
        emit controller_reached_target(this->current_target);
    else
        emit controller_ready_to_move();
}

void Controller::moving()
{
    if (this->status == BUSY || this->status == ADD_TARGET || this->status == FREE)
    {
        this->status = BUSY;
        this->floor_timer.start();
        qDebug() << "Лифт на этаже №" << this->current_floor;
        this->direction = this->current_floor > this->current_target ? DOWN : UP;
        if (this->current_floor == this->current_target)
        {
            this->floor_timer.stop();
            emit controller_reached_target(this->current_target);
        }
        else
        {
            this->current_floor += this->direction;
        }
    }
}

void Controller::free()
{
    if (this->status == ADD_TARGET || this->status == BUSY)
    {
        this->status = FREE;
        emit button_arr[current_floor - 1]->unpress_signal();
        if (targets.empty())
        {
            emit controller_free();
        }
        else
        {
            // status = BUSY;
            find_target();
            emit controller_ready_to_move();
        }
    }
}

void Controller::find_target()
{
    auto begin = this->targets.begin();
    this->current_target = *begin;
    this->targets.erase(begin);
}

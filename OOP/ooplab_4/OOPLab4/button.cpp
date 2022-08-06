#include "button.h"
#include "qdebug.h"

Button::Button(QWidget *parent) : QPushButton(parent)
{
    QObject::connect(this, &Button::clicked, 
                     this, &Button::press);
    QObject::connect(this, &Button::unpress_signal,
                     this, &Button::unpress);

    this->current_state = NOTACTIVE;
    this->current_button_floor = 1;
}

void Button::set_floor(const int &floor)
{
    this->current_button_floor = floor;
}

void Button::press()
{
    if (current_state != NOTACTIVE)
        return;
    this->update();

    qDebug() <<  "ВЫЗОВ ЭТАЖ № " << this->current_button_floor;
    this->current_state = ACTIVE;
    this->setDisabled(true);

    emit press_signal(this->current_button_floor);
}

void Button::unpress()
{
    if (current_state != ACTIVE)
        return;
    this->update();

    this->current_state = NOTACTIVE;
    this->setDisabled(false);
}


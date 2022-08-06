#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <iostream>
#include <QMainWindow>
#include "ui_mainwindow.h"
#include "return_codes.h"
#include "err_message.h"
#include "menu.h"

#define WIN_X 600
#define WIN_Y 500

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:

    void on_load_btn_clicked();

    void on_move_btn_clicked();

    void on_rot_btn_clicked();

    void on_scale_btn_clicked();

private:
    Ui::MainWindow *ui;
};

ret_code start_draw(Ui::MainWindow *ui);

#endif // MAINWINDOW_H


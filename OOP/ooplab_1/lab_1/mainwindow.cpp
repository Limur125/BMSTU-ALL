#include "mainwindow.h"

MainWindow::MainWindow(QWidget *parent):
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    ui->statusbar->showMessage("Лабораторная работа №1, Золотухин Алексей ИУ7-44Б");
    QGraphicsScene *scene = new QGraphicsScene(this);
    ui->graphicsView->setScene(scene);
    ui->graphicsView->setAlignment(Qt::AlignTop | Qt::AlignLeft);
    scene->setSceneRect(0, 0, WIN_X, WIN_Y);

    menu_t choose = {.task = START};
    choose.render_field.scene = scene;
    menu(choose);
}


MainWindow::~MainWindow()
{
    menu_t choose = {.task = FINISH};
    menu(choose);
    delete ui;
}


ret_code start_draw(Ui::MainWindow *ui)
{
    canvas_t canv_init;

    canv_init.scene = ui->graphicsView->scene();
    canv_init.h = ui->graphicsView->height();
    canv_init.w = ui->graphicsView->width();

    menu_t choose = {.task = RENDER};
    choose.render_field = canv_init;

    ret_code rc = menu(choose);
    return rc;
}


void MainWindow::on_load_btn_clicked()
{
    menu_t choose = {.task = LOAD};

    QString text = ui->load_le->text();

    std::string str = text.toStdString();
    choose.filename = str.c_str();

    ret_code rc = menu(choose);

    if (rc == OK)
        rc = start_draw(ui);

    if (rc != OK)
        err_message(rc);
}


void MainWindow::on_move_btn_clicked()
{
    menu_t choose = {.task = MOVE};

    choose.move_field.dx = ui->dx_sb->value();
    choose.move_field.dy = ui->dy_sb->value();
    choose.move_field.dz = ui->dz_sb->value();

    ret_code rc = menu(choose);

    if (rc == OK)
        rc = start_draw(ui);

    if (rc != OK)
        err_message(rc);
}


void MainWindow::on_rot_btn_clicked()
{
    menu_t choose = {.task = ROTATE};

    choose.rotate_field.ax = ui->ax_sb->value();
    choose.rotate_field.ay = ui->ay_sb->value();
    choose.rotate_field.az = ui->az_sb->value();

    ret_code rc = menu(choose);

    if (rc == OK)
        rc = start_draw(ui);

    if (rc != OK)
        err_message(rc);
}


void MainWindow::on_scale_btn_clicked()
{
    menu_t choose = {.task = SCALE};

    choose.scale_field.kx = ui->kx_sb->value();
    choose.scale_field.ky = ui->ky_sb->value();
    choose.scale_field.kz = ui->kz_sb->value();

    ret_code rc = menu(choose);

    if (rc == OK)
        rc = start_draw(ui);

    if (rc != OK)
        err_message(rc);
}

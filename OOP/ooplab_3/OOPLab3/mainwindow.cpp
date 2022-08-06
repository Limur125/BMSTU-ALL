#include "mainwindow.h"

#include "Drawer.h"
#include "AbstractFactory.h"
#include "Exceptions.h"
#include "DrawerDirector.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget* parent) : QMainWindow(parent), ui(new Ui::MainWindow), facade_viewer(new Facade), index_obj(-1)
{
    ui->setupUi(this);
    scene_view = make_shared<QGraphicsScene>();
    set_scene(scene_view);
    ui->graphicsView->setScene(scene_view.get());
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::render()
{
    if (ui->comboBoxCamera->count() == 0)
    {
        QMessageBox::information(nullptr, "Warning", "Choose camera to render");
        return;
    }

    director.set_scene(scene_view);
    shared_ptr<AbstractFactory> factory;
    auto canvas = director.get_drawer(factory);

    canvas->clear();

    shared_ptr<BaseAction> command = make_shared<RenderScene>(canvas);
    facade_viewer->execute(command);

    ui->graphicsView->setScene(this->scene_view.get());
}

void MainWindow::on_pushButton_AddCamera_clicked()
{
    string cam_name = string("camera_") + to_string(++index_obj);

    try
    {
        shared_ptr<BaseAction> command = make_shared<AddCamera>(index_obj);
        facade_viewer->execute(command);
        ui->comboBoxCamera->addItem(cam_name.c_str());
        if (ui->comboBoxCamera->count() == 1)
        {
            shared_ptr<BaseAction> command = make_shared<SetCamera>(index_obj);
            facade_viewer->execute(command);
            if (ui->comboBoxModel->count() > 0)
                render();
        }
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }
}

void MainWindow::on_pushButton_AddModel_clicked()
{
    QString file = QFileDialog::getOpenFileName(this,
        QString::fromUtf8("Открыть файл"),
        QDir::currentPath(),
        "text files (*.txt)");
    if (file.isEmpty())
        return;

    string model_name = string("model_") + to_string(++index_obj);
    string file_name = file.toLocal8Bit().constData();

    try
    {
        shared_ptr<BaseAction> command = make_shared<LoadFigure>(file_name, index_obj);
        facade_viewer->execute(command);
        render();
        ui->comboBoxModel->addItem(model_name.c_str());
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }
}

void MainWindow::on_pushButton_SetCamera_clicked()
{
    string cam_name = ui->comboBoxCamera->currentText().toStdString();
    size_t obj_ind = stoull(cam_name.substr(7));
    try
    {
        shared_ptr<BaseAction> command = make_shared<SetCamera>(obj_ind);
        facade_viewer->execute(command);
        if (ui->comboBoxCamera->count() > 0)
            render();
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }
}

void MainWindow::on_pushButton_moveModel_clicked()
{
    if (ui->comboBoxModel->count() == 0)
        return;

    string obj_name = ui->comboBoxModel->currentText().toStdString();
    size_t obj_ind = stoull(obj_name.substr(6));
    double x = ui->doubleSpinBoxDx->value();
    double y = ui->doubleSpinBoxDy->value();
    double z = ui->doubleSpinBoxDz->value();
    try
    {
        shared_ptr<BaseAction> command = make_shared<MoveFigure>(obj_ind, x, y, z);
        facade_viewer->execute(command);
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }

    render();
}

void MainWindow::on_pushButton_scaleModel_clicked()
{
    if (ui->comboBoxModel->count() == 0)
        return;

    string obj_name = ui->comboBoxModel->currentText().toStdString();
    size_t obj_ind = stoull(obj_name.substr(6));
    double x = ui->doubleSpinBoxKx->value();
    double y = ui->doubleSpinBoxKy->value();
    double z = ui->doubleSpinBoxKz->value();

    try
    {
        shared_ptr<BaseAction> command = make_shared<ScaleFigure>(obj_ind, x, y, z);
        facade_viewer->execute(command);
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }

    render();
}

void MainWindow::on_pushButton_rotateModel_clicked()
{
    if (ui->comboBoxModel->count() == 0)
        return;

    string obj_name = ui->comboBoxModel->currentText().toStdString();
    size_t obj_ind = stoull(obj_name.substr(6));
    double x = ui->doubleSpinBoxOx->value();
    double y = ui->doubleSpinBoxOy->value();
    double z = ui->doubleSpinBoxOz->value();

    try
    {
        shared_ptr<BaseAction> command = make_shared<RotateFigure>(obj_ind, x, y, z);
        facade_viewer->execute(command);
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }

    render();
}

void MainWindow::on_pushButton_moveCamera_clicked()
{
    if (ui->comboBoxModel->count() == 0)
        return;

    string cam_name = ui->comboBoxCamera->currentText().toStdString();
    size_t obj_ind = stoull(cam_name.substr(7));
    double x = ui->doubleSpinBoxDx_c->value();
    double y = ui->doubleSpinBoxDy_c->value();
    double z = ui->doubleSpinBoxDz_c->value();

    try
    {
        shared_ptr<BaseAction> command = make_shared<MoveCamera>(obj_ind, x, y, z);
        facade_viewer->execute(command);
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }

    render();
}

void MainWindow::on_pushButton_rotateCamera_clicked()
{
    if (ui->comboBoxModel->count() == 0)
        return;

    string cam_name = ui->comboBoxCamera->currentText().toStdString();
    size_t obj_ind = stoull(cam_name.substr(7));
    double angle_x = ui->doubleSpinBoxOx_c->value();
    double angle_y = ui->doubleSpinBoxOy_c->value();
    double angle_z = ui->doubleSpinBoxOz_c->value();

    try
    {
        shared_ptr<BaseAction> command = make_shared<RotateCamera>(obj_ind, angle_x, angle_y, angle_z);
        facade_viewer->execute(command);
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }

    render();
}

void MainWindow::on_pushButton_RemoveCamera_clicked()
{
    if (ui->comboBoxCamera->count() == 0)
        return;

    try
    {
        string cam_name = ui->comboBoxCamera->currentText().toStdString();
        size_t obj_ind = stoull(cam_name.substr(7));
        shared_ptr<BaseAction> command = make_shared<DeleteCamera>(obj_ind);
        facade_viewer->execute(command);
        ui->comboBoxCamera->removeItem(ui->comboBoxCamera->currentIndex());
        if (ui->comboBoxCamera->count() == 0)
        {
            QMessageBox::information(nullptr, "Warning", "Choose camera to render");
            scene_view->clear();

            return;
        }

        render();
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }
}

void MainWindow::on_pushButton_RemoveModel_clicked()
{
    if (ui->comboBoxCamera->count() == 0)
        return;

    try
    {
        string model_name = ui->comboBoxModel->currentText().toStdString();
        size_t obj_ind = stoull(model_name.substr(6));
        shared_ptr<BaseAction> command = make_shared<DeleteFigure>(obj_ind);
        facade_viewer->execute(command);
        ui->comboBoxModel->removeItem(ui->comboBoxModel->currentIndex());
        if (ui->comboBoxCamera->count() == 0)
        {
            QMessageBox::information(nullptr, "Warning", "Choose camera to render");
            scene_view->clear();

            return;
        }
        if (ui->comboBoxModel->count() == 0)
        {
            scene_view->clear();

            return;
        }

        render();
    }
    catch (BaseException& ex)
    {
        QMessageBox::warning(this, "Error message", QString(ex.what()));
    }
}

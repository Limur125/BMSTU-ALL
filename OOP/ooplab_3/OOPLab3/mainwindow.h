#pragma once

#include <cmath>

#include <QFileDialog>
#include <QMainWindow>
#include <QMessageBox>
#include <memory>

#include "Actions.h"
#include "AbstractFactory.h"
#include "DrawerDirector.h"
#include "Facade.h"

using namespace std;

namespace Ui
{
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget* parent = nullptr);
    ~MainWindow();
    void set_scene(std::shared_ptr<QGraphicsScene>& scene_view) { this->scene_view = scene_view; }

private slots:

    void on_pushButton_AddModel_clicked();

    void on_pushButton_AddCamera_clicked();

    void on_pushButton_SetCamera_clicked();

    void on_pushButton_RemoveCamera_clicked();

    void on_pushButton_RemoveModel_clicked();

    void on_pushButton_moveModel_clicked();

    void on_pushButton_scaleModel_clicked();

    void on_pushButton_rotateModel_clicked();

    void on_pushButton_moveCamera_clicked();

    void on_pushButton_rotateCamera_clicked();

private:
    DrawerDirector director;
    Ui::MainWindow* ui;
    shared_ptr<QGraphicsScene> scene_view;
    unique_ptr<Facade> facade_viewer;
    size_t index_obj;
    void render();
};

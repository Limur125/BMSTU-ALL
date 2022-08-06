#ifndef LOADMANAGER_H
#define LOADMANAGER_H

#include <memory>
#include <string>
#include "BaseManager.h"
#include "FigureDirector.h"
#include "Figure.h"
#include "FileLoader.h"

using namespace std;

class LoadManager : public BaseManager
{
    shared_ptr<FigureDirector> figdirector;
public:
    LoadManager();
    shared_ptr<Figure> load_figure(const string& filename);
};

class LoadManagerSingleton
{
public:
    LoadManagerSingleton() = delete;
    static LoadManager& instance();
};


#endif // LOADMANAGER_H

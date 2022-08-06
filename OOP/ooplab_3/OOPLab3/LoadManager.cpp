#include "LoadManager.h"

LoadManager::LoadManager()
{
    figdirector = make_shared<FigureDirector>(make_shared<FileLoader>(), make_shared<FigureBuilder>());
}

shared_ptr<Figure> LoadManager::load_figure(const string& filename)
{
    return figdirector->get(filename);
}

LoadManager& LoadManagerSingleton::instance()
{
    static unique_ptr<LoadManager> manager = make_unique<LoadManager>();
    return *manager;
}
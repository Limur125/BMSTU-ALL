#ifndef BASEFIGUREDIRECTOR_H
#define BASEFIGUREDIRECTOR_H

#include <memory>
#include <string>
#include "Figure.h"
#include "SourceLoader.h"
#include "BaseFigureBuilder.h"

using namespace std;
class BaseFigureDirector
{
protected:
    shared_ptr<SourceLoader> loader;
    shared_ptr<BaseFigureBuilder> builder;
public:
    virtual ~BaseFigureDirector() = default;
    virtual shared_ptr<Figure> get(string filename) = 0;
};

#endif // BASEFIGUREDIRECTOR_H

#ifndef FIGUREDIRECTOR_H
#define FIGUREDIRECTOR_H

#include <memory>
#include <string>

#include "BaseFigureDirector.h"
#include "SourceLoader.h"
#include "FigureBuilder.h"
#include "Figure.h"
#include "Exceptions.h"

using namespace std;

class FigureDirector : public BaseFigureDirector
{
    bool check_links(vector<Point>& points, vector<Link>& links);
public:
    FigureDirector(shared_ptr<SourceLoader> sl, shared_ptr<BaseFigureBuilder> fb) { loader = sl; builder = fb; }
    ~FigureDirector() = default;
    shared_ptr<Figure> get(string filename) override;
};


#endif // FIGUREDIRECTOR_H

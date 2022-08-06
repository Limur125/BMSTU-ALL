#include "FigureDirector.h"


bool FigureDirector::check_links(vector<Point>& points, vector<Link>& links)
{
    for (auto& link : links)
        if (link.get_begin() >= points.size() || link.get_end() >= points.size())
            return true;
    return false;
}

shared_ptr<Figure> FigureDirector::get(string filename)
{
    builder->reset();
    loader->open(filename);
    vector<Point> points = this->loader->read_points();
    vector<Link> links = this->loader->read_links();
    loader->close();

    if (check_links(points, links))
        throw FileFormatException(__FILE__, typeid(*this).name(), __LINE__);

    builder->build_points(points);
    builder->build_links(links);

    return builder->get_figure();
}

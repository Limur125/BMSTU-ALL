#include "FileLoader.h"

FileLoader::~FileLoader()
{
    close();
}

void FileLoader::open(std::string source_name)
{
    if (is_open())
        close();

    file.open(source_name);
    if (!file)
        throw FileOpenException(__FILE__, typeid(*this).name(), __LINE__);
}

bool FileLoader::is_open() const
{
    return file.is_open();
}

void FileLoader::close()
{
    file.close();
    file.clear();
}

vector<Point> FileLoader::read_points()
{
    if (!is_open())
        throw FileReadException(__FILE__, typeid(*this).name(), __LINE__);

    size_t n_points = 0;
    file >> n_points;

    if (n_points < 1)
        throw FileFormatException(__FILE__, typeid(*this).name(), __LINE__);

    vector<Point> points;

    for (size_t i = 0; i < n_points; ++i)
    {
        Point p;
        if (!(file >> p))
            throw FileFormatException(__FILE__, typeid(*this).name(), __LINE__);
        points.push_back(p);
    }
    return points;
}

vector<Link> FileLoader::read_links()
{
    if (!is_open())
        throw FileReadException(__FILE__, typeid(*this).name(), __LINE__);

    size_t n_links = 0;
    file >> n_links;

    if (n_links < 1)
        throw FileFormatException(__FILE__, typeid(*this).name(), __LINE__);

    vector<Link> links;

    for (size_t i = 0; i < n_links; ++i)
    {
        Link l;
        if (!(file >> l))
            throw FileFormatException(__FILE__, typeid(*this).name(), __LINE__);
        links.push_back(l);
    }

    return links;
}

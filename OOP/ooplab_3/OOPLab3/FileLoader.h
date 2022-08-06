#ifndef FILELOADER_H
#define FILELOADER_H

#include <fstream>
#include <vector>
#include <string>

#include "SourceLoader.h"
#include "Point.h"
#include "Link.h"
#include "Exceptions.h"

class FileLoader : public SourceLoader
{
    ifstream file;
public:
    FileLoader() = default;
    ~FileLoader();

    void open(string source_name) override;
    bool is_open() const override;
    void close() override;
    vector<Point> read_points() override;
    vector<Link> read_links() override;
};


#endif // FILELOADER_H

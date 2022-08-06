#include "Exceptions.h"

BaseException::BaseException(string filename,
    string classname,
    int numberline,
    string info)
{
    this->infoerror = "\n\nFile: " + filename +
        "\nClass: " + classname +
        "\nLine: " + to_string(numberline) +
        "\nInfo: " + info;
}

const char* BaseException::what() const noexcept
{
    return (this->infoerror).c_str();
}


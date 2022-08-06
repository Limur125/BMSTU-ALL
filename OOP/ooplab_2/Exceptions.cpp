#include "Exceptions.h"

Exceptions::Exceptions(std::string filename,
                           std::string classname,
                           int numberline,
                           std::string info)
{
    this->infoerror = "\n\nFile: " + filename +
                "\nClass: " + classname +
                "\nLine: " + std::to_string(numberline) +
                "\nInfo: " + info;
}

const char *Exceptions::what() const noexcept
{
    return (this->infoerror).c_str();
}


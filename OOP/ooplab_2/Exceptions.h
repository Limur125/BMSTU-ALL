#ifndef _SET_EXCEPTIONS_H_
#define _SET_EXCEPTIONS_H_

#include <exception>
#include <string>
#include <iostream>

class Exceptions: public std::exception
{
protected:
    std::string infoerror;
public:
    Exceptions(std::string filename,
                 std::string classname,
                 int numberline,
                 std::string info);

    virtual const char* what() const noexcept;

    virtual ~Exceptions() = default;
};

class IndexOutOfRangeException: public Exceptions
{
public:
    IndexOutOfRangeException(std::string filename, std::string classname,
                int numberline, std::string info = "Index out of range!"):
        Exceptions(filename, classname, numberline, info) {}
    const char *what() const noexcept override
    {
        return infoerror.c_str();
    }
};

class MemomryAllocateException: public Exceptions, public std::bad_alloc
{
public:
    MemomryAllocateException(std::string filename, std::string classname,
                int numberline, std::string info = "Allocation Error!"):
        Exceptions(filename, classname, numberline, info) {}
    const char *what() const noexcept override
    {
        return infoerror.c_str();
    }
};

class ExpiredPointerException: public Exceptions
{
public:
    ExpiredPointerException(std::string filename, std::string classname,
                int numberline, std::string info = "Invalid pointer!"):
        Exceptions(filename, classname, numberline, info) {}
    const char *what() const noexcept override
    {
        return infoerror.c_str();
    }
};


#endif
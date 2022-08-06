#ifndef EXCEPTIONS_H
#define EXCEPTIONS_H

#include <exception>
#include <string>

using namespace std;

class BaseException: public exception
{
protected:
    string infoerror;
public:
    BaseException(string filename,
        string classname,
        int numberline,
        string info);

    virtual const char* what() const noexcept;

    virtual ~BaseException() = default;
};

class FileOpenException: public BaseException
{
public:
	FileOpenException(string filename, string classname,
        int numberline, string info = "Can't open file"): BaseException(filename, classname, numberline, info) {}
    const char* what() const noexcept override { return infoerror.c_str(); }
};

class FileReadException : public BaseException
{
public:
    FileReadException(string filename, string classname,
        int numberline, string info = "Can't read the file") : BaseException(filename, classname, numberline, info) {}
    const char* what() const noexcept override { return infoerror.c_str(); }
};

class FileFormatException : public BaseException
{
public:
    FileFormatException(string filename, string classname,
        int numberline, string info = "Invalid file format!") : BaseException(filename, classname, numberline, info) {}
    const char* what() const noexcept override { return infoerror.c_str(); }
};

class CameraException : public BaseException
{
public:
    CameraException(string filename, string classname,
        int numberline, string info = "Invalid camera!") : BaseException(filename, classname, numberline, info) {}
    const char* what() const noexcept override { return infoerror.c_str(); }
};

class CanvasException : public BaseException
{
public:
    CanvasException(string filename, string classname,
        int numberline, string info = "Invalid canvas!") : BaseException(filename, classname, numberline, info) {}
    const char* what() const noexcept override { return infoerror.c_str(); }
};


#endif // EXCEPTIONS_H

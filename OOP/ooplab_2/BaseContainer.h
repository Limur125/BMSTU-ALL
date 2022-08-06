#ifndef _BASE_CONTAINER_H_
#define _BASE_CONTAINER_H_

#include <cstdlib>

class BaseContainer
{
protected:
    size_t len;

public:
    explicit BaseContainer() = default;
    explicit BaseContainer(size_t count) noexcept: len(count){}
    virtual ~BaseContainer() = default;
    bool is_empty() const noexcept;
    size_t get_size() const noexcept;
    operator bool() const noexcept { return bool(len); }
};

#endif //_BASE_CONTAINER_H_

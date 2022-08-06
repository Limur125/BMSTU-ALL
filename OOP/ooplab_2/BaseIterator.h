#ifndef _BASE_ITERATOR_H_
#define _BASE_ITERATOR_H_

#include <cstdlib>

class BaseIterator
{
public:
    BaseIterator() = default;
    explicit BaseIterator(const BaseIterator &iterator) noexcept: index(iterator.index), len(iterator.len) {}
    BaseIterator(size_t ind, size_t len) noexcept : index(ind), len(len) {}
    virtual ~BaseIterator() = default;
protected:
    size_t index;
    size_t len;
};


#endif
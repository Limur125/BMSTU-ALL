#ifndef _ITERATOR_H_
#define _ITERATOR_H_

#include <iterator>
#include <memory>
#include "Set.h"
#include "Exceptions.h"
#include "BaseIterator.h"

template<typename Type>
class Set;

template <typename Type>
class Iterator: std::iterator<std::bidirectional_iterator_tag, Type>, public BaseIterator
{
public:
    Iterator() = default;
    Iterator(Set<Type> &set, size_t ind) noexcept : ptr(set.a), BaseIterator(ind, set.len) {}
    Iterator(const Iterator<Type> &iter) noexcept;
    
    Type &operator*();
    const Type &operator*() const;
    Type *operator->();
    const Type *operator->() const;

    operator bool () const;

    Iterator<Type> &operator=(const Iterator<Type> &iterator) noexcept;

    bool operator==(const Iterator<Type> &cmp) const;
    bool operator!=(const Iterator<Type> &cmp) const;

    Iterator<Type> &operator++();
    Iterator<Type> operator++(int);
    // Iterator<Type> operator+(size_t number);

    Iterator<Type> &operator--();
    Iterator<Type> operator--(int);
private:
    std::weak_ptr<Type[]> ptr;
protected:
    Type* get_ptr() const noexcept;
    void exception_check(int cur_line) const;
    void validity_check(int cur_line) const;
};

template <typename Type>
class ConstIterator: std::iterator<std::bidirectional_iterator_tag, Type>, public BaseIterator
{
public:
    ConstIterator() = default;
    ConstIterator(const Set<Type> &set, size_t ind) noexcept : ptr(set.a), BaseIterator(ind, set.len) {}
    ConstIterator(const ConstIterator<Type> &iter) noexcept;

    const Type &operator*() const;
    const Type *operator->() const;

    operator bool ();

    ConstIterator<Type> &operator=(const ConstIterator<Type> &iterator) noexcept;

    bool operator==(const ConstIterator<Type> &cmp) const;
    bool operator!=(const ConstIterator<Type> &cmp) const;

    ConstIterator<Type> &operator++();
    ConstIterator<Type> operator++(int);
    // ConstIterator<Type> operator+(size_t number);

    ConstIterator<Type> &operator--();
    ConstIterator<Type> operator--(int);
private:
    std::weak_ptr<Type[]> ptr;
protected:
    Type* get_ptr() const noexcept;
    void exception_check(int cur_line) const;
    void validity_check(int cur_line) const;
};

#include "Iterator.hpp"

#endif
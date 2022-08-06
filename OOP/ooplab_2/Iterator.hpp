#ifndef _ITERATOR_HPP_
#define _ITERATOR_HPP_

#include "Iterator.h"

template<typename Type>
Type *Iterator<Type>::get_ptr() const noexcept
{
    std::shared_ptr<Type[]> copied = (this->ptr).lock();
    return copied.get() + this->index;
}

template<typename Type>
Iterator<Type>::Iterator(const Iterator<Type> &iter) noexcept: BaseIterator()
{
    this->ptr = iter.ptr;
    this->index = iter.index;
    this->len = iter.len;
}

template<typename Type>
Type &Iterator<Type>::operator*()
{
    this->exception_check(__LINE__);
    this->validity_check(__LINE__);
    return *(this->get_ptr());
}

template<typename Type>
const Type &Iterator<Type>::operator*() const
{
    this->exception_check(__LINE__);
    this->validity_check(__LINE__);
    return *(this->get_ptr());
}

template<typename Type>
Type *Iterator<Type>::operator->()
{
    this->exception_check(__LINE__);
    this->validity_check(__LINE__);
    return this->get_ptr();
}

template<typename Type>
const Type *Iterator<Type>::operator->() const
{
    this->exception_check(__LINE__);
    this->validity_check(__LINE__);
    return this->get_ptr();
}

template<typename Type>
Iterator<Type> &Iterator<Type>::operator=(const Iterator<Type>& iterator) noexcept
{
    this->ptr = iterator.ptr;
    return *this;
}

template<typename Type>
Iterator<Type> Iterator<Type>::operator++(int)
{
    this->exception_check(__LINE__);
    ++(*this);
    return *this;
}

template<typename Type>
Iterator<Type> &Iterator<Type>::operator++()
{
    this->exception_check(__LINE__);
    ++(this->index);
    return *this;
}

//template<typename Type>
//Iterator<Type> Iterator<Type>::operator+(size_t number)
//{
//    this->exception_check(__LINE__);
//    (this->index) += number;
//    return *this;
//}

template<typename Type>
Iterator<Type> Iterator<Type>::operator--(int)
{
    this->exception_check(__LINE__);
    --(*this);
    return *this;
}

template<typename Type>
Iterator<Type> &Iterator<Type>::operator--()
{
    this->exception_check(__LINE__);
    --(this->index);
    return *this;
}

template<typename Type>
bool Iterator<Type>::operator==(const Iterator<Type> &cmp) const
{
    this->exception_check(__LINE__);
    return this->index == cmp.index;
}

template<typename Type>
bool Iterator<Type>::operator!=(const Iterator<Type> &cmp) const
{
    this->exception_check(__LINE__);
    return this->index != cmp.index;
}

template<typename Type>
Iterator<Type>::operator bool() const
{
    this->exception_check(__LINE__);

    if (index >= len || len == 0)
    {
        return false;
    }
    else
    {
        return true;
    }
}

template<typename Type>
void Iterator<Type>::exception_check(int cur_line) const
{
    if ((this->ptr).expired())
        throw ExpiredPointerException(__FILE__, typeid(*this).name(), cur_line);
}

template<typename Type>
void Iterator<Type>::validity_check(int cur_line) const
{
    if (this->index >= this->len)
        throw IndexOutOfRangeException(__FILE__, typeid(*this).name(), cur_line);
}

template<typename Type>
Type *ConstIterator<Type>::get_ptr() const noexcept
{
    std::shared_ptr<Type[]> copied = (this->ptr).lock();
    return copied.get() + this->index;
}

template<typename Type>
ConstIterator<Type>::ConstIterator(const ConstIterator<Type> &iter) noexcept: BaseIterator()
{
    this->ptr = iter.ptr;
    this->index = iter.index;
    this->len = iter.len;
}

template<typename Type>
const Type &ConstIterator<Type>::operator*() const
{
    this->exception_check(__LINE__);
    this->validity_check(__LINE__);
    return *(this->get_ptr());
}

template<typename Type>
const Type *ConstIterator<Type>::operator->() const
{
    this->exception_check(__LINE__);
    this->validity_check(__LINE__);
    return this->get_ptr();
}

template<typename Type>
ConstIterator<Type> &ConstIterator<Type>::operator=(const ConstIterator<Type>& iterator) noexcept
{
    this->ptr = iterator.ptr;
    return *this;
}

template<typename Type>
ConstIterator<Type> ConstIterator<Type>::operator++(int)
{
    this->exception_check(__LINE__);
    ++(*this);
    return *this;
}

template<typename Type>
ConstIterator<Type> &ConstIterator<Type>::operator++()
{
    this->exception_check(__LINE__);
    ++(this->index);
    return *this;
}

//template <typename Type>
//ConstIterator<Type> ConstIterator<Type>::operator+(size_t number)
//{
//    this->exception_check(__LINE__);
//    (this->index) += number;
//    return *this;
//}

template<typename Type>
ConstIterator<Type> ConstIterator<Type>::operator--(int)
{
    this->exception_check(__LINE__);
    --(*this);
    return *this;
}

template<typename Type>
ConstIterator<Type> &ConstIterator<Type>::operator--()
{
    this->exception_check(__LINE__);
    --(this->index);
    return *this;
}

template<typename Type>
bool ConstIterator<Type>::operator==(const ConstIterator<Type> &cmp) const
{
    this->exception_check(__LINE__);
    return this->index == cmp.index;
}

template<typename Type>
bool ConstIterator<Type>::operator!=(const ConstIterator<Type> &cmp) const
{
    this->exception_check(__LINE__);
    return this->index != cmp.index;
}

template<typename Type>
ConstIterator<Type>::operator bool()
{
    this->exception_check(__LINE__);

    if (index >= len || len == 0)
    {
        return false;
    }
    else
    {
        return true;
    }
}

template<typename Type>
void ConstIterator<Type>::exception_check(int cur_line) const
{
    if ((this->ptr).expired())
        throw ExpiredPointerException(__FILE__, typeid(*this).name(), cur_line);
}

template<typename Type>
void ConstIterator<Type>::validity_check(int cur_line) const
{
    if (this->index >= this->len)
        throw IndexOutOfRangeException(__FILE__, typeid(*this).name(), cur_line);
}


#endif
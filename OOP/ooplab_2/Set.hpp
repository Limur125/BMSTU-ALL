#ifndef _SET_HPP_
#define _SET_HPP_

#include "Set.h"

template <typename Type>
Set<Type>::Set() noexcept : BaseContainer(0)
{
    this->allocated_len = 0;
    this->a = nullptr;
}

template <typename Type>
Set<Type>::Set(size_t size, ...): BaseContainer(0)
{
    va_list args;
    va_start(args, size);
    this->allocated_len = size;
    try
    {
        std::shared_ptr<Type[]> p(new Type[this->allocated_len]);
        this->a = p;
    }
    catch(MemomryAllocateException &e)
    {
        throw e;
    }
    for (;size > 0; size--)
    {
        Type element = va_arg(args, Type);
        this->add(element);
    }
    va_end(args);
}

template <typename Type>
template <typename Iter>
Set<Type>::Set(Iter it_beg, Iter it_end): BaseContainer(0)
{
    this->allocated_len = 0;
    this->a = nullptr;
    auto iter = it_beg;

    for (; iter != it_end; ++iter)
        this->add(*iter);
}

template <typename Type>
Set<Type>::Set(Type arr[], size_t size): BaseContainer(0)
{
    this->allocated_len = size;
    try
    {
        std::shared_ptr<Type[]> p(new Type[this->allocated_len]);
        this->a = p;
    }
    catch(MemomryAllocateException &e)
    {
        throw e;
    }
    for (size_t i = 0; i < size; i++)
        this->add(arr[i]);
}

template<typename Type>
Set<Type>::Set(std::initializer_list<Type> lst) :BaseContainer(0)
{
    this->allocated_len = 0;
    this->a = nullptr;
    auto iter = lst.begin();

    for (; iter != lst.end(); ++iter)
        this->add(*iter);
}

template<typename Type>
Set<Type>::Set(const Type& elem): BaseContainer(0)
{
    this->allocated_len = 0;
    this->a = nullptr;
    this->add(elem);
}

template<typename Type>
Set<Type> &Set<Type>::operator=(const Set<Type> &s)
{
    if (this == &s)
        return *this;

    this->allocated_len = s.allocated_len;
    this->len = s.len;
    try
    {
        (this->a).reset();
        std::shared_ptr<Type[]> sp(new Type[this->allocated_len]);
        this->a = sp;
    }
    catch (MemomryAllocateException &e)
    {
        throw e;
    }
    ConstIterator<Type> it2 = s.cbegin();

    for (Iterator<Type> it1 = this->begin(); it1 != this->end() && it2 != s.cend(); it1++, it2++)
        *it1 = *it2;

    return *this;
}

template<typename Type>
Set<Type> &Set<Type>::operator=(const Set<Type> &&s) noexcept
{
    if (this == &s)
        return *this;
    this->allocated_len = s.allocated_len;
    this->len = s.len;
    (this->a).reset();
    this->a = s.a;
    return *this;
}

template<typename Type>
inline Set<Type>& Set<Type>::operator=(std::initializer_list<Type> list)
{
    this->allocated_len = 0;
    this->a = nullptr;
    auto iter = list.begin();

    for (; iter != list.end(); ++iter)
        this->add(*iter);
}

template<typename Type>
Set<Type>::Set(const Set<Type> &s): BaseContainer()
{
    if (this == &s)
        return;

    this->allocated_len = s.allocated_len;
    this->len = s.len;
    try
    {
        (this->a).reset();
        std::shared_ptr<Type[]> sp(new Type[this->allocated_len]);
        this->a = sp;
    }
    catch (MemomryAllocateException &e)
    {
        throw e;
    }

    ConstIterator<Type> it2 = s.cbegin();

    for (Iterator<Type> it1 = this->begin(); it1 != this->end() && it2 != s.cend(); it1++, it2++)
        *it1 = *it2;
}

template<typename Type>
Set<Type>::Set(const Set<Type> &&s) noexcept
{
    if (this == &s)
        return;
    this->allocated_len = s.allocated_len;
    this->len = s.len;
    (this->a).reset();
    this->a = s.a;
}

template<typename Type>
Set<Type>::~Set()
{
    (this->a).reset();
}

template<typename Type>
bool Set<Type>::in_set(const Type &elem) const
{
    if (this->is_empty())
        return false;

    bool res = false;
    for (ConstIterator<Type> it = this->cbegin(); !res && it != this->cend(); it++)
        if (elem == *it)
            res = true;

    return res;
}

template<typename Type>
bool Set<Type>::add(const Type &elem)
{
    if (this->in_set(elem))
        return false;

    if (this->len + 1 < this->allocated_len)
    {
        (this->len)++;
        Iterator<Type> it = this->end();
        it--;
        *it = elem;
    }
    else
    {
        std::shared_ptr<Type[]> tmp;
        try
        {
            if (this->allocated_len == 0)
                this->allocated_len = 1;
            std::shared_ptr<Type[]> sp(new Type[this->allocated_len * 2]);
            tmp = sp;
        }
        catch (MemomryAllocateException &e)
        {
            throw e;
        }

        this->allocated_len *= 2;

        for (size_t i = 0; i < (this->len); i++)
            tmp.get()[i] = (this->a).get()[i];

        (this->a).reset();
        this->a = tmp;
        (this->len)++;
        Iterator<Type> it = this->end();
        it--;
        *it = elem;
    }
    return true;
}

template<typename Type>
bool Set<Type>::discard(const Type &elem)
{
    if (!(this->in_set(elem)))
        return false;

    Iterator<Type> it = this->begin();
    for (; it != this->end() && *it != elem ; it++);

    if (it == this->end())
        return false;

    *it = *(--(this->end()));
    this->len--;

    return true;
}

template<typename Type>
Set<Type> &Set<Type>::set_union(const Set<Type> &s1)
{
    for (ConstIterator<Type> it = s1.cbegin(); it != s1.cend(); it++)
        this->add(*it);

    return *this;
}

template<typename Type>
Set<Type> &Set<Type>::set_difference(const Set<Type> &s1)
{
    for (ConstIterator<Type> it = s1.cbegin(); it != s1.cend(); it++)
        if (this->in_set(*it))
            this->discard(*it);

    return *this;
}

template<typename Type>
Set<Type> &Set<Type>::set_crossing(const Set<Type> &s1)
{
    (*this) = (*this | s1) / (*this ^ s1);

    return *this;
}

template<typename Type>
Set<Type> &Set<Type>::set_sym_diff(const Set<Type> &s1)
{
    (*this) = (*this / s1) | (s1 / *this);
    return *this;
}

template<typename Type>
Set<Type> &Set<Type>::operator|=(const Set<Type> &s2)
{
    this->set_union(s2);
    return *this;
}

template<typename Type>
Set<Type> operator|(const Set<Type> &left, const Set<Type> &right)
{
    Set<Type> res = Set<Type>(left);
    res.set_union(right);
    return res;
}

template<typename Type>
Set<Type> operator&(const Set<Type> &left, const Set<Type> &right)
{
    Set<Type> res = Set<Type>(left);
    res.set_crossing(right);
    return res;
}

template<typename Type>
Set<Type> &Set<Type>::operator&=(const Set<Type> &s2)
{
    this->set_crossing(s2);
    return *this;
}

template<typename Type>
Set<Type> operator/(const Set<Type> &left, const Set<Type> &right)
{
    Set<Type> res = Set<Type>(left);
    res.set_difference(right);
    return res;
}

template<typename Type>
Set<Type> &Set<Type>::operator/=(const Set<Type> &s2)
{
    this->set_difference(s2);
    return *this;
}

template<typename Type>
Set<Type> operator^(const Set<Type> &left, const Set<Type> &right)
{
    Set<Type> res = Set<Type>(left);
    res.set_sym_diff(right);
    return res;
}

template<typename Type>
Set<Type> &Set<Type>::operator^=(const Set<Type> &s2)
{
    this->set_sym_diff(s2);
    return *this;
}

template<typename Type>
bool Set<Type>::is_subsetof(const Set<Type> &s1) const
{
    if (this->is_empty())
        return true;

    bool res = true;

    for (ConstIterator<Type> i = this->cbegin(); res && i != this->cend(); i++)
        if (!s1.in_set(*i))
            res = false;

    return res;
}

template<typename Type>
bool operator>=(const Set<Type> &s1, const Set<Type> &s2)
{
    return s2.is_subsetof(s1);
}

template<typename Type>
bool operator<=(const Set<Type> &s1, const Set<Type> &s2)
{
    return s1.is_subsetof(s2);
}

template<typename Type>
bool operator==(const Set<Type>  &s1, const Set<Type> &s2)
{
    return s1.is_subsetof(s2) && s2.is_subsetof(s1);
}

template<typename Type>
bool operator!=(const Set<Type> &s1, const Set<Type> &s2)
{
    return !(s1.is_subsetof(s2) && s2.is_subsetof(s1));
}

template<typename Type>
bool operator>(const Set<Type> &s1, const Set<Type> &s2)
{
    return s2.is_subsetof(s1) && !s1.is_subsetof(s2);
}

template<typename Type>
bool operator<(const Set<Type> &s1, const Set<Type> &s2)
{
    return s1.is_subsetof(s2) && !s2.is_subsetof(s1);
}

template<typename Type>
Iterator<Type> Set<Type>::begin() noexcept
{
    Iterator<Type> it(*this, 0);
    return it;
}
template<typename Type>
Iterator<Type> Set<Type>::end() noexcept
{
    Iterator<Type> it(*this, this->len);
    return it;
}
template<typename Type>
Iterator<Type> Set<Type>::begin() const noexcept
{
    Iterator<Type> it(*this, 0);
    return it;
}
template<typename Type>
Iterator<Type> Set<Type>::end() const noexcept
{
    Iterator<Type> it(*this, this->len);
    return it;
}

template<typename Type>
ConstIterator<Type> Set<Type>::cbegin() const noexcept
{
    ConstIterator<Type> it(*this, 0);
    return it;
}
template<typename Type>
ConstIterator<Type> Set<Type>::cend() const noexcept
{
    ConstIterator<Type> it(*this, this->len);
    return it;
}

template<typename Type>
std::ostream& operator<<(std::ostream& os, const Set<Type>& s)
{
    os << "{";
    for (auto i = s.cbegin(); i != s.cend(); i++)
        os << " " << *i;
    os << " }";
    return os;
}


#endif
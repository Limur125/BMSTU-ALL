#ifndef _SET_H_
#define _SET_H_

#include <memory>
#include <cstdarg>
#include <iostream>
#include "Iterator.h"
#include "BaseContainer.h"
#include "Exceptions.h"

using namespace std;

template <typename Type>
class Set : public BaseContainer
{
public:
    friend Iterator<Type>;
    friend ConstIterator<Type>;
    Set() noexcept;
    Set(size_t size, ...);
    Set(Type arr[], size_t size);
    Set(std::initializer_list<Type> list);
    template <typename Iter>
    Set(Iter it_beg, Iter it_end);
    explicit Set(const Type &elem);
    explicit Set(const Set<Type> &set);
    Set(const Set<Type> &&set) noexcept;
    Set<Type> &operator=(const Set<Type> &s);
	Set<Type> &operator=(const Set<Type> &&s) noexcept;
    Set<Type> &operator=(std::initializer_list<Type> list);

    virtual ~Set() override;

    bool in_set(const Type& elem) const;
    bool add(const Type& elem);
    bool discard(const Type& elem);


    // Объединение
    Set<Type>& set_union(const Set<Type>& s1);
    Set<Type>& operator|=(const Set<Type>& s2);
    template <typename T>
    friend Set<Type> operator|(const Set<Type>& left, const Set<Type>& right);


    // Разность
    Set<Type>& set_difference(const Set<Type>& s1);
    Set<Type>& operator/=(const Set<Type>& s2);
    template <typename T>
    friend Set<Type> operator/(const Set<Type>& left, const Set<Type>& right);


    // Пересечение																	
    Set<Type>& set_crossing(const Set<Type>& s1);
    Set<Type>& operator&=(const Set<Type>& s2);
    template <typename T>
    friend Set<Type> operator&(const Set<Type>& left, const Set<Type>& right);


    // Симметрическая разность
    Set<Type>& set_sym_diff(const Set<Type>& s1);
    Set<Type>& operator^=(const Set<Type>& s2);
    template <typename T>
    friend Set<Type> operator^(const Set<Type>& left, const Set<Type>& right);

    // Операции сравнения																	
    bool is_subsetof(const Set<Type>& s1) const;

    template <typename T>
    friend bool operator>=(const Set<Type>& s1, const Set<Type>& s2);
    template <typename T>
    friend bool operator<=(const Set<Type>& s1, const Set<Type>& s2);
    template <typename T>
    friend bool operator==(const Set<Type>& s1, const Set<Type>& s2);
    template <typename T>
    friend bool operator!=(const Set<Type>& s1, const Set<Type>& s2);
    template <typename T>
    friend bool operator>(const Set<Type>& s1, const Set<Type>& s2);
    template <typename T>
    friend bool operator<(const Set<Type>& s1, const Set<Type>& s2);

    // Итераторы
    ConstIterator<Type> cbegin() const noexcept;
    ConstIterator<Type> cend() const noexcept;

    // Потоковый вывод
    template <typename T>
    friend std::ostream& operator<<(std::ostream& os, const Set<Type>& s);

protected:
    Iterator<Type> begin() noexcept;
    Iterator<Type> end() noexcept;
    Iterator<Type> begin() const noexcept;
    Iterator<Type> end() const noexcept;
private:
    size_t allocated_len;
    std::shared_ptr<Type[]> a;
};

#include "Set.hpp"

#endif // _SET_H_
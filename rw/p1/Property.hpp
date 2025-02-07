#pragma once

#include <stdlib.h>

class ReadWrite;

template <typename Type, typename Owner, typename Access>
class Property
{

};

template<typename Type, typename Owner>
class Property<typename Type, typename Owner, ReadWrite>
{
protected:
    typedef Type(Owner::* getter)();
    typedef void (Owner::* setter)(Type);
    Owner* m_owner;
    getter m_getter;
    setter m_setter;
public:
    // Оператор приведения типа. Реализует геттер.
    operator Type()
    {
        return (m_owner->*m_getter)();
    }
    // Оператор присваивания. Реализует сеттер.
    void operator =(Type data)
    {
        (m_owner->*m_setter)(data);
    }

    Property() :
        m_owner(NULL),
        m_getter(NULL),
        m_setter(NULL)
    {
    }

    Property(Owner* const owner, getter getmethod, setter setmethod) :
        m_owner(owner),
        m_getter(getmethod),
        m_setter(setmethod)
    {
    }

    void init(Owner* const owner, getter getmethod, setter setmethod)
    {
        m_owner = owner;
        m_getter = getmethod;
        m_setter = setmethod;
    }
};
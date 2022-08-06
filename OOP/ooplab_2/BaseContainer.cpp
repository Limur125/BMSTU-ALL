#include "BaseContainer.h"

bool BaseContainer::is_empty() const noexcept
{
    return this->len == 0;
}

size_t BaseContainer::get_size() const noexcept
{
    return this->len;
}

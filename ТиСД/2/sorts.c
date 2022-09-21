#include "sorts.h"
// Сортировка таблицы абонентов пузырьком
void t_bublesort(phone_t table[], size_t n)
{
    int f = 1;
    while (f)
    {
        f = 0;
        for (size_t i = 0; i < n - 1; i++)
            if (strcmp(table[i].surname, table[i + 1].surname) > 0)
            {
                f = 1;
                phone_t tmp = table[i];
                table[i] = table[i + 1];
                table[i + 1] = tmp;
            }
    }
}

// Сортировка таблицы абонентов пузырьком
void t_shekersort(phone_t a[], size_t n)
{
    size_t left = 0, right = n - 1;
    int flag = 1;
    while ((left < right) && flag)
    {
        flag = 0;
        for (size_t i = left; i < right; i++)
            if (strcmp(a[i].surname, a[i + 1].surname) > 0)
            {
                phone_t t = a[i];
                a[i] = a[i + 1];
                a[i + 1] = t;
                flag = 1;
            }
        right--;
        for (size_t i = right; i > left; i--)
            if (strcmp(a[i].surname, a[i - 1].surname) < 0)
            {
                phone_t t = a[i];
                a[i] = a[i - 1];
                a[i - 1] = t;
                flag = 1;
            }
        left++;
    }
}

void k_shekersort(key_t a[], size_t n)
{
    size_t left = 0, right = n - 1;
    int flag = 1;
    while ((left < right) && flag)
    {
        flag = 0;
        for (size_t i = left; i < right; i++)
            if (strcmp(a[i].surname, a[i + 1].surname) > 0)
            {
                key_t t = a[i];
                a[i] = a[i + 1];
                a[i + 1] = t;
                flag = 1;
            }
        right--;
        for (size_t i = right; i > left; i--)
            if (strcmp(a[i].surname, a[i - 1].surname) < 0)
            {
                key_t t = a[i];
                a[i] = a[i - 1];
                a[i - 1] = t;
                flag = 1;
            }
        left++;
    }
}

// Сортировка таблицы ключей пузырьком
void k_bublesort(key_t table[], size_t n)
{
    int f = 1;
    while (f)
    {
        f = 0;
        for (size_t i = 0; i < n - 1; i++)
            if (strcmp(table[i].surname, table[i + 1].surname) > 0)
            {
                f = 1;
                key_t tmp = table[i];
                table[i] = table[i + 1];
                table[i + 1] = tmp;
            }
    }
}
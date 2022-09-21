#include <stdio.h>
#include <string.h>
#include "hash.h"

size_t hash_func1(char s[], size_t table_size)
{
    size_t res = 0;
    for (size_t i = 0; s[i] != 0; i++)
        res += s[i];
    return res % table_size;
}

size_t hash_func2(char s[], size_t table_size)
{
    size_t res = 0;
    for (size_t i = 0; s[i] != 0; i++)
        res += s[i] * i;
    return res % table_size;
}

hash_t *create(char s[])
{
    hash_t *tmp = malloc(sizeof(*tmp));
    if (tmp != NULL)
    {
        strcpy(tmp->s, s);
        tmp->next = NULL;
    }
    return tmp;
}

void insert_hash(hash_t *(table[]), size_t n, size_t (*hash_func)(char *, size_t), hash_t *h)
{
    size_t index = hash_func(h->s, n);
    if (table[index] == NULL)
        table[index] = h;
    else
    {
        hash_t *cur;
        for (cur = table[index]; cur->next != NULL; cur = cur->next);
        cur->next = h;
    }
}

hash_t *search_hash(hash_t *(table[]), size_t n, size_t (*hash_func)(char *, size_t), char s[], int *cmps)
{
    size_t index = hash_func(s, n);
    hash_t *cur;
    *cmps = 0;
    for (cur = table[index]; cur != NULL; cur = cur->next)
    {
        (*cmps)++;
        if (strcmp(cur->s, s) == 0)
            return cur;
    }
    return cur;
}

void print_table(hash_t *table[], size_t n)
{
    for (size_t i = 0; i < n; i++)
        if(table[i] != NULL)
        {
            for (hash_t *cur = table[i]; cur != NULL; cur = cur->next)
                printf("%s ", cur->s);
            printf("\n");
        }
        else
            printf("NULL\n");
}
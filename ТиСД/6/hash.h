#ifndef _HASH_H_
#define _HASH_H_

#include <stdlib.h>

#define STR_LEN 50

typedef struct hash
{
    char s[STR_LEN];
    struct hash *next;
}   hash_t;

size_t hash_func1(char s[], size_t table_size);
size_t hash_func2(char s[], size_t table_size);
hash_t *create(char s[]);
void insert_hash(hash_t *(table[]), size_t n, size_t (*hash_func)(char *, size_t), hash_t *h);
hash_t *search_hash(hash_t *(table[]), size_t n, size_t (*hash_func)(char *, size_t), char s[], int *cmps);
void print_table(hash_t *table[], size_t n);


#endif
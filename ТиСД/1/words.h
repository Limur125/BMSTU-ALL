#ifndef _WORDS_H_
#define _WORDS_H_

#include <stdio.h>

#define MAX_WORD_LEN 257
#define MAX_WORDS 256
#define STR_LEN 250

#define SCAN_ERROR 2
#define TOO_MANY_CHARS 3

typedef char str_t[STR_LEN];
typedef char word_t[MAX_WORD_LEN];
typedef word_t words_t[MAX_WORDS];

int split(char *str, words_t a, size_t *n, char *delims, size_t word_len);
int input(str_t s, FILE *f);

#endif
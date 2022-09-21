#ifndef _SPARSE_H_
#define _SPARSE_H_

#include <stdlib.h>
#include <stdio.h>
#include <sys/time.h>

typedef struct
{
    int *a;
    size_t an;
    size_t *ia;
    size_t *ja ;
    size_t r;
    size_t c;
} sparse_t;

int input_matrix_s(FILE *f, sparse_t *s, int form);
void prints(sparse_t s);
sparse_t sum_s(sparse_t a, sparse_t b);
int process_s(int argc, char **argv, int m);


#endif
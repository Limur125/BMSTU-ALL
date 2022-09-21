#ifndef _MATRIX_H_
#define _MATRIX_H_

#include <stdlib.h>
#include <stdio.h>
#include <sys/time.h>

typedef struct
{
    int **m;
    int *mp;
    size_t r;
    size_t c;
} matrix_t;

int process_m(int argc, char **argv, int m);
void freeall_m(matrix_t m);
void printm(matrix_t m);
matrix_t sum_m(matrix_t a, matrix_t b);
int input_matrix_m(FILE *f, matrix_t *m);

#endif
#ifndef _GRAPH_H_
#define _GRAPH_H_

#include <stdio.h>

#define V 20

typedef struct
{
    size_t v;
    int m[V][V];
} matrix_t;

void deisktr(matrix_t a, matrix_t *w);
void matrix_export_to_dot(FILE *f, const char *graph_name, matrix_t a, int m);

#endif
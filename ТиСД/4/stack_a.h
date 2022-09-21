#ifndef _STACK_A_H_
#define _STACK_A_H_

#include <stdlib.h>

#define MAX_N_A 1000

typedef struct
{
    size_t n;
    size_t mn;
    int *stack;
} stack_a_t;

int append_sa(stack_a_t *s, int a);
int create_sa(stack_a_t *s);
void print_sa(stack_a_t s);
int remove_sa(stack_a_t *s, int mode, int *removed);
int copy_sa(stack_a_t src, stack_a_t *dst);


#endif
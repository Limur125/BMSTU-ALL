#ifndef _STACK_L_H_
#define _STACK_L_H_

#include <stdlib.h>

#define MAX_N_L 1000

typedef struct node
{
    int d;
    struct node *next;
} stack_l_t;

stack_l_t *create_stack_node(int a);
stack_l_t *append_sl(stack_l_t *head, stack_l_t *stack_node);
void print_sl(stack_l_t *head);
stack_l_t *remove_sl(stack_l_t *head, int mode, int *rem_int, stack_l_t **rem_node);
stack_l_t *copy_sl(stack_l_t *src);

#endif
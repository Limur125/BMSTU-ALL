#ifndef _QUEUE_A_H_
#define _QUEUE_A_H_

#include <stdlib.h>

#include "request.h"

#define MAX_N_A_R 10000

#define MAX_N_A_I 10000

#define QUEUE_OVERFLOW 11
#define EMPTY_QUEUE 12


typedef struct
{
    request_t queue[MAX_N_A_R];
    int n;
    int pout;
    int pin;
} queue_a_r_t;

typedef struct
{
    int queue[MAX_N_A_I];
    int n;
    int pout;
    int pin;
} queue_a_i_t;

int add_qa_r(queue_a_r_t *q, request_t a);
int remove_qa_r(queue_a_r_t *q, request_t *a);
int add_qa_i(queue_a_i_t *q, int a);
int remove_qa_i(queue_a_i_t *q, int *a);
void print_qa_i(queue_a_i_t q);

#endif
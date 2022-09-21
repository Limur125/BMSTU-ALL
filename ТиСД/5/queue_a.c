#include <string.h>
#include <stdio.h>


#include "queue_a.h"

int add_qa_r(queue_a_r_t *q, request_t a)
{
    if(q->n == MAX_N_A_R)
        return QUEUE_OVERFLOW;
    q->queue[q->pin] = a;
    q->pin++;
    q->n++;
    if (q->pin == MAX_N_A_R)
        q->pin = 0;
    return EXIT_SUCCESS;
}

int remove_qa_r(queue_a_r_t *q, request_t *a)
{
    if (q->n == 0)
        return EMPTY_QUEUE;
    *a = q->queue[q->pout];
    q->n--;
    q->pout++;
    if (q->pout == MAX_N_A_R)
        q->pout = 0;
    return EXIT_SUCCESS;
}

int add_qa_i(queue_a_i_t *q, int a)
{
    if(q->n >= MAX_N_A_I)
        return QUEUE_OVERFLOW;
    q->queue[q->pin] = a;
    q->pin++;
    q->n++;
    if (q->pin == MAX_N_A_I)
        q->pin = 0;
    return EXIT_SUCCESS;
}

int remove_qa_i(queue_a_i_t *q, int *a)
{
    if (q->n == 0)
        return EMPTY_QUEUE;
    *a = q->queue[q->pout];
    q->queue[q->pout] = 0;
    q->n--;
    q->pout++;
    if (q->pout == MAX_N_A_I)
        q->pout = 0;
    return EXIT_SUCCESS;
}

void print_qa_i(queue_a_i_t q)
{
    printf("PIn: %d\n", q.pin);
    printf("POut: %d\n", q.pout);
    printf("N: %d\nQueue: ", q.n);
    for (int i = q.pout, j = 0; j < q.n; i++, j++)
    {   
        if (i == MAX_N_A_I)  
            i = 0;
        printf("%d ", q.queue[i]);
    }
    printf("\n");
    printf("Queue: ");
    for (int i = 0; i < MAX_N_A_I; i++)
        printf("%d ", q.queue[i]);
    printf("\n");
}
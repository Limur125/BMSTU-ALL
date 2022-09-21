#ifndef _QUEUE_L_H_
#define _QUEUE_L_H_

#include "request.h"

typedef struct node_t
{
    int val;
    struct node_t *next;
} node_t;

typedef struct
{
    node_t *head;
    node_t *tail;
} queue_l_t;

typedef struct node_r_t
{
    request_t val;
    struct node_r_t *next;
} node_r_t;

typedef struct
{
    node_r_t *head;
    node_r_t *tail;
} queue_l_r_t;

node_t *create_queue_node(int a);
queue_l_t add_ql(queue_l_t q, node_t *n);
queue_l_t remove_ql(queue_l_t q, int *rm, node_t **rmn);
void print_ql(queue_l_t q);
node_r_t *create_queue_node_r(request_t a);
queue_l_r_t add_qlr(queue_l_r_t q, node_r_t *n);
queue_l_r_t remove_qlr(queue_l_r_t q, request_t *rm);

#endif
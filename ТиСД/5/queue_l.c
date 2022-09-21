#include <stdlib.h>
#include <stdio.h>
#include "queue_l.h"

node_t *create_queue_node(int a)
{
    node_t *tmp = malloc(sizeof(*tmp));
    if (tmp)
    {
        tmp->val = a;
        tmp->next = NULL;
    }
    return tmp;
}

queue_l_t add_ql(queue_l_t q, node_t *n)
{
    if (q.head == NULL)
    {
        q.head = n;
        q.tail = n;
        return q;
    }
    q.tail->next = n;
    q.tail = q.tail->next;
    return q;
}

queue_l_t remove_ql(queue_l_t q, int *rm, node_t **rmn)
{
    if (q.head == NULL)
        return q;
    *rmn = q.head;
    *rm = q.head->val;
    q.head = q.head->next;
    free(*rmn);
    return q;
}

void print_ql(queue_l_t q)
{
    node_t *cur = q.head;
    int n;
    for (n = 0; cur; cur = cur->next, n++);
    printf("Выделено паямти: %lld\nРазмер очереди: %d\nОчередь:\n", n * sizeof(cur), n);
    for(cur = q.head; cur; cur = cur->next)
        printf("%d %p\n", cur->val, (void*) cur);
}

node_r_t *create_queue_node_r(request_t a)
{
    node_r_t *tmp = malloc(sizeof(*tmp));
    if (tmp)
    {
        tmp->val = a;
        tmp->next = NULL;
    }
    return tmp;
}

queue_l_r_t add_qlr(queue_l_r_t q, node_r_t *n)
{
    if (q.head == NULL)
    {
        q.head = n;
        q.tail = n;
        return q;
    }
    q.tail->next = n;
    q.tail = q.tail->next;
    return q;
}

queue_l_r_t remove_qlr(queue_l_r_t q, request_t *rm)
{
    if (q.head == NULL)
        return q;
    node_r_t *rmn;
    rmn = q.head;
    *rm = q.head->val;
    q.head = q.head->next;
    free(rmn);
    return q;
}
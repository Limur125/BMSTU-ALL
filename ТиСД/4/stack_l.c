#include <string.h>
#include <stdio.h>

#include "stack_l.h"

stack_l_t *create_stack_node(int a)
{
    stack_l_t *stack_node = malloc(sizeof(*stack_node));

    if (stack_node)
    {
        stack_node->d = a;
        stack_node->next = NULL;        
    }
        
    return stack_node;
}

stack_l_t *append_sl(stack_l_t *head, stack_l_t *stack_node)
{
    stack_l_t *cur = head;
    size_t n;
    if (head == NULL)
        return stack_node;
    for (n = 0; cur->next; cur = cur->next, n++);
    if (n + 1 == MAX_N_L)
    {
        puts("Стек переполнен.");
        return head;
    }
    cur->next = stack_node;
    return head;
}

stack_l_t *remove_sl(stack_l_t *head, int mode, int *rem_int, stack_l_t **rem_node)
{
    if (head == NULL)
    {
        if (mode)
            puts("Стек пуст.");
        return NULL;
    }
    stack_l_t *cur = head, *prev = NULL;
    for (; cur->next; cur = cur->next)
        prev = cur;
    *rem_int = cur->d;
    *rem_node = cur;
    free(cur);
    if (prev)
        prev->next = NULL;
    else 
        return NULL;
    return head;
}

void print_sl(stack_l_t *head)
{
    stack_l_t *cur = head;
    int n;
    for (n = 0; cur; cur = cur->next, n++);
    printf("Выделено паямти: %lld\nРазмер стека: %d\nСтек:\n", n * sizeof(cur), n);
    for (cur = head; cur; cur = cur->next)
        printf("%d %p\n", cur->d, (void*) cur);
}

stack_l_t *copy_sl(stack_l_t *src)
{
    stack_l_t *tmp = create_stack_node(src->d);
    stack_l_t *dst = tmp;
    stack_l_t *src_c = src->next;  
    while(src_c != NULL)
    {
        tmp = create_stack_node(src_c->d);
        dst = append_sl(dst, tmp); 
        src_c = src_c->next;
    }
    return dst;
}
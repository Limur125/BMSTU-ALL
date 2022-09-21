#include <string.h>
#include <stdio.h>

#include "stack_a.h"

int create_sa(stack_a_t *s)
{
    s->mn = 2;
    s->stack = malloc(s->mn * sizeof(*s->stack));
    if (s->stack == NULL)
    {
        puts("Ошибка выделения памяти.");
        return EXIT_FAILURE;
    }
    s->n = 0;
    return EXIT_SUCCESS;
}

int append_sa(stack_a_t *s, int a)
{
    if (s->n == MAX_N_A)
    {
        puts("Стек переполнен.");
        return EXIT_FAILURE;
    }
    if (s->mn == s->n)
    {
        s->mn *= 2;
        int *tmp = realloc(s->stack, s->mn * sizeof(*s->stack));
        if (tmp == NULL)
        {
            puts("Ошибка выделения памяти.");
            return EXIT_FAILURE;
        }
        s->stack = tmp;
    }
    s->n++;
    s->stack[s->n - 1] = a;
    return EXIT_SUCCESS;
}

int remove_sa(stack_a_t *s, int mode, int *removed)
{
    if (s->n == 0)
    {
        if (mode)
            puts("Стек пуст.");
        return EXIT_FAILURE;
    }
    *removed = s->stack[--s->n];
    if (s->mn / 2 != 0 && s->mn / 2 >= s->n)
    {
        s->mn /= 2;
        // print_sa(*s);
        int *tmp = realloc(s->stack, s->mn * sizeof(*s->stack));
        if (tmp == NULL)
        {
            if (mode)
                puts("Ошибка выделения памяти.");
            return EXIT_FAILURE;
        }
        s->stack = tmp;
    }
    return EXIT_SUCCESS;
}

void print_sa(stack_a_t s)
{
    printf("Выделено паямти: %zu\nРазмер стека: %zu\nСтек: ", s.mn * sizeof(*s.stack), s.n);
    for (size_t i = 0; i < s.n; i++)
        printf("%d ", s.stack[i]);
    printf("\n");
}

int copy_sa(stack_a_t src, stack_a_t *dst)
{
    dst->n = src.n;
    dst->mn = src.mn;
    dst->stack = malloc(dst->mn * sizeof(*dst->stack));
    if (dst->stack == NULL)
    {
        puts("Ошибка выделения памяти.");
        return EXIT_FAILURE;
    }
    for (size_t i = 0; i < dst->n; i++)
        dst->stack[i] = src.stack[i];
    return EXIT_SUCCESS;   
}
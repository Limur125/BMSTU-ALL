#include "matrix.h"
int input_matrix_m(FILE *f, matrix_t *m)
{
    if (f == stdin)
        puts("Введите размеры");
    if (fscanf(f, "%zu", &(m->r)) != 1)
        return EXIT_FAILURE;
    if (fscanf(f, "%zu", &(m->c)) != 1 /*&& m->c != m->r*/)
        return EXIT_FAILURE;

    m->m = malloc(m->r * sizeof(int*));
    m->mp = malloc(m->r * m->c * sizeof(int));
    for (size_t i = 0; i < m->r; i++)
        m->m[i] = m->mp + i * m->c;
    if (f == stdin)
        puts("Введите матрицу");
    for (size_t i = 0; i < m->r; i++)
        for (size_t j = 0; j < m->c; j++)
            fscanf(f, "%d",&(m->m[i][j]));
    return EXIT_SUCCESS;
}

matrix_t sum_m(matrix_t a, matrix_t b)
{
    matrix_t res;
    res.c = a.c;
    res.r = a.r;
    res.m = malloc(res.r * sizeof(int*));
    res.mp = malloc(res.r * res.c * sizeof(int));
    for (size_t i = 0; i < res.r; i++)
        res.m[i] = res.mp + i * res.c;
    for (size_t i = 0; i < a.r; i++)
        for (size_t j = 0; j < a.c; j++)
            res.m[i][j] = a.m[i][j] + b.m[i][j];
    return res;
}

void printm(matrix_t m)
{   
    for (size_t i = 0; i < m.r; i++)
    {
        for (size_t j = 0; j < m.c; j++)
            printf("%4d ", m.m[i][j]);
        printf("\n");
    }
}

void freeall_m(matrix_t m)
{
    free(m.m);
    free(m.mp);
}

int process_m(int argc, char **argv, int m)
{
    setbuf(stdout, NULL);
    matrix_t ma, mb;
    FILE *f = stdin;
    if (argc == 2)
    {
        f = fopen(argv[1], "r");
        if(!f)
            return EXIT_FAILURE;
    }
    if (f == stdin)
        puts("Классический вид");
    if (input_matrix_m(f, &ma))
    {
        puts("Некорректная матрица");
        if (argc == 2)
            fclose(f);
        return EXIT_FAILURE;
    }
    if (argc != 2)
    {
        printm(ma);
        puts("\n");
    }
    if (input_matrix_m(f, &mb))
    {
        puts("Некорректная матрица");
        if (argc == 2)
            fclose(f);
        return EXIT_FAILURE;
    }
    if (mb.r != ma.r || mb.c != ma.c)
    {
        puts("Несовпадающие размеры");
        return EXIT_FAILURE;
    }
    if (argc != 2)
    {
        printm(mb);
        puts("\n");
    }
    struct timeval start, end;
    long long int res = 0;
    matrix_t s;
    for (int i = 0; i < m; i++)
    {
        gettimeofday(&start, NULL);
        s = sum_m(ma, mb);
        gettimeofday(&end, NULL);
        res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
    }
    if (argc != 2)
        printm(s);
    printf("Размеры: %zu %zu\n", s.r, s.c);
    freeall_m(mb);
    freeall_m(ma);
    freeall_m(s);
    printf("Время суммирования матриц (к): %lld\n", res / m);
    if (argc == 2)
        fclose(f);
    return EXIT_SUCCESS;
}
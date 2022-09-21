#include <stdlib.h>
#include <stdio.h>

#include "graph.h"

int main(void)
{
    matrix_t a;
    FILE *f = fopen("in.txt", "r");
    if (fscanf(f, "%zu", &a.v) != 1)
    {
        puts("Ошибка размерности таблицы смежности.");
        fclose(f);
        return 2;
    }
    for (size_t i = 0; i < a.v; i++)
        for (size_t j = 0; j < a.v; j++)
            if (fscanf(f, "%d", &a.m[i][j]) != 1)
            {
                puts("Ошибка чтения данных.");
                fclose(f);
                return 3;
            }
    puts("Таблица смежности:");
    printf("%6c", ' ');
    for (size_t i = 0; i < a.v; i++)
        printf("%3zu", i);
    puts("\n");
    for (size_t i = 0; i < a.v; i++)
    {
        printf("%3zu", i);
        printf("%3c", ' ');
        for (size_t j = 0; j < a.v; j++)
            printf("%3d", a.m[i][j]);
        puts("");
    }
    puts("\n");
    FILE *fgr = fopen("graph.gv", "w");
    matrix_export_to_dot(fgr, "Graph_1", a, 0);
    matrix_t w;
    w.v = a.v;
    deisktr(a, &w);
    matrix_export_to_dot(fgr, "Graph_2", w, 1);
    puts("Таблица стоимостей:");
    printf("%6c", ' ');
    for (size_t i = 0; i < w.v; i++)
        printf("%3zu", i);
    puts("\n");
    for (size_t i = 0; i < w.v; i++)
    {
        printf("%3zu", i);
        printf("%3c", ' ');
        for (size_t j = 0; j < w.v; j++)
            printf("%3d", w.m[i][j]);
        puts("");
    }
    fclose(f);
    fclose(fgr);
    return 0;
}
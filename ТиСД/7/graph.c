#include <stdlib.h>

#include "graph.h"

void deisktr(matrix_t a, matrix_t *w)
{
    for (size_t s = 0; s < a.v; s++)
    {
        int distance[V], index;
        int visited[V];
        for (size_t i = 0; i < a.v; i++)
        {
            distance[i] = INT_MAX;
            visited[i] = 0;
        }

        distance[s] = 0;

        for (size_t j = 0; j < a.v - 1; j++)
        {
            int min = INT_MAX;
            for (size_t i = 0; i < a.v; i++)
                if (!visited[i] && distance[i] <= min)
                {
                    min = distance[i]; 
                    index = i;
                }
            size_t u = index;
            visited[u] = 1;
            for (size_t i = 0; i < a.v; i++)
                if (!visited[i] && a.m[u][i] && distance[u]!=INT_MAX && distance[u] + a.m[u][i] < distance[i])
                    distance[i] = distance[u] + a.m[u][i];
        }

        for (size_t i = 0; i < a.v; i++)
            w->m[s][i] = distance[i];
    }
}

void matrix_export_to_dot(FILE *f, const char *graph_name, matrix_t a, int m)
{
    if (m == 0)
        fprintf(f, "digraph graph_1 {\n");
    fprintf(f, "subgraph %s {\n", graph_name);
    for (size_t i = 0; i < a.v; i++)
        for (size_t j = 0; j < a.v; j++)
            if (a.m[i][j] != 0)
                fprintf(f, "g%d_%zu -> g%d_%zu [label=%d];\n", m + 1, i, m + 1, j, a.m[i][j]);
    fprintf(f, "}\n");
    if (m == 1)
        fprintf(f, "}\n");
}

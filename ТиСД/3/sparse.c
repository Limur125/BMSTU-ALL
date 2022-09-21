#include "sparse.h"
int input_matrix_s(FILE *f, sparse_t *s, int form)
{
    s->a = calloc(s->r * s->c, sizeof(int));
    s->ia = malloc(s->r * s->c * sizeof(size_t));
    s->ja = malloc((s->c + 1) * sizeof(size_t));

    for (size_t i = 0; i < s->c; i++)
        s->ja[i] = s->c * s->r;
    if (f == stdin)
        puts("Введите матрицу. Если формат ввода координатный то для окончания ввода введите некоректый элемент (символ).");
    if (form == 2)
        for (size_t i = 0; i < s->r; i++)
            for (size_t j = 0; j < s->c; j++)
                if (fscanf(f, "%d", s->a + j * s->r + i) != 1)
                    return EXIT_FAILURE;
    if (form == 1)
    {
        size_t l, h;
        while (scanf("%zu", &h) == 1 && h < s->r)
        {
            if(scanf("%zu", &l) != 1 && l > s->c)
                break;
            if(scanf("%d", s->a + l * s->r + h) != 1)
                break;
        }
    }
    size_t ka = 0;
    for (size_t j = 0; j < s->c; j++)
        for (size_t i = 0; i < s->r; i++)
            if (s->a[j * s->r + i] != 0)
            {   
                if (s->ja[j] == s->r * s->c)
                    s->ja[j] = ka;
                s->a[ka] = s->a[j * s->r + i];
                s->ia[ka++] = i;
            }
    s->ja[s->c] = ka;
    for (size_t i = s->c; i > 0; i--)
        if (s->ja[i - 1] == s->c * s->r)
            s->ja[i - 1] = s->ja[i];
    s->an = ka;
    return EXIT_SUCCESS;
}

sparse_t sum_s(sparse_t a, sparse_t b)
{
    sparse_t res;
    res.an = 0;
    res.c = a.c;
    res.r = a.r;
    res.a = malloc(res.c * res.r * sizeof(int));
    res.ia = malloc(res.c * res.r * sizeof(size_t));
    res.ja = malloc((res.c + 1) * sizeof(size_t));
    size_t k, j;
    for (size_t i = 0; i < res.c; i++)
    {
        res.ja[i] = res.an;
        j = a.ja[i];
        k = b.ja[i]; 
        while(k < b.ja[i + 1] || j < a.ja[i + 1])
        {
            if (j != a.ja[i + 1] && k != b.ja[i + 1] && a.ia[j] == b.ia[k])
            {
                //printf("a %zu < %zu %zu < %zu\n", k, b.ja[i + 1], j, a.ja[i + 1]);
                res.a[res.an] = b.a[k] + a.a[j];
                res.ia[res.an] = a.ia[j];
                res.an++;
                k++;
                j++;
            }
            else if (k == b.ja[i + 1] || (j != a.ja[i + 1] && a.ia[j] < b.ia[k]))
            { 
                //printf("b %zu < %zu %zu < %zu\n", k, b.ja[i + 1], j, a.ja[i + 1]);
                res.a[res.an] = a.a[j];
                res.ia[res.an] = a.ia[j];
                res.an++;
                j++;
            }
            else if (j == a.ja[i + 1] || (k != b.ja[i + 1] && a.ia[j] > b.ia[k]))
            {
                //printf("c %zu < %zu %zu < %zu\n", k, b.ja[i + 1], j, a.ja[i + 1]);
                res.a[res.an] = b.a[k];
                res.ia[res.an] = b.ia[k];
                res.an++;
                k++;
            } 
        }
    }
    res.ja[res.c] = res.an;
    return res;
}

void prints(sparse_t s)
{   
    printf("ROW: %zu COL: %zu\n", s.r, s.c);
    printf("AN - N: %zu\n", s.an);
    printf("AN: ");
    for (size_t i = 0; i < s.an; i++)
        printf("%5d ", s.a[i]);
    printf("\nIA: ");
    for (size_t i = 0; i < s.an; i++)
        printf("%5zu ", s.ia[i]);
    printf("\nJA: ");
    for (size_t i = 0; i< s.c + 1; i++)
    printf("%5zu ", s.ja[i]);
    printf("\n");
}
void freeall_s(sparse_t s)
{
    free(s.a);
    free(s.ia);
    free(s.ja);
}

int process_s(int argc, char **argv, int m)
{
    setbuf(stdout, NULL);
    sparse_t sa, sb;
    FILE *f = stdin;
    int form = 2;
    if (argc == 2)
    {
        f = fopen(argv[1], "r");
        if(!f)
            return EXIT_FAILURE;
    }
    if (f == stdin)
    {
        puts("Рaзреженный вид");
        puts("Bыберите формат ввода:\n 1. Координатный (строка столбец значение).\n 2. Обычный.");
        if (scanf("%d", &form) != 1 && (form != 1 || form != 2))
            return EXIT_FAILURE;
    }
    if (f == stdin)
        puts("Введите размеры");
    if (fscanf(f, "%zu", &(sa.r)) != 1)
        return EXIT_FAILURE;
    if (fscanf(f, "%zu", &(sa.c)) != 1 /*&& s->c != s->r*/)
        return EXIT_FAILURE;

    if (input_matrix_s(f, &sa, form))
    {
        puts("Некорректная матрица");
        if (argc == 2)
            fclose(f);
        return EXIT_FAILURE;
    }
    if (argc != 2)
    {
        prints(sa);
        puts("\n");
    }
    fflush(stdin);
    if (f == stdin)
        puts("Введите размеры");
    if (fscanf(f, "%zu", &(sb.r)) != 1 )
        return EXIT_FAILURE;
    if (fscanf(f, "%zu", &(sb.c)) != 1 /*&& s->c != s->r*/)
        return EXIT_FAILURE;

    if (sb.r != sa.r || sb.c != sa.c)
    {
        puts("Несовпадающие размеры");
        return EXIT_FAILURE;
    }
    
    if (input_matrix_s(f, &sb, form))
    {
        puts("Некорректная матрица");
        if (argc == 2)
            fclose(f);
        return EXIT_FAILURE;
    }
    
    if (argc != 2)
    {
        prints(sb);
        puts("\n");
    }
    struct timeval start, end;
    long long int res = 0;
    sparse_t s;
    for (int i = 0; i < m; i++)
    {
        gettimeofday(&start, NULL);
        s = sum_s(sa, sb);
        gettimeofday(&end, NULL);
        res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
    }
    if (m == 1)
        prints(s);
    printf("Размеры: %zu %zu\n", s.r, s.c);
    printf("Время суммирования матриц (р): %lld\n", res / m);
    freeall_s(sa);
    freeall_s(sb);
    freeall_s(s);
    if (argc == 2)
        fclose(f);
    return EXIT_SUCCESS;
}

        
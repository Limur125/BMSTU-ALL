#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/time.h>

#include "tree.h"
#include "hash.h"

#define FILE_ERROR 2
#define TABLE_SIZE 53

int main(void)
{
    setbuf(stdout, NULL);
    int choice;
    tree_t *root = NULL;
    tree_t *root_bal = NULL;
    size_t table_n = 19;
    hash_t *(table[TABLE_SIZE]) = {NULL};
    FILE *f = fopen("data.txt", "r");
    if (f == NULL)
        return FILE_ERROR;
    char tmps[STR_LEN];
    size_t (*hash_f)(char *, size_t) = &hash_func1;
    while (fgets(tmps, STR_LEN, f))
    {
        if (strcmp(tmps, "\n") == 0)
            continue;
        tmps[strlen(tmps) - 1] = 0;
        tree_t *t = create_node(tmps);
        tree_t *tb = create_node(tmps);
        root = add(root, t);
        root_bal = add_bal(root_bal, tb);
        root_bal = balance(root_bal);
        hash_t *h = create(tmps);
        insert_hash(table, table_n, hash_f, h);
    }
    long long int resroot = 0, resrootbal = 0, resfile = 0, reshash = 0;
    int countroot = 0, countrootbal = 0, countfile = 0, counthash = 0;
    int reconst_cmps;
    puts("Введите количество сравнений после которого произвести реструктуризацию таблицы:");
    if (scanf("%d", &reconst_cmps) != 1)
        return EXIT_FAILURE;
    puts("Меню:\n"
        "   1. Поиск дерево.\n"
        "   2. Просмотр дерево.\n"
        "   3. Поиск баланс дерево.\n"
        "   4. Просмотр баланс дерево.\n"
        "   5. Поиск хэш таблица.\n"
        "   6. Просмотр хэш таблицы.\n"
        "   7. Поиск файл.\n"
        "   8. Сравнение.\n"
        "0. Выход.\n"
        "Выберите пункт меню: ");
    if (scanf("%d", &choice) != 1)
        choice = -1;
    fflush(stdin);
    while (choice)
    {
        switch (choice)
        {
            case 1:
            {  
                puts("Введите искомое слово:");
                char ts[STR_LEN];
                int cs;
                fgets(ts, STR_LEN, stdin);
                ts[strlen(ts) - 1] = 0;
                struct timeval start, end;
                gettimeofday(&start, NULL);
                tree_t *t = search(root, ts, &cs);
                gettimeofday(&end, NULL);
                
                if (t != NULL)
                {
                    resroot += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                    countroot++;
                    printf("Искомое слово %s найдено за %d сравнений\n", t->s, cs);
                }
                else
                    printf("Искомое слово не найдено\n");
                break;
            }
            case 2:
            {
                FILE *fbst = fopen("bst.gv", "w");
                tree_export_to_dot(fbst, "binary_search_tree", root);
                fclose(fbst);
                break;
            }
            case 3:
            {
                puts("Введите искомое слово:");
                char ts[STR_LEN];
                int cs;
                fgets(ts, STR_LEN, stdin);
                ts[strlen(ts) - 1] = 0;
                struct timeval start, end;
                gettimeofday(&start, NULL);
                tree_t *t = search(root_bal, ts, &cs);
                gettimeofday(&end, NULL);
                if (t != NULL)
                {
                    resrootbal += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                    countrootbal++;
                    printf("Искомое слово %s найдено за %d сравнений\n", t->s, cs);
                }
                else
                    printf("Искомое слово не найдено\n");
                break;
            }
            case 4:
            {
                FILE *fbbst = fopen("bbst.gv", "w");
                tree_export_to_dot(fbbst, "balaced_binary_search_tree", root_bal);
                fclose(fbbst);
                break;
            }
            case 5:
            {
                puts("Введите искомое слово:");
                char ts[STR_LEN];
                int cs;
                fgets(ts, STR_LEN, stdin);
                ts[strlen(ts) - 1] = 0;
                struct timeval start, end;
                gettimeofday(&start, NULL);                
                hash_t *h = search_hash(table, table_n, hash_f, ts, &cs);
                gettimeofday(&end, NULL);
                if (h != NULL)
                {
                    printf("Искомое слово %s найдено за %d сравнений\n",h->s, cs);
                    reshash += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                    counthash++;
                }
                else
                    printf("Искомое слово не найдено\n");
                if (cs > reconst_cmps)
                {
                    rewind(f);
                    table_n = TABLE_SIZE;
                    hash_f = &hash_func2;
                    for (size_t i = 0; i < table_n; i++)
                        table[i] = NULL;
                    while (fgets(tmps, STR_LEN, f))
                    {
                        if (strcmp(tmps, "\n") == 0)
                            continue;
                        tmps[strlen(tmps) - 1] = 0;
                        hash_t *h = create(tmps);
                        insert_hash(table, table_n, hash_f, h);
                    }
                }
                break;
            }
            case 6:
            {
                print_table(table, table_n);  
                break;
            }
            case 7:
            {
                puts("Введите искомое слово:");
                char ts[STR_LEN], tss[STR_LEN];
                int cs = 0;
                int found = 0;
                fgets(ts, STR_LEN, stdin);
                ts[strlen(ts) - 1] = 0;
                rewind(f);
                struct timeval start, end;
                gettimeofday(&start, NULL);                
                while (fgets(tss, STR_LEN, f))
                {
                    cs++;
                    tss[strlen(tss) - 1] = 0;
                    if (strcmp(tss, ts) == 0)
                    {
                        printf("Искомое слово найдено за %d сравнений\n", cs);
                        found = 1;
                        break;
                    }
                }
                gettimeofday(&end, NULL);
                if (found)
                {
                    resfile += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                    countfile++;
                }
                if (!found)
                    printf("Искомое слово не найдено\n");
                break;
            }
            case 8:
            {
                long long int resroot = 0, resrootbal = 0, resfile = 0, reshash = 0;
                int countroot = 0, countrootbal = 0, countfile = 0, counthash = 0;
                int csr = 0, csrb = 0, csh = 0, csf = 0;
                char ts[STR_LEN];
                for (int i = 0; i < 10; i++)
                {
                    rewind(f);
                    while(fgets(ts, STR_LEN, f))
                    {
                        int cs;
                        ts[strlen(ts) - 1] = 0;

                        struct timeval start, end;
                        gettimeofday(&start, NULL);
                        search(root, ts, &cs);
                        gettimeofday(&end, NULL);
                        csr += cs;
                        resroot += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                        countroot++;                    

                        gettimeofday(&start, NULL);
                        search(root_bal, ts, &cs);
                        gettimeofday(&end, NULL);
                        csrb += cs;
                        resrootbal += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                        countrootbal++;

                        gettimeofday(&start, NULL);                
                        search_hash(table, table_n, hash_f, ts, &cs);
                        gettimeofday(&end, NULL);
                        csh += cs;
                        reshash += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                        counthash++;

                        char  tss[STR_LEN];
                        fpos_t pos;
                        fgetpos(f, &pos);
                        rewind(f);

                        gettimeofday(&start, NULL);                
                        while (fgets(tss, STR_LEN, f))
                        {
                            csf++;
                            tss[strlen(tss) - 1] = 0;
                            if (strcmp(tss, ts) == 0)
                                break;
                        }
                        gettimeofday(&end, NULL);
                        fsetpos(f, &pos);
                        resfile += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                        countfile++;
                    }
                }
                if ((double)csh / counthash > reconst_cmps)
                {
                    rewind(f);
                    table_n = TABLE_SIZE;
                    hash_f = &hash_func2;
                    for (size_t i = 0; i < table_n; i++)
                        table[i] = NULL;
                    while (fgets(tmps, STR_LEN, f))
                    {
                        if (strcmp(tmps, "\n") == 0)
                            continue;
                        tmps[strlen(tmps) - 1] = 0;
                        hash_t *h = create(tmps);
                        insert_hash(table, table_n, hash_f, h);
                    }
                }
                printf("Время поиска в дереве: %lf\n", (double)resroot / countroot);
                printf("Количество сравнений в дереве: %lf\n", (double)csr / countroot);
                printf("Время поиска в сбалансированном дереве: %lf\n", (double)resrootbal / countrootbal);
                printf("Количество сравнений в сбалансированном дереве: %lf\n", (double)csrb / countrootbal);
                printf("Время поиска в хэш-таблице:%lf\n", (double)reshash / counthash);
                printf("Количество сравнений в таблице: %lf\n", (double)csh / counthash);
                printf("Время поиска в файле: %lf\n", (double)resfile / countfile);
                printf("Количество сравнений в файле: %lf\n", (double)csf / countfile);
            }
        }
        puts("Выберите пункт меню:");
        fflush(stdin);
        if (scanf("%d", &choice) != 1)
            choice = -1;
        fflush(stdin);
    }
    return EXIT_SUCCESS;
}
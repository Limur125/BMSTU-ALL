#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/time.h>

#include "stack_a.h"
#include "stack_l.h"

int main(void)
{
    setbuf(stdout, NULL);
    int choice;
    stack_a_t sa;
    stack_l_t *head = NULL;
    create_sa(&sa);
    puts("Меню:\n"
        "   Операции со стеком-массивом:\n"
        "       1. Добавить элементы в стек.\n"
        "       2. Удалить элементы из стека.\n"
        "       3. Распечатать убывающие серии последовательности целых чисел в обратном порядке.\n"
        "       4. Просмотр состояния стека.\n"
        "       5. Время очистки заполненного стека.\n"
        "   Операции со стеком-списком:\n"
        "       6. Добавить элементы в стек.\n"
        "       7. Удалить элементы из стека.\n"
        "       8. Просмотр состояния стека c адресами.\n"
        "       9. Распечатать убывающие серии последовательности целых чисел в обратном порядке.\n"
        "       10. Время очистки стека.\n"
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
                int n;
                puts("Введите количество добавляемых элементов.");
                if (scanf("%d", &n) != 1 || n < 1)
                {
                    puts("Некорректное число.");
                    break;
                }
                for (int i = 0; i < n; i++)
                {
                    int tmp;
                    if (scanf("%d", &tmp) != 1)
                    {
                        puts("Некорректный ввод.");
                        break;
                    }
                    if (append_sa(&sa, tmp))
                        break;
                }
                break;
            }
            case 2:
            {
                int n;
                puts("Введите количество удаляемых элементов.");
                if (scanf("%d", &n) != 1 || n < 1)
                {
                    puts("Некорректное число.");
                    break;
                }
                printf("Удаленные элементы: ");
                int rm;
                for (int i = 0; i < n; i++)
                {
                    if (remove_sa(&sa, 1, &rm))
                        break;
                    printf("%d ", rm);
                }
                printf("\n");
                break;
            }
            case 3:
            {
                printf("Убывающие послетовательности в обратном порядке: \n");
                size_t n = sa.n;
                int p = INT_MIN;
                int rm;
                for (size_t i = 0; i < n; i++)
                {
                    if (remove_sa(&sa, 1, &rm))
                        break;
                    if (rm > p)
                    {
                        printf("%d ", rm);
                        p = rm;
                    }
                    else
                    {
                        if (sa.n == 0)
                            break;
                        int tmp;
                        if (remove_sa(&sa, 1, &tmp))
                            break;
                        if (tmp > rm)
                            printf("\n%d ", rm);
                        if (append_sa(&sa, tmp))
                            break;
                        p = rm;
                    }
                }
                printf("\n");
                break;
            }
            case 4:
            {
                print_sa(sa);
                break;
            }
            case 5:
            {
                stack_a_t sa_copy, tmp;
                create_sa(&tmp);
                for (size_t i = 0; i < MAX_N_A; i++)
                    append_sa(&tmp, rand() % 10);
                struct timeval start, end;
                long long int res = 0;
                for (int j = 0; j < 50; j++)
                {
                    int rm;
                    copy_sa(tmp, &sa_copy);
                    size_t n = sa_copy.n;
                    gettimeofday(&start, NULL);
                    for (size_t i = 0; i < n; i++)
                        if (remove_sa(&sa_copy, 0, &rm))
                            break;
                    gettimeofday(&end, NULL);
                    res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                    free(sa_copy.stack);
                }
                free(tmp.stack);
                printf("Время очистки стека: %lld\n", res / 50);
                break;
            }
            case 6:
            {
                int n;
                puts("Введите количество добавляемых элементов.");
                if (scanf("%d", &n) != 1 || n < 1)
                {
                    puts("Некорректное число.");
                    break;
                }
                for (int i = 0; i < n; i++)
                {
                    int tmp;
                    if (scanf("%d", &tmp) != 1)
                    {
                        puts("Некорректный ввод.");
                        break;
                    }
                    stack_l_t *tmp_node = create_stack_node(tmp);
                    if (tmp_node == NULL)
                    {
                        puts("Ошибка выделения памяти.");
                        break;
                    }
                    head = append_sl(head, tmp_node);
                }
                break;
            }
            case 7:
            {
                int n;
                puts("Введите количество удаляемых элементов.");
                if (scanf("%d", &n) != 1 || n < 1)
                {
                    puts("Некорректное число.");
                    break;
                }
                printf("Удаленные элементы:\n");
                for (int i = 0; i < n; i++)
                {
                    int rm;
                    stack_l_t *rmn;
                    head = remove_sl(head, 1, &rm, &rmn);
                    printf("%d %p\n", rm, (void*) rmn);
                    if (head == NULL)
                        break;
                }
                printf("\n");
                break;
            }
            case 8:
            {
                print_sl(head);
                break;
            }
            case 9:
            {
                printf("Убывающие послетовательности в обратном порядке:\n");
                int p = INT_MIN;
                int rm;
                stack_l_t *rmn;
                while (head)
                {
                    head = remove_sl(head, 1, &rm, &rmn);
                    if (rm > p)
                    {
                        printf("%d %p\n", rm, (void*) rmn);
                        p = rm;
                    }
                    else
                    {
                        if (head == NULL)
                            break;
                        int tmp;
                        stack_l_t *trmn;
                        head = remove_sl(head, 1, &tmp, &trmn);
                        if (tmp > rm)
                            printf("\n%d %p\n", rm, (void*) rmn);
                        stack_l_t *tmp_node = create_stack_node(tmp);
                        if (tmp_node == NULL)
                        {
                            puts("Ошибка выделения памяти.");
                            break;
                        }
                        head = append_sl(head, tmp_node);
                        p = rm;
                    }
                }
                printf("\n");
                break;
            }
            case 10:
            {
                stack_l_t *sl_copy_h, *tmp_h = NULL;
                int rm;
                stack_l_t *rmn;
                for (size_t i = 0; i < MAX_N_L; i++)
                {
                    stack_l_t *tmp_n = create_stack_node(rand() % 10);
                    tmp_h = append_sl(tmp_h, tmp_n);
                }
                struct timeval start, end;
                long long int res = 0;
                for (int j = 0; j < 50; j++)
                {
                    sl_copy_h = copy_sl(tmp_h);
                    gettimeofday(&start, NULL);
                    while (sl_copy_h)
                        sl_copy_h = remove_sl(sl_copy_h, 0, &rm, &rmn);
                    gettimeofday(&end, NULL);
                    res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
                }
                printf("Время очистки стека: %lld\n", res / 3);
                while (tmp_h)
                        tmp_h = remove_sl(tmp_h, 0, &rm, &rmn);
                break;
            }
        }
        puts("Выберите пункт меню:");
        fflush(stdin);
        if (scanf("%d", &choice) != 1)
            choice = -1;
        fflush(stdin);
    }
    free(sa.stack);
    return EXIT_SUCCESS;
}
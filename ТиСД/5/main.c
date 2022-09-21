#include <stdio.h>
#include <sys/time.h>
#include <stdlib.h> 
#include <math.h>
#include "queue_a.h"
#include "queue_l.h"
#include "request.h"

#define P 0.8
#define CLOCK 0.005

int main(void)
{
    setbuf(stdout, NULL);
    srand(time(NULL));
    int choice;
    queue_a_i_t qai = { 0 };
    queue_l_t ql = { 0 };
    puts("Меню:\n"
        "   Операции с очередью-массивом:\n"
        "       1. Добавить элементы в очередь.\n"
        "       2. Удалить элементы из очереди.\n"
        "       3. Просмотр состояния очереди.\n"
        "       4. Задание по варианту.\n"
        "   Операции со стеком-списком:\n"
        "       5. Добавить элементы в очередь.\n"
        "       6. Удалить элементы из очереди.\n"
        "       7. Просмотр состояния очереди c адресами.\n"
        "       8. Задание по варианту.\n"
        "   9. Анализ эфективности работы.\n"
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
                puts("Введите добавляемые элементы.");
                for (int i = 0; i < n; i++)
                {
                    int tmp;
                    if (scanf("%d", &tmp) != 1)
                    {
                        puts("Некорректный ввод.");
                        break;
                    }
                    if (add_qa_i(&qai, tmp))
                    {
                        puts("Переполнение очереди.");
                        break;
                    }
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
                    if (remove_qa_i(&qai, &rm))
                    {   
                        puts("Пустая очередь.");
                        break;
                    }
                    printf("%d ", rm);
                }
                printf("\n");
                break;
            }
            case 3:
            {
                print_qa_i(qai);
                break;
            }
            case 4:
            {
                queue_a_r_t qar= { 0 };
                double wta, wtb;
                double sta, stb;
                printf("Введите границы интервала времени поступления:\n");
                scanf("%lf%lf", &wta, &wtb);
                printf("Введите границы интервала времени обслуживания:\n");
                scanf("%lf%lf", &sta, &stb);
                int exit_req = -1;
                int f = 0, d = 0;
                int servised_req = -1;
                int entered_req = 0;
                double ctime = 0, itime = 0, wtime = 0;
                request_t er = create_req(wta, wtb, sta, stb);
                request_t sr = create_req(wta, wtb, sta, stb);
                er.etime = -1e-8;
                sr.stime = -1e-8;
                int idle = 0;
                while (exit_req < 1000)
                {
                    if (er.etime < 0)
                    {
                        entered_req++;
                        if (add_qa_r(&qar, er) == QUEUE_OVERFLOW)
                        {
                            f = 1; 
                            puts("Очередь переполнена");
                            break;
                        }
                        er = create_req(wta, wtb, sta, stb);
                    }
                    if (sr.stime < 0)
                    {
                        if (((double) rand()) / RAND_MAX < P && servised_req && !idle )
                        {
                            servised_req++;
                            sr.wtime = 0;
                            sr.stime = ((double) rand()) / RAND_MAX * (stb - sta) + sta;
                            add_qa_r(&qar, sr);
                        }
                        else if(!idle)
                        {
                            servised_req++;
                            exit_req++;   
                            d = 1;
                        }
                        request_t tmp;
                        if (remove_qa_r(&qar, &tmp) != EMPTY_QUEUE)
                        {
                            idle = 0;
                            wtime += tmp.wtime ;
                            sr = tmp;
                        }
                        else
                        {
                            idle = 1;
                        }
                    }
                    ctime += CLOCK;
                    if (idle)
                        itime += CLOCK;
                    for (int i = qar.pout, j = 0; j < qar.n; i++, j++)
                    {   
                        if (i == MAX_N_A_R)  
                            i = 0;
                        qar.queue[i].wtime += CLOCK;
                    }
                    sr.stime -= CLOCK;
                    er.etime -= CLOCK;
                    if (exit_req % 100 == 0 && d)
                    {
                        d = 0;
                        printf("Обслужено заявок: %d\n"
                            "Время выполнения: %.3lf\n"
                            "Принято в очередь: %d\n"
                            "Время простоя: %.3lf\n"
                            "Среднее время ожидания: %.3lf\n"
                            "Срабатывания аппарата: %d\n\n\n",\
                            exit_req, ctime, entered_req, itime, wtime / servised_req, servised_req);
                    }
                }
                if (f)
                    break;
                double x = 1000.0;
                double s = 0.0;
                double t = ((double) stb + sta) / 2;
                while (x > 1)
                {
                    s += (1 - P) * x * t;
                    t = t + ((double) stb + sta) / 2;
                    x *= P;
                }
                s = fmax(s, 500 * (wta + wtb));
                
                printf("Время выполнения: %.3lf\n"
                    "Ожидаемое время выполнения: %.3lf\n"
                    "Погрешность: %.3lf%%\n"
                    "Принято в очередь: %d\n"
                    "Ожидаемое количество заявок: %.3lf\n"
                    "Погрешность: %.3lf%%\n"
                    "Время простоя: %.3lf\n"
                    "Среднее время ожидания: %.3lf\n"
                    "Срабатывания аппарата: %d\n"
                    "Обслужено заявок: %d\n",\
                    ctime, s, 100 * (ctime - s) / s,\
                    entered_req, ctime * 2 / (wta + wtb), 100 * (entered_req - ctime * 2 / (wta + wtb)) / (2 * ctime / (wta + wtb)),\
                    itime, wtime / servised_req, servised_req, exit_req);
                    break;
            }
            case 5:
            {
                int n;
                puts("Введите количество добавляемых элементов.");
                if (scanf("%d", &n) != 1 || n < 1)
                {
                    puts("Некорректное число.");
                    break;
                }
                puts("Введите добавляемые элементы.");
                for (int i = 0; i < n; i++)
                {
                    int tmp;
                    if (scanf("%d", &tmp) != 1)
                    {
                        puts("Некорректный ввод.");
                        break;
                    }
                    node_t *tmp_node = create_queue_node(tmp);
                    if (tmp_node == NULL)
                    {
                        puts("Ошибка выделения памяти.");
                        break;
                    }
                    ql = add_ql(ql, tmp_node);
                }
                break;
            }
            case 6:
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
                    if (ql.head == NULL)
                    {
                        puts("Пустая очередь.");
                        break;
                    }
                    int rm;
                    node_t *rmn;
                    ql = remove_ql(ql, &rm, &rmn);
                    printf("%d %p\n", rm, (void*) rmn);
                }
                printf("\n");
                break;
            }
            case 7:
            {
                print_ql(ql);
                break;
            }
            case 8:
            {
                queue_l_r_t qlr= { 0 };
                double wta, wtb;
                double sta, stb;
                printf("Введите границы интервала времени поступления:\n");
                scanf("%lf%lf", &wta, &wtb);
                printf("Введите границы интервала времени обслуживания:\n");
                scanf("%lf%lf", &sta, &stb);
                int exit_req = -1;
                int f = 0, d = 0;
                int servised_req = -1;
                int entered_req = 0;
                double ctime = 0, itime = 0, wtime = 0;
                request_t er = create_req(wta, wtb, sta, stb);
                request_t sr = create_req(wta, wtb, sta, stb);
                er.etime = -1e-8;
                sr.stime = -1e-8;
                int idle = 0;
                while (exit_req < 1000)
                {
                    if (er.etime < 0)
                    {
                        entered_req++;
                        qlr = add_qlr(qlr, create_queue_node_r(er));
                        er = create_req(wta, wtb, sta, stb);
                    }
                    if (sr.stime < 0)
                    {
                        if (((double) rand()) / RAND_MAX < P && servised_req && !idle )
                        {
                            servised_req++;
                            sr.wtime = 0;
                            sr.stime = ((double) rand()) / RAND_MAX * (stb - sta) + sta;
                            qlr = add_qlr(qlr, create_queue_node_r(sr));
                        }
                        else if(!idle)
                        {
                            servised_req++;
                            exit_req++;   
                            d = 1;
                        }
                        if (qlr.head != NULL)
                        {
                            request_t tmp;
                            qlr = remove_qlr(qlr, &tmp);
                            idle = 0;
                            wtime += tmp.wtime ;
                            sr = tmp;
                        }
                        else
                        {
                            idle = 1;
                        }
                    }
                    ctime += CLOCK;
                    if (idle)
                        itime += CLOCK;
                    for (node_r_t *cur = qlr.head; cur; cur = cur->next)
                        cur->val.wtime += CLOCK;
                    sr.stime -= CLOCK;
                    er.etime -= CLOCK;
                    if (exit_req % 100 == 0 && d)
                    {
                        d = 0;
                        printf("Обслужено заявок: %d\n"
                            "Время выполнения: %.3lf\n"
                            "Принято в очередь: %d\n"
                            "Время простоя: %.3lf\n"
                            "Среднее время ожидания: %.3lf\n"
                            "Срабатывания аппарата: %d\n\n\n",\
                            exit_req, ctime, entered_req, itime, wtime / servised_req, servised_req);
                    }
                }
                if (f)
                    break;
                double x = 1000.0;
                double s = 0.0;
                double t = ((double) stb + sta) / 2;
                while (x > 1)
                {
                    s += (1 - P) * x * t;
                    t = t + ((double) stb + sta) / 2;
                    x *= P;
                }
                s = fmax(s, 500 * (wta + wtb));
                
                printf("Время выполнения: %.3lf\n"
                    "Ожидаемое время выполнения: %.3lf\n"
                    "Погрешность: %.3lf%%\n"
                    "Принято в очередь: %d\n"
                    "Ожидаемое количество заявок: %.3lf\n"
                    "Погрешность: %.3lf%%\n"
                    "Время простоя: %.3lf\n"
                    "Среднее время ожидания: %.3lf\n"
                    "Срабатывания аппарата: %d\n"
                    "Обслужено заявок: %d\n",\
                    ctime, s, 100 * (ctime - s) / s,\
                    entered_req, ctime * 2 / (wta + wtb), 100 * (entered_req - ctime * 2 / (wta + wtb)) / (2 * ctime / (wta + wtb)),\
                    itime, wtime / servised_req, servised_req, exit_req);
                    break;   
            }
            case 9:
            {
                queue_a_i_t qa = { 0 };
                struct timeval start, stop;
                long long int resa = 0, resr = 0;
                for (int i = 0; i < 100; i++)
                {
                    gettimeofday(&start, NULL);
                    for (int j = 0; j < 1000; j++)
                        add_qa_i(&qa, 4);
                    gettimeofday(&stop, NULL);
                    resa += (stop.tv_sec - start.tv_sec) * 1000000LL + (stop.tv_usec - start.tv_usec);
                    int t;
                    gettimeofday(&start, NULL);
                    for (int j = 0; j < 1000; j++)
                        remove_qa_i(&qa, &t);
                    gettimeofday(&stop, NULL);
                    resr += (stop.tv_sec - start.tv_sec) * 1000000LL + (stop.tv_usec - start.tv_usec);
                }
                printf("%30s|%30s|%30s\n","Вид очереди", "Добавление", "Удаление");
                printf("-------------------------------------------------------------------------------------------\n");
                printf("%25s|%25lld|%25lld\n", "Массив", resa / 100, resr / 100);
                resa = 0, resr = 0;
                queue_l_t qli = { 0 };
                for (int i = 0; i < 100; i++)
                {
                    gettimeofday(&start, NULL);
                    for (int j = 0; j < 1000; j++)
                        qli = add_ql(qli, create_queue_node(4));
                    gettimeofday(&stop, NULL);
                    resa += (stop.tv_sec - start.tv_sec) * 1000000LL + (stop.tv_usec - start.tv_usec);
                    int t;
                    node_t *r;
                    gettimeofday(&start, NULL);
                    for (int j = 0; j < 1000; j++)
                        qli = remove_ql(qli, &t, &r);
                    gettimeofday(&stop, NULL);
                    resr += (stop.tv_sec - start.tv_sec) * 1000000LL + (stop.tv_usec - start.tv_usec);
                }
                printf("--------------------------------------------------------------------------------------------\n");
                printf("%25s|%25lld|%25lld\n", "Список", resa / 100, resr / 100);
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
 
 


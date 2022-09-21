#include "phone.h"
#include "sorts.h"
// Функция исполняющая меню
int menu(int pos, phone_t table[], key_t keys[], size_t *tn, size_t max_tn)
{
    switch (pos)
    {
    case 1:
    {
        puts("Ввод добавление абонента в таблицу.");
        phone_t phone = { 0 };
        fscanf_phone(stdin, &phone);
        if (*tn == max_tn)
            return EXIT_FAILURE;
        (*tn)++;
        table[*tn - 1] = phone;
        keys[*tn - 1].prev_i = *tn - 1;
        strcpy(keys[*tn - 1].surname, phone.surname);
        return EXIT_SUCCESS;
    }
    case 2:
    {
        puts("Удаление абонента из таблицы.\n Введите номер абонента: ");
        char del_s[NUMBERLEN];
        size_t del_i;
        scanf("%s", del_s);
        for (size_t i = 0; i < *tn; i++)
            if (!strcmp(table[i].number, del_s))
                del_i = i;
        for (size_t i = del_i; i < *tn - 1; i++)
            table[i] = table[i + 1];
        (*tn)--;
        for (size_t i = 0; i < *tn; i++)
        {
            keys[i].prev_i = i;
            strcpy(keys[i].surname, table[i].surname);
        }
        return EXIT_SUCCESS;
    }
    case 3:
    {
        puts("Поиск друзей у которых ДР через неделю.\nВведите сегодняшнюю дату (dd.mm.yyyy): ");
        int d, m, y, d7, m7;
        scanf("%d.%d.%d", &d, &m, &y);
        if (!is_date(d, m, y))
            return EXIT_SUCCESS;
        int months[] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        if (y % 400 == 0 || (y % 4 == 0 && y % 100 != 0))
            months[1]++;
        d7 = d + 7;
        m7 = m;
        if (d7 > months[m - 1])
        {
            d7 -= months[m - 1];
            m7++;
        }
        m7 %= 12;  
        for (size_t i = 0; i < *tn; i++)
            if (table[i].status == 'p')
            {
                if (m == m7 && m == table[i].status_u.p.month)
                {
                    if (table[i].status_u.p.day >= d && table[i].status_u.p.day <= d7)
                        print_phone(table[i]);
                }
                else
                {
                    if (table[i].status_u.p.month == m && table[i].status_u.p.day >= d)
                        print_phone(table[i]);
                    else if (table[i].status_u.p.month == m7 && table[i].status_u.p.day <= d7)
                        print_phone(table[i]);
                }
            }
        return EXIT_SUCCESS;
    }
    case 4:
    { 
        puts("Сортировка таблицы ключей...");
        k_bublesort(keys, *tn);
        for (size_t i = 0; i < *tn; i++)
        {
            printf("%02zu", i);
            print_key(keys[i]);
        }
        return EXIT_SUCCESS;
    }
    case 5:
    {
        puts("Сортировка исходной таблицы...");
        t_bublesort(table, *tn);        
        for (size_t i = 0; i < *tn; i++)
            print_phone(table[i]);
        for (size_t i = 0; i < *tn; i++)
        {
            keys[i].prev_i = i;
            strcpy(keys[i].surname, table[i].surname);
        }
        return EXIT_SUCCESS;
    }
    case 6:
    {
        puts("Сортировка таблицы ключей и вывод исходной таблицы по ней...");
        k_bublesort(keys, *tn);
        for (size_t i = 0; i < *tn; i++)
            print_phone(table[keys[i].prev_i]);
        return EXIT_SUCCESS;
    }
    case 7:
    {
        phone_t t_copy[60];
        key_t k_copy[60];
        struct timeval start, end;
        long long int res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                k_copy[i] = keys[i];
            gettimeofday(&start, NULL);
            k_bublesort(k_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Сортировка с помощью таблицы ключей: %lld\n", res / 100);
        res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                t_copy[i] = table[i];
            gettimeofday(&start, NULL);
            t_bublesort(t_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Сортировка исходной таблицы: %lld\n", res / 100);
        return EXIT_SUCCESS;
    }
    case 8:
    {   
        phone_t t_copy[60];
        struct timeval start, end;
        long long int res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                t_copy[i] = table[i];
            gettimeofday(&start, NULL);
            t_shekersort(t_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Шейкер сортировка : %lld\n", res / 100);
        res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                t_copy[i] = table[i];
            gettimeofday(&start, NULL);
            t_bublesort(t_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Сортировка пузырьком: %lld\n", res / 100);
        return EXIT_SUCCESS;
    }
    case 9:
    {
        for (size_t i = 0; i < *tn; i++)
            print_phone(table[i]);
        return EXIT_SUCCESS;
    }
    case 10:
    {
        key_t k_copy[60];
        struct timeval start, end;
        long long int res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                k_copy[i] = keys[i];
            gettimeofday(&start, NULL);
            k_bublesort(k_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Сортировка таблицы ключей сортировкой пузырьком: %lld\n", res / 100);
        res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                k_copy[i] = keys[i];
            gettimeofday(&start, NULL);
            k_shekersort(k_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Сортировка таблицы ключей шейкер сортировкой: %lld\n", res / 100);
        return EXIT_SUCCESS;
    }
    case 11:
    {
        phone_t t_copy[60];
        key_t k_copy[60];
        struct timeval start, end;
        long long int res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                k_copy[i] = keys[i];
            gettimeofday(&start, NULL);
            k_shekersort(k_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Сортировка с помощью таблицы ключей: %lld\n", res / 100);
        res = 0;
        for (size_t i = 0; i < 100; i++)
        {
            for (size_t i = 0; i < *tn; i++)
                t_copy[i] = table[i];
            gettimeofday(&start, NULL);
            t_shekersort(t_copy, *tn);
            gettimeofday(&end, NULL);
            res += (end.tv_sec - start.tv_sec) * 1000000LL + (end.tv_usec - start.tv_usec);
        }
        printf("Сортировка исходной таблицы: %lld\n", res / 100);
        return EXIT_SUCCESS;
    }
    default:
        return EXIT_FAILURE;
    }
}

int main(void)
{
    setbuf(stdout, NULL);
    FILE *f = fopen("data.txt", "r");
    size_t n;
    fscanf(f, "%zu\n", &n);
    phone_t t[60];
    key_t k[60];
    for (size_t i = 0; i < n; i++)
        read_file(f, &t[i]);
    for (size_t i = 0; i < n; i++)
    {
        k[i].prev_i = i;
        strcpy(k[i].surname, t[i].surname);
    }
    puts("Меню:\n"
        "1. Добавление номера.\n"
        "2. Удаление номера.\n"
        "3. Поиск друзей, у кого ДР через неделю.\n"
        "4. Сортировка таблицы ключей и вывод ее на экран.\n"
        "5. Сортировка исходной таблицы и вывод ее на экран.\n"
        "6. Сортировка таблицы ключей и вывод по ней исходной таблицы.\n"
        "7. Вывод разницы между сортировками таблицы ключей и исходной таблицы.\n"
        "8. Вывод разницы между сортировками исходной таблицы методом \"пузырька\" и \"шейкер\".\n"
        "9. Вывод исходной таблицы.\n"
        "10. Сравнение разных методов сортировки для таблицы ключей.\n"
        "0. Выход.");
    int choice;
    puts("Введите выбор пункта меню: ");
    scanf("%d", &choice);
    fflush(stdin);
    while (!menu(choice, t, k, &n, 60))
    {
        puts("Введите выбор пункта меню: ");
        scanf("%d", &choice);
        fflush(stdin);
    }
    return 0;
}
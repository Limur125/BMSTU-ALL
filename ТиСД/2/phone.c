#include "phone.h"
#include "sorts.h"
// Ввод абонента с клавиатуры
int fscanf_phone(FILE *f, phone_t *phone)
{
    phone_t tmp = { 0 };
    puts("Введите фамилию абонента:");
    if (!fgets(tmp.surname, SURNAMELEN, f))
        return EXIT_FAILURE;
    char *pos = strrchr(tmp.surname, '\n');
    if (strlen(tmp.surname) == (SURNAMELEN - 1) && !pos)
        return EXIT_FAILURE;
    if (pos)
        *pos = 0;

    puts("Введите имя абонента:");
    if (!fgets(tmp.name, NAMELEN, f))
        return EXIT_FAILURE;
    pos = strrchr(tmp.name, '\n');
    if (strlen(tmp.name) == (NAMELEN - 1) && !pos)
        return EXIT_FAILURE;
    if (pos)
        *pos = 0;
    puts("Введите номер абонента:");
    if (!fgets(tmp.number, NUMBERLEN, f))
        return EXIT_FAILURE;
    pos = strrchr(tmp.number, '\n');
    if (strlen(tmp.number) == (NUMBERLEN - 1) && !pos)
        return EXIT_FAILURE;
    if (pos)
        *pos = 0;
    puts("Введите адрес абонента:");
    if (!fgets(tmp.adress, ADRESSLEN, f))
        return EXIT_FAILURE;
    pos = strrchr(tmp.adress, '\n');
    if (strlen(tmp.adress) == (ADRESSLEN - 1) && !pos)
        return EXIT_FAILURE;
    if (pos)
        *pos = 0;
    puts("Введите статус номера (personal или official):");
    char st[STATUSLEN];
    if (!fgets(st, STATUSLEN, f))
        return EXIT_FAILURE;

    if (!strcmp(st, "personal\n"))
    {
        puts("Введите дату рождения (дд.мм.гггг):");
        if (fscanf(f, "%d.", &tmp.status_u.p.day) != 1)
            return EXIT_FAILURE;
        if (fscanf(f, "%d.", &tmp.status_u.p.month) != 1)
            return EXIT_FAILURE;
        if (fscanf(f, "%d", &tmp.status_u.p.year) != 1)
            return EXIT_FAILURE;

        if (!is_date(tmp.status_u.p.day, tmp.status_u.p.month, tmp.status_u.p.year))
            return EXIT_FAILURE;
        tmp.status = 'p';
        *phone = tmp;
        return EXIT_SUCCESS;
    }
    else if(!strcmp(st, "official\n"))
    {
        puts("Введите должность абонента:");
        if (!fgets(tmp.status_u.o.post, POSTLEN, f))
            return EXIT_FAILURE;
        pos = strrchr(tmp.status_u.o.post, '\n');
        if (strlen(tmp.status_u.o.post) == (POSTLEN - 1) && !pos)
            return EXIT_FAILURE;
        if (pos)
            *pos = 0;
        puts("Введите место работы абонента:");
        if (!fgets(tmp.status_u.o.organization, ORGLEN, f))
            return EXIT_FAILURE;
        pos = strrchr(tmp.status_u.o.organization, '\n');
        if (strlen(tmp.status_u.o.organization) == (ORGLEN - 1) && !pos)
            return EXIT_FAILURE;
        if (pos)
            *pos = 0;
        tmp.status = 'o';
        *phone = tmp;
        return EXIT_SUCCESS;
    }
    return EXIT_FAILURE;
}

// Проверка корректности даты
int is_date(int day, int month, int year)
{
    int months[] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))
        months[1]++;
    if (year > 0 && month > 0 && month <= 12 && day > 0 && day <= months[month - 1])
        return 1;
    return 0;
}

// Печать абонента
void print_phone(phone_t p)
{
    printf("%15s", p.surname);
    printf("%15s", p.name);
    printf("%15s", p.number);
    printf("%55s", p.adress);
    switch (p.status)
    {
        case 'o':
        {
            printf("%45s", p.status_u.o.post);
            printf("%40s\n", p.status_u.o.organization);

            break;
        }
        case 'p':
        {
            printf("     %02d.%02d.%4d\n", p.status_u.p.day, p.status_u.p.month, p.status_u.p.year);
        }    
    }
}

// Печать ключа
void print_key(key_t k)
{
    printf("%4zu", k.prev_i);
    printf("%20s\n", k.surname);
}

// Считывание информации об абонентах из файла
void read_file(FILE *f, phone_t *p)
{
    phone_t tmp = { 0 };
    fgets(tmp.surname, SURNAMELEN, f);
    tmp.surname[strlen(tmp.surname) - 1] = 0;
    fgets(tmp.name, NAMELEN, f);
    tmp.name[strlen(tmp.name) - 1] = 0;
    fgets(tmp.number, NUMBERLEN, f);
    tmp.number[strlen(tmp.number) - 1] = 0;
    fgets(tmp.adress, ADRESSLEN, f);
    tmp.adress[strlen(tmp.adress) - 1] = 0;
    char st[STATUSLEN];
    fgets(st, STATUSLEN, f);
    if (!strcmp(st, "personal\n"))
    {
        fscanf(f, "%d.", &tmp.status_u.p.day);
        fscanf(f, "%d.", &tmp.status_u.p.month);
        fscanf(f, "%d\n", &tmp.status_u.p.year);
        tmp.status = 'p';
        *p = tmp;
        return;
    }
    else if(!strcmp(st, "official\n"))
    {
        fgets(tmp.status_u.o.post, POSTLEN, f);
        tmp.status_u.o.post[strlen(tmp.status_u.o.post) - 1] = 0;
        fgets(tmp.status_u.o.organization, ORGLEN, f);
        tmp.status_u.o.organization[strlen(tmp.status_u.o.organization) - 1] = 0;
        tmp.status = 'o';
        *p = tmp;
        return;
    }
}

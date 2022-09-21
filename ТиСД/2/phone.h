#ifndef _PHONE_H_
#define _PHONE_H_

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <sys/time.h>

#define SURNAMELEN 15
#define NAMELEN 15
#define NUMBERLEN 15
#define POSTLEN 30
#define ORGLEN 40
#define ADRESSLEN 55
#define STATUSLEN 10
// #pragma pack(push, 1)
typedef struct
{
    char post[POSTLEN];
    char organization[ORGLEN];
} official_t;

typedef struct
{
    int day;
    int month;
    int year;
} personal_t;

typedef union 
{
    personal_t p;
    official_t o;
} status_t;

typedef struct
{
    char surname[SURNAMELEN];
    char name[NAMELEN];
    char number[NUMBERLEN];
    char adress[ADRESSLEN];
    char status;
    status_t status_u;
} phone_t;

typedef struct
{
    size_t prev_i;
    char surname[SURNAMELEN];
} key_t;
// #pragma pack(pop)
// int a(void)
// {sizeof(phone_t);
// sizeof(key_t)}
int fscanf_phone(FILE *f, phone_t *phone);
int is_date(int day, int month, int year);
int menu(int pos, phone_t table[], key_t keys[], size_t *tn, size_t max_tn);
void print_phone(phone_t p);
void print_key(key_t k);
void read_file(FILE *f, phone_t *p);




#endif

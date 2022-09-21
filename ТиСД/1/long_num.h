#ifndef _LONG_NUM_H_
#define _LONG_NUM_H_

#include <stdlib.h>

#define MANT_LEN 31
#define EXP_LEN 5

typedef struct
{
    int sign_m;
    int mantissa[MANT_LEN];
    size_t len_m;
    int expon;
} long_num_t;

void strip_zeros(int a[], size_t *n);
void remove_sign(char *s);
int is_integer(char *s);
int strtoln(char *s, long_num_t *ln);
int is_greater(int a[], size_t na, int b[], size_t nb);
void sub_upd(int a[], size_t *na, int b[], size_t nb);
long_num_t longdiv(long_num_t a, long_num_t b);
long_num_t normalize(long_num_t a);

#endif
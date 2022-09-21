#include <string.h>
#include <ctype.h>


#include "words.h"
#include "long_num.h"


// Убирает нули с конца и начала числа
void strip_zeros(int a[], size_t *n)
{
    int *s1 = a, *s2 = a;
    for (size_t i = 0; a[i] == 0 && i < *n; i++)
        s2++;
    size_t tmp = s2 - s1;
    size_t i;
    for (i = 0; i < *n; i++)
        s1[i] = s2[i];
    for (; i < MANT_LEN - 1; i++) 
        s1[i] = 0;
    *n -= tmp;
}

// Убирает знак из строки
void remove_sign(char *s)
{
    if (strlen(s) && strchr("+-", s[0]))
    {
        size_t j;
        for (j = 1; s[j]; j++)
            s[j - 1] = s[j];
        s[j - 1] = s[j];
    }
}
// Проверка целое ли число
int is_integer(char *s)
{
    int sign = 0; 
    char signs[] = "+-";
    for (size_t i = 0; s[i]; i++)
        if (!isdigit(s[i]))
        {
            if (i == 0 && strchr(signs, s[i]) && sign == 0)
                sign++;
            else
                return 0;
        }
    return 1;        
}
// Проверка вещественного числа и перевод его в структуру
int strtoln(char *s, long_num_t *ln)
{

    int counte = 0, counts = 0, countp = 0;
    for (size_t i = 0; s[i]; i++)
    {
        if (strchr("eE", s[i]))
            counte++;
        if (counte > 1)
            return 0;
        if (s[i] == '.')
            countp++;
        if (s[i] == '.' && counte == 1)
            return 0;
        if (countp > 1)
            return 0;
        if (strchr("+-", s[i]))
            counts++;
        if (counts > 2)
            return 0;
    }
    words_t a;
    size_t n;
    split(s, a, &n, "eE.", MAX_WORD_LEN);

    for (size_t i = 0; i < n; i++)
        if (!(is_integer(a[i])))
            return 0;

    if (n == 3)
    {
        if (strlen(a[1]) && strchr("+-", a[1][0]))
            return 0;
        if (a[0][0] == '-')
            ln->sign_m = 1;
        ln->expon = atoi(a[2]);

        for (size_t i = 0; i < n; i++)
            remove_sign(a[i]);

        ln->len_m = strlen(a[0]) + strlen(a[1]);
        if ((ln->len_m) > MANT_LEN - 1 || strlen(a[2]) > EXP_LEN) 
            return 0;

        if (!((strlen(a[0]) || strlen(a[1])) && strlen(a[2])))
            return 0;

        size_t len = strlen(a[0]);
        size_t k = 0;
        for (size_t i = 0; i < len; i++)
            (ln->mantissa)[k++] = a[0][i] - '0';
        len = strlen(a[1]);
        ln->expon -= (int) len;
        for (size_t i = 0; i < len; i++)
            (ln->mantissa)[k++] = a[1][i] - '0';
        return 1;
    }
    if (n == 2)
    {
        if (countp == 1)
        {
            if (strlen(a[1]) && strchr("+-", a[1][0]))
                return 0;
            if (a[0][0] == '-')
                ln->sign_m = 1;
            for (size_t i = 0; i < n; i++)
                remove_sign(a[i]);

            if (!(strlen(a[0]) || strlen(a[1])))
                return 0;
            ln->len_m = strlen(a[0]) + strlen(a[1]);
            if ((ln->len_m) > MANT_LEN - 1) 
                return 0;

            size_t len = strlen(a[0]);
            size_t k = 0;
            for (size_t i = 0; i < len; i++)
                (ln->mantissa)[k++] = a[0][i] - '0';
            len = strlen(a[1]);
            ln->expon -= (int) len;
            for (size_t i = 0; i < len; i++)
                (ln->mantissa)[k++] = a[1][i] - '0'; 
        }
        if (counte == 1)
        {   
            if (a[0][0] == '-')
                ln->sign_m = 1;
            ln->expon = atoi(a[1]);
            for (size_t i = 0; i < n; i++)
                remove_sign(a[i]);
                
            if (!(strlen(a[0]) && strlen(a[1])))
                return 0;
            ln->len_m = strlen(a[0]);
            if ((ln->len_m) > MANT_LEN - 1 || strlen(a[1]) > EXP_LEN) 
                return 0;
            size_t k = 0;
            for (size_t i = 0; i < (ln->len_m); i++)
                (ln->mantissa)[k++] = a[0][i] - '0';
        }
        return 1;
    }
    if (n == 1)
    {
        if (a[0][0] == '-')
            ln->sign_m = 1;
        for (size_t i = 0; i < n; i++)
            remove_sign(a[i]);
        ln->len_m = strlen(a[0]);
        if ((ln->len_m) > MANT_LEN - 1 || (ln->len_m) == 0) 
            return 0;
        size_t k = 0;
        for (size_t i = 0; i < (ln->len_m); i++)
            (ln->mantissa)[k++] = a[0][i] - '0';
        return 1;
    }

    return 0;
}

// Сравнивает мантиссы чисел
int is_greater(int a[], size_t na, int b[], size_t nb)
{
    if (na > nb)
        return 1;
    if (na < nb)
        return -1;
    for (size_t i = 0; i < na; i++)
    {
        if (a[i] > b[i])
            return 1;
        if (a[i] < b[i])
            return -1;
    } 
    return 0;
}

// Вычитает одну мантиссу из другой
void sub_upd(int a[], size_t *na, int b[], size_t nb)
{
    if (is_greater(a, *na, b, nb) == 0)
    {
        for (size_t i = 0; i < *na; i++)
            a[i] = 0;
        *na = 0;
        return;
    }
    size_t j = nb - 1;
    for (size_t i = *na - 1; i < *na; i--)
    {
        int t = 0;
        if (j < nb)
            t = b[j];
        while (a[i] < t)
        {
            a[i - 1]--;
            a[i] += 10;
        }
        a[i] -= t;
        j--;
    }
    int *s1 = a, *s2 = a;
    for (size_t i = 0; a[i] == 0; i++)
        s2++;
    size_t tmp = s2 - s1;
    size_t i;
    for (i = 0; i < *na; i++)
        s1[i] = s2[i];
    for (; i < MANT_LEN - 1; i++) 
        s1[i] = 0;
    *na -= tmp;
}

// Деление
long_num_t longdiv(long_num_t a, long_num_t b)
{
    long_num_t c = { .sign_m = 0, .mantissa = { 0 }, .len_m = 0, .expon = 0 };
    int tmp1[31] = { 0 };
    size_t tmpl = b.len_m;
    size_t k = b.len_m; 
    for (size_t i = 0; i < b.len_m; i++)
        tmp1[i] = (a.mantissa)[i];
    if (is_greater(tmp1, tmpl, b.mantissa, b.len_m) < 0)
    {
        if (b.len_m < a.len_m)
            tmp1[b.len_m] = a.mantissa[b.len_m];    
        tmpl++;
        k++;
        a.expon--;
    }
    for (size_t i = 0; i < MANT_LEN; i++)
    {
        while (is_greater(tmp1, tmpl, b.mantissa, b.len_m) >= 0)
        {
            sub_upd(tmp1, &tmpl, b.mantissa, b.len_m);
            c.mantissa[i]++;
        }
        c.len_m++;
        if (k < a.len_m)
            tmp1[tmpl] = a.mantissa[k++];
        tmpl++;
        strip_zeros(tmp1, &tmpl);
    }
    c.expon = a.expon - b.expon + 1;
    if (c.mantissa[MANT_LEN - 1] >= 5)
        c.mantissa[MANT_LEN - 2]++;
    for (size_t i = MANT_LEN - 2; i > 0; i--)
        if (c.mantissa[i] == 10)
        {
            c.mantissa[i] = 0;
            c.mantissa[i - 1]++;
        }
    c.sign_m = a.sign_m ^ b.sign_m;
    return c;
}


long_num_t normalize(long_num_t a)
{
    int *s1 = a.mantissa, *s2 = a.mantissa;
    for (size_t i = 0; a.mantissa[i] == 0 && a.len_m > i; i++)
    {
        s2++;
        a.expon--;
    }
    size_t tmp = s2 - s1;
    size_t i;
    for (i = 0; i < a.len_m; i++)
        s1[i] = s2[i];
    for (; i < MANT_LEN - 1; i++) 
        s1[i] = 0;
    a.len_m -= tmp;
    i = a.len_m - 1;
    while (a.mantissa[i] == 0)
    {    
        a.len_m--;
        a.expon++;
        i--;
    }
    a.expon += a.len_m + tmp;
    return a;
}

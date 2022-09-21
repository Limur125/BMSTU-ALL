#include "words.h"
#include "long_num.h"

#define CONVERSION_ERROR 5
#define ZERO_DIVISION 4
#define MACHINE_ZERO 6
#define NOT_A_DECIMAL 7
#define OVERFLOW 8
int main(void)
{
    setbuf(stdout, NULL);
    printf("Данная программа выполняет делние целого числа на действительное.\n"
        "Дробная и целая часть должны быть разделены точкой. Допустимо отсутствие\n"
        "одной из частей. Допускается отсутствие знака. Допустимые символы '.', '+',\n"
        "'-', 'e', 'E' (латиница). При вводе в експоненциальной форме:\n"
        "Длина мантиссы не превышает 30 символов, порядка - не более 5.\n\n");
    printf("                    |                              |\n");
    printf("Введите целое число:");
    str_t s;
    int rc;
    if ((rc = input(s, stdin)) != 0)
    {
        printf("Ошибка сканирования строки.\nReturn code = %d", rc);
        return rc;
    }
    long_num_t a = { .sign_m = 0, .mantissa = { 0 }, .len_m = 0, .expon = 0 };
    words_t tmp;
    size_t n;
    split(s, tmp, &n, "eE.", MAX_WORD_LEN);
    if (n != 1)
    {
        printf("Введено не целое число.\nReturn code = %d", NOT_A_DECIMAL );
        return NOT_A_DECIMAL;
    }
    if(!strtoln(s, &a))
    {
        printf("Некорректное число. Ошибка перевода числа в строку\nReturn code: %d", CONVERSION_ERROR);
        return CONVERSION_ERROR;
    }
    a = normalize(a);

    printf("                             |                              Е     |\n");
    printf("Введите действительное число:");
    if ((rc = input(s, stdin)) != 0)
    {
        printf("Ошибка сканирования строки.\nReturn code = %d", rc);
        return rc;
    }
    long_num_t b = { .sign_m = 0, .mantissa = { 0 }, .len_m = 0, .expon = 0 };
    if(!strtoln(s, &b))
        {
        printf("Некорректное число. Ошибка перевода числа в строку\nReturn code: %d", CONVERSION_ERROR);
        return CONVERSION_ERROR;
    }
    b = normalize(b);

    if (b.len_m > MANT_LEN - 1)
    {
        printf("Ошибка деления на ноль.\nReturn code: %d", ZERO_DIVISION);
        return ZERO_DIVISION;
    }
    if (a.len_m > MANT_LEN - 1)
    {
        printf("Результат деления:\n0.0");
        return EXIT_SUCCESS;
    }
    long_num_t c = longdiv(a, b);
    if (c.expon < -99999)
    {
        printf("Слишком маленький порядок. Достигнут магинный ноль\nReturn code: %d", MACHINE_ZERO);
        return MACHINE_ZERO;
    }
    if (c.expon > 99999)
    {
        printf("Слишком большой порядок. Переполнение.\nReturn code: %d", OVERFLOW);
        return OVERFLOW;
    }
    printf("Результат деления:\n");
    printf("%c0.", '+' + 2 * c.sign_m);
    for (size_t i = 0; i < MANT_LEN - 1; i++)
        printf("%d", c.mantissa[i]);
    printf("E%d",c.expon);
    return EXIT_SUCCESS;
}
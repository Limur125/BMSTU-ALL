#include <stdlib.h>
#include <string.h>
#include "words.h"

// Разрез строки на "слова"
int split(char *str, words_t a, size_t *n, char *delims, size_t word_len)
{
    size_t i = 0, j = 0, k = 0;
    a[0][0] = 0;
    while (str[k])
    {
        if (!strchr(delims, str[k]))
            a[i][j++] = str[k];
        else 
        {
            a[i++][j] = 0;
            j = 0;
        }
        k++;
        if (j > word_len)
            return EXIT_FAILURE;
    }
    a[i][j] = 0;
    *n = i + 1;
    return EXIT_SUCCESS;
}
// Функция ввода строки
int input(str_t s, FILE *f)
{
    if (!fgets(s, STR_LEN, f))
        return SCAN_ERROR;

    char *pos = strrchr(s, '\n');
    if (strlen(s) == (STR_LEN - 1) && !pos)
        return TOO_MANY_CHARS;
    if (pos)
        *pos = 0;
    return EXIT_SUCCESS;
}

#ifndef _SHA1_H_
#define _SHA1_H_

#include <stdint.h>


#ifndef _SHA_enum_
#define _SHA_enum_
enum
{
    shaSuccess = 0,
    shaNull,            /* Null pointer parameter */
    shaInputTooLong,    /* входные данные слишком длины */
    shaStateError       /* called Input after Result */
};
#endif
#define SHA1HashSize 20

typedef struct SHA1Context
{
    uint32_t Intermediate_Hash[SHA1HashSize / 4]; /* Дайджест сообщения  */

    uint32_t Length_Low;            /* Длина сообщения в битах     */
    uint32_t Length_High;           /* Длина сообщения в битах     */

    /* Индекс массива блока сообщения   */
    int_least16_t Message_Block_Index;
    uint8_t Message_Block[64];      /* 512-битовые блоки сообщения */

    int Computed;               /* дайджест вычислен?         */
    int Corrupted;             /* Дайджест сообщения поврежден? */
} SHA1Context;

/*
 *  Прототипы функций
 */

int SHA1Reset(SHA1Context*);
int SHA1Input(SHA1Context*,
    const uint8_t*,
    unsigned int);
int SHA1Result(SHA1Context*,
    uint8_t Message_Digest[SHA1HashSize]);

#endif
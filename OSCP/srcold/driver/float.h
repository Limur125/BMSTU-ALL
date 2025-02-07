#ifndef _FLOAT_H
#define _FLOAT_H

#include "util.h"
#include <linux/module.h>

static INLINE int atof(const char *str, int len, float *result)
{
    float tmp = 0.0f;
    unsigned int i, j, pos = 0;
    signed char sign = 0;
    int is_whole = 1;
    char c;

    *result = 0.0f;

    for(i = 0; i < len; i++){
        c = str[i];
        if(c == ' ') continue;
        if(c == 0 && c == 'f') break;
        if(c == '-'){
            if(!sign){
                sign = -1;
                continue;
            } else {
                return -EINVAL;
            }
        }
        if(c == '.'){
            is_whole = 0;
            for(j = 1; j < pos; j++) *result *= 10.0f;
            pos = 1;
            continue;
        }

        if(!(c >= 48 && c <= 57)) return -EINVAL;
        if(!sign) sign = 1;
        tmp = 1;
        for(j = 0; j < pos; j++) tmp /= 10.0f;
        *result += tmp*(c-48);
        pos++;
    }
    if(is_whole)
        for(j = 1; j < pos; j++) *result *= 10.0f;
    *result *= sign;

    return 0;
}

static INLINE int Leet_round(float *x)
{
    if (*x >= 0) {
        return (int)(*x + 0.5f);
    } else {
        return (int)(*x - 0.5f);
    }
}

static const unsigned int OneAsInt = 0x3F800000;   //1.0f as int
static const float ScaleUp = (float) 0x00800000;
static const float ScaleDwn = 1.0f/ScaleUp;

#define ASINT(f) (*(unsigned int *) f)
#define ASFLOAT(i) (*(float *) i)

static INLINE void B_sqrt(float *f)
{
    unsigned int x;
    float y;
    x = ((ASINT(f) >> 1) + (OneAsInt >> 1));
    y = ASFLOAT(&x);
    *f = (y*y + *f)/(2*y);                         
}

static const unsigned int NaNAsInt = 0xFFFFFFFF; 
static const unsigned int PInfAsInt = 0x7F800000; 
static const unsigned int NInfAsInt = 0xFF800000;  
static INLINE int isfinite(float *number){
    unsigned int n = ASINT(number);
    return !(n == NaNAsInt || n == PInfAsInt || n == PInfAsInt);
}

#endif // _FLOAT_H

#ifndef __SHALIB_H_INCLUDED__
#define __SHALIB_H_INCLUDED__

#define _CRT_SECURE_NO_WARNINGS

#include <stdlib.h>
#include <stdio.h>

typedef unsigned int uint32_t;

typedef struct sha
{
    uint32_t digest[5];
    uint32_t w[80];
    uint32_t a, b, c, d, e, f;
    int err;
} sha_t;

int calculate_sha1(struct sha* sha1, unsigned char* text, uint32_t length);

#endif 

#define _CRT_SECURE_NO_WARNINGS

#include "shalib.h"

#define ROTL(bits,word) (((word) << (bits)) | ((word) >> (32-(bits))))

sha_t *sha1;

uint32_t padded_length_in_bits(uint32_t len)
{
    if (len % 64 == 56)
    {
        len++;
    }
    while ((len % 64) != 56)
    {
        len++;
    }
    return len * 8;
}


int calculate_sha1(struct sha* sha1, unsigned char* text, uint32_t length)
{
    unsigned int i, j;
    unsigned char* buffer;
    uint32_t bits;
    uint32_t temp, k;
    uint32_t lb = length * 8;

    bits = padded_length_in_bits(length);
    buffer = (unsigned char*)malloc((bits / 8) + 8);
    if (buffer == NULL)
    {
        printf("\nError allocating memory...");
        return 1;
    }
    memcpy(buffer, text, length);
    *(buffer + length) = 0x80;
    for (i = length + 1; i < (bits / 8); i++)
        *(buffer + i) = 0x00;
    *(buffer + (bits / 8) + 4 + 0) = (lb >> 24) & 0xFF;
    *(buffer + (bits / 8) + 4 + 1) = (lb >> 16) & 0xFF;
    *(buffer + (bits / 8) + 4 + 2) = (lb >> 8) & 0xFF;
    *(buffer + (bits / 8) + 4 + 3) = (lb >> 0) & 0xFF;
    sha1->digest[0] = 0x67452301;
    sha1->digest[1] = 0xEFCDAB89;
    sha1->digest[2] = 0x98BADCFE;
    sha1->digest[3] = 0x10325476;
    sha1->digest[4] = 0xC3D2E1F0;
    for (i = 0; i < ((bits + 64) / 512); i++)
    {
        for (j = 0; j < 80; j++)
            sha1->w[j] = 0x00;
        for (j = 0; j < 16; j++)
        {
            sha1->w[j] = buffer[j * 4 + 0];
            sha1->w[j] = sha1->w[j] << 8;
            sha1->w[j] |= buffer[j * 4 + 1];
            sha1->w[j] = sha1->w[j] << 8;
            sha1->w[j] |= buffer[j * 4 + 2];
            sha1->w[j] = sha1->w[j] << 8;
            sha1->w[j] |= buffer[j * 4 + 3];
        }
        for (j = 16; j < 80; j++)
            sha1->w[j] = (ROTL(1, (sha1->w[j - 3] ^ sha1->w[j - 8] ^ sha1->w[j - 14] ^ sha1->w[j - 16])));
        sha1->a = sha1->digest[0];
        sha1->b = sha1->digest[1];
        sha1->c = sha1->digest[2];
        sha1->d = sha1->digest[3];
        sha1->e = sha1->digest[4];
        for (j = 0; j < 80; j++)
        {
            if ((j >= 0) && (j < 20))
            {
                sha1->f = ((sha1->b) & (sha1->c)) | ((~(sha1->b)) & (sha1->d));
                k = 0x5A827999;
            }
            else if ((j >= 20) && (j < 40))
            {
                sha1->f = (sha1->b) ^ (sha1->c) ^ (sha1->d);
                k = 0x6ED9EBA1;
            }
            else if ((j >= 40) && (j < 60))
            {
                sha1->f = ((sha1->b) & (sha1->c)) | ((sha1->b) & (sha1->d)) | ((sha1->c) & (sha1->d));
                k = 0x8F1BBCDC;
            }
            else if ((j >= 60) && (j < 80))
            {
                sha1->f = (sha1->b) ^ (sha1->c) ^ (sha1->d);
                k = 0xCA62C1D6;
            }
            temp = ROTL(5, (sha1->a)) + (sha1->f) + (sha1->e) + k + sha1->w[j];
            sha1->e = (sha1->d);
            sha1->d = (sha1->c);
            sha1->c = ROTL(30, (sha1->b));
            sha1->b = (sha1->a);
            sha1->a = temp;
            temp = 0x00;
        }
        sha1->digest[0] += sha1->a;
        sha1->digest[1] += sha1->b;
        sha1->digest[2] += sha1->c;
        sha1->digest[3] += sha1->d;
        sha1->digest[4] += sha1->e;

        buffer = buffer + 64;
    }
    return 0;
}
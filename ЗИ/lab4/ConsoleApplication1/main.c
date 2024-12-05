#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "sign.h"4
#include "rsa.h"

typedef unsigned int uint32_t;


static void fprint_hex(FILE* f, uint8_t* ptr, int len)
{
    int i;
    for (i = 0; i < len; i++)
        fprintf(f, "%2.2x", ptr[i]);
    fprintf(f, "\n");
}

int main(int argc, char* argv[])
{
    char inFile[50];
    int rc = 0;
    fgets(inFile, 50, stdin);
    inFile[strlen(inFile) - 1] = 0;
    FILE* fp = fopen(inFile, "r");
    if (fp == NULL)
    {
        printf("\nError opening file..\nExiting....");
        return 1;
    }
    fseek(fp, 0, SEEK_END);
    uint32_t file_len = ftell(fp);
    fseek(fp, 0, SEEK_SET);
    unsigned char *buff = (unsigned char*)malloc(file_len);
    if (!buff)
    {
        printf("\nError allocating memory..");
        rc = 1;
        goto close_fs;
    }
    fread(buff, file_len, 1, fp);
    fclose(fp);
    char n[2048], d[2048], e[2048];
    rsa_generate_key_pair(n, d, e, NULL, 20);
    fprintf(stdout, "N = %s\nD = %s\n", n, d);
    fprintf(stdout, "N = %s\nE = %s\n", n, e);
    uint8_t zero5[] = "00000";
    uint8_t res_zero[20];
    SHA1Context context;
    SHA1Reset(&context);
    SHA1Input(&context, zero5, 5);
    SHA1Result(&context, res_zero);
    fprintf(stdout, "SHA 00000 = ");
    fprint_hex(stdout, res_zero, 20);
    char* digest_e;
    digest_e = sign(buff, file_len, n, d);
    if (digest_e == NULL)
    {
        printf("\n Error calculating SHA1 hash.. \n");
        rc = 1;
        goto out;
    }
    fprintf(stdout, "\nEncrypted SHA = %s\n", digest_e);
    int r = verify(buff, file_len, n, e, digest_e);
    fprintf(stdout, "Verify result = %d", r);
    printf("\n");
out:
    free(buff);
close_fs:
    fclose(fp);        
    return rc;
}


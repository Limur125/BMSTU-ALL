#define _CRT_SECURE_NO_WARNINGS

#include "sign.h"


#define MAX_SIGNATURE_LEN  2048
#define HASH_LEN 20

static void fprint_hex(FILE* f, uint8_t* ptr, int len)
{
    int i;
    for (i = 0; i < len; i++)
        fprintf(f, "%2.2x", ptr[i]);
    fprintf(f, "\n");
}

char * sign(char *message, int len, char *n, char *d)
{
    uint8_t hash[HASH_LEN + 1];
    char *signature, * hash_dec;
    signature = malloc(MAX_SIGNATURE_LEN);
    //sha_t sha;
    SHA1Context context;
    int rc = SHA1Reset(&context);
    if (rc)
        return NULL;
    rc = SHA1Input(&context, message, len);
    if (rc)
        return NULL;
    rc = SHA1Result(&context, hash);
    //int rc = calculate_sha1(&sha, message, len);
    if (rc)
        return NULL;
    //memcpy(hash, sha.digest, HASH_LEN);
    hash_dec = rsa_bin2dec(hash, 20);
    fprintf(stdout, "\nGenerated digest = %s", hash_dec);
    rsa_encrypt(signature, hash_dec, strlen(hash_dec), n, d);
    return signature;
}

int verify(char *message, int len, char *n, char *e, char *signature)
{
    uint8_t hash[HASH_LEN + 1];
    char your_hash[MAX_SIGNATURE_LEN], *my_hash;
    int match;
    SHA1Context context;
    int rc = SHA1Reset(&context);
    if (rc)
        return NULL;
    rc = SHA1Input(&context, message, len);
    if (rc)
        return NULL;
    rc = SHA1Result(&context, hash);
    if (rc)
        return NULL;
    my_hash = rsa_bin2dec(hash, HASH_LEN);

    rsa_decrypt(your_hash, signature, strlen(signature), n, e);
    printf("Decrypted Hash value = %s\n", your_hash);
    if(strcmp(my_hash, your_hash) == 0)
        match = 1;
    else
        match = 0;
    free(my_hash);
    return match;
}

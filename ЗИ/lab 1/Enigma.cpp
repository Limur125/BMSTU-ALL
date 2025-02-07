#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>

#define ROTOR_SIZE 26
#define ROTOR_NUM 3

int getline(char** lp, FILE* f)
{
    size_t lp_len = 0;
    size_t alloc_len = 20;
    *lp = (char*)malloc(alloc_len * sizeof(char));
    if (*lp == NULL)
        return EXIT_FAILURE;
    char lp_char;
    int rc_scanf;
    rc_scanf = fscanf(f, "%c", &lp_char);
    while (rc_scanf == 1 && lp_char != '\n')
    {
        (*lp)[lp_len++] = lp_char;
        if (lp_len == alloc_len)
        {
            alloc_len *= 2;
            char* tmp = (char*)realloc(*lp, alloc_len * sizeof(char));
            if (tmp == NULL)
            {
                free(*lp);
                *lp = NULL;
                return EXIT_FAILURE;
            }
            *lp = tmp;
        }
        if (lp_len > 2000)
        {
            free(*lp);
            *lp = NULL;
            return EXIT_FAILURE;
        }
        rc_scanf = fscanf(f, "%c", &lp_char);
    }
    (*lp)[lp_len] = 0;
    return EXIT_SUCCESS;
}

char* create_array(int size, char array_s[])
{
    char* ar = (char*)malloc(sizeof(char) * size);
    if (ar == NULL)
        return NULL;
    memcpy(ar, array_s, size * sizeof(char));
    return ar;
}

typedef struct enigma_t 
{
    int counter;
    int size_rotor;
    int num_rotors;
    char* reflector;
    char* com_panel;
    char** rotors;
} enigma_t;

enigma_t* enigma_new(char** rotors, char* reflector, char* com_panel)
{
    enigma_t* enigma = (enigma_t*)malloc(sizeof(enigma_t));
    if (enigma == NULL)
        return NULL;
    enigma->size_rotor = ROTOR_SIZE;
    enigma->num_rotors = ROTOR_NUM;
    enigma->counter = 2;
    enigma->reflector = reflector;
    enigma->rotors = rotors;
    enigma->com_panel = com_panel;
    return enigma;
}

char enigma_rotor_find(enigma_t* enigma, int num, char ch, int* rc) 
{
    for (int i = 0; i < enigma->size_rotor; i++) 
        if (enigma->rotors[num][i] == ch) 
        {
            *rc = 0;
            return i + 'A';
        }
    *rc = 1;
    return 0;
}

char enigma_com_panel_find(enigma_t* enigma, char ch, int* rc)
{
    for (int i = 0; i < enigma->size_rotor; i++)
        if (enigma->com_panel[i] == ch)
        {
            *rc = 0;
            return i + 'A';
        }
    *rc = 1;
    return 0;
}

void enigma_rotor_shift(enigma_t* enigma, int num) 
{
    char temp = enigma->rotors[num][enigma->size_rotor - 1];
    for (int i = enigma->size_rotor - 1; i > 0; i--) 
        enigma->rotors[num][i] = enigma->rotors[num][i - 1];
    enigma->rotors[num][0] = temp;
}

char enigma_encrypt(enigma_t* enigma, char ch, int* rc) 
{
    int rotor_queue;
    char new_ch;
    if (ch - 'A' >= enigma->size_rotor) {
        *rc = 0;
        return 0;
    }
    new_ch = enigma->com_panel[ch - 'A'];
    for (int i = 0; i < enigma->num_rotors; i++)
        new_ch = enigma->rotors[i][new_ch - 'A'];
    new_ch = enigma->reflector[new_ch - 'A'];
    for (int i = enigma->num_rotors - 1; i >= 0; i--) 
    {
        new_ch = enigma_rotor_find(enigma, i, new_ch, rc);
        if (*rc != 0) 
            return 0;
    }
    new_ch = enigma_com_panel_find(enigma, new_ch, rc);
    rotor_queue = 1;
    enigma->counter += 1;
    for (int i = 0; i < enigma->num_rotors; i++) 
    {
        if (enigma->counter % rotor_queue == 0) 
            enigma_rotor_shift(enigma, i);
        rotor_queue *= enigma->size_rotor;
    }
    *rc = 0;
    return new_ch;
}

void enigma_free(enigma_t* enigma) 
{
    for (int i = 0; i < enigma->num_rotors; i++) 
        free(enigma->rotors[i]);
    free(enigma->rotors);
    free(enigma->reflector);
    free(enigma);
}

int main(void) 
{
    //char alphabet[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    char reflector_s[] = { 'Z', 'Y', 'X', 'W', 'V', 'U', 'T', 'S', 'R', 'Q', 'P', 'O', 'N', 'M', 'L', 'K', 'J', 'I', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A' };
    char* reflector = create_array(ROTOR_SIZE, reflector_s);
    if (reflector == NULL)
        return 1;

    char com_panel_s[] = { 'B', 'A', 'D', 'C', 'F', 'E', 'H', 'G', 'J', 'I', 'L', 'K', 'N', 'M', 'P', 'O', 'R', 'Q', 'T', 'S', 'V', 'U', 'X', 'W', 'Z', 'Y' };
    char* com_panel = create_array(ROTOR_SIZE, com_panel_s);
    if (com_panel == NULL)
    {
        free(reflector);
        return 1;
    }

    char rotor1_s[] = { 'Z','X','Y','W','V','U','T','S','R','Q','P','O','N','M','L','K','J','I','H','G','F','E','D','C','B','A' };
    char* rotor1 = create_array(ROTOR_SIZE, rotor1_s);
    if (rotor1 == NULL)
    {
        free(com_panel);
        free(reflector);
        return 1;
    }

    char rotor2_s[] = { 'U', 'V', 'W', 'X', 'Y', 'Z', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' };
    char* rotor2 = create_array(ROTOR_SIZE, rotor2_s);
    if (rotor2 == NULL)
    {
        free(rotor1);
        free(com_panel);
        free(reflector);
        return 1;
    }

    char rotor3_s[] = { 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A', 'Y', 'X', 'W', 'V', 'U', 'Z', 'I', 'J', 'T', 'S', 'R', 'Q', 'P', 'O', 'N', 'M', 'L', 'K' };
    char* rotor3 = create_array(ROTOR_SIZE, rotor3_s);
    if (rotor3 == NULL)
    {
        free(rotor2);
        free(rotor1);
        free(com_panel);
        free(reflector);
        return 1;
    }

    char** rotors = (char**)malloc(sizeof(char) * ROTOR_SIZE * ROTOR_NUM);
    if (rotors == NULL)
    {
        free(rotor3);
        free(rotor2);
        free(rotor1);
        free(com_panel);
        free(reflector);
        return 1;
    }
    rotors[0] = rotor1, rotors[1] = rotor2, rotors[2] = rotor3;

    enigma_t* enigma = enigma_new(rotors, reflector, com_panel);
    if (enigma == NULL)
    {
        free(rotors);
        free(rotor3);
        free(rotor2);
        free(rotor1);
        free(com_panel);
        free(reflector);
        return 1;
    }
    char* message, *cipher;
    getline(&message, stdin);
    if (message == NULL)
    {
        enigma_free(enigma);
        return 1;
    }
    cipher = (char*)malloc(strlen(message) * sizeof(char) + 1);
    if (cipher == NULL)
    {
        enigma_free(enigma);
        return 1;
    }
    int rc = 0;
    int i = 0;
    for (;rc == 0 && message[i] != 0; i++)
        cipher[i] = enigma_encrypt(enigma, message[i], &rc);
    cipher[i] = 0;
    fprintf(stdout, "%s", cipher);
    enigma_free(enigma);
    free(message);
    free(cipher);
    return rc;
}

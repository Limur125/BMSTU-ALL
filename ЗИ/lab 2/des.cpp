#include "des.h"
#include "des_key.h"
#include "des_tables.h"

DES::DES(ui64 key)
{
    keygen(key);
}

ui64 DES::encrypt(ui64 block)
{
    return des(block, false);
}

ui64 DES::decrypt(ui64 block)
{
    return des(block, true);
}

ui64 DES::encrypt(ui64 block, ui64 key)
{
    DES des(key);
    return des.des(block, false);
}

ui64 DES::decrypt(ui64 block, ui64 key)
{
    DES des(key);
    return des.des(block, true);
}

void DES::keygen(ui64 key)
{
    ui64 permuted_choice_1 = 0;
    for (ui8 i = 0; i < 56; i++)
    {
        permuted_choice_1 <<= 1;
        permuted_choice_1 |= (key >> (64 - PC1[i])) & LB64_MASK;
    }

    ui32 C = (ui32)  ((permuted_choice_1 >> 28) & 0x000000000fffffff);
    ui32 D = (ui32)  (permuted_choice_1 & 0x000000000fffffff);

    for (ui8 i = 0; i < 16; i++)
    {
        for (ui8 j = 0; j < ITERATION_SHIFT[i]; j++)
        {
            C = (0x0fffffff & (C << 1)) | (0x00000001 & (C >> 27));
            D = (0x0fffffff & (D << 1)) | (0x00000001 & (D >> 27));
        }

        ui64 permuted_choice_2 = (((ui64) C) << 28) | (ui64) D;

        sub_key[i] = 0; 
        for (ui8 j = 0; j < 48; j++)
        {
            sub_key[i] <<= 1;
            sub_key[i] |= (permuted_choice_2 >> (56-PC2[j])) & LB64_MASK;
        }
    }
}

ui64 DES::des(ui64 block, bool mode)
{
    block = ip(block);

    ui32 L = (ui32) (block >> 32) & L64_MASK;
    ui32 R = (ui32) (block & L64_MASK);

    for (ui8 i = 0; i < 16; i++)
    {
        ui32 F;
        if (mode)
            F = f(R, sub_key[15 - i]);
        else
            F = f(R, sub_key[i]);
        ui32 temp = R;
        R = L ^ F;
        L = temp;
    }

    block = (((ui64) R) << 32) | (ui64) L;
    return fp(block);
}

ui64 DES::ip(ui64 block)
{
    ui64 result = 0;
    for (ui8 i = 0; i < 64; i++)
    {
        result <<= 1;
        result |= (block >> (64-IP[i])) & LB64_MASK;
    }
    return result;
}

ui64 DES::fp(ui64 block)
{
    ui64 result = 0;
    for (ui8 i = 0; i < 64; i++)
    {
        result <<= 1;
        result |= (block >> (64-FP[i])) & LB64_MASK;
    }
    return result;
}

ui32 DES::f(ui32 R, ui64 k)
{
    ui64 s_input = 0;
    for (ui8 i = 0; i < 48; i++)
    {
        s_input <<= 1;
        s_input |= (ui64) ((R >> (32-EXPANSION[i])) & LB32_MASK);
    }
    s_input = s_input ^ k;
    ui32 s_output = 0;
    for (ui8 i = 0; i < 8; i++)
    {
        char s = (s_input >> (42 - 6 * i)) & 0x3f;
        char row = ((s >> 4) & 0b10) | s & 1;
        char column = (s >> 1) & 0b1111;

        s_output <<= 4;
        s_output |= (ui32) (SBOX[i][16*row + column] & 0x0f);
    }

    ui32 f_result = 0;
    for (ui8 i = 0; i < 32; i++)
    {
        f_result <<= 1;
        f_result |= (s_output >> (32 - PBOX[i])) & LB32_MASK;
    }

    return f_result;
}
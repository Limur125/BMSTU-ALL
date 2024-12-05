#ifndef DES_H
#define DES_H

#include <cstdint>
using namespace std;

typedef uint64_t ui64;
typedef uint32_t ui32;
typedef uint8_t ui8;

class DES
{
public:
    DES(ui64 key);
    ui64 des(ui64 block, bool mode);

    ui64 encrypt(ui64 block);
    ui64 decrypt(ui64 block);

    static ui64 encrypt(ui64 block, ui64 key);
    static ui64 decrypt(ui64 block, ui64 key);

private:
    void keygen(ui64 key);

    ui64 ip(ui64 block);
    ui64 fp(ui64 block);

    void feistel(ui32 &L, ui32 &R, ui32 F);
    ui32 f(ui32 R, ui64 k);


    ui64 sub_key[16]; 
};

#endif

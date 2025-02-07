#ifndef FILEENCRYPTION_H
#define FILEENCRYPTION_H

#include <iostream>
#include <fstream>
#include <string>
#include "AES.h"

using namespace std;

class FileEncryption
{
public:
    FileEncryption(char key[]);
    int encrypt(string input, string output);

private:
    char key[16];
    char iv[16];
    AESModeOfOperation aes;
};

#endif

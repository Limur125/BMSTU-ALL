#include "fileencryption.h"

FileEncryption::FileEncryption(char key[])
{
    memcpy(this->key, key, 16);
    memset(iv, 0, 16);
    aes = AESModeOfOperation(iv, key);
}

int FileEncryption::encrypt(string input, string output)
{
    ifstream ifile;
    ofstream ofile;
    char inbuffer[16], outbuffer[16];

    if(input.length()  < 1) 
        ifile = ifstream(stdin);
    else
        ifile.open(input, ios::binary | ios::in | ios::ate);

    if(output.length() < 1) 
        ofile = ofstream(stdout);
    else
        ofile.open(output, ios::binary | ios::out);
    size_t size = ifile.tellg();
    ifile.seekg(0, ios::beg);


    size_t block = size / 16;

    for(size_t i = 0; i < block; i++)
    {
        ifile.read(inbuffer, 16);
        aes.Encrypt(inbuffer, 16, outbuffer);
        ofile.write(outbuffer, 16);
    }
    if (size % 16 != 0)
    {
        int padding = 16 - (size % 16);

        memset(inbuffer, 0, 16);
        ifile.read(inbuffer, 16 - padding);
        aes.Encrypt(inbuffer, 16, outbuffer);
        ofile.write(outbuffer, 16);
    }
    ifile.close();
    ofile.close();
    return 0;
}

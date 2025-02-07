#include "fileencryption.h"

FileEncryption::FileEncryption(ui64 key) :
    des(key, (ui64) 0x0000000000000000)
{
}

int FileEncryption::encrypt(string input, string output)
{
    return cipher(input, output, false);
}

int FileEncryption::decrypt(string input, string output)
{
    return cipher(input, output, true);
}

int FileEncryption::cipher(string input, string output, bool mode)
{
    ifstream ifile;
    ofstream ofile;
    ui64 buffer;

    if(input.length()  < 1) 
        ifile = ifstream(stdin);
    else
        ifile.open(input, ios::binary | ios::in | ios::ate);

    if(output.length() < 1) 
        ofile = ofstream(stdout);
    else
        ofile.open(output, ios::binary | ios::out);

    ui64 size = ifile.tellg();
    ifile.seekg(0, ios::beg);

    ui64 block = size / 8;
    if(mode) block--;

    for(ui64 i = 0; i < block; i++)
    {
        ifile.read((char*) &buffer, 8);
        if (mode)
            buffer = des.decrypt(buffer);
        else
            buffer = des.encrypt(buffer);

        ofile.write((char*) &buffer, 8);
    }

    if(mode == false)
    {
        ui8 padding = 8 - (size % 8);

        if (padding == 0)
            padding  = 8;

        buffer = (ui64) 0;
        if(padding != 8)
            ifile.read((char*) &buffer, 8 - padding);

        ui8 shift = padding * 8;
        buffer <<= shift;
        buffer  |= (ui64) 0x0000000000000001 << (shift - 1);

        buffer = des.encrypt(buffer);
        ofile.write((char*) &buffer, 8);
    }
    else
    {
        ifile.read((char*) &buffer, 8);
        buffer = des.decrypt(buffer);

        ui8 padding = 0;

        while(!(buffer & 0x00000000000000ff))
        {
            buffer >>= 8;
            padding++;
        }

        buffer >>= 8;
        padding++;

        if(padding != 8)
            ofile.write((char*) &buffer, 8 - padding);
    }

    ifile.close();
    ofile.close();
    return 0;
}

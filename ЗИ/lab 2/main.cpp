#include <iostream>

using namespace std;

#include "fileencryption.h"
#include "tests.h"

void usage()
{
    cout << "Usage: cppDES -e/-d key [input-file] [output-file]" << endl;
}

int main(int argc, char **argv)
{
    alltests();
    return 0;
    if(argc < 3)
    {
        cout << "des -e/-d key [input-file] [output-file]" << endl;
        return EXIT_FAILURE;
    }

    string enc_dec = argv[1];
    if(enc_dec != "-e" && enc_dec != "-d")
    {
        cout << "des -e/-d key-file [input-file] [output-file]" << endl;
        return EXIT_FAILURE;
    }

    string input,output;
    if(argc > 3)
        input  = argv[3];
    if(argc > 4)
        output = argv[4];

    string filename_key = argv[2];
    ifstream key_file(filename_key, ios::binary | ios::in);
    ui64 key_r, key = 0;
    key_file >> key_r;
    for (int i = 0; i < 8; i++)
    {
        key |= key & 0xfe;
        key_r >>= 8;
    }
    FileEncryption fe(key);

    if(enc_dec == "-e")
        return fe.encrypt(input, output);
    if(enc_dec == "-d")
        return fe.decrypt(input, output);

    return 0;
}

#include <iostream>
#include <fstream>

#include "fileencryption.h"
#include "AES.h"

using namespace std;

int main(int argc, char **argv)
{
    /*
    if(argc < 2)
    {
        cout << "aes key [input-file] [output-file]" << endl;
        return EXIT_FAILURE;
    }
    */
    string input = "C:\\zolot\\ÇÈ\\lab3\\x64\\Debug\\out.txt", output = "C:\\zolot\\ÇÈ\\lab3\\x64\\Debug\\out1.txt";
    if (argc > 2)
        input  = argv[2];
    if(argc > 3)
        output = argv[3];

    string filename_key = "C:\\zolot\\ÇÈ\\lab3\\x64\\Debug\\key.txt";//argv[2];
    char key[16] = { 0 };
    ifstream key_file(filename_key, ios::in);
    
    key_file.read(key, 16);

    FileEncryption fe(key);

    return fe.encrypt(input, output);

    return 0;
}

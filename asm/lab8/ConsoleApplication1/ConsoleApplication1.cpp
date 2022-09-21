#include <iostream>
constexpr auto N = 100;

extern "C"
{
    void strcopy(char* dest, char* src, int len);
}

int str_len(const char* s)
{
    int len;
    __asm
    {
        xor ax, ax
        mov edi, s
        mov ecx, 0ffffffffh
        repne scasb
        not ecx
        dec ecx
        mov len, ecx
    }
    return len;
}

int main()
{
    char s1[N] = "Hello world!";
    int len = str_len(s1);
    std::cout << s1 << '\n' << len << std::endl;
    char s2[N] = "Hello world!";
    char copy[N] = { 0 };
    std::cout << "src: " << s2 << std::endl;
    std::cout << "copy(old): " << copy << std::endl;
    strcopy(copy, s2, len);
    std::cout << "copy(new): " << copy << std::endl;
    char s3[N] = "Hello world!";
    std::cout << "src: " << s3 + 6 << std::endl;
    std::cout << "s3(old): " << s3 << std::endl;
    strcopy(s3, s3 + 6, len);
    std::cout << "s3(new): " << s3 << std::endl;
    char s4[N] = "Hello world!";
    std::cout << "src: " << s4 << std::endl;
    std::cout << "s4(old): " << s4 +6 << std::endl;
    strcopy(s4 + 6, s4, len);
    std::cout << "s4(new): " << s4 + 6 << std::endl;
}

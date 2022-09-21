#include <iostream>
#include <ctime>
constexpr auto N = 10000000;
using namespace std;

double asm_dot_prod(const double a[], const double b[], int len)
{
    double res = 0;
    for (const double* pend = a + len; a < pend; a++, b++)
    {
        double ta = *a;
        double tb = *b;
        __asm
        {
            movsd xmm0, ta
            mulsd xmm0, tb
            addsd xmm0, mmword ptr[res]
            movsd mmword ptr[res], xmm0
        }
    }
    return res;
}

double c_dot_prod(const double a[], const double b[], int len)
{
    double res = 0;
    for (const double* pend = a + len; a < pend; a++, b++)
    {
        double ta = *a;
        double tb = *b;
        res += ta * tb;
    }
    return res;
}

int main()
{
    double a[] = { 1, 2, 3, 0 };
    double b[] = { 5, 6, 7, 0 };
    clock_t res = 0;
    for (int i = 0; i < N; i++)
    {
        clock_t start = clock();
        asm_dot_prod(a, b, 4);
        res += clock() - start;
    }
    cout << "ASM Dot Product time: " << res << endl;

    res = 0;
    for (int i = 0; i < N; i++)
    {
        clock_t start = clock();
        c_dot_prod(a, b, 4);
        res += clock() - start;
    }
    cout << "C++ Dot Product time: " << res << endl;

}

#include "sin_acc.h"
using namespace std;

void sin_compare(void)
{ 
    double res;

    cout << "C++ sin(3.14): " << setprecision(15) << sin(3.14) << endl;
    cout << "C++ sin(3.141596): " << setprecision(15) << sin(3.141596) << endl;

    __asm
    {
        fldpi
        fsin
        fstp res
    }
    cout << "FPU sin(3.141596): " << setprecision(15) << res << endl;

    cout << "C++ sin(3.14 / 2): " << setprecision(15) << sin(3.14 / 2) << endl;
    cout << "C++ sin(3.141596 / 2): " << setprecision(15) << sin(3.141596 / 2) << endl;

    double res1;
    int a = 1;
    double b = 2;
    __asm
    {
        fldpi
        fdiv b
        fsin
        fstp res1
    }
    cout << "FPU sin(3.141596 / 2): " << setprecision(15) << res1 << endl;

}
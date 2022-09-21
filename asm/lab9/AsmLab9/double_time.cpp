#include "double_time.h"
constexpr auto N = 1000000;

using namespace std;

void sum_double_c(double a, double b)
{
	double res;
	res = a + b;
}

void mult_double_c(double a, double b)
{
	double res;
	res = a * b;
}

void sum_double_a(double a, double b)
{
	double res;
	__asm
	{
		fld b
		fadd a
		fstp res
	}
}

void mult_double_a(double a, double b)
{
	double res;
	__asm
	{
		fld b
		fmul a
		fstp res
	}
}

void double_compare(void)
{
	double a = 213.343, b = 584.262;
	cout << "Bit test: " << sizeof(double) * CHAR_BIT << endl;

	clock_t res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		sum_double_c(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "C++ Sum test: " << res << endl;
	
	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		mult_double_c(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "C++ Mult test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		sum_double_a(a, b);
		clock_t end = clock();
		res += end - start;
	}
	cout << "ASM Sum test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		mult_double_a(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "ASM Mul test: " << res << endl;
}
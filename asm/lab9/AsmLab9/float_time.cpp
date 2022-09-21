#include "float_time.h"
constexpr auto N = 100000;

using namespace std;

void sum_float_c(float a, float b)
{
	float res;
	res = a + b;
}

void mult_float_c(float a, float b)
{
	float res;
	res = a * b;
}

void sum_float_a(float a, float b)
{
	float res;

	__asm
	{
		fld b
		fadd a
		fstp res
	}
}

void mult_float_a(float a, float b)
{
	float res;
	__asm
	{
		fld b
		fmul a
		fstp res
	}
}

void float_compare(void)
{
	float a = 213.343, b = 584.262;
	cout << "Bit test: " << sizeof(float) * CHAR_BIT << endl;

	clock_t res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		sum_float_c(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "C++ Sum test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		mult_float_c(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "C++ Mult test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		sum_float_a(a, b);
		clock_t end = clock();
		res += end - start;
	}
	cout << "ASM Sum test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		mult_float_a(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "ASM Mul test: " << res << endl;
}
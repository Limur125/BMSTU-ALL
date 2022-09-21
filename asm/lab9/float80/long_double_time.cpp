#include "long_double_time.h"
constexpr auto N = 1000000;

using namespace std;

void sum_long_double_c(__float80 a, __float80 b)
{
	__float80 res;
	res = a + b;
}

void mult_long_double_c(__float80 a, __float80 b)
{
	__float80 res;
	res = a * b;
}

void sum_long_double_a(__float80 a, __float80 b )
{
	__float80 res;
	__asm__
	(
		"fld %2\n\t"
		"fld %1\n\t"

		"faddp\n\t"

		"fstp %0"
		: "=m"(res)
		: "m"(a), "m"(b)
	);
}

void mult_long_double_a(__float80 a, __float80 b)
{
	__float80 res;
	__asm__(
		"fld %2\n\t"
		"fld %1\n\t"

		"fmulp\n\t"

		"fstp %0"
		: "=m"(res)
		: "m"(a), "m"(b)
	);
}

void long_double_compare(void)
{
	__float80 a = 213.343, b = 584.262;
	cout << "Bit test: __float80" << endl;

	clock_t res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		sum_long_double_c(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "C++ Sum test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		mult_long_double_c(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "C++ Mult test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		sum_long_double_a(a, b);
		clock_t end = clock();
		res += end - start;
	}
	cout << "ASM Sum test: " << res << endl;

	res = 0;
	for (int i = 0; i < N; i++)
	{
		clock_t start = clock();
		mult_long_double_a(a, b);
		clock_t end = clock();
		res += end - start;
	}

	cout << "ASM Mul test: " << res << endl;
}

int main(void)
{
	long_double_compare();
}
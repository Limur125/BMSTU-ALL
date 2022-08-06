#include "Set.h"
#include <array>
using namespace std;

int main()
{
	try
	{
		//ConstIterator<int> it;
		//{
		//	Set<int> g(3);
		//	it = g.cbegin();
		//}
		//it = ;
		//cout << *it << endl;

		Set<int> a, d, e;
		Set<long long int> b = { 1, 2 };
		
		cout << b << endl;
		
		a = Set<int>(2, 0, 1);

		int c[3] = { 3, 2, 1 };

		array<int, 3> g = { 1, 2, 3 };

		Set<int> h;

		h = Set<int>(g.begin(), g.end());

		cout << h << endl;

		d = Set<int>(c, 3);

		e = Set<int>();
		e = d ^ a;
		cout << e << " = " << d << " ^ " << a << endl;
		e = a | d;
		cout << e << " = " << a << " | " << d << endl;
		e = d / a;
		cout << e << " = " << d << " / " << a << endl;
		e = d & a;
		cout << e << " = " << a << " & " << d << endl;
		e = d;
		e ^= a;
		cout << e << " = " << d << " ^ " << a << endl;
		e = d;
		e |= a;
		cout << e << " = " << d << " | " << a << endl;
		e = d;
		e /= a;
		cout << e << " = " << d << " / " << a << endl;
		e = d;
		e &= a;
		cout << e << " = " << d << " & " << a << endl;

		Set<long> s1{ 1, 2, 3, 4 };
		Set<long> s2{ 1, 2, 3, 4, 5, 6 };
		Set<long> s3{ 0, 1, 2, 3, 4 };
		Set<long> s4{ 1, 2, 3, 4, 5, 6 };
		Set<long> s5{ 0, 1, 2, 3 };

		cout << s1 << " < " << s2 << " = " << (s1 < s2) << endl;
		cout << s1 << " > " << s2 << " = " << (s1 > s2) << endl;
		cout << s1 << " == " << s2 << " = " << (s1 == s2) << endl;
		cout << s1 << " != " << s2 << " = " << (s1 != s2) << endl;
		cout << s3 << " < " << s2 << " = " << (s3 < s2) << endl;
		cout << s4 << " < " << s2 << " = " << (s4 < s2) << endl;
		cout << s4 << " <= " << s2 << " = " << (s4 <= s2) << endl;
		cout << s4 << " == " << s2 << " = " << (s4 == s2) << endl;
		cout << s4 << " >= " << s2 << " = " << (s4 >= s2) << endl;
		cout << "(" << s1 << " | " << s5 << ") == " << s3 << " = " << ((s1|s5) == s3) << endl;

	}
	catch (const Exceptions &error)
	{
		cerr << error.what() << endl;
		return 1;
	}
	return 0;
}
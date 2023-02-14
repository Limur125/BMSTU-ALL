#include <Windows.h>
#include <ctime>
extern "C"
{
	_declspec(dllexport) double getCPUTime()
	{
		FILETIME createTime;
		FILETIME exitTime;
		FILETIME kernelTime;
		FILETIME userTime;
		if (GetProcessTimes(GetCurrentProcess(), &createTime, &exitTime, &kernelTime, &userTime) != -1) {
			ULARGE_INTEGER li = { {userTime.dwLowDateTime, userTime.dwHighDateTime } };
			return li.QuadPart;
		}
	}
	_declspec(dllexport) long mclock()
	{
		return clock();
	}
}


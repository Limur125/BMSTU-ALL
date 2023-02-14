#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <stdbool.h>
#include <stdio.h>
#include <time.h>

HANDLE can_read;
HANDLE can_write;
HANDLE mutex;

LONG waiting_writers_count = 0;
LONG waiting_readers_count = 0;
LONG active_readers_count = 0;
bool is_writer_active = false;

HANDLE reader_threads[5];
HANDLE writer_threads[3];

int readers_id[5];
int writers_id[3];

int value = 0;

void start_read()
{
	InterlockedIncrement(&waiting_readers_count);
	if (is_writer_active || WaitForSingleObject(can_write, 0) == WAIT_OBJECT_0)
		WaitForSingleObject(can_read, INFINITE);
	WaitForSingleObject(mutex, INFINITE);
	InterlockedDecrement(&waiting_readers_count);
	InterlockedIncrement(&active_readers_count);
	SetEvent(can_read); 
	ReleaseMutex(mutex);
}

void stop_read()
{
	InterlockedDecrement(&active_readers_count);
	if (active_readers_count == 0)
		SetEvent(can_write);
}

void start_write()
{
	InterlockedIncrement(&waiting_writers_count);
	if (active_readers_count > 0 || is_writer_active)
		WaitForSingleObject(can_write, INFINITE);
	InterlockedDecrement(&waiting_writers_count);
	is_writer_active = true;
	ResetEvent(can_write);
}

void stop_write()
{
	is_writer_active = false;
	if (WaitForSingleObject(can_write, 0) == WAIT_OBJECT_0)
		SetEvent(can_read);
	else
		SetEvent(can_write);
}

DWORD WINAPI reader(LPVOID param)
{
	int id = *(int*)param;
	int sleepTime;
	for (int i = 0; i < 8; i++)
	{
		sleepTime = rand() % 200 + 100;
		Sleep(sleepTime);
		start_read();
		printf("\033[33mReader %d; value = %d; sleep %d.\n", id, value, sleepTime);
		stop_read();
	}
	return 0;
}

DWORD WINAPI writer(LPVOID param)
{
	int id = *(int*)param;
	int sleepTime;
	for (int i = 0; i < 8; i++)
	{
		sleepTime = rand() % 400 + 100;
		Sleep(sleepTime);
		start_write();
		value = value + 1;
		printf("\033[34mWriter %d; value = %d; sleep %d.\n", id, value, sleepTime);
		stop_write();
	}
	return 0;
}

int main(void)
{
	setbuf(stdout, NULL);
	srand(time(NULL));
	mutex = CreateMutex(NULL, FALSE, NULL);
	if (mutex == NULL)
	{
		perror("CreateMutex\n");
		exit(1);
	}
	if ((can_write = CreateEvent(NULL, TRUE, FALSE, NULL)) == NULL)
	{
		perror("CreateEvent (can_write)");
		exit(1);
	}
	if ((can_read = CreateEvent(NULL, FALSE, FALSE, NULL)) == NULL)
	{
		perror("CreateEvent (can_read)");
		exit(1);
	}
	for (int i = 0; i < 3; i++)
	{
		writers_id[i] = i;
		if ((writer_threads[i] = CreateThread(NULL, 0, &writer, writers_id + i, 0, NULL)) == NULL)
		{
			perror("CreateThread (writer)");
			exit(1);
		}
	}
	for (int i = 0; i < 5; i++)
	{
		readers_id[i] = i;
		if ((reader_threads[i] = CreateThread(NULL, 0, &reader, readers_id + i, 0, NULL)) == NULL)
		{
			perror("CreateThread (reader)");
			exit(1);
		}
	}
	WaitForMultipleObjects(3, writer_threads, TRUE, INFINITE);
	WaitForMultipleObjects(5, reader_threads, TRUE, INFINITE);
	for (int i = 0; i < 5; i++)
		CloseHandle(reader_threads[i]);
	for (int i = 0; i < 3; i++)
		CloseHandle(writer_threads[i]);
	CloseHandle(can_read);
	CloseHandle(can_write);
	CloseHandle(mutex);
	return 0;
}
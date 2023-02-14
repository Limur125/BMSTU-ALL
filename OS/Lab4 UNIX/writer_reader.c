#include <stdio.h>
#include <unistd.h>
#include <sys/wait.h>
#include <sys/sem.h>
#include <sys/stat.h>
#include <sys/shm.h>

int* counter = NULL;

struct sembuf sem_start_read[5] = { {1, 1, 0}, {3, 0, 0}, {2, 0, 0}, {0, 1, 0}, {1, -1, 0} };
struct sembuf sem_stop_read[1] = { {0, -1, 0} };
struct sembuf sem_start_write[6] = { {WAIT_WRITERS, V, 0}, {ACTIVE_READERS, S, 0},
 {CAN_WRITE, S, 0}, {CAN_WRITE, V, 0}, {CAN_READ, V, 0}, {WAIT_WRITERS, P, 0} };
struct sembuf sem_stop_write[2] = {	{CAN_READ, P, 0}, {CAN_WRITE, P, 0} };

int start_write(int sem_id)
{
	return semop(sem_id, sem_start_write, 6);
}

int stop_write(int sem_id)
{
	return semop(sem_id, sem_stop_write, 2);
}

void writer_run(const int sem_id, const int writer_id)
{
	int sleep_time = rand() % 2 + 1;
	sleep(sleep_time);
	int rc = start_write(sem_id);
	if (rc == -1)
	{
		perror("start write semop error");
		exit(1);
	}
	(*counter)++;
	printf("\e[1;32mWriter #%d \twrite: \t%d \tsleep: %d\e[0m\n", writer_id, *counter, sleep_time);
	rc = stop_write(sem_id);
	if (rc == -1)
	{
		perror("stop write semop error");
		exit(1);
	}
}

void writer_create(const int sem_id, const int writer_id)
{
	pid_t childpid;
	if ((childpid = fork()) == -1)
	{
		perror("writer fork error");
		exit(1);
	}
	else if (childpid == 0)
	{
		while (1)
			writer_run(sem_id, writer_id);
		exit(0);
	}
}

int start_read(int sem_id)
{
	return semop(sem_id, sem_start_read, 5);
}

int stop_read(int sem_id)
{
	return semop(sem_id, sem_stop_read, 1);
}

void reader_run(const int sem_id, const int reader_id)
{
	int sleep_time = rand() % 2 + 1;
	sleep(sleep_time);
	int rc = start_read(sem_id);
	if (rc == -1)
	{
		perror("start read semop error");
		exit(1);
	}
	printf("\e[1;33mReader #%d \tread: \t%d \tsleep: %d\e[0m \n", reader_id, *counter, sleep_time);
	rc = stop_read(sem_id);
	if (rc == -1)
	{
		perror("stop read semop error");
		exit(1);
	}
}

void reader_create(const int sem_id, const int reader_id)
{
	pid_t childpid;
	if ((childpid = fork()) == -1)
	{
		perror("reader fork");
		exit(1);
	}
	else if (childpid == 0)
	{
		while (1)
			reader_run(sem_id, reader_id);
		exit(0);
	}
}

int main(void)
{
	int sem_descr;
	int status;
	int perms = S_IRUSR | S_IWUSR | S_IRGRP | S_IROTH;
	int shmid = shmget(IPC_PRIVATE, sizeof(int), perms);
	if (shmid == -1)
	{
		perror("shmget error");
		exit(1);
	}
	counter = shmat(shmid, NULL, 0);
	if (counter == (char*)-1)
	{
		perror("shmat error");
		exit(1);
	}
	*counter = 0;
	sem_descr = semget(IPC_PRIVATE, 4, IPC_CREAT | perms);
	if (sem_descr == -1)
	{
		perror("semget error");
		exit(1);
	}
	if (semctl(sem_descr, CAN_WRITE, SETVAL, 0) == -1)
	{
		perror("semclt error");
		exit(1);
	}
	if (semctl(sem_descr, CAN_READ, SETVAL, 0) == -1)
	{
		perror("semclt error");
		exit(1);
	}
	if (semctl(sem_descr, ACTIVE_READERS, SETVAL, 0) == -1)
	{
		perror("semclt error");
		exit(1);
	}
	if (semctl(sem_descr, WAIT_WRITERS, SETVAL, 0) == -1)
	{
		perror("semclt error");
		exit(1);
	}
	for (int i = 0; i < 5; i++)
		reader_create(sem_descr, i + 1);
	for (int i = 0; i < 3; i++)
		writer_create(sem_descr, i + 1);
	for (int i = 0; i < 8; i++)
		wait(&status);
	if (shmdt(counter) == -1)
		perror("shmdt error");
	if (semctl(sem_descr, 0, IPC_RMID, 0) == -1)
	{
		perror("shmclt");
		exit(1);
	}
	return 0;
}

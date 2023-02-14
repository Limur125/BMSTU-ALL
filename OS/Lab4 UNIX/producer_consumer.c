#include <stdio.h>
#include <stdlib.h>
#include <sys/sem.h>
#include <sys/shm.h>
#include <time.h>
#include <wait.h>
#include <unistd.h>
#include <sys/stat.h>


struct sembuf producer_enter_cr[2] = { {1, -1, 0}, {2, -1, 0} };
struct sembuf producer_exit_cr[2] = { {2, 1, 0}, {0, 1, 0} };
struct sembuf consumer_enter_cr[2] = { {0, -1, 0}, {2, -1, 0} };
struct sembuf consumer_exit_cr[2] = { {2, 1, 0}, {1, 1, 0} };

char symb = 'a';

void write_buffer(char* buffer, char symb)
{
	*(char*)(buffer[24]) = symb;
	(char*)(buffer[24])++;
}

void read_buffer(char* buffer, char* dst)
{
	*dst = *(char*)(buffer[28]);
	(char*)(buffer[28])++;
}

void buffer_init(char* buf)
{
	(char*)(buf[24]) = buf;
	(char*)(buf[28]) = buf;
}
void producer_run(char* buffer, int sem_id, int prod_id)
{
	int sleep_time = rand() % 2 + 1;
	sleep(sleep_time);
	int rc = semop(sem_id, producer_enter_cr, 2);
	if (rc == -1)
	{
		perror("Producer enter semop error");
		exit(1);
	}
	char s = symb++;
	s = 'a' + (s % ('z' - 'a'));
	if (write_buffer(buffer, s) == -1)
		exit(1);
	printf(" \e[1;32mProducer #%d \twrite: \t%c \tsleep: %d\e[0m \n", prod_id, s, sleep_time);
	rc = semop(sem_id, producer_exit_cr, 2);
	if (rc == -1)
	{
		perror("Producer exit semop error");
		exit(1);
	}
}

void producer_create(char* const buffer, const int prod_id, const int sem_id)
{
	pid_t childpid;
	if ((childpid = fork()) == -1)
	{
		perror("Producer fork error");
		exit(1);
	}
	else if (!childpid)
	{
		for (int i = 0; i < 8; i++)
			producer_run(buffer, sem_id, prod_id);
		exit(0);
	}
}

void consumer_run(char* const buffer, const int sem_id, const int cons_id)
{
	char ch;
	int sleep_time = rand() % 4 + 1;
	sleep(sleep_time);
	int rc = semop(sem_id, consumer_enter_cr, 2);
	if (rc == -1)
	{
		perror("Consumer enter semop error");
		exit(1);
	}
	if (read_buffer(buffer, &ch) == -1)
		exit(1);
	printf(" \e[1;33mConsumer #%d \tread:  \t%c \tsleep: %d\e[0m\n", cons_id, ch, sleep_time);
	rc = semop(sem_id, consumer_exit_cr, 2);
	if (rc == -1)
	{
		perror("Consumer exit semop error");
		exit(1);
	}
}

void consumer_create(char* const buffer, const int con_id, const int sem_id)
{
	pid_t childpid;
	if ((childpid = fork()) == -1)
	{
		perror("Consumer for error");
		exit(1);
	}
	else if (!childpid)
	{
		for (int i = 0; i < 8; i++)
			consumer_run(buffer, sem_id, con_id);
		exit(0);
	}
}

int main(void)
{
	setbuf(stdout, NULL);
	srand(time(NULL));
	int sem_descr;
	int perms = S_IRUSR | S_IWUSR | S_IRGRP | S_IROTH;
	int shmid = shmget(IPC_PRIVATE, 32, IPC_CREAT | perms);
	if (shmid == -1)
	{
		perror("shmget error");
		exit(1);
	}
	char* buffer = shmat(shmid, 0, 0);
	if (buffer == (char*)-1)
	{
		perror("shmat error");
		exit(1);
	}
	buffer_init(buffer);
	sem_descr = semget(IPC_PRIVATE, 3, IPC_CREAT | perms);
	if (sem_descr == -1)
	{
		perror("semget error");
		exit(1);
	}
	if (semctl(sem_descr, 2, SETVAL, 1) == -1)
	{
		perror("semctl error");
		exit(1);
	}
	if (semctl(sem_descr, 1, SETVAL, 24) == -1)
	{
		perror("semctl error");
		exit(1);
	}
	if (semctl(sem_descr, 0, SETVAL, 0) == -1)
	{
		perror("semctl error");
		exit(1);
	}
	for (int i = 0; i < 3; i++)
		producer_create(buffer, i + 1, sem_descr);
	for (int i = 0; i < 3; i++)
		consumer_create(buffer, i + 1, sem_descr);
	for (size_t i = 0; i < 6; i++)
	{
		int status;
		if (wait(&status) == -1)
		{
			perror("wait error");
			exit(1);
		}
		if (!WIFEXITED(status))
			printf("One of children terminated abnormally\n");
	}
	if (shmctl(shmid, IPC_RMID, NULL))
	{
		perror("shmctl error");
		exit(1);
	}
	if (shmdt((void*)buffer) == -1)
	{
		perror("shmdt error");
		exit(1);
	}
	if (semctl(sem_descr, 0, IPC_RMID, 0) == -1)
	{
		perror("semctl error");
		exit(1);
	}
	return 0;
}

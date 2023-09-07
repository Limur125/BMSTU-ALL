#include "lamp.h"
#include <stdio.h>
#include <pthread.h>
#include <stdbool.h>
#include <unistd.h>

struct targ_t
{
	int num;
	int res;
	int id;
};

int choosing[40] = { 0 };
int number[40] = { 0 };
int counter = 'a';
int num = 0;

void *get_number(void *arg)
{
	struct targ_t *targ = arg;
	int i = num++;
	targ->id = i;
	choosing[i] = 1;
	int max = 0;
	for (int j = 0; j < 40; j++)
		if (number[j] > max)
			max = number[j];
	number[i] = max + 1;
	targ->num = number[i];
	choosing[i] = 0;
	return 0;
}

void *bakery(void *arg)
{
	struct targ_t *targ = arg;
	int i = targ->id;
	for (int j = 0; j < 40; j++) 
	{
		if(i != j)
		{
			while (choosing[j]);
			while ((number[j] != 0) && (number[j] < number[i]));
		}
	}
	targ->res = ++counter;
	number[i] = 0; 
	return 0;
}

struct BAKERY *
bakery_proc_1_svc(struct BAKERY *argp, struct svc_req *rqstp)
{
	static struct BAKERY  result;
	switch (argp->op)
	{
		case GET_NUMBER:
		{
			pthread_t tid;
			struct targ_t tres;
			pthread_create(&tid, NULL, get_number, &tres);
			pthread_join(tid, NULL);
			result.arg2 = argp->arg;
			result.arg = tres.num;
			result.op = tres.id;
			break;
		}
		case ENTER_CR:	
		{
			pthread_t tid;
			struct targ_t tres;
			tres.id = argp->arg;
			pthread_create(&tid, NULL, bakery, &tres);
			pthread_join(tid, NULL);
			result.arg2 = argp->arg2;
			result.result = tres.res;
			result.op = tres.id;
			break;
		}
	}

	return &result;
}

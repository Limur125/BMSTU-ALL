#include <stdio.h>
#include <fcntl.h>
#include <pthread.h>
#include <unistd.h>
#include <stdlib.h> 

void *thread_func(void *args)
{
    int flag = 1;
    FILE *fs = fdopen(*((int*)args), "r");
    char buff2[20];
    setvbuf(fs, buff2, _IOFBF, 20);

    while (flag == 1)
    {
        char c;
        if ((flag = fscanf(fs, "%c", &c)) == 1)
            fprintf(stdout, "thread 2 read: %c\n", c);
    }
}
int main(void)
{
    setbuf(stdout, NULL);
    pthread_t thread;
    int fd = open("alphabet.txt", O_RDONLY);

    FILE *fs1 = fdopen(fd, "r");
    char buff1[20];
    setvbuf(fs1, buff1, _IOFBF, 20);



    if (pthread_create(&thread, NULL, thread_func, (void *)(&fd)) != 0)
	{
		perror("Error in pthread_create\n");
		exit(-1);
	}
	
    int flag = 1;
    while (flag == 1)
    {
        char c;
        if ((flag = fscanf(fs1, "%c", &c)) == 1)
        {
            fprintf(stdout, "thread 1 read: %c\n", c);
        }
    }
    pthread_join(thread, NULL);
    close(fd);
    return 0;
}

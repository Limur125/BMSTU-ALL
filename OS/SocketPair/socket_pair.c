#include <sys/types.h>
#include <sys/socket.h>
#include <stdlib.h>
#include <stdio.h>
#include <errno.h>
#include <string.h>
#include <unistd.h>
#include <wait.h>
#define BUF_SIZE 1024

int main(int argc, char ** argv)
{
	int sockets[2];
	int buf;
	pid_t cpid[2];
	if (socketpair(AF_UNIX, SOCK_STREAM, 0, sockets) < 0) 
    {
		perror("socketpair() failed");
		exit(1);
    }
	for (int i = 0; i < 2; i++)
	{
	    if ((cpid[i] = fork()) == -1)
        {
	        perror("Error fork()");
	        exit(1);
	    }
	    else if (cpid[i] == 0)
	    {
	    	pid_t cbuf, pid = getpid();
	        close(sockets[1]);
			write(sockets[0], &pid, sizeof(pid));
            printf("Child %d sent pid\n", pid);
            read(sockets[0], &cbuf, sizeof(cbuf));
            close(sockets[0]);
            printf("Child %d read %d\n", pid, cbuf);
	        return 0;
	    }
	}
	close(sockets[0]);
	read(sockets[1], &buf, sizeof(buf));
	printf("Parent read %d\n", buf);
	buf*=10;
	write(sockets[1], &buf, sizeof(buf));
	printf("Parent reply %d\n", buf);
	read(sockets[1], &buf, sizeof(buf));
	printf("Parent read %d\n", buf);
	buf*=10;
	write(sockets[1], &buf, sizeof(buf));
	close(sockets[1]);
	printf("Parent reply %d\n", buf);
	return 0;
}

#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <signal.h>
#define SERVADDR "socket_server"

int sockfd;

struct message
{
	int pid;
	char m[64];
};

void finish_program(int sig_n)
{
    unlink(SERVADDR);
    close(sockfd);
    exit(0);
}


int main(void)
{
	signal(SIGINT, finish_program);
	sockfd = socket(AF_UNIX, SOCK_DGRAM, 0);
	if (sockfd == -1)
	{
		perror("socket error");
		exit(-1);
	}
	struct sockaddr server;
	server.sa_family = AF_UNIX;
	strcpy(server.sa_data, SERVADDR);
	if (bind(sockfd, &server, strlen(server.sa_data) + sizeof(server.sa_family)) == -1)
    {
        perror("bind error");
        close(sockfd);
        exit(-1);
    }
    int i = 0;
    while (1)
    {
    	struct message rec;
        int rc = recvfrom(sockfd, &rec, sizeof(rec),  0, NULL, NULL);
		if (rc < 0)
		{
		    perror("recvfrom error");
		    unlink(SERVADDR);
		    close(sockfd);
		    exit(-1);
		}
		struct sockaddr client;
    	client.sa_family = AF_UNIX;
    	sprintf(client.sa_data, "%d", rec.pid);
		sleep(1);
		printf("Server recieved: %s\n", rec.m);
		int l = strlen(rec.m);
		rec.m[l] = i++ + '0';
		rec.m[l + 1] = 0;
		if (sendto(sockfd, rec.m, strlen(rec.m) + 1, 0, &client, strlen(client.sa_data) + sizeof(client.sa_family)) == -1)
		{
			perror("sendto error");
			unlink(SERVADDR);
			close(sockfd);
			exit(-1);
		}
		printf("Server replyed: %s\n", rec.m);
	}
	return 0;
}

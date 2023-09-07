#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>

int main(void)
{	
	int sockfd = socket(AF_UNIX, SOCK_DGRAM, 0);
	if (sockfd == -1)
	{
		perror("socket error");
		exit(-1);
	}
	struct sockaddr client;
	client.sa_family = AF_UNIX;
	char pid_mes[14];
	sprintf(pid_mes, "%d", getpid());
	strcpy(client.sa_data, pid_mes);
	if (bind(sockfd, &client, strlen(client.sa_data) + sizeof(client.sa_family)) == -1)
    {
        perror("bind error");
        close(sockfd);
        exit(-1);
    }
    struct sockaddr server;
    server.sa_family = AF_UNIX;
    strcpy(server.sa_data, "socket_server");
	if (sendto(sockfd, pid_mes, strlen(pid_mes), 0, &server, strlen(server.sa_data) + sizeof(server.sa_family)) == -1)
	{
		perror("sendto error");
		unlink(client.sa_data);
		close(sockfd);
		exit(-1);
	}
	printf("Client sent: %s\n", pid_mes);
    char buf[128];
    int rc = recvfrom(sockfd, buf, sizeof(buf),  0, NULL, NULL);
    if (rc < 0)
    {
        perror("recvfrom error");
        unlink(client.sa_data);
        close(sockfd);
        exit(-1);
    }
    printf("Client recieved: %s\n", buf);
    unlink(client.sa_data);
    close(sockfd);
    return 0;
}


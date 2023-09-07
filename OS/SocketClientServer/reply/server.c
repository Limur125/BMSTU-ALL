#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>

int main(void)
{
	int sockfd = socket(AF_UNIX, SOCK_DGRAM, 0);
	if (sockfd == -1)
	{
		perror("socket error");
		exit(-1);
	}
	struct sockaddr server;
	server.sa_family = AF_UNIX;
	strcpy(server.sa_data, "socket_server");
	if (bind(sockfd, &server, strlen(server.sa_data) + sizeof(server.sa_family)) == -1)
    {
        perror("bind error");
        close(sockfd);
        exit(-1);
    }
	char rec[128];
    int rc = recvfrom(sockfd, &rec, sizeof(rec),  0, NULL, NULL);
	if (rc < 0)
	{
	    perror("recvfrom error");
	    unlink("socket_server");
	    close(sockfd);
	    exit(-1);
	}
	struct sockaddr client;
	client.sa_family = AF_UNIX;
	strcpy(client.sa_data, rec);
	sleep(1);
	printf("Server recieved: %s\n", rec);
	int l = strlen(rec);
	rec[l] = '0';
	rec[l + 1] = 0;
	if (sendto(sockfd, rec, strlen(rec) + 1, 0, &client, strlen(client.sa_data) + sizeof(client.sa_family)) == -1)
	{
		perror("sendto error");
		unlink("socket_server");
		close(sockfd);
		exit(-1);
	}
	printf("Server replyed: %s\n", rec);
	unlink("socket_server");
    close(sockfd);
	return 0;
}

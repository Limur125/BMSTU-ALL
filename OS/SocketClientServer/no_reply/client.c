#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>

int main(int argc, char **argv)
{	
	int sockfd = socket(AF_UNIX, SOCK_DGRAM, 0);
    if (sockfd == -1)
    {
        perror("socket error");
        exit(-1);
    }
	struct sockaddr server;
    server.sa_family = AF_UNIX;
    strcpy(server.sa_data, "socketlab");
	char* buf = argv[1];
	if (sendto(sockfd, buf, strlen(buf) + 1, 0, &server, strlen(server.sa_data) + sizeof(server.sa_family)) == -1)
	{
		perror("sendto error");
		close(sockfd);
		exit(-1);
	}
	printf("Client sent: %s\n", buf);
    close(sockfd);
    return 0;
}


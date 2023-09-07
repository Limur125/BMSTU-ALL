#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>

int main(void)
{
	struct sockaddr server;
	int sockfd = socket(AF_UNIX, SOCK_DGRAM, 0);
	if (sockfd == -1)
	{
		perror("socket error");
		exit(-1);
	}
	server.sa_family = AF_UNIX;
	strcpy(server.sa_data, "socketlab");
	if (bind(sockfd, &server, strlen(server.sa_data) + sizeof(server.sa_family)) == -1)
    {
        perror("bind error");
        close(sockfd);
        exit(-1);
    }
    while(1)
    {
		char buf[128];
		int rc = recvfrom(sockfd, buf, sizeof(buf),  0, NULL, NULL);
		if (rc < 0)
		{
		    perror("recvfrom error");
		    unlink("socketlab");
		    close(sockfd);
		    exit(-1);
		}
		printf("Server recieved: %s\n", buf);
    }
    unlink("socketlab");
    close(sockfd);
    return 0;
}

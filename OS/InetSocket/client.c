#include <unistd.h>
#include <string.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <stdlib.h>
#include <arpa/inet.h>
#define LEN_LINE 128
int main(int argc, char** argv)
{
	int sockfd;
	struct sockaddr_in servaddr;
	
	if (argc != 2)
	{
		perror("argv error");
		exit(-1);
	}
	sockfd = socket(AF_INET, SOCK_STREAM, 0);
	memset(&servaddr, 0, sizeof(servaddr));
	servaddr.sin_family = AF_INET;
	servaddr.sin_port = htons(9877);
	inet_pton(AF_INET, argv[1], &servaddr.sin_addr);
	connect(sockfd, (struct sockaddr*) &servaddr, sizeof(servaddr));
	char sendmsg[LEN_LINE], recvmsg[LEN_LINE];
	sprintf(sendmsg, "%d", getpid());
	printf("Sent %s\n", sendmsg);
	write(sockfd, sendmsg, strlen(sendmsg));
	if (read(sockfd, recvmsg, LEN_LINE) == 0)
	{
		perror("read recv");
		exit(-1); 
	}
	printf("Recieved %s\n", recvmsg);
	return 0;
}

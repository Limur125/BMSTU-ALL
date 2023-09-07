#include <unistd.h>
#include <stdio.h>
#include <sys/socket.h>
#include <sys/epoll.h>
#include <stdlib.h>
#include <netinet/in.h>
#include <string.h>
#include <errno.h>
#include <fcntl.h>
#define STRING_LEN 128
#define MAX_EVENTS 10

void do_use_fd(int connfd)
{
	ssize_t n;
	int servpid = getpid();
	char recvmsg[STRING_LEN], sendmsg[STRING_LEN];
	again:
	while ((n = read(connfd, recvmsg, STRING_LEN)) > 0)
	{
		sprintf(sendmsg, "%d ", servpid);
		recvmsg[n] = 0;
		strcat(sendmsg, recvmsg);
		write(connfd, sendmsg, STRING_LEN);
	}
	if (n < 0 && errno == EINTR)
		goto again;
}

int main(int argc, char** argv)
{
	int listenfd, connfd;
	socklen_t clilen;
	struct sockaddr_in cliaddr, servaddr;
	listenfd = socket(AF_INET, SOCK_STREAM, 0);
	memset(&servaddr, 0, sizeof(servaddr));
	servaddr.sin_family = AF_INET;
	servaddr.sin_addr.s_addr = htonl(INADDR_ANY);
	servaddr.sin_port = htons(9877);
	bind(listenfd, (struct sockaddr*) &servaddr, sizeof(servaddr));
	listen(listenfd, 1024);
	struct epoll_event ev, events[MAX_EVENTS];
    int nfds, epollfd;
    epollfd = epoll_create1(0);
    if (epollfd == -1) 
    {
    	perror("epoll_create1");
    	exit(-1);
    }
    ev.events = EPOLLIN;
    ev.data.fd = listenfd;
    if (epoll_ctl(epollfd, EPOLL_CTL_ADD, listenfd, &ev) == -1) 
    {
    	perror("epoll_ctl: listen_sock");
    	exit(-1);
    }
	for(;;)
	{	
		nfds = epoll_wait(epollfd, events, MAX_EVENTS, -1);
        if (nfds == -1) 
        {
        	perror("epoll_wait");
            exit(-1);
        }
        for (int n = 0; n < nfds; ++n) 
        {
        	if (events[n].data.fd == listenfd) 
        	{
        		clilen = sizeof(cliaddr);
            	connfd = accept(listenfd, (struct sockaddr*) &cliaddr, &clilen);
                if (connfd == -1) 
                {
                    perror("accept");
                    exit(-1);
                }
				fcntl(connfd, F_SETFL, O_NONBLOCK);
                ev.events = EPOLLIN | EPOLLET;
                ev.data.fd = connfd;
                if (epoll_ctl(epollfd, EPOLL_CTL_ADD, connfd, &ev) == -1) 
                {
                    perror("epoll_ctl: conn_sock");
                    exit(-1);
                }
            } 
            else 
            {
               do_use_fd(events[n].data.fd);
            }
        }
	}
}

#ifndef SERVER_H
#define SERVER_H

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <unistd.h>
#include <netdb.h> // for getnameinfo()

// Usual socket headers
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>

#include "errors.h"
#include "string-utilities.h"
#include "thread_pool.h"

#define BACKLOG 100		
#define INPUT_BUFFER_SIZE 8096	
#define THREAD_POOL_CAPACITY 128

typedef struct r {
	char *method;
	char *route;
} Request;

int serve(uint16_t port);
int acceptTCPConnection(int serverSocket, LogData *log, int* clientSockets);
void handleHTTPClient(void *arg);
int router(char *request, int clientSocket, char **filename, int *mode);
void report(struct sockaddr_in *serverAddress);
void setHostServiceFromSocket(struct sockaddr_in *socketAddress, char *hostBuffer, char *serviceBuffer);
void logConnection(LogData *log);
void freeLog(LogData *log);

#endif

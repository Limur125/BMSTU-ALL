#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <sys/select.h>
#include "server.h"

#define MAX_PORT_DIGITS 6 

typedef struct threadData
{
	LogData *log;
	int socket;
} threadData_t;

int serve(uint16_t port)
{
	threadpool_t *tm = threadpool_create(THREAD_POOL_CAPACITY, MAX_QUEUE, 0);

	int serverSocket = socket(AF_INET, SOCK_STREAM,	0);
	int reuse = 1;
	if (setsockopt(serverSocket, SOL_SOCKET, SO_REUSEADDR, &reuse, sizeof(reuse)) < 0) {
		errorHandler(ERROR, "setsockopt() failed", 0);
		return EXIT_FAILURE;
	}

	struct sockaddr_in serverAddress;
	serverAddress.sin_family = AF_INET;
	serverAddress.sin_port = htons(port);
	serverAddress.sin_addr.s_addr = htonl(INADDR_LOOPBACK);

	int bound = bind(serverSocket, (struct sockaddr *) &serverAddress, sizeof(serverAddress));
	if (bound != 0) {
		char portString[23 + MAX_PORT_DIGITS];
		sprintf(portString, "Socket not bound port %d", port);
		errorHandler(ERROR, portString, 0);
		return EXIT_FAILURE;
	}

	int listening = listen(serverSocket, BACKLOG);
	if (listening < 0) {
		printf("Error: The server is not listening.\n");
		return EXIT_FAILURE;
	}
	fd_set client_fds;

	report(&serverAddress);
	int clientSocket;
	int clientSockets[1024];
	for (int i = 0; i< 1024; i++)
		clientSockets[i]=-1;
	while(1) {
		LogData *log = calloc(1, sizeof(*log));
		log->clientAddr = calloc(1, sizeof(*log->clientAddr));
		log->req = NULL;
		clientSocket = acceptTCPConnection(serverSocket, log, clientSockets);
		if (clientSocket == -1)
			return EXIT_FAILURE;
		FD_ZERO(&client_fds);
		for (int i = 0; i< 1024; i++)
			if(clientSockets[i] >= 0)
			{
				FD_SET(clientSockets[i], &client_fds);
			}
		if (select(clientSocket+1, &client_fds, NULL,NULL,NULL)==-1)	
		{
			return EXIT_FAILURE;
		}
		for (int i = 0; i < 1024; i++)
			if(clientSockets[i] > 0 && FD_ISSET(clientSockets[i], &client_fds))
			{
				threadData_t *data = calloc(1, sizeof(threadData_t));
				LogData *logt = calloc(1, sizeof(*log));
				logt->clientAddr = calloc(1, sizeof(*log->clientAddr));
				logt->req = NULL;
				memcpy(logt->clientAddr, log->clientAddr, sizeof(*log->clientAddr));
				data->log = logt;
				data->socket = clientSockets[i];
				clientSockets[i] = -1;
				threadpool_add(tm, handleHTTPClient, data, 0);
			}
		freeLog(log);
	}
	return 0;
}

int acceptTCPConnection(int serverSocket, LogData *log, int* clientSockets) {
	struct sockaddr_storage genericClientAddr;
	socklen_t clientAddrLen = sizeof(genericClientAddr);
	memset(&genericClientAddr, 0, clientAddrLen);
	
	int clientSocket = accept(serverSocket, (struct sockaddr *)&genericClientAddr, &clientAddrLen);
	if (clientSocket < 0) {
		fprintf(stderr, "accept() failed");
		return -1;
	}
	int i = 0;
	for(i = 0; i< 1024; i++)
		if (clientSockets[i] < 0)
		{
			clientSockets[i] =clientSocket;
			break;
		}
	if (i >= 1024)
	{
		return -1;
	}
	log->clientAddr->sa_family = ((struct sockaddr *)&genericClientAddr)->sa_family;
	memcpy(log->clientAddr->sa_data, ((struct sockaddr *)&genericClientAddr)->sa_data, 14);
	return clientSocket;
}

void handleHTTPClient(void *arg)
{
	
	threadData_t* data = arg;
	int clientSocket = data->socket;
	LogData* log = data->log;
	char recvBuffer[INPUT_BUFFER_SIZE];
	memset(recvBuffer, 0, sizeof(char) * INPUT_BUFFER_SIZE);
	
	int nBytesReceived = recv(clientSocket, recvBuffer, sizeof(recvBuffer), 0);
	if (nBytesReceived < 0) {
		errorHandler(FORBIDDEN, "Failed to read request.", clientSocket);
		freeLog(log);
		free(data);
		close(clientSocket);
		return;	
	}
	if (nBytesReceived < INPUT_BUFFER_SIZE) {
		recvBuffer[nBytesReceived] = 0;
	} 

	char *response = NULL;
	char *filename = NULL;
	char *req = NULL;
	firstLine(recvBuffer, &req);
	char *tmpReq = realloc(log->req, (strlen(req) * sizeof(*(log->req))) + 1);
	if (tmpReq == NULL) 
	{
		ErrorSystemMessage("Memory allocation: realloc() failure.");
		free(response);
		free(filename);
		freeLog(log);
		free(data);
		close(clientSocket);
		return;	
	}
	log->req = tmpReq;
	strcpy(log->req, req);
	free(req);
	int mode;
	if (router(recvBuffer, clientSocket, &filename, &mode) != 0)
	{
		free(response);
		free(filename);
		freeLog(log);
		free(data);
		close(clientSocket);
		return;
	}

	ssize_t mimeTypeIndex = -1;
	if ((mimeTypeIndex = fileTypeAllowed(filename)) == -1) {
		free(filename);
		errorHandler(FORBIDDEN, "mime type\nRequested file type not allowed.", clientSocket);
		free(response);
		freeLog(log);
		free(data);
		close(clientSocket);
		return;
	}

	size_t responseLen;

	if (setResponse(filename, &response, OK, mimeTypeIndex, clientSocket, log, mode, &responseLen) != 0) {
		free(response);
		freeLog(log);
		free(data);
		close(clientSocket);
		return;
	}

	send(clientSocket, response, responseLen, 0);
	free(response);
	free(filename);
	logConnection(log);
	free(data);
	close(clientSocket);
}

int router(char *request, int clientSocket, char **filename, int *mode) {
	if((strncmp(request, "GET ", 4) && strncmp(request, "get ", 4)) && (strncmp(request, "HEAD ", 5) && strncmp(request, "head ", 5))) {
		errorHandler(NOT_ALLOWED, "Only simple GET and HEAD operation supported", clientSocket);
		return 1;
	}
	*mode = 1;
	if (!strncmp(request, "HEAD ", 5) || !strncmp(request, "head ", 5))
	{
		*mode = 0;
	}
	size_t requestLen = strlen(request);
	for (size_t i = *mode ? 4 : 5; i < requestLen; i++) {
		if (request[i] == ' ') {
			request[i] = 0;
			break;
		}
	}

	if (strstr(request, "..")) {
		errorHandler(FORBIDDEN, "Parent directory (..) path names are forbidden.", clientSocket);
		return 1;
	}

	if(!strncmp(&request[0], "GET /\0", 6) || !strncmp(&request[0], "get /\0", 6)) {
		(void)strcpy(request, "GET /index.html");
	}

	if(!strncmp(&request[0], "HEAD /\0", 7) || !strncmp(&request[0], "head /\0", 7)){
		(void)strcpy(request, "HEAD /index.html");
	}

	size_t filenameLen = strlen(request) - (*mode ? 5 : 6);
	*filename = calloc(filenameLen + 1, sizeof(**filename));
	for (size_t i = *mode ? 5 : 6, j = 0; j < filenameLen; i++, j++) {
		(*filename)[j] = request[i];
	}

	return 0;
}

void report(struct sockaddr_in *serverAddress)
{
	char hostBuffer[INET6_ADDRSTRLEN];
	char serviceBuffer[NI_MAXSERV];
	socklen_t addr_len = sizeof(*serverAddress);
	int err = getnameinfo(
			(struct sockaddr *) serverAddress,
			addr_len,
			hostBuffer,
			sizeof(hostBuffer),
			serviceBuffer,
			sizeof(serviceBuffer),
			NI_NUMERICHOST
			);
	if (err != 0) {
		printf("It's not working!!\n");
	}
	printf("Server listening on http://%s:%s\n", hostBuffer, serviceBuffer);
}

void setHostServiceFromSocket(struct sockaddr_in *socketAddress, char *hostBuffer, char *serviceBuffer)
{
	socklen_t addr_len = sizeof(*socketAddress);
	int err = getnameinfo(
			(struct sockaddr *) socketAddress,
			addr_len,
			hostBuffer,
			INET6_ADDRSTRLEN,
			serviceBuffer,
			NI_MAXSERV,
			NI_NUMERICHOST
			);
	if (err != 0) {
		errorHandler(ERROR, "getnameinfo()", 0);
		return;
	}
}

void logConnection(LogData *log)
{
	char *IPString = NULL;
	
	switch(log->clientAddr->sa_family) {
	    case AF_INET: {
		struct sockaddr_in *addr_in = (struct sockaddr_in *)log->clientAddr;
		IPString = calloc(INET_ADDRSTRLEN + 1, sizeof(*IPString));
		inet_ntop(AF_INET, &(addr_in->sin_addr), IPString, INET_ADDRSTRLEN);
		break;
	    }
	    case AF_INET6: {
		struct sockaddr_in6 *addr_in6 = (struct sockaddr_in6 *)log->clientAddr;
		IPString = calloc(INET6_ADDRSTRLEN + 1, sizeof(*IPString));
		inet_ntop(AF_INET6, &(addr_in6->sin6_addr), IPString, INET6_ADDRSTRLEN);
		break;
	    }
	    default:
		break;
	}
	char *timeString = NULL;
	char *status = "200";

	timestamp(&timeString);
	printf("%s - - [%s] \"%s\" %s %lu\n", IPString, timeString, log->req, status, log->size);
	free(timeString);
	free(IPString);
	freeLog(log);
}

void freeLog(LogData *log)
{
	free(log->clientAddr);
	free(log->req);
	free(log);
}

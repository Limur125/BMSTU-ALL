#ifndef STRING_UTILITIES_H
#define STRING_UTILITIES_H

#include <string.h>
#include <stdio.h>
#include <errno.h>
#include <stdlib.h>
#include <time.h>
#include "errors.h"

enum statusCode {
	ERROR =	42,
	LOG = 44,
	OK = 200,
	FORBIDDEN = 403,
	NOT_FOUND = 404,
	NOT_ALLOWED = 405,
};

typedef struct l {
	size_t size;
	struct sockaddr *clientAddr;
	char *req;
} LogData;

int stringFromFile(char *file, char **buffer, size_t *bufferLen);
int setHeader(char **header, int status, ssize_t mimeTypeIndex, size_t bodyLen);
int setBody(char **body, char filename[], size_t *bodyLen);
int setStatusString(char **statusString, enum statusCode s);
int setResponse(char *filename, char **response, int status, ssize_t mimeTypeIndex,
		int clientSocket, LogData *log, int mode, size_t *responseLen);
ssize_t fileTypeAllowed(char *buffer);
void timestamp(char **t);
ssize_t firstLine(char *str, char **line);

#endif

#ifndef ERRORS_H
#define ERRORS_H

#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include "string-utilities.h"

void errorHandler(int type, char *msg, int socketFd);
void ErrorUserMessage(const char *msg, const char *detail);
void ErrorSystemMessage(const char *msg);
int sendErrorResponse(char* filename, int status, int socketFd);

#endif

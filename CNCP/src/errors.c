#include "errors.h"

void errorHandler(int type, char *msg, int socketFd)
{
	switch (type) {
		case ERROR:
			ErrorSystemMessage(msg);
			break;
		case FORBIDDEN:
			sendErrorResponse("responses/403.html", FORBIDDEN, socketFd);
			ErrorUserMessage("FORBIDDEN", msg);
			break;
		case NOT_FOUND:
			sendErrorResponse("responses/404.html", NOT_FOUND, socketFd); 
			ErrorUserMessage("NOT_FOUND", msg);
			break;
		case NOT_ALLOWED:
			sendErrorResponse("responses/405.html", NOT_ALLOWED, socketFd); 
			ErrorUserMessage("NOT_ALLOWED", msg);
			break;
	}
}

int sendErrorResponse(char* filename, int status, int socketFd)
{
	char *body = NULL;
	size_t bodyLen;
	if (stringFromFile(filename, &body, &bodyLen) != 0) {
		free(body);
		ErrorSystemMessage(filename);
		return 1;
	}
	char *header = NULL;
	setHeader(&header, status, 9, bodyLen); 
	
	char *response = calloc(strlen(header) + bodyLen + 1, sizeof(*response));
	strcpy(response, header);
	strcat(response, body);
	
	write(socketFd, response, strlen(response) + 1);

	free(response);
	free(body);
	return 0;
}

void ErrorUserMessage(const char *msg, const char *detail)
{
	fputs(msg, stderr);
	fputs(": ", stderr);
	fputs(detail, stderr);
	fputc('\n', stderr);
}

void ErrorSystemMessage(const char *msg)
{
	perror(msg);
}



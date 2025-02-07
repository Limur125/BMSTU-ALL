#include "string-utilities.h"

#define MAX_TIME_CHARS 32

static const struct {
	char *ext;
	char *filetype;
} extensions[] = {			
	{ "css", "text/css" },		// 0	
	{ "gif", "image/gif" },		// 1 
	{ "jpg", "image/jpg" },		// 2 
	{ "png", "image/png" },		// 3 
	{ "ico", "image/ico" },		// 4 
	{ "zip", "image/zip" },		// 5 
	{ "gz",  "image/gz"  },		// 6 
	{ "tar", "image/tar" },		// 7 
	{ "htm", "text/html" },		// 8 
	{ "html", "text/html" },	// 9 
	{ "jpeg", "image/jpeg" },	// 10
	{ "js", "text/javascript" },	// 11
	{ "svg", "image/svg+xml"}, //12
	{ "swf", "application/x-shockwave-flash"}, //13
	{ 0, 0 }			// NULL for end
};

int stringFromFile(char *filename, char **buffer, size_t *bufferSize)
{
	int errnum;
	FILE *fp = fopen(filename, "r");
	if (!fp) {
		errnum = errno;
		fprintf(stderr, "Error opening file %s: %s\n", filename, strerror(errnum));
		return 1;
	}
	if (fseek(fp, 0L, SEEK_END) != 0) {
		fprintf(stderr, "Can't get file size\n");
		return 1;
	}
	*bufferSize = ftell(fp);
	if (fseek(fp, 0L, SEEK_SET) != 0) {
		fprintf(stderr, "Can't reset file to beginning.\n");
		return 1;
	}

	*buffer = calloc(*bufferSize + 1, sizeof(**buffer));
	if (*buffer == NULL) {
		fprintf(stderr, "Error allocating memory.\n");
		return 1;
	}

	fread(*buffer, sizeof(**buffer), *bufferSize, fp);
	fclose(fp);	
	return 0;
}

int setResponse(char *filename, char **response, int status, ssize_t mimeTypeIndex,
		int clientSocket,
		LogData *log, int mode, size_t *responseLen)
{
	char *body = NULL;
	size_t bodyLen;
	if(setBody(&body, filename, &bodyLen) != 0) {
		free(body);
		free(filename);
		errorHandler(NOT_FOUND, "Not found", clientSocket);
		return 1;
	}
	log->size = bodyLen;
	char *header = NULL;
	setHeader(&header, status, mimeTypeIndex, bodyLen); 
	*response = calloc(strlen(header) + bodyLen + 1, sizeof(**response));
	strcpy(*response, header);
	*responseLen = strlen(header);
	if(mode)
	{
		memcpy(*response + *responseLen, body, bodyLen);
		*responseLen += bodyLen;
	}
	free(body);
	free(header);
	return 0;
}

int setHeader(char **header, int status, ssize_t mimeTypeIndex, size_t bodyLength)
{
	char *template =
		"HTTP/1.1 %d %s\n"
		"Content-Type:%s; charset=utf-8\n"
		"Content-Length: %lu\n"
		"Connection: close\r\n\n";

	char *statusString = NULL;
	setStatusString(&statusString, status);
	char *fileType = extensions[mimeTypeIndex].filetype;

	size_t headerLength = snprintf(NULL, 0, template, status, statusString, fileType, bodyLength);
	*header = calloc(headerLength + 1, sizeof(**header));
	sprintf(*header, template, status, statusString, fileType, bodyLength);
	
	free(statusString);
	return 0;
}

int setBody(char **body, char filename[], size_t *bodyLen)
{
	return stringFromFile(filename, body, bodyLen); 
}

int setStatusString(char **statusString, enum statusCode s)
{
	char *tmp;
	size_t nChars;
	switch(s) {
		case ERROR:
			tmp = "ERROR";
			break;
		case OK:
			tmp = "OK";
			break;
		case FORBIDDEN:
			tmp = "FORBIDDEN";
			break;
		case NOT_FOUND:
		       	tmp = "NOT FOUND";
			break;
		default:
			tmp = "UNKNOWN";
	}
	nChars = strlen(tmp);
	*statusString = calloc(nChars + 1, sizeof(**statusString));
	strcpy(*statusString, tmp);
	return 0;
}

ssize_t fileTypeAllowed(char *filename)
{
	size_t extLen = 0;
	ssize_t index = -1;
	const char *filenameExtension = strrchr(filename, '.');
	if (!filenameExtension) {
		return -1;
	}

	for(size_t i = 0; extensions[i].ext != 0; i++) {
		extLen = strlen(extensions[i].ext);
		if(!strncmp(filenameExtension + 1, extensions[i].ext, extLen)) {
			index = i;
			break;
		}
	}
	return index;
}

void timestamp(char **str)
{
	time_t now = time(0);
	struct tm *gtm = gmtime(&now);
	char timeBuffer[MAX_TIME_CHARS] = {0};
	size_t n = strftime(timeBuffer, MAX_TIME_CHARS, "%d/%b/%G:%T %Z", gtm);
	if (n == 0) {
		errorHandler(ERROR, "strftime() error", 0);	
		return;
	}
	*str = calloc(n + 1, sizeof(**str));
	strcpy(*str, timeBuffer);
}

ssize_t firstLine(char *str, char **line)
{
	ssize_t n = -1;
	n = strcspn(str, "\n");
	*line = calloc(n + 1, sizeof(**line));
	if (*line == NULL) {
		fprintf(stderr, "calloc() failed.");
		return n;
	}
	strncpy(*line, str, n - 1);
	return n;
}


#ifndef __SIGN_H__
#define __SIGN_H__

#define _CRT_SECURE_NO_WARNINGS

#include "shalib.h"
#include "rsa.h"
#include "sha.h"

int verify(char *message, int len, char *n, char *e, char *signature);
char * sign(char *message, int len, char *n, char *d);

#endif

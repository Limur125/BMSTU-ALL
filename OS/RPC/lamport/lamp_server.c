/*
 * This is sample code generated by rpcgen.
 * These are only templates and you can use them
 * as a guideline for developing your own functions.
 */

#include "lamp.h"

struct BAKERY *
bakery_proc_1_svc(struct BAKERY *argp, struct svc_req *rqstp)
{
	static struct BAKERY  result;

	/*
	 * insert server code here
	 */

	return &result;
}
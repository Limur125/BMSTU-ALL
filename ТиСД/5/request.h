#ifndef _REQUEST_H_
#define _REQUEST_H_

typedef struct
{
    double stime;
    double wtime;
    double etime;
} request_t;

request_t create_req(double ea, double eb, double sa, double sb);

#endif
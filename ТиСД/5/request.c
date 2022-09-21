#include <stdlib.h>
#include "request.h"

request_t create_req(double ea, double eb, double sa, double sb)
{
    request_t r;
    r.etime = (double) rand() / RAND_MAX * (eb - ea) + ea;
    r.stime = (double) rand() / RAND_MAX * (sb - sa) + sa;
    r.wtime = 0;
    return r;
}
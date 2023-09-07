/*
 * filename: calculator.x
     * function: Define constants, non-standard data types and the calling process in remote calls
 */
/* #include <pthread.h> */
const GET_NUMBER = 0;
const ENTER_CR = 1;

struct BAKERY
{
    int op;
    void* tid;
    int arg;
    int arg2;
    int arg3;
    int result;
};

program BAKERY_PROG
{
    version BAKERY_VER
    {
        struct BAKERY BAKERY_PROC(struct BAKERY) = 1;
    } = 1; /* Version number = 1 */
} = 0x20000001; /* RPC program number */

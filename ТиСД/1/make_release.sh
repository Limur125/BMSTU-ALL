#!/bin/bash
keys="gcc -std=c99 -Wall -Werror -Wextra -Wpedantic -Wfloat-conversion -Wfloat-equal -Wvla -O2 -c"
$keys main.c
$keys words.c
$keys long_num.c
gcc main.o words.o long_num.o -o app.exe 
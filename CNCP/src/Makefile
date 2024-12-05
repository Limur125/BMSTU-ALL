CFLAGS = -std=gnu99 -fPIE -W -Wall -pedantic -g -O3
CC = gcc ${CFLAGS}
BIN = bin
NAME = http-server

$(NAME): ${BIN}/main.o ${BIN}/server.o ${BIN}/errors.o ${BIN}/string-utilities.o ${BIN}/thread_pool.o
	$(CC) -o ${BIN}/$@ $^

${BIN}/main.o: main.c server.h string-utilities.h thread_pool.h errors.h
	$(CC) -c -o $@ $<

${BIN}/%.o: %.c %.h
	$(CC) -c -o $@ $<

.PHONY: clean
clean:
	-rm ${BIN}/$(NAME) ${BIN}/*.o




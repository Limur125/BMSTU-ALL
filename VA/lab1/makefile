CC := gcc
FLAGS := -std=c99 -Wall -Werror -Wextra -Wpedantic -Wfloat-conversion -Wfloat-equal -pedantic -Wvla
SRCS := $(wildcard *.c)

.PHONY: clean unit release

app.exe: main.o
	$(CC) -o $@ $^

%.o: %.c
	$(CC) $(FLAGS) -c $< -o $@

%.d: %.c
	$(CC) -MM $< > $@ 

DEPSRC := $(patsubst %.c,%.d,$(SRCS))

include $(DEPSRC)

clean:
	rm -f *.exe *.o *.d

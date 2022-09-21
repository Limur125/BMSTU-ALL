CC := gcc
FLAGS := -std=c99 -Wall -Werror -Wextra -Wpedantic -Wfloat-conversion -Wfloat-equal -pedantic -Wvla -O0
SRCS := $(wildcard *.c)

app.exe: main.o queue_a.o request.o queue_l.o
	$(CC) -o $@ $^

%.o: %.c
	$(CC) $(FLAGS) -c $< -o $@

%.d: %.c
	$(CC) -M $< > $@

DEP := $(SRCS:.c=.d)

include $(DEP)

.PHONY: clean

clean:
	rm -f *.exe *.o *.gcov *.gcno *.gcda *.d


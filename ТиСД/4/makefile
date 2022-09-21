CC := gcc
FLAGS := -std=c99 -Wall -Werror -Wextra -Wpedantic -Wfloat-conversion -Wfloat-equal -pedantic -Wvla -O0
DEP := $(wildcard *.d)

app.exe: main.o stack_a.o stack_l.o
	$(CC) -o $@ $^

%.o: %.c
	$(CC) $(FLAGS) -c $< -o $@

%.d: %.c
	$(CC) -M $< > $@

include $(DEP)


.PHONY: clean

clean:
	rm -f *.exe *.o *.gcov *.gcno *.gcda *.d


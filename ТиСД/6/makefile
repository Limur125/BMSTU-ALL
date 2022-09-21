CC := gcc
FLAGS := -std=c99 -Wall -Werror -Wextra -Wpedantic -Wfloat-conversion -Wfloat-equal -pedantic -Wvla
SRCS := $(wildcard *.c)

app.exe: main.o tree.o hash.o
	$(CC) -o $@ $^

%.o: %.c
	$(CC) $(FLAGS) -c $< -o $@

%.d: %.c
	$(CC) -MM $< > $@


DEP := $(patsubst %.c,%.d,$(SRCS))

include $(DEP)

.PHONY: clean func unit debug release

clean:
	rm -f *.exe *.o *.d

func: debug
	./func_tests/all_test.sh
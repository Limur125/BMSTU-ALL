CC := gcc
FLAGS := -std=c99 -Wall -Werror -Wextra -Wpedantic -Wfloat-conversion -Wfloat-equal -pedantic -Wvla -O0


app.exe: main.o phone.o sorts.o
	$(CC) -o $@ $^
phone.o: phone.c phone.h sorts.h
	$(CC) $(FLAGS) -c $< -o $@
main.o: main.c phone.h sorts.h
	$(CC) $(FLAGS) -c $< -o $@
sorts.o: sorts.c sorts.h
	$(CC) $(FLAGS) -c $< -o $@

.PHONY: clean

clean:
	rm -f *.exe *.o


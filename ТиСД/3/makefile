CC := gcc
FLAGS := -std=c99 -Wall -Werror -Wextra -Wpedantic -Wfloat-conversion -Wfloat-equal -pedantic -Wvla -O0


app.exe: main.o matrix.o sparse.o
	$(CC) -o $@ $^
sparse.o: sparse.c sparse.h
	$(CC) $(FLAGS) -c $< -o $@
main.o: main.c matrix.h sparse.h
	$(CC) $(FLAGS) -c $< -o $@
matrix.o: matrix.c matrix.h
	$(CC) $(FLAGS) -c $< -o $@

.PHONY: clean

clean:
	rm -f *.exe *.o *.gcov *.gcno *.gcda


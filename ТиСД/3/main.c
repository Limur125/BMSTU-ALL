#include "matrix.h"
#include "sparse.h"
int main(int argc, char **argv)
{
    setbuf(stdout, NULL);
    int choice = 3;
    if (argc != 2)
    {
        puts("1. Сложение матриц в классическом виде");
        puts("2. Сложение матриц в разреженном виде");
        puts("3. Сравнение сложения матриц в разных видах (может работать долго)");
        puts("Выберите пункт меню:");
        if (scanf("%d", &choice) != 1)
            return EXIT_FAILURE;
    }
    switch (choice)
    {
    case 1:
    {
        process_m(argc, argv, 1);
        return EXIT_SUCCESS;
    }
    case 2:
    {
        process_s(argc, argv, 1);
        return EXIT_SUCCESS;
    }
    case 3:
    {
        process_m(argc, argv, 10);
        fflush(stdin);
        process_s(argc, argv, 10);
        return EXIT_SUCCESS;
    }
    default:
        EXIT_FAILURE;
    }
}
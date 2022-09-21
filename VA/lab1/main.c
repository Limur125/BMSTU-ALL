#include <stdio.h>
#include <math.h>
#include <stdlib.h>

#define MAX_POINTS_COUNT 50
#define COL_COUNT 3

int parse_table(double matr[][COL_COUNT], int *row, FILE *f)
{
    if (fscanf(f, "%d", row) != 1 && *row > MAX_POINTS_COUNT)
        return EXIT_FAILURE;
    for (int i = 0; i < *row; i++)
        for (int j = 0; j < COL_COUNT; j++)
            if (fscanf(f, "%lf", &(matr[i][j])) != 1)
                return EXIT_FAILURE;
    return EXIT_SUCCESS;
}

void split_matr(double matr[][COL_COUNT], int row, double x[], double y[], double d[])
{
    for (int i = 0; i < row; i++)
    {
        x[i] = matr[i][0];
        y[i] = matr[i][1];
        d[i] = matr[i][2];
    }
}

double input_x(void)
{
    printf("Enter X: \n");
    int flag = 0;
    double x;
    while (flag == 0)
        if (scanf("%lf", &x) == 1)
            flag = 1;
        else
            printf("Some error! Try again");
    return x;
}

int strip_arr(double src[], double dst[], int i_b, int i_e)
{
    int dst_len = 0;
    for (int i = i_b; i < i_e; i++)
        dst[dst_len++] = src[i];
    return dst_len;
}

void find_x0_xn(double arr[], int row, int power, double arg, int *ind_x0, int *ind_xn)
{
    int index_x = 0;
    while (arg > arr[index_x++]);
    *ind_x0 = index_x - power / 2 - 1;
    *ind_xn = index_x + (power / 2) + (power % 2) - 1;
    if (*ind_xn > row - 1)
    {
        *ind_x0 -= *ind_xn - row + 1;
        *ind_xn = row - 1;
    }
    else if (*ind_x0 < 0)
    {
        *ind_xn -= *ind_x0;
        *ind_x0 = 0;
    }
}

void div_diff(double x[], double y[], int node, double *poly[], int *poly_n)
{
    *poly_n = 0;
    for (int i = 0; i < node; i++)
        poly[(*poly_n)++] = calloc(node + 1, sizeof(double));
    for (int i = 0; i < node; i++)
    {
        poly[i][0] = x[i];
        poly[i][1] = y[i];
    }
    int i = 2;
    int new_node = node - 1;
    while (i < (node + 1))
    {
        int j = 0;
        while (j < new_node)
        {
            poly[j][i] = (poly[j + 1][i - 1] - poly[j][i - 1]) / (poly[i - 1][0] - poly[0][0]);
            j++;
        }
        i++;
        new_node--;
    }
}

double polinom_n(double x[], double y[], int node, double arg)
{
    double *poly[MAX_POINTS_COUNT];
    int poly_n;
    div_diff(x, y, node, poly, &poly_n);
    double arg_y = poly[0][1];
    int i = 2;
    while (i < node + 1)
    {
        int j = 0;
        double p = 1;
        while (j < i - 1)
            p *= (arg - poly[j++][0]);
        arg_y += poly[0][i++] * p;
    }
    return arg_y;
}

void hermite_interpolate(int node, double x[], double y[], double d[], int n, double *poly[], int *poly_n)
{
    *poly_n = 0;
    for (int i = 0; i < 2 * n; i++)
        poly[(*poly_n)++] = calloc(2 * node + 3, sizeof(double));
    int i = 0;
    for (int j = 0; j < n; j++)
    {
        poly[i][0] = x[j];
        poly[i][1] = y[j];
        poly[i++][2] = d[j];
        poly[i][0] = x[j];
        poly[i++][1] = y[j];
    }
    i = 2;
    for (int j = 0; j < (*poly_n) - 1; j++)
        if (j % 2 == 1)
            poly[j][i] = (poly[j][1] - poly[j + 1][1]) / (poly[j][0] - poly[j + 1][0]);
    i = 3;
    int new_node = node - 2;
    while (i < *poly_n)
    {
        int j = 0;
        while (j < new_node)
        {
            poly[j][i] = (poly[j + 1][i - 1] - poly[j][i - 1]) / (poly[i - 1][0] - poly[0][0]);
            j++;
        }
        i++;
        new_node--;
    }
}

double polynom_h(double *poly[], int node, double arg)
{
    double y = poly[0][1];
    int i = 2;
    while (i < node + 2)
    {
        int j = 0;
        double p = 1;
        while (j < i - 1)
            p *= (arg - poly[j++][0]);
        y += poly[0][i++] * p;
    }
    return y;
}

void bubblesort(double a[], int n, double b[])
{
    int f = 1;
    while (f)
    {
        f = 0;
        for (int i = 0; i < n - 1; i++)
            if (a[i] > a[i + 1])
            {
                f = 1;
                double tmp = a[i];
                a[i] = a[i + 1];
                a[i + 1] = tmp;
                tmp = b[i];
                b[i] = b[i + 1];
                b[i + 1] = tmp;
            }
    }
}

int main(int argc, char **argv)
{
    setbuf(stdout, NULL);
    if (argc != 2)
        return EXIT_FAILURE;
    FILE *filein = fopen(argv[1], "r");
    if (filein == NULL)
        return EXIT_FAILURE;

    double a[MAX_POINTS_COUNT][COL_COUNT];
    int row;
    double x = input_x();
    parse_table(a, &row, filein);
    double coords_x[MAX_POINTS_COUNT], coords_y[MAX_POINTS_COUNT], coords_d[MAX_POINTS_COUNT];
    split_matr(a, row, coords_x, coords_y, coords_d);
    for (int i = 0; i < row; i++)
    {
        for (int j = 0; j < COL_COUNT; j++)
            printf("%lf ", a[i][j]);
        printf("\n");
    }

    printf("\nИнтерполяция с помощью полинома Ньютона и Эрмита\n"
          "| n |    x    | п. Ньютона | п. Эрмита |\n");

    for (int n = 0; n < 5; n++)
    {
        printf("| %d | %.5lf |", n, x);
        int flag = 0;
        for (int i = 0; i < row; i++)
            if (fabs(x - a[i][0]) < 1e-8)
            {
                printf("   %.5lf  |", a[i][1]);
                flag = 1;
            }

        if (!flag)
        {
            int x0, xn;
            find_x0_xn(coords_x, row, n, x, &x0, &xn);
            double ax[MAX_POINTS_COUNT], ay[MAX_POINTS_COUNT], ad[MAX_POINTS_COUNT];
            int ax_n = strip_arr(coords_x, ax, x0, xn + 1);
            strip_arr(coords_y, ay, x0, xn + 1);
            strip_arr(coords_d, ad, x0, xn + 1);
            if (ax_n)
            {
                double my_root = polinom_n(ax, ay, n + 1, x);
                printf("   %.5lf  |", my_root);
            }
            double *poly[2 * MAX_POINTS_COUNT];
            int poly_n;
            hermite_interpolate(n, ax, ay, ad, ax_n, poly, &poly_n);
            double my_root2 = polynom_h(poly, n, x);
            printf("  %.5lf  |\n", my_root2);
        }
    }
    printf("\nОбратная интерполяция с помощью полинома Ньютона\n"
          "| n |     x     |   Корень   |\n");
    for (int n = 0; n < 5; n++)
    {
        printf("| %d |  %.5lf  |", n, 0.0);
        int flag = 0;
        for (int i = 0; i < row; i++)
            if (fabs(a[i][1]) < 1e-8)
            {
                printf("  %lf |\n", a[i][1]);
                flag = 1;
            }
        if (!flag)
        {
            int y0, yn;
            bubblesort(coords_y, row, coords_x);
            find_x0_xn(coords_y, row, n, 0.0, &y0, &yn);
            double ax[MAX_POINTS_COUNT], ay[MAX_POINTS_COUNT];
            int ay_n = strip_arr(coords_y, ay, y0, yn + 1);
            strip_arr(coords_x, ax, y0, yn + 1);
            if (ay_n)
            {
                double my_root = polinom_n(ay, ax, n + 1, 0.0);
                printf("   %.5lf  |\n", my_root);
            }
        }
    }
    fclose(filein);
    return EXIT_SUCCESS;
}

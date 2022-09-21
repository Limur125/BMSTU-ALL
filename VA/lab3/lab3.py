def div_diff(z, node):
    for i in range(node):
        pol = []
        for j in range(node - i):
            buf = (z[i + 1][j] - z[i + 1][j + 1]) / (z[0][j] - z[0][j + i + 1])
            pol.append(buf)
        z.append(pol)

    return z


def polinom_n(z, node, arg):
    pol = div_diff(z, node)
    y = 0
    buf = 1
    for i in range(node + 1):
        y += buf * pol[i + 1][0]
        buf *= (arg - pol[0][i])

    return y

def polinom_sec_der_3n(z, arg):
    pol = div_diff(z, 3)
    y = 2 * (pol[2][0] + 3 * pol[3][0] * (3 * arg - pol[0][0] - pol[0][1] - pol[0][2]))
    # print(y)
    return y 

def run_method(count, arr_Hn, y, c0, cn):
    arr_A = [0 for i in range(count)]
    arr_B = [0 for i in range(count)]
    arr_D = [0 for i in range(count)]
    arr_F = [0 for i in range(count)]
    arr_C = [0 for i in range(count)]
    arr_Kci = [0 for i in range(count)]
    arr_Etta = [0 for i in range(count)]

    arr_C[1] = c0
    arr_C[-1] = cn
    arr_Etta[2] = c0

    for i in range(2, count - 1):
        arr_A[i] = arr_Hn[i - 1]
        arr_D[i] = arr_Hn[i]

        arr_B[i] = 2 * (arr_A[i] + arr_D[i])

        arr_F[i] = 3 * ((y[i] - y[i - 1]) / arr_Hn[i] - (y[i - 1] - y[i - 2]) / arr_Hn[i - 1])

        arr_Kci[i + 1] = -arr_D[i] / (arr_B[i] + arr_A[i] * arr_Kci[i])
        arr_Etta[i + 1] = (arr_F[i] - arr_A[i] * arr_Etta[i]) / (arr_B[i] + arr_A[i] * arr_Kci[i])

    for i in range(count - 2, 0, -1):
        arr_C[i] = arr_Kci[i+ 1] * arr_C[i + 1] + arr_Etta[i + 1]

    return arr_C

def interp_spline(x, y, x_arg, c0=0, cn=0):
    arr_a = [0 for i in range(len(x))]
    arr_b = [0 for i in range(len(x))]
    arr_d = [0 for i in range(len(x))]
    arr_Hn = [0 for i in range(len(x))]

    for i in range(1, len(x)):
        arr_Hn[i] = x[i] - x[i - 1]

    arr_c = run_method(len(x), arr_Hn, y, c0, cn) + [0]

    for i in range(len(x) - 1, 0, -1):
        arr_a[i] = y[i - 1]
        arr_d[i] = (arr_c[i + 1] - arr_c[i]) / (3 * arr_Hn[i])
        arr_b[i] = (y[i] - y[i - 1]) / arr_Hn[i] - (arr_Hn[i] / 3) * (arr_c[i + 1] + 2 * arr_c[i])

    # print(x)
    # print(y)
    # print(arr_a)
    # print(arr_b)
    # print(arr_c)
    # print(arr_d)
    
    found_ix = found_in_x(x_arg, x)

    x1 = x_arg - x[found_ix - 1]
    x2 = x1 * x1
    x3 = x1 * x1 * x1

    result = arr_a[found_ix] + arr_b[found_ix] * x1 + arr_c[found_ix] * x2 + arr_d[found_ix] * x3

    return result


def found_in_x(x, table):
    find = 0
    for i in range(len(table)):
        if (x < table[i]):
            find = i
            break
    return find

x_arr = []
y_arr = []
with open('C:\msys64\home\zolot\VA\lab3\in.txt', 'r') as f:
    for line in f:
        x, y = map(float, line.split())
        x_arr.append(x)
        y_arr.append(y)
    
x_arg = float(input("Введите число х:"))

res = interp_spline(x_arr, y_arr, x_arg)
print("Для заданного значения X ({}) соответствует следующее значение Y = {}\n\n".format(x_arg, res))

psd0=polinom_sec_der_3n([x_arr, y_arr], x_arr[0])
psd1=polinom_sec_der_3n([x_arr, y_arr], x_arr[-1])

res = interp_spline(x_arr, y_arr, x_arg, c0=psd0)
print("Для заданного значения X ({}) соответствует следующее значение Y = {}\n\n".format(x_arg, res))

res = interp_spline(x_arr, y_arr, x_arg, psd0, psd1)
print("Для заданного значения X ({}) соответствует следующее значение Y = {}\n\n".format(x_arg, res))

res = polinom_n([x_arr, y_arr], 3, x_arg)
print("Для заданного значения X ({}) соответствует следующее значение Y = {}\n\n".format(x_arg, res))


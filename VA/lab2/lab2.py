import math

def F(x, y, z):
    '''
        Функция, используемая в программе.
    '''
    return x ** 2 + y ** 2 + z ** 2


def create_table():
    '''
        Загрузка таблицы в программу.
    '''
    table = [[[0, 1, 4, 9, 16], [1, 2, 5, 10, 17], [4, 5, 8, 13, 20], [9, 10, 13, 18, 25], [16, 17, 20, 25, 32]], \
             [[1, 2, 5, 10, 17], [2, 3, 6, 11, 18], [5, 6, 9, 14, 21], [10, 11, 14, 19, 26], [17, 18, 21, 26, 33]], \
             [[4, 5, 8, 13, 20], [5, 6, 9, 14, 21], [8, 9, 12, 17, 24], [13, 14, 17, 22, 29], [20, 21, 24, 29, 36]], \
             [[9, 10, 13, 18, 25], [10, 11, 14, 19, 26], [13, 14, 17, 22, 29], [18, 19, 22, 27, 34], [25, 26, 29, 34, 41]], \
             [[16, 17, 20, 25, 32], [17, 18, 21, 26, 33], [20, 21, 24, 29, 36], [25, 26, 29, 34, 41], [32, 33, 36, 41, 48]]]
    # table = []
    # for i in range(5):
    #     j_t = []
    #     for j in range(5):
    #         k_t = []
    #         for k in range(5):
    #             k_t.append(F(i, j, k))
    #         j_t.append(k_t)
    #     table.append(j_t)

    # Разбиение на нужные массивы (X, Y, Z)
    x = [0, 1, 2, 3, 4]
    y = [0, 1, 2, 3, 4]
    z = [0, 1, 2, 3, 4]
    u = table
    return u, z, x, y


def find_x0_xn(data, power, arg):
    '''
        Нахождение начального и конечного индекса в таблице (x/y0 и x/yn).
    '''
   # print("find_x0_xn1")
    index_x = 0
    
    while arg > data[index_x]:
        index_x += 1
        if arg < data[index_x]:
            index_x -= 1
            break

    index_x0 = index_x - math.ceil(power / 2) + 1
    index_xn = index_x + math.ceil(power / 2) + ((power - 1) % 2)

    if index_xn > len(data) - 1:
        index_xn = len(data)
    elif index_x0 < 0:
        index_x0 = 0

    return index_x0, index_xn


def div_diff(z, node):
    '''
        Расчет разделенных разниц для полинома Ньютона
    '''
    for i in range(node):
        pol = []
        for j in range(node - i):
            buf = (z[i + 1][j] - z[i + 1][j + 1]) / (z[0][j] - z[0][j + i + 1])
            pol.append(buf)
        z.append(pol)

    return z


def polinom_n(z, node, arg):
    '''
        Расчет значение функции от заданного аргумента.
        Полином Ньютона.
    '''
    pol = div_diff(z, node)
    y = 0
    buf = 1
    for i in range(node + 1):
        y += buf * pol[i + 1][0]
        buf *= (arg - pol[0][i])

    return y 

def double_inter(z, x , y, node_x, node_y, arg_x, arg_y):
    y1 = [polinom_n([x, z[i]], node_x, arg_x) for i in range(node_y + 1)]
    z1 = polinom_n([y, y1], node_y, arg_y)
    return z1

def multid_interp(u, x, y, z, power_x, power_y, power_z, arg_x, arg_y, arg_z):
    '''
        Алгоритм многомерной интерполяции.
    '''
    index_x0, index_xn = find_x0_xn(x, power_x + 1, arg_x)
    index_y0, index_yn = find_x0_xn(y, power_y + 1, arg_y)
    index_z0, index_zn = find_x0_xn(z, power_z + 1, arg_z)

    x = x[index_x0:index_xn]
    y = y[index_y0:index_yn]
    z = z[index_z0:index_zn]
    u = u[index_z0:index_zn]

    for i in range(power_z + 1):
        u[i] = u[i][index_y0:index_yn]
    for i in range(power_z + 1):
        for j in range(power_y + 1):
            u[i][j] = u[i][j][index_x0:index_xn]


    y1 = [double_inter(u[i], x, y, power_y, power_x, arg_y, arg_x) for i in range(power_z + 1)]
    z1 = polinom_n([z, y1], power_z, arg_z)

    return z1


def input_x(str):
    '''
        Ввод аргумента. (в случае ошибки дается еще попытка)
    '''
    print(str)
    flag = 0
    x = 0
    while flag == 0:
        x = input()
        try:
            val = float(x)
            flag = 1
        except ValueError:
            print("Some error! Try again")
    return val


def main():
    u, x, y, z = create_table()
    arg_x = input_x('Enter X: ')
    arg_y = input_x('Enter Y: ')
    arg_z = input_x('Enter Z: ')
    arr_n = [1, 2, 3]

    print("\n|  nx  |  ny  |  nz  |   x   |   y   |   z   | Found U | F(x, y, z) | Error  |")
    for n in arr_n:
        found_u = multid_interp(u, x, y, z, n, n, n, arg_x, arg_y, arg_z)
        
        print("|   %d  |   %d  |   %d  |  %.2f |  %.2f |  %.2f | %.5f | %.5f | %.4f |" \
            % (n, n, n, arg_x, arg_y, arg_z, found_u, F(arg_x, arg_y, arg_z), 
            abs(found_u - F(arg_x, arg_y, arg_z))))


if __name__ == "__main__":
    main()
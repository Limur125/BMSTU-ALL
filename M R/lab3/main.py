from statistics import median
from random import Random
from prettytable import PrettyTable
from math import sqrt

N_RANDOMS = 10000
eps = 1e-6
N_OUTPUT = 15

numbers_limits = {1: [0, 9], 2: [10, 99], 3: [100, 999]}

n20table = [
    [ (1, 6), (1, 6), (1, 6), (1, 6), (1, 6), (1, 6), (1, 6), (1, 6), (1, 6), (1, 6), (2, 6), (2, 6), (2, 6), (2, 6), (2, 6), (2, 6), (2, 6), (2, 6), (2, 6) ],
    [ (1, 6), (1, 8), (1, 8), (1, 8), (2, 8), (2, 8), (2, 8), (2, 8), (2, 8), (2, 8), (2, 8), (2, 8), (2, 8), (3, 8), (3, 8), (3, 8), (3, 8), (3, 8), (3, 8) ],
    [ (1, 6), (1, 8), (1, 9), (2, 9), (2, 9), (2, 10), (3, 10), (3, 10), (3, 10), (3, 10), (3, 10), (3, 10), (3, 10), (3, 10), (4, 10), (4, 10), (4, 10), (4, 10), (4, 10) ],
    [ (1, 6), (1, 8), (2, 9), (2, 10), (3, 10), (3, 11), (3, 11), (3, 12), (3, 12), (4, 12), (4, 12), (4, 12), (4, 12), (4, 12), (4, 12), (4, 12), (5, 12), (5, 12), (5, 12) ],
    [ (1, 6), (2, 8), (2, 9), (3, 10), (3, 11), (3, 12), (3, 12), (4, 13), (4, 13), (4, 13), (4, 13), (5, 14), (5, 14), (5, 14), (5, 14), (5, 14), (5, 14), (6, 14), (6, 14) ],
    [ (1, 6), (2, 8), (2, 10), (3, 11), (3, 13), (4, 13), (4, 14), (4, 14), (5, 14), (5, 14), (5, 14), (5, 15), (5, 15), (6, 15), (6, 16), (6, 16), (6, 16), (6, 16), (6, 16) ],
    [ (1, 6), (2, 8), (3, 10), (3, 11), (3, 12), (4, 14), (4, 14), (5, 14), (5, 15), (5, 15), (6, 16), (6, 16), (6, 16), (6, 16), (6, 17), (7, 17), (7, 17), (7, 17), (7, 17) ],
    [ (1, 6), (2, 8), (3, 10), (3, 12), (4, 13), (4, 14), (5, 14), (5, 15), (5, 16), (6, 16), (6, 16), (6, 17), (7, 17), (7, 18), (7, 18), (7, 18), (8, 18), (8, 18), (8, 18) ],
    [ (1, 6), (2, 8), (3, 10), (3, 12), (4, 13), (5, 14), (5, 15), (5, 16), (6, 16), (6, 17), (7, 17), (7, 18), (7, 18), (7, 18), (8, 19), (8, 19), (8, 19), (8, 20), (8, 20) ],
    [ (1, 6), (2, 8), (3, 10), (4, 12), (4, 13), (5, 14), (5, 15), (6, 16), (6, 17), (7, 17), (7, 18), (7, 19), (8, 19), (8, 19), (8, 20), (9, 20), (9, 20), (9, 21), (9, 21) ],
    [ (2, 6), (2, 8), (3, 10), (4, 12), (4, 13), (5, 14), (6, 16), (6, 16), (7, 17), (7, 18), (7, 19), (8, 19), (8, 20), (8, 20), (9, 21), (9, 20), (9, 21), (10, 22), (10, 22)],
    [ (2, 6), (2, 8), (3, 10), (4, 12), (5, 14), (5, 15), (6, 16), (6, 17), (7, 18), (7, 19), (8, 19), (8, 20), (9, 20), (9, 21), (9, 21), (10, 22), (10, 22), (10, 23), (10, 23)],
    [ (2, 6), (2, 8), (3, 10), (4, 12), (5, 14), (5, 15), (6, 16), (7, 17), (7, 18), (8, 19), (8, 20), (9, 20), (9, 21), (9, 22), (10, 22), (10, 23), (10, 23), (11, 23), (11, 24)],
    [ (2, 6), (3, 8), (3, 10), (4, 12), (5, 14), (6, 15), (6, 16), (7, 18), (7, 18), (8, 19), (8, 20), (9, 21), (9, 22), (10, 22), (10, 23), (11, 23), (11, 23), (11, 24), (12, 25)],
    [ (2, 6), (3, 8), (4, 10), (4, 12), (5, 14), (6, 16), (6, 17), (7, 18), (8, 19), (8, 20), (9, 21), (9, 21), (10, 22), (10, 23), (11, 23), (11, 24), (11, 25), (12, 25), (12, 25)],
    [ (2, 6), (3, 8), (4, 10), (4, 12), (5, 14), (6, 16), (7, 17), (7, 18), (8, 19), (9, 20), (9, 21), (10, 22), (10, 23), (11, 23), (11, 24), (11, 25), (12, 25), (12, 26), (13, 26)],
    [ (2, 6), (3, 8), (4, 10), (5, 12), (5, 14), (6, 16), (7, 17), (8, 18), (8, 19), (9, 20), (9, 21), (10, 22), (10, 23), (11, 23), (11, 25), (12, 25), (12, 26), (13, 26), (13, 27)],
    [ (2, 6), (3, 8), (4, 10), (5, 12), (6, 14), (6, 16), (7, 17), (8, 18), (8, 20), (9, 21), (10, 22), (10, 23), (11, 23), (11, 24), (12, 25), (12, 26), (13, 26), (13, 27), (13, 27)],
    [ (2, 6), (3, 8), (4, 10), (5, 12), (6, 14), (6, 16), (7, 17), (8, 18), (8, 20), (9, 21), (10, 22), (10, 23), (11, 24), (12, 25), (12, 25), (13, 26), (13, 27), (13, 27), (13, 28)]
]



normal_quantiles = {1: -2.326, 5: -1.645, 95: 1.645, 99: 2.326}

def random_from_table():
    with open('random_ints.txt') as file:
        lines = file.readlines()

    numbers = list()
    for line in lines:
        numbers.extend(list(map(int, line.strip().split())))
    numbers = numbers[:N_RANDOMS]

    one = [number % 10 for number in numbers]
    two = [10 + number % 90 for number in numbers]
    three = [100 + number % 900 for number in numbers]

    return one, two, three

current = 3
def my_random(low:int, high:int):
    global current
    m = 0xa49bf943;
    a = 3480753743;
    c = 1958496823;

    current = (a * current + c) % m;
    result = int(low + current % (high - low));
    return result;

def random_from_alg():
    one = [my_random(*numbers_limits[1]) for _ in range(N_RANDOMS)]
    two = [my_random(*numbers_limits[2]) for _ in range(N_RANDOMS)]
    three = [my_random(*numbers_limits[3]) for _ in range(N_RANDOMS)]
    return one, two, three


def calc_coef(random_numbers: list[int]):
    random_numbers_no_dup = list(set(random_numbers))
    random_numbers_no_dup.sort()
    m = median(random_numbers_no_dup)
    signs = []
    for n in random_numbers:
        if n < m:
            signs.append("-")
        elif n > m:
            signs.append("+")
    prev = 0
    n1 = 0
    n2 = 0
    n = 0
    for s in signs:
        if s != prev:
            n += 1
        if s == "+":
            n1 += 1
        if s == "-":
            n2 += 1
        prev = s
    if max(n1, n2) > 20:
        z = (abs(n - (2 * n1 * n2) / (n1 + n2) - 1) - 0.5)/(sqrt((2 * n1 * n2 * (2 * n1 * n2 - (n1 + n2)))/((n1 + n2) * (n1 + n2) * (n1 + n2 + 1))))
        if (z <= normal_quantiles[1] or z >= normal_quantiles[99]):
            return z, "max(n1, n2) > 20 z = ", "Числа не случайные"
        else:
            return z, "max(n1, n2) > 20 z = ", "Числа случайные"
    else:
        N1, N2 = n20table[n2 - 2][n1 - 2] 
        if (n <= N1 or n >= N2):
            return n, "max(n1, n2) <= 20 N = ", "Числа не случайные"
        else:
            return n, "max(n1, n2) <= 20 N = ", "Числа случайные"


def check_limit_values():
    tests = [
        [1, list(range(10))],
        [1, list(range(9, -1, -1))],
        [1, [1, 3, 1, 3, 1, 3, 1, 3, 1, 3]],
        [2, list(range(10, 100))],#[:N_OUTPUT]],
        [2, list(range(99, 9, -1))],#[:N_OUTPUT]],
        [2, [10, 30, 10, 30, 10, 30, 10, 30, 10, 30]],
        [3, list(range(100, 1000))],#[:N_OUTPUT]],
        [3, list(range(999, 99, -1))],#[:N_OUTPUT]],
        [3, [100, 300, 100, 300, 100, 300, 100, 300, 100, 300]],
    ]
    print('\n\n\nТестирование на предельных значениях')
    for digits, arr in tests:
        print()
        print(f'Разрядность: {digits}')
        print(f'Последовательность: {arr[:min(len(arr), N_OUTPUT)]}')
        coef, cond, anal = calc_coef(arr)
        print(f'Мера случайности: {cond + str(coef)}')
        print(f'Итог: {anal}')


def main():
    indexes = [i for i in range(N_OUTPUT)]

    for alg, alg_name in [[random_from_table, 'Табличный метод'], [random_from_alg, 'Алгоритмический метод']]:
        res_table = PrettyTable()
        one, two, three = alg()
        res_table.add_column("№", indexes + ['Мера случайности', 'Итог'])

        one_coef, one_cond, one_anal = calc_coef(one)
        two_coef, two_cond, two_anal = calc_coef(two)
        three_coef, three_cond, three_anal = calc_coef(three)
        res_table.add_column('1 разряд', one[:N_OUTPUT] + [one_cond+str(one_coef), one_anal])
        res_table.add_column('2 разряда', two[:N_OUTPUT] + [two_cond + str(two_coef), two_anal])
        res_table.add_column('3 разряда', three[:N_OUTPUT] + [three_cond + str(three_coef), three_anal])

        print(f"\t\t\t{alg_name}")
        print(res_table)

    check_limit_values()

    print("\n\n\n")
    print("Введите последовательность чисел через пробел")
    arr = list(map(int, input().split()))
    coef, cond, anal = calc_coef(arr)
    print("Коэффициент: ", cond + str(coef))
    print(anal)


if __name__ == '__main__':
    main()

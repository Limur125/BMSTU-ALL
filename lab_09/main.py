from tkinter import *
from tkinter import messagebox, ttk
import numpy as np
import matplotlib.pyplot as plt
from itertools import combinations

from time import time, sleep

import colorutils as cu

def rgb_to_hex(rgb):
    return f'#{rgb[0]:02x}{rgb[1]:02x}{rgb[2]:02x}'

WIN_WIDTH = 1600
WIN_HEIGHT = 900
WIN_COLOR = 'dark slate gray'

CV_WIDE = 900
CV_HEIGHT = 900
CV_COLOR = "#000000" #rgb_to_hex((253, 245, 230)) #f3e6ff" #"#cce6ff"
MAIN_TEXT_COLOR = "#b566ff" #"lightblue" a94dff
FONT_COLOR = 'linen'

BTN_TEXT_COLOR = "#4d94ff"

LINES = []
FIRST_POINT = []
LOG = []

X_DOT = 0
Y_DOT = 1

win = Tk()
win['bg'] = WIN_COLOR
win.geometry("%dx%d" %(WIN_WIDTH, WIN_HEIGHT))
win.attributes("-fullscreen", True)

canv = Canvas(win, width = CV_WIDE, height = CV_HEIGHT, bg = CV_COLOR)
canv.place(x = 0, y = 0)

canv.delete("all")

image_canvas = PhotoImage(width = CV_WIDE + 100, height = CV_HEIGHT + 100)
canv.create_image((CV_WIDE // 2 + 50, CV_HEIGHT // 2 + 50) , image = image_canvas, state = NORMAL)

is_set_rect = False


def check_option(option):
    messagebox.showinfo("Выбран", "Выбрана опция %d" %(option))


def clear_canvas():
    canv.delete("all")


def get_fill_check_color(collor_fill):
    return (int(collor_fill[1:3], 16), int(collor_fill[3:5], 16), int(collor_fill[5:7], 16))


def reboot_prog():
    global figure
    global cutter

    canv.delete("all")

    cutter = []
    figure = []

def is_maked(object):
    maked = False

    if (len(object) > 3):
        if ((object[0][0] == object[len(object) - 1][0]) and (object[0][1] == object[len(object) - 1][1])):
            maked = True

    return maked



def add_dot_cutter_click(event):
    x = event.x
    y = event.y

    add_dot_cutter(x, y)


def read_dot_cutter():
    try:
        x = float(cutter_x_entry.get())
        y = float(cutter_y_entry.get())
    except:
        messagebox.showerror("Ошибка", "Неверно введены координаты точки")
        return

    add_dot_cutter(int(x), int(y))


def add_dot_cutter(x, y, last = True):
    if (is_maked(cutter)): # для задания нового отсекателя
            cutter.clear()
            canv.delete("all")
            draw_figure()            

    cutter_color = "white"

    cutter.append([x, y])
    cur_dot = len(cutter) - 1

    if (len(cutter) > 1):
        canv.create_line(cutter[cur_dot - 1], cutter[cur_dot], fill = cutter_color)        

def del_dot_cutter():
    if (is_maked()):
        return

    cur_dot = len(cutter) - 1

    if (len(cutter) == 0):
        return

    if (len(cutter) > 1):
        canv.create_line(cutter[cur_dot - 1], cutter[cur_dot], fill = CV_COLOR)

    # Find index for a table
    cutter.pop(len(cutter) - 1)


def make_cutter():
    if (is_maked(cutter)):
        messagebox.showerror("Ошибка", "Фигура уже замкнута")
        return

    cur_dot = len(cutter)

    if (cur_dot < 3):
        messagebox.showerror("Ошибка", "Недостаточно точек, чтобы замкнуть фигуру")
        return

    add_dot_cutter(cutter[0][0], cutter[0][1], last = False)


# Figure

def add_dot_figure_click(event):
    x = event.x
    y = event.y

    add_dot_figure(x, y)


def read_dot_figure():
    try:
        x = float(figure_x_entry.get())
        y = float(figure_y_entry.get())
    except:
        messagebox.showerror("Ошибка", "Неверно введены координаты точки")
        return

    add_dot_figure(int(x), int(y))


def add_dot_figure(x, y, last = True):
    if (is_maked(figure)): # для задания нового отсекаемого
            reboot_prog() # ???
            

    figure_color = "blue"

    figure.append([x, y])
    cur_dot = len(figure) - 1

    if (len(figure) > 1):
        canv.create_line(figure[cur_dot - 1], figure[cur_dot], fill = figure_color)
        

def del_dot_figure():
    if (is_maked(figure)):
        return

    cur_dot = len(figure) - 1

    if (len(figure) == 0):
        return

    if (len(figure) > 1):
        canv.create_line(figure[cur_dot - 1], figure[cur_dot], fill = CV_COLOR)
    figure.pop(len(figure) - 1)


def make_figure():
    if (is_maked(figure)):
        messagebox.showerror("Ошибка", "Фигура уже замкнута")
        return

    cur_dot = len(figure)

    if (cur_dot < 3):
        messagebox.showerror("Ошибка", "Недостаточно точек, чтобы замкнуть фигуру")
        return

    add_dot_figure(figure[0][0], figure[0][1], last = False)


def draw_figure():
    figure_color = "blue"

    canv.create_polygon(figure, outline = figure_color, fill=CV_COLOR)

def get_vector(dot1, dot2):
    return [dot2[X_DOT] - dot1[X_DOT], dot2[Y_DOT] - dot1[Y_DOT]]


def vector_mul(vec1, vec2):
    return (vec1[0] * vec2[1] - vec1[1] * vec2[0])


def scalar_mul(vec1, vec2):
    return (vec1[0] * vec2[0] + vec1[1] * vec2[1])


def line_koefs(x1, y1, x2, y2):
    a = y1 - y2
    b = x2 - x1
    c = x1*y2 - x2*y1

    return a, b, c


def solve_lines_intersection(a1, b1, c1, a2, b2, c2):
    opr = a1*b2 - a2*b1
    opr1 = (-c1)*b2 - b1*(-c2)
    opr2 = a1*(-c2) - (-c1)*a2

    if (opr == 0):
        return -5, -5 # прямые параллельны

    x = opr1 / opr
    y = opr2 / opr

    return x, y


def is_coord_between(left_coord, right_coord, dot_coord):
    return (min(left_coord, right_coord) <= dot_coord) \
            and (max(left_coord, right_coord) >= dot_coord)


def is_dot_between(dot_left, dot_right, dot_intersec):
    return is_coord_between(dot_left[X_DOT], dot_right[X_DOT], dot_intersec[X_DOT]) \
            and is_coord_between(dot_left[Y_DOT], dot_right[Y_DOT], dot_intersec[Y_DOT])


def are_connected_sides(line1, line2):

    if ((line1[0][X_DOT] == line2[0][X_DOT]) and (line1[0][Y_DOT] == line2[0][Y_DOT])) \
            or ((line1[1][X_DOT] == line2[1][X_DOT]) and (line1[1][Y_DOT] == line2[1][Y_DOT])) \
            or ((line1[0][X_DOT] == line2[1][X_DOT]) and (line1[0][Y_DOT] == line2[1][Y_DOT])) \
            or ((line1[1][X_DOT] == line2[0][X_DOT]) and (line1[1][Y_DOT] == line2[0][Y_DOT])):
        return True

    return False


def extra_check(object): # чтобы не было пересечений
    
    lines = []

    for i in range(len(object) - 1):
        lines.append([object[i], object[i + 1]]) # разбиваю многоугольник на линии

    combs_lines = list(combinations(lines, 2)) # все возможные комбинации сторон

    for i in range(len(combs_lines)):
        line1 = combs_lines[i][0]
        line2 = combs_lines[i][1]

        if (are_connected_sides(line1, line2)):
            print("Connected")
            continue

        a1, b1, c1 = line_koefs(line1[0][X_DOT], line1[0][Y_DOT], line1[1][X_DOT], line1[1][Y_DOT])
        a2, b2, c2 = line_koefs(line2[0][X_DOT], line2[0][Y_DOT], line2[1][X_DOT], line2[1][Y_DOT])

        dot_intersec = solve_lines_intersection(a1, b1, c1, a2, b2, c2)

        if (is_dot_between(line1[0], line1[1], dot_intersec)) \
                and (is_dot_between(line2[0], line2[1], dot_intersec)):
            return True

    return False


def check_polygon(): # через проход по всем точкам, поворот которых должен быть все время в одну сторону
    if (len(cutter) < 3):
        return False

    sign = 0

    if (vector_mul(get_vector(cutter[1], cutter[2]), get_vector(cutter[0], cutter[1])) > 0):
        sign = 1
    else:
        sign = -1

    for i in range(3, len(cutter)):
        if sign * vector_mul(get_vector(cutter[i - 1], cutter[i]), get_vector(cutter[i - 2], cutter[i - 1])) < 0:
            return False

    check = extra_check(cutter)

    print("\n\nResult:", check, "\n\n")

    if (check):
        return False

    if (sign < 0):
        cutter.reverse()

    return True


def get_normal(dot1, dot2, pos):
    f_vect = get_vector(dot1, dot2)
    pos_vect = get_vector(dot2, pos)

    if (f_vect[1]):
        normal = [1, -f_vect[0] / f_vect[1]]
    else:
        normal = [0, 1]

    if (scalar_mul(pos_vect, normal) < 0):
        normal[0] = -normal[0]
        normal[1] = -normal[1]

    return normal


def is_visible(dot, f_dot, s_dot):
    vec1 = get_vector(f_dot, s_dot)
    vec2 = get_vector(f_dot, dot)

    if (vector_mul(vec1, vec2) <= 0):
        return True
    else:
        return False


def get_lines_parametric_intersec(line1, line2, normal):
    d = get_vector(line1[0], line1[1])
    w = get_vector(line2[0], line1[0])

    d_scalar = scalar_mul(d, normal)
    w_scalar = scalar_mul(w, normal)

    t = -w_scalar / d_scalar

    dot_intersec = [line1[0][X_DOT] + d[0] * t, line1[0][Y_DOT] + d[1] * t]

    return dot_intersec


def sutherland_hodgman_algorythm(cutter_line, position, prev_result):
    cur_result = []

    dot1 = cutter_line[0]
    dot2 = cutter_line[1]

    normal = get_normal(dot1, dot2, position)

    prev_vision = is_visible(prev_result[-2], dot1, dot2)

    for cur_dot_index in range(-1, len(prev_result)):
        cur_vision = is_visible(prev_result[cur_dot_index], dot1, dot2)

        if (prev_vision):
            if (cur_vision):
                cur_result.append(prev_result[cur_dot_index])
            else:
                figure_line = [prev_result[cur_dot_index - 1], prev_result[cur_dot_index]]

                cur_result.append(get_lines_parametric_intersec(figure_line, cutter_line, normal))
        else:
            if (cur_vision):
                figure_line = [prev_result[cur_dot_index - 1], prev_result[cur_dot_index]]

                cur_result.append(get_lines_parametric_intersec(figure_line, cutter_line, normal))

                cur_result.append(prev_result[cur_dot_index])

        prev_vision = cur_vision

    return cur_result


# TODO

def cut_area():

    if (not is_maked(cutter)):
        messagebox.showinfo("Ошибка", "Отсекатель не замкнут")
        return

    if (not is_maked(figure)):
        messagebox.showinfo("Ошибка", "Отекаемый многоугольник не замкнут")
        return

    if (extra_check(figure)):
        messagebox.showinfo("Ошибка", "Отекаемое должно быть многоугольником")
        return

    if (len(cutter) < 3):
        messagebox.showinfo("Ошибка", "Не задан отсекатель")
        return

    if (not check_polygon()):
        messagebox.showinfo("Ошибка", "Отсекатель должен быть выпуклым многоугольником")
        return

    result = figure.copy()

    for cur_dot_ind in range(-1, len(cutter) - 1):
        line = [cutter[cur_dot_ind], cutter[cur_dot_ind + 1]]

        position_dot = cutter[cur_dot_ind + 1]

        result = sutherland_hodgman_algorythm(line, position_dot, result)

        if (len(result) <= 2):
            return

    draw_result_figure(result)


def draw_result_figure(figure_dots):
    fixed_figure = remove_odd_sides(figure_dots)

    res_color = "red"

    for line in fixed_figure:
        canv.create_line(line[0], line[1], fill = res_color)


# Odd sides
def make_unique(sides):

    for side in sides:
        side.sort()

    return list(filter(lambda x: (sides.count(x) % 2) == 1, sides))


def is_dot_in_side(dot, side):
    if abs(vector_mul(get_vector(dot, side[0]), get_vector(side[1], side[0]))) <= 1e-6:
        if (side[0] < dot < side[1] or side[1] < dot < side[0]):
            return True
    return False


def get_sides(side, rest_dots):
    dots_list = [side[0], side[1]]

    for dot in rest_dots:
        if is_dot_in_side(dot, side):
            dots_list.append(dot)

    dots_list.sort()

    sections_list = list()

    for i in range(len(dots_list) - 1):
        sections_list.append([dots_list[i], dots_list[i + 1]])

    return sections_list


def remove_odd_sides(figure_dots):
    all_sides = list()
    rest_dots = figure_dots[2:]

    for i in range(len(figure_dots)):
        cur_side = [figure_dots[i], figure_dots[(i + 1) % len(figure_dots)]]

        all_sides.extend(get_sides(cur_side, rest_dots))

        rest_dots.pop(0)
        rest_dots.append(figure_dots[i])

    return make_unique(all_sides)



def add_paral_line_cutter(event):
    print("Pressed: Space", event.x, event.y)

    if (len(cutter) < 1):
        return

    dif_x = abs(event.x - cutter[len(cutter) - 1][X_DOT])
    dif_y = abs(event.y - cutter[len(cutter) - 1][Y_DOT])

    if (dif_x > dif_y):
        add_dot_cutter(event.x, cutter[len(cutter) - 1][Y_DOT])
    else:
        add_dot_cutter(cutter[len(cutter) - 1][X_DOT], event.y)


def add_paral_line_figure(event):
    print("Pressed: Control_L", event.x, event.y)

    if (len(figure) < 1):
        return

    dif_x = abs(event.x - figure[len(figure) - 1][X_DOT])
    dif_y = abs(event.y - figure[len(figure) - 1][Y_DOT])

    if (dif_x > dif_y):
        add_dot_figure(event.x, figure[len(figure) - 1][Y_DOT])
    else:
        add_dot_figure(figure[len(figure) - 1][X_DOT], event.y)

cutter = []
figure = []

lines = []

canv.bind("<1>", add_dot_cutter_click)
canv.bind("<3>", add_dot_figure_click)

win.bind('<space>', add_paral_line_cutter)
win.bind('<Control_L>', add_paral_line_figure)

# Add cutter

add_dot_text = Label(win, text = "Добавить точку отсекателя", width = 43, font="-size 16", bg = WIN_COLOR, fg = FONT_COLOR)
add_dot_text.place(x = CV_WIDE + 20, y = 10)

cutter_x_text = Label(text = "x: ", font="-size 14", bg = WIN_COLOR, fg = FONT_COLOR)
cutter_x_text.place(x = CV_WIDE + 30, y = 50)

cutter_x_entry = Entry(font="-size 14", width = 9)
cutter_x_entry.place(x = CV_WIDE + 90, y = 50)

cutter_y_text = Label(text = "y: ", font="-size 14", bg = WIN_COLOR, fg = FONT_COLOR)
cutter_y_text.place(x = CV_WIDE + 30, y = 90)

cutter_y_entry = Entry(font="-size 14", width = 9)
cutter_y_entry.place(x = CV_WIDE + 90, y = 90)

cutter_add_dot_btn = Button(win, text = "Добавить точку", font="-size 14", command = lambda: read_dot_cutter())
cutter_add_dot_btn.place(x = CV_WIDE + 250, y = 50)

cutter_del_dot_btn = Button(win, text = "Удалить крайнюю", font="-size 14", command = lambda: del_dot_cutter())
cutter_del_dot_btn.place(x = CV_WIDE + 245, y = 90)

make_cutter_btn = Button(win, text = "Замкнуть отсекатель", font="-size 14", command = lambda: make_cutter())
make_cutter_btn.place(x = CV_WIDE + 180, y = 150)


# Add line

figure_add_dot_text = Label(win, text = "Добавить отрезок", width = 43, font="-size 16", bg = WIN_COLOR, fg = FONT_COLOR)
figure_add_dot_text.place(x = CV_WIDE + 20, y = 290)

figure_x_text = Label(text = "Начало x: ", font="-size 14", bg = WIN_COLOR, fg = FONT_COLOR)
figure_x_text.place(x = CV_WIDE + 20, y = 330)

figure_x_entry = Entry(font="-size 14", width = 9)
figure_x_entry.place(x = CV_WIDE + 130, y = 330)

figure_y_text = Label(text = "y: ", font="-size 14", bg = WIN_COLOR, fg = FONT_COLOR)
figure_y_text.place(x = CV_WIDE + 20, y = 370)

figure_y_entry = Entry(font="-size 14", width = 9)
figure_y_entry.place(x = CV_WIDE + 130, y = 370)

figure_add_dot_btn = Button(win, text = "Добавить точку", font="-size 14", command = lambda: read_dot_figure())
figure_add_dot_btn.place(x = CV_WIDE + 360, y = 330)

figure_del_dot_btn = Button(win, text = "Удалить крайнюю", font="-size 14", command = lambda: del_dot_figure())
figure_del_dot_btn.place(x = CV_WIDE + 360, y = 370)

make_figure_btn = Button(win, text = "Замкнуть многоугольник", font="-size 14", command = lambda: make_figure())
make_figure_btn.place(x = CV_WIDE + 190, y = 405)

cut_btn = Button(win, text = "Отсечь", width = 18, height = 2, font="-size 14", command = lambda: cut_area())
cut_btn.place(x = CV_WIDE + 20, y = 630)

clear_btn = Button(win, text = "Очистить экран", width = 18, height = 2, font="-size 14", command = lambda: reboot_prog())
clear_btn.place(x = CV_WIDE + 390, y = 630)

exit_btn = Button(win, text = "Выход", font="-size 15", command = lambda: win.quit(), width = 12, height=2)
exit_btn.place(x = CV_WIDE + 240, y = 630)



win.mainloop()

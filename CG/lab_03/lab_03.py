from tkinter import *
from tkinter import messagebox, ttk
from math import copysign, sqrt, acos, degrees, pi, sin, cos, radians, floor, fabs
import copy
import numpy as np
import matplotlib.pyplot as plt

WIN_WIDTH = 1500
WIN_HEIGHT = 900

CV_WIDE = 900
CV_HEIGHT = 900
CV_COLOR = "old lace" #f3e6ff" #"#cce6ff"
MAIN_TEXT_COLOR = "#b566ff" #"lightblue" a94dff
TEXT_COLOR = "#ce99ff"

BOX_COLOR = "#dab3ff"

WIN_COLOR = 'dark slate gray'
FONT_COLOR = 'linen'

win = Tk()
win.attributes("-fullscreen", True)
win['bg'] = WIN_COLOR
win.geometry("%dx%d" %(WIN_WIDTH, WIN_HEIGHT))

SCALING = 1

canv = Canvas(win, width = CV_WIDE, height = CV_HEIGHT, bg = CV_COLOR)
canv.place(x = 0, y = 0)

ZERO = canv.create_oval(CV_WIDE // 2, CV_HEIGHT // 2, CV_WIDE // 2, CV_HEIGHT // 2, state=HIDDEN)

def sign(a):
    if (a < 0):
        return -1
    elif (a == 0):
        return 0
    else:
        return 1

def rgb_to_hex(rgb):
    return f'#{rgb[0]:02x}{rgb[1]:02x}{rgb[2]:02x}'

def draw_line_io():
    cur_method_ind = method_cmbb.current()
    try:
        x1 = float(x1_line.get())
        x2 = float(x2_line.get())
        y1 = float(y1_line.get())
        y2 = float(y2_line.get())
    except:
        messagebox.showerror("Ошибка", "Неверно введены координаты")
        return

    cur_color_ind = color_cmbb.current()

    draw_line(x1, x2, y1, y2, cur_color_ind, cur_method_ind)

def draw_spektr_io():
    try:
        line_len = float(len_line.get())
        angle_spin = float(angle.get())
    except:
        messagebox.showerror("Ошибка", "Неверно введены координаты")
        return

    if (line_len <= 0):
        messagebox.showerror("Ошибка", "Длина линии должна быть выше нуля")
        return

    if (angle_spin <= 0):
        messagebox.showerror("Ошибка", "Угол должен быть больше нуля")
        return

    spin = 0

    while (spin <= 2 * pi):
        x2 = cos(spin) * line_len
        y2 = sin(spin) * line_len

        draw_line(0, x2, 0, y2, color_cmbb.current(), method_cmbb.current())

        spin += radians(angle_spin)

def steps_measure():

    try:
        line_len = float(len_line.get())
    except:
        messagebox.showerror("Ошибка", "Неверно введены координаты")
        return

    if (line_len <= 0):
        messagebox.showerror("Ошибка", "Длина линии должна быть выше нуля")
        return

    x0 = canv.coords(ZERO)[0]
    y0 = canv.coords(ZERO)[1]

    spin = 0

    angle_spin = [i for i in range(0, 91, 2)]

    cda_steps = []
    wu_steps = []
    bres_int_steps = []
    bres_float_steps = []
    bres_smooth_steps = []

    while (spin <= pi / 2 + 0.01):
        x2 = x0 + cos(spin) * line_len
        y2 = y0 + sin(spin) * line_len
        
        cda_steps.append(draw_cda(x0, y0, x2, y2, (255, 255, 255), step_count = True))
        wu_steps.append(draw_wu(x0, y0, x2, y2, (255, 255, 255), step_count = True))
        bres_int_steps.append(draw_brez_int(x0, y0, x2, y2, (255, 255, 255), step_count = True))
        bres_float_steps.append(draw_brez_float(x0, y0, x2, y2, (255, 255, 255), step_count = True))
        bres_smooth_steps.append(draw_brez_smooth(x0, y0, x2, y2, (255, 255, 255), step_count = True))

        spin += radians(2)


    plt.figure(figsize = (15, 6))

    plt.title("Замеры ступенчатости для различных методов\n{0} - длина отрезка".format(line_len))

    plt.xlabel("Угол (в градусах)")
    plt.ylabel("Количество ступенек")

    plt.subplot(2, 2, 1)
    plt.bar(angle_spin, cda_steps, label="ЦДА")
    plt.yticks(np.floor(np.linspace(0, max(cda_steps), 5)))
    plt.xticks(np.arange(91, step = 5))
    plt.legend()

    plt.subplot(2, 2, 2)
    plt.bar(angle_spin, wu_steps, label = "Ву")
    plt.yticks(np.floor(np.linspace(0, max(wu_steps), 5)))
    plt.xticks(np.arange(91, step = 5))
    plt.legend()

    plt.subplot(2, 2, 3)
    plt.bar(angle_spin, bres_float_steps, label="Брезенхем (float/int)")
    plt.yticks(np.floor(np.linspace(0, max(bres_float_steps), 5)))
    plt.xticks(np.arange(91, step = 5))
    plt.legend()

    plt.subplot(2, 2, 4)
    plt.bar(angle_spin, bres_smooth_steps, label="Брезенхем\n(сглаживание)")
    plt.yticks(np.floor(np.linspace(0, max(bres_smooth_steps), 5)))
    plt.xticks(np.arange(91, step = 5))
    plt.legend()

    plt.show()

def draw_line(x1, x2, y1, y2, cur_color_ind, cur_method_ind):
    match cur_color_ind:
        case 0:
            color = (0, 0, 0)
        case 1:
            color = (0, 0, 255)
        case 2:
            color = (255, 0, 0)
        case 3:
            color = (253, 245, 230)


    match cur_method_ind:
        case 0:
            points_coords = draw_cda(x1, y1, x2, y2, color)
        case 1:
            points_coords = draw_brez_float(x1, y1, x2, y2, color)
        case 2:
            points_coords = draw_brez_int(x1, y1, x2, y2, color)
        case 3:
            points_coords = draw_brez_smooth(x1, y1, x2, y2, color)
        case 4:
            points_coords = draw_wu(x1, y1, x2, y2, color)
    render_line(points_coords)

def draw_cda(x1, y1, x2, y2, color, step_count=False):

    l = fabs(x1 - x2)
    if fabs(x1 - x2) < fabs(y1 - y2):
        l = fabs(y1 - y2)
    if not l:
        return [x1, y1, color]
    dx = (x2 - x1) / l
    dy = (y2 - y1) / l
    l = int(l)
    points = [[round(x1 + dx * i), round(y1 + dy * i), color] for i in range(0, l + 1)]
    steps = min(round(dy * l), round(dx * l))
    if step_count:
        return steps
    return points

def draw_brez_float(x1, y1, x2, y2, color, step_count=False):
    x = x1
    y = y1
    steps = 0
    dx = x2 - x1
    dy = y2 - y1
    sx = sign(dx)
    sy = sign(dy)
    dx = abs(dx)
    dy = abs(dy)
    obmen = 0
    if dx < dy:
        obmen = 1
        dx, dy = dy, dx
    m = dy / dx
    e = m - 0.5
    points = []
    for i in range (0, int(dx) + 1):
        points.append([x, y, color])
        x_prev = x
        y_prev = y
        if e >= 0:
            if obmen == 1:
                x += sx
            else:
                y += sy
            e -= 1
        if obmen == 1:
            y += sy
        else:
            x += sx
        e += m
        if step_count:
            if not((x_prev == x and y_prev != y) or
                    (x_prev != x and y_prev== y)):
                steps += 1
    if step_count:
        return steps
    return points

def draw_brez_int(x1, y1, x2, y2, color, step_count=False):
    x = x1
    y = y1
    dx = x2 - x1
    dy = y2 - y1
    sx = sign(dx)
    sy = sign(dy)
    dx = abs(dx)
    dy = abs(dy)
    obmen = 0
    steps = 0
    if dx < dy:
        obmen = 1
        dx, dy = dy, dx
    e = 2 * dy - dx
    points = []
    for i in range (0, int(dx) + 1):
        points.append([x, y, color])
        x_prev = x
        y_prev = y
        if e >= 0:
            if obmen == 1:
                x += sx
            else:
                y += sy
            e = e - 2 * dx
        if obmen == 1:
            y += sy
        else:
            x += sx
        e = e + 2 * dy
        if step_count:
            if not((x_prev == x and y_prev != y) or
                    (x_prev != x and y_prev== y)):
                steps += 1
    if step_count:
        return steps
    return points

def intens_correct(color, intens):
    c1 = color[0] + intens
    c2 = color[1] + intens
    c3 = color[2] + intens
    c1 = 255 if c1 > 255 else c1
    c2 = 255 if c2 > 255 else c2
    c3 = 255 if c3 > 255 else c3
    return c1, c2, c3

def draw_brez_smooth(x1, y1, x2, y2, color, step_count=False):
    x = x1
    y = y1
    dx = x2 - x1
    dy = y2 - y1
    steps = 0
    sx = sign(dx)
    sy = sign(dy)
    dx = abs(dx)
    dy = abs(dy)
    obmen = 0
    intens = 255
    if dx < dy:
        obmen = 1
        dx, dy = dy, dx
    m = dy / dx
    e = intens / 2
    m *= intens
    w = intens - m
    points = []
    for i in range (0, int(dx) + 1):
        points.append([x, y, intens_correct(color, round(e))])
        x_prev = x
        y_prev = y
        if e < w:
            if obmen == 1:
                y += sy
            else:
                x += sx
            e += m
        else:
            y += sy
            x += sx
            e -= w
        if step_count:
            if not((x_prev == x and y_prev != y) or
                    (x_prev != x and y_prev== y)):
                steps += 1
    if step_count:
        return steps
    return points

def draw_wu(x1, y1, x2, y2, color, step_count=False):
    dx = x2 - x1
    dy = y2 - y1

    m = 1
    step = 1
    intens = 255

    points = []

    steps = 0

    if (fabs(dy) > fabs(dx)):
        m = dx / dy
        m1 = m

        if (y1 > y2):
            m1 *= -1
            step *= -1

        y_end = round(y2) - 1 if (dy < dx) else (round(y2) + 1)

        for y_cur in range(round(y1), y_end, step):
            d1 = x1 - floor(x1)
            d2 = 1 - d1

            point1 = [int(x1) + 1, y_cur, intens_correct(color, round(fabs(d2) * intens))]

            point2 = [int(x1), y_cur, intens_correct(color, round(fabs(d1) * intens))]

            if step_count and y_cur < y2:
                if (int(x1) != int(x1 + m)):
                    steps += 1

            points.append(point1)
            points.append(point2)

            x1 += m1
    
    else:
        m = dy / dx

        m1 = m

        if (x1 > x2):
            step *= -1
            m1 *= -1

        x_end = round(x2) - 1 if (dy > dx) else (round(x2) + 1)

        for x_cur in range(round(x1), x_end, step):
            d1 = y1 - floor(y1)
            d2 = 1 - d1

            point1 = [x_cur, int(y1) + 1, intens_correct(color, round(fabs(d2) * intens))]

            point2 = [x_cur, int(y1), intens_correct(color, round(fabs(d1) * intens))]

            if step_count and x_cur < x2:
                if (int(y1) != int(y1 + m)):
                    steps += 1

            points.append(point1)
            points.append(point2)

            y1 += m1
    if step_count:
        return steps
    return points

def render_line(coords):
    # print(coords)
    x0 = canv.coords(ZERO)[0]
    y0 = canv.coords(ZERO)[1]
    for pixel in coords: 
        x = pixel[0] * SCALING + x0
        y = -pixel[1] * SCALING + y0
        canv.create_polygon(x, y , x + SCALING, y, x + SCALING, y + SCALING, x, y + SCALING, fill=rgb_to_hex(pixel[2]))

def zoom(event):
    global SCALING
    if (event.delta > 0):
        SCALING *= 1.1
        canv.scale("all", event.x, event.y, 1.1, 1.1)
    elif (event.delta < 0):
        SCALING *= 0.9
        canv.scale("all", event.x, event.y, 0.9, 0.9)

def clear_picture():
    global ZERO, SCALING
    canv.delete(ALL)
    ZERO = canv.create_oval(CV_WIDE // 2, CV_HEIGHT // 2, CV_WIDE // 2, CV_HEIGHT // 2, state=HIDDEN)
    SCALING = 1
    
def aboutprog():
    messagebox.showinfo(title='О программе', message='Реализовать различные алгоритмы построения одиночных отрезков. Отрезок задается координатой начала, координатой конца и цветом.\n\
Сравнить визуальные характеристики отрезков, построенных разными алгоритмами, с помощью построения пучка отрезков, с заданным шагом.\n\
Сравнение со стандартным алгоритмом. Задаются начальные и конечные координаты; рисуется отрезок разными методами. Отрисовка отрезка другим цветом и методом поверх первого, для проверки совпадения. Предоставить пользователю возможность выбора двух цветов – цвета фона и цвета рисования. Алгоритмы выбирать из выпадающего списка.\n\
- ЦДА\n\
- Брезенхем действительные числа\n\
- Брезенхем целые числа\n\
- Брезенхем с устранением ступенчатости\n\
- ВУ\n\
Построение гистограмм по количеству ступенек в зависимости от угла наклона.')

# Method

method_text = Label(win, text = "Алгоритм построения отрезка", width = 43, font="-size 16", bg=WIN_COLOR, fg= FONT_COLOR, )
method_text.place(x = CV_WIDE + 20, y = 30)

method_cmbb = ttk.Combobox(win, state='readonly', width=40, font="-size 16")
win.option_add('*TCombobox*Listbox.font', '-size 14')
method_cmbb['values'] = ['ЦДА', 'Брезенхем действительные числа', 'Брезенхем целые числа', 'Брезенхем с устранением ступенчатости', 'ВУ']
method_cmbb.place(x = CV_WIDE + 20, y = 70)
method_cmbb.current(0)

# Color
color_text = Label(win, text = "Цвет отрезка", width = 43, font="-size 16", bg=WIN_COLOR, fg= FONT_COLOR)
color_text.place(x = CV_WIDE + 20, y = 210)

color_cmbb = ttk.Combobox(win, state='readonly', width=40, font="-size 16")
win.option_add('*TCombobox*Listbox.font', '-size 14')
color_cmbb['values'] = ['Черный', 'Синий', 'Красный', 'Фоновый']
color_cmbb.place(x = CV_WIDE + 20, y = 250)
color_cmbb.current(0)

# Line

line_text = Label(win, text = "Нарисовать отрезок", width = 43, font="-size 16", bg=WIN_COLOR, fg= FONT_COLOR)
line_text.place(x = CV_WIDE + 20, y = 370)

# Point 1
x1_line_text = Label(text = "x1: ", font="-size 14", bg=WIN_COLOR, fg= FONT_COLOR)
x1_line_text.place(x = CV_WIDE + 70, y = 405)

x1_line = Entry(font="-size 14", width = 9)
x1_line.place(x = CV_WIDE + 100, y = 405)

y1_line_text = Label(text = "y1: ", font="-size 14", bg=WIN_COLOR, fg= FONT_COLOR)
y1_line_text.place(x = CV_WIDE + 330, y = 405)

y1_line = Entry(font="-size 14", width = 9)
y1_line.place(x = CV_WIDE + 360, y = 405)


# Point 2
x2_line_text = Label(text = "x2: ", font="-size 14", bg=WIN_COLOR, fg= FONT_COLOR)
x2_line_text.place(x = CV_WIDE + 70, y = 455)

x2_line = Entry(font="-size 14", width = 9)
x2_line.place(x = CV_WIDE + 100, y = 455)

y2_line_text = Label(text = "y2: ", font="-size 14", bg=WIN_COLOR, fg= FONT_COLOR)
y2_line_text.place(x = CV_WIDE + 330, y = 455)

y2_line = Entry(font="-size 14", width = 9)
y2_line.place(x = CV_WIDE + 360, y = 455)

draw_line_btn = Button(win, text = "Нарисовать", font="-size 14", command = lambda: draw_line_io(), width = 15)
draw_line_btn.place(x = CV_WIDE + 210, y = 490)


# Spektr
line_text = Label(win, text = "Нарисовать спектр", width = 43, font="-size 16", bg=WIN_COLOR, fg= FONT_COLOR)
line_text.place(x = CV_WIDE + 20, y = 550)

# Data
len_line_text = Label(text = "Длина отрезка: ", font="-size 14", bg=WIN_COLOR, fg= FONT_COLOR)
len_line_text.place(x = CV_WIDE + 30, y = 590)

len_line = Entry(font="-size 14", width = 9)
len_line.place(x = CV_WIDE + 170, y = 590)

angle_text = Label(text = "Шаг угла°: ", font="-size 14", bg=WIN_COLOR, fg= FONT_COLOR)
angle_text.place(x = CV_WIDE + 370, y = 590)

angle = Entry(font="-size 14", width = 9)
angle.place(x = CV_WIDE + 465, y = 590)

draw_spektr_btn = Button(win, text = "Нарисовать", font="-size 14", command = lambda: draw_spektr_io(), width = 15)
draw_spektr_btn.place(x = CV_WIDE + 210, y = 640)

# Control buttons

compare_steps_btn = Button(win, text = "Сравнить\nступенчатость", font="-size 15", command = lambda: steps_measure(), width = 15, height = 2)
compare_steps_btn.place(x = CV_WIDE + 400, y = 720)


clear_win_btn = Button(win, text = "Очистить экран", font="-size 15", command = lambda: clear_picture(), width = 15, height=2)
clear_win_btn.place(x = CV_WIDE + 210, y = 720)

exit_btn = Button(win, text = "Выход", font="-size 15", command = lambda: win.quit(), width = 15, height=2)
exit_btn.place(x = CV_WIDE + 20, y = 720)


credits = Button(win, text = "Об авторе", font="-size 8", command = lambda: messagebox.showinfo(title='Об авторе', message='Золотухин Алексей ИУ7-44Б'), width = 15, height = 2)
credits.place(x = 10, y = 10)

about_prog = Button(win, text = "О программе", font="-size 8", command = lambda: aboutprog(), width = 15, height = 2)
about_prog.place(x = 10, y = 55)

canv.bind('<MouseWheel>', zoom)

win.mainloop()
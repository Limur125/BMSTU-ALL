from tkinter import Tk, Button, Label, Entry, END, Listbox, Canvas, Radiobutton, LEFT, RIGHT, IntVar, PhotoImage, Spinbox
from tkinter import messagebox, ttk
from math import sqrt,exp, pi, sin, cos
import numpy as np
from numpy import arange


WIN_WIDTH = 1600
WIN_HEIGHT = 900
WIN_COLOR = 'dark slate gray'

CV_WIDE = 900
CV_HEIGHT = 900
CV_COLOR = "#000000" #rgb_to_hex((253, 245, 230)) #f3e6ff" #"#cce6ff"
MAIN_TEXT_COLOR = "#b566ff" #"lightblue" a94dff
FONT_COLOR = 'linen'

BTN_TEXT_COLOR = "#4d94ff"


# Define

X_DOT = 0
Y_DOT = 1
Z_DOT = 2

FROM = 0
TO = 1
STEP = 2

FROM_SPIN_BOX = -1000.0
TO_SPIN_BOX = 1000.0
STEP_SPIN_BOX = 0.1

DEFAULT_SCALE = 45
DEFAULT_ANGLE = 30


win = Tk()
win['bg'] = WIN_COLOR
win.geometry("%dx%d" %(WIN_WIDTH, WIN_HEIGHT))
win.attributes("-fullscreen", True)

canv = Canvas(win, width = CV_WIDE, height = CV_HEIGHT, bg = CV_COLOR)
canv.place(x = 0, y = 0)

canv.delete("all")

# For spins
trans_matrix = []


def set_trans_matrix():
    global trans_matrix

    trans_matrix.clear()

    for i in range(4):
        tmp_arr = []

        for j in range(4):
            tmp_arr.append(int(i == j))
            
        trans_matrix.append(tmp_arr)



def check_option(option):
    messagebox.showinfo("Выбран", "Выбрана опция %d" %(option))


def clear_canvas():
    canv.delete("all")


def get_fill_check_color(collor_fill):
    return (int(collor_fill[1:3], 16), int(collor_fill[3:5], 16), int(collor_fill[5:7], 16))


def reboot_prog():

    canv.delete("all")


def parse_funcs(func_num):
    match func_num:
        case 0:
            func = lambda x, z: sqrt(x * x + z * z)
        case 1:
            func = lambda x, z: sin(x * x + z * z)
        case 2:
            func = lambda x, z: sqrt(exp(x * x * z * z))
        case 3:
            func = lambda x, z: 1 / (1 + x * x) + 1 / (1 + z * z)

    return func


def read_limits():
    try:
        x_from = float(x_from_entry.get())
        x_to = float(x_to_entry.get())
        x_step = float(x_step_entry.get())

        x_limits = [x_from, x_to, x_step]

        z_from = float(z_from_entry.get())
        z_to = float(z_to_entry.get())
        z_step = float(z_step_entry.get())

        z_limits = [z_from, z_to, z_step]
    
        return x_limits, z_limits
    except:
        return -1, -1


def rotate_matrix(matrix):
    global trans_matrix

    res_matrix = [[0 for i in range(4)] for j in range(4)]

    for i in range(4):
        for j in range(4):
            for k in range(4):
                res_matrix[i][j] += trans_matrix[i][k] * matrix[k][j]

    trans_matrix = res_matrix


def spin_x():
    try:
        angle = float(x_spin_entry.get()) / 180 * pi
    except:
        messagebox.showerror("Ошибка", "Угол - число")
        return

    if (len(trans_matrix) == 0):
        messagebox.showerror("Ошибка", "График не задан")
        return

    rotating_matrix = [[1, 0, 0, 0],
                     [0, cos(angle), sin(angle), 0],
                     [0, -sin(angle), cos(angle), 0],
                     [0, 0, 0, 1]]

    rotate_matrix(rotating_matrix)

    build_graph()


def spin_y():
    try:
        angle = float(y_spin_entry.get()) / 180 * pi
    except:
        messagebox.showerror("Ошибка", "Угол - число")
        return

    if (len(trans_matrix) == 0):
        messagebox.showerror("Ошибка", "График не задан")
        return

    rotating_matrix = [[cos(angle), 0, -sin(angle), 0],
                     [0, 1, 0, 0],
                     [sin(angle), 0, cos(angle), 0],
                     [0, 0, 0, 1]   ]

    rotate_matrix(rotating_matrix)

    build_graph()


def spin_z():
    try:
        angle = float(z_spin_entry.get()) / 180 * pi
    except:
        messagebox.showerror("Ошибка", "Угол - число")
        return

    if (len(trans_matrix) == 0):
        messagebox.showerror("Ошибка", "График не задан")
        return

    rotating_matrix = [[cos(angle), sin(angle), 0, 0],
                     [-sin(angle), cos(angle), 0, 0],
                     [0, 0, 1, 0],
                     [0, 0, 0, 1]   ]

    rotate_matrix(rotating_matrix)

    build_graph()

def trans_dot(dot, scale_param):
    dot.append(1) 

    res_dot = [0, 0, 0, 0]

    for i in range(4):
        for j in range(4):
            res_dot[i] += dot[j] * trans_matrix[j][i]

    for i in range(3):
        res_dot[i] *= 20

    res_dot[0] += CV_WIDE // 2
    res_dot[1] += CV_HEIGHT // 2

    return res_dot[:3]


def is_visible(dot):
    return (0 <= dot[X_DOT] <= CV_WIDE) and \
            (0 <= dot[Y_DOT] <= CV_HEIGHT)


def draw_pixel(x, y):
    color = 'blue'
    canv.create_line(x, y, x + 1, y + 1, fill = color)


def draw_dot(x, y, high_horizon, low_horizon):
    if (not is_visible([x, y])):
        return False

    if (y > high_horizon[x]):
        high_horizon[x] = y
        draw_pixel(x, y)
    elif (y < low_horizon[x]):
        low_horizon[x] = y
        draw_pixel(x, y)

    return True


def draw_horizon_part(dot1, dot2, high_horizon, low_horizon):
    if (dot1[X_DOT] > dot2[X_DOT]):
        dot1, dot2 = dot2, dot1

    dx = dot2[X_DOT] - dot1[X_DOT]
    dy = dot2[Y_DOT] - dot1[Y_DOT]

    if (dx > dy):
        l = dx
    else:
        l = dy

    dx /= l
    dy /= l

    x = dot1[X_DOT]
    y = dot1[Y_DOT]

    for _ in range(int(l) + 1):
        if (not draw_dot(round(x), y, high_horizon, low_horizon)):
            return

        x += dx
        y += dy


def draw_horizon(function, high_horizon, low_horizon, limits, z, scale_param):
    f = lambda x: function(x, z)

    prev = None

    for x in arange(limits[FROM], limits[TO] + limits[STEP], limits[STEP]):
        cur = trans_dot([x, f(x), z], scale_param)

        if (prev):
            draw_horizon_part(prev, cur, high_horizon, low_horizon)

        prev = cur


def draw_horizon_limits(f, x_limits, z_limits, scale_param):
    color = 'blue'

    for z in arange(z_limits[FROM], z_limits[TO] + z_limits[STEP], z_limits[STEP]):
        dot1 = trans_dot([x_limits[FROM], f(x_limits[FROM], z), z], scale_param)
        dot2 = trans_dot([x_limits[FROM], f(x_limits[FROM], z + x_limits[STEP]), z + x_limits[STEP]], scale_param)

        canv.create_line(dot1[X_DOT], dot1[Y_DOT], dot2[X_DOT], dot2[Y_DOT], fill = color)

        dot1 = trans_dot([x_limits[TO], f(x_limits[TO], z), z], scale_param)
        dot2 = trans_dot([x_limits[TO], f(x_limits[TO], z + x_limits[STEP]), z + x_limits[STEP]], scale_param)

        canv.create_line(dot1[X_DOT], dot1[Y_DOT], dot2[X_DOT], dot2[Y_DOT], fill = color)


def build_graph(new_graph = False, scale_param = DEFAULT_SCALE):
    reboot_prog()

    if (new_graph):
        set_trans_matrix()

        rotating_matrix = [[1, 0, 0, 0],
                     [0, cos(pi), sin(pi), 0],
                     [0, -sin(pi), cos(pi), 0],
                     [0, 0, 0, 1]]
        rotate_matrix(rotating_matrix)   

        rotating_matrix = [[cos(pi / 4), 0, -sin(pi / 4), 0],
                     [0, 1, 0, 0],
                     [sin(pi / 4), 0, cos(pi / 4), 0],
                     [0, 0, 0, 1]   ]

        rotate_matrix(rotating_matrix)

        rotating_matrix = [[1, 0, 0, 0],
                     [0, cos(pi / 6), sin(pi / 6), 0],
                     [0, -sin(pi / 6), cos(pi / 6), 0],
                     [0, 0, 0, 1]]

        rotate_matrix(rotating_matrix)

    f = parse_funcs(func_cmbb.current())

    x_limits, z_limits = read_limits()

    print(x_limits, z_limits)

    high_horizon = [0 for i in range(CV_WIDE + 1)]
    low_horizon = [CV_HEIGHT for i in range(CV_WIDE + 1)]

    for z in arange(z_limits[FROM], z_limits[TO] + z_limits[STEP], z_limits[STEP]):
        draw_horizon(f, high_horizon, low_horizon, x_limits, z, scale_param)

    draw_horizon_limits(f, x_limits, z_limits, scale_param)

func_cmbb = ttk.Combobox(win, state='readonly', width=40, font="-size 16")
win.option_add('*TCombobox*Listbox.font', '-size 14')
func_cmbb['values'] = ['sqrt(x * x + z * z)', 'sin(x * x + z * z)', 'sqrt(exp(x * x * z * z))', '1 / (1 + x * x) + 1 / (1 + z * z)']
func_cmbb.place(x = CV_WIDE + 60, y = 50)
func_cmbb.current(3)

add_dot_text = Label(win, text = "Функция", width = 43, font="-size 16", bg=WIN_COLOR , fg = FONT_COLOR)
add_dot_text.place(x = CV_WIDE + 20, y = 10)

figure_add_dot_text = Label(win, text = "Пределы", width = 43, font="-size 16", bg=WIN_COLOR , fg = FONT_COLOR)
figure_add_dot_text.place(x = CV_WIDE + 20, y = 225)

x_limit_text = Label(text = "Ось X", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
x_limit_text.place(x = CV_WIDE + 30, y = 265)

x_from_text = Label(text = "от: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
x_from_text.place(x = CV_WIDE + 150, y = 265)
x_from_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
x_from_entry.place(x = CV_WIDE + 190, y = 265)

x_to_text = Label(text = "до: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
x_to_text.place(x = CV_WIDE + 295, y = 265)
x_to_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
x_to_entry.place(x = CV_WIDE + 330, y = 265)

x_step_text = Label(text = "шаг: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
x_step_text.place(x = CV_WIDE + 430, y = 265)
x_step_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
x_step_entry.place(x = CV_WIDE + 480, y = 265)

x_from_entry.delete(0, END)
x_from_entry.insert(0, "-10")
x_to_entry.delete(0, END)
x_to_entry.insert(0, "10")
x_step_entry.delete(0, END)
x_step_entry.insert(0, "0.1")

z_limit_text = Label(text = "Ось Z", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
z_limit_text.place(x = CV_WIDE + 30, y = 315)

z_from_text = Label(text = "от: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
z_from_text.place(x = CV_WIDE + 150, y = 315)
z_from_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
z_from_entry.place(x = CV_WIDE + 190, y = 315)

z_to_text = Label(text = "до: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
z_to_text.place(x = CV_WIDE + 295, y = 315)
z_to_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
z_to_entry.place(x = CV_WIDE + 330, y = 315)

z_step_text = Label(text = "шаг: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
z_step_text.place(x = CV_WIDE + 430, y = 315)
z_step_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
z_step_entry.place(x = CV_WIDE + 480, y = 315)

z_from_entry.delete(0, END)
z_from_entry.insert(0, "-10")
z_to_entry.delete(0, END)
z_to_entry.insert(0, "10")
z_step_entry.delete(0, END)
z_step_entry.insert(0, "0.1")

figure_add_dot_text = Label(win, text = "Вращение", width = 43, font="-size 16", bg=WIN_COLOR , fg = FONT_COLOR)
figure_add_dot_text.place(x = CV_WIDE + 20, y = 380)

x_spin_text = Label(text = "OX: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
x_spin_text.place(x = CV_WIDE + 120, y = 420)

x_spin_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
x_spin_entry.place(x = CV_WIDE + 190, y = 420)

x_spin_btn = Button(win, text = "Повернуть", width = 12, height = 1, font="-size 14", command = lambda: spin_x())
x_spin_btn.place(x = CV_WIDE + 330, y = 415)

x_spin_entry.delete(0, END)
x_spin_entry.insert(0, str(DEFAULT_ANGLE))

y_spin_text = Label(text = "OY: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
y_spin_text.place(x = CV_WIDE + 120, y = 465)

y_spin_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
y_spin_entry.place(x = CV_WIDE + 190, y = 465)

y_spin_btn = Button(win, text = "Повернуть", width = 12, height = 1, font="-size 14", command = lambda: spin_y())
y_spin_btn.place(x = CV_WIDE + 330, y = 460)

y_spin_entry.delete(0, END)
y_spin_entry.insert(0, str(DEFAULT_ANGLE))

z_spin_text = Label(text = "OZ: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
z_spin_text.place(x = CV_WIDE + 120, y = 510)

z_spin_entry = Spinbox(font="-size 14", from_ = FROM_SPIN_BOX, to=TO_SPIN_BOX, increment=STEP_SPIN_BOX, width = 6)
z_spin_entry.place(x = CV_WIDE + 190, y = 510)

z_spin_btn = Button(win, text = "Повернуть", width = 12, height = 1, font="-size 14", command = lambda: spin_z())
z_spin_btn.place(x = CV_WIDE + 330, y = 505)

z_spin_entry.delete(0, END)
z_spin_entry.insert(0, str(DEFAULT_ANGLE))

cut_btn = Button(win, text = "Результат", width = 18, height = 2, font="-size 14", command = lambda: build_graph(new_graph = True))
cut_btn.place(x = CV_WIDE + 20, y = 630)

clear_btn = Button(win, text = "Очистить экран", width = 18, height = 2, font="-size 14", command = lambda: reboot_prog())
clear_btn.place(x = CV_WIDE + 350, y = 630)

win.mainloop()

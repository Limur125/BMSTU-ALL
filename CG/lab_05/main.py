from tkinter import *
from tkinter import messagebox, ttk
from math import sqrt, acos, degrees, pi, sin, cos, radians, floor, fabs
import copy
import numpy as np
import matplotlib.pyplot as plt

from time import time, sleep

import colorutils as cu

def rgb_to_hex(rgb):
    return f'#{rgb[0]:02x}{rgb[1]:02x}{rgb[2]:02x}'

WIN_WIDTH = 1600
WIN_HEIGHT = 900
WIN_COLOR = 'dark slate gray'

CV_WIDE = 900
CV_HEIGHT = 900
CV_COLOR = rgb_to_hex((253, 245, 230)) #f3e6ff" #"#cce6ff"
MAIN_TEXT_COLOR = "#b566ff" #"lightblue" a94dff
FONT_COLOR = 'linen'

BTN_TEXT_COLOR = "#4d94ff"

BOX_COLOR = "#dab3ff"
BOX_WIDTH = 50

LINES = []
FIRST_POINT = []
LOG = []

win = Tk()
win['bg'] = WIN_COLOR
win.geometry("%dx%d" %(WIN_WIDTH, WIN_HEIGHT))
win.attributes("-fullscreen", True)

canv = Canvas(win, width = CV_WIDE, height = CV_HEIGHT, bg = CV_COLOR)

canv.delete("all")

image_canvas = PhotoImage(width = CV_WIDE, height = CV_HEIGHT, palette=CV_COLOR)
canv.create_image((CV_WIDE / 2, CV_HEIGHT / 2), image = image_canvas, state = NORMAL)

canv.place(x = 0, y = 0)

def point_io():
    try:
        x = int(x_entry.get()) + CV_WIDE // 2
        y = -int(y_entry.get()) + CV_HEIGHT // 2
    except:
        messagebox.showerror("Ошибка", "Неверно введены координаты точки")
        return
    add_dot(x, y)

def add_dot_click(event):
    x = int(event.x)
    y = int(event.y)

    add_dot(x, y)

def add_dot(x, y):
    global LINES, FIRST_POINT, LOG
    LOG.append([LINES, FIRST_POINT])
    print(LOG)
    if len(LINES) == 0 or len((LINES[-1])[-1]) > 1:
        LINES.append([])
        FIRST_POINT = [x, y]
    if len(LINES[-1]) and len((LINES[-1])[-1]) < 2:
        ((LINES[-1])[-1]).append([x, y])
    LINES[-1].append([[x, y]])
    if len(LINES[-1]) > 1:
        canv.create_line(tuple((LINES[-1])[-2]))

def close_dot_click(event):
    close_lines()

def close_lines():
    global LINES
    LOG.append([LINES.copy(), FIRST_POINT])
    if len((LINES[-1])[-1]) < 2:
        ((LINES[-1])[-1]).append(FIRST_POINT)
        canv.create_line(tuple((LINES[-1])[-1]))

def fill_io(color_i, delay):
    canv.update()
    for line in LINES:
        for point in line:
            if len(point) != 2:
                return

    match color_i:
        case 0:
            color = (0, 0, 255)
        case 1:
            color = (255, 0, 0)
        case 2:
            color = (0, 255, 0)
    start = time()
    for line in LINES:
        for i in range(len(line)):
            x1 = line[i][0][0]
            x2 = line[i][1][0]
            y1 = line[i][0][1]
            y2 = line[i][1][1]

            coords = cda(x1, y1, x2, y2)
            if line[i - 1][0][1] > y1 and y1 > y2:
                coords.pop(0)
            if line[i - 1][0][1] <= y1 and y1 <= y2:
                coords.pop(0)
            for point in coords:
                if len(point) == 1:
                    continue
                y = point[1]
                for x in range(point[0], CV_WIDE):
                    tmp = image_canvas.get(x, y)
                    if image_canvas.get(x, y) != color:
                        image_canvas.put(rgb_to_hex(color), to=(x, y))
                    else:
                        image_canvas.put(CV_COLOR, to=(x, y))
                if delay == 1:
                    canv.update()
                    res = time() - start
                    time_label.configure(text='Время: {:.2f} с'.format(res))
    res = time() - start
    time_label.configure(text='Время: {:.2f} с'.format(res))
    


def cda(x1, y1, x2, y2):
    l = fabs(y1 - y2)
    if not l:
        return [[x1, y1]]
    dx = (x2 - x1) / l
    dy = (y2 - y1) / l
    l = int(l)
    points = [[round(x1 + dx * i), round(y1 + dy * i)] for i in range(0, l + 1)]
    return points


def reboot_prog():
    global LINES, FIRST_POINT, LOG, image_canvas

    canv.delete(ALL)

    LINES = []
    FIRST_POINT = []
    LOG = []

    image_canvas = PhotoImage(width = CV_WIDE, height = CV_HEIGHT)
    canv.create_image((CV_WIDE / 2, CV_HEIGHT / 2), image = image_canvas, state = NORMAL)

def cancel_action():
    global LINES, FIRST_POINT, LOG
    print(LINES)
    print(FIRST_POINT)
    print(LOG)
    LINES = LOG[-1][0]
    FIRST_POINT = LOG[-1][1]
    print(LINES)
    print(FIRST_POINT)
    LOG.pop()
    canv.delete(ALL)
    canv.create_image((CV_WIDE / 2, CV_HEIGHT / 2), image = image_canvas, state = NORMAL)
    for fig in LINES:
        for line in fig:
            print(line)
            if len(line) == 2:
                canv.create_line(tuple(line))

def config_rbs(a):
    match a:
        case 0:
            draw_delay.configure(fg='pink')
            draw_without_delay.configure(fg=FONT_COLOR)
        case 1:
            draw_without_delay.configure(fg='pink')
            draw_delay.configure(fg=FONT_COLOR)

def aboutprog():
    messagebox.showinfo(title='О программе', message='Демонстрация работы алгоритма заполнения по ребрам.')
# Add dot

color_text = Label(win, text = "Цвет отрезка", width = 43, font="-size 16", bg=WIN_COLOR, fg=FONT_COLOR)
color_text.place(x = CV_WIDE + 20, y = 240)

color_cmbb = ttk.Combobox(win, state='readonly', width=40, font="-size 16")
win.option_add('*TCombobox*Listbox.font', '-size 14')
color_cmbb['values'] = ['Синий', 'Красный', 'Зеленый']
color_cmbb.place(x = CV_WIDE + 60, y = 280)
color_cmbb.current(0)

add_dot_text = Label(win, text = "Добавить точку", width = 43, font="-size 16", bg=WIN_COLOR, fg=FONT_COLOR)
add_dot_text.place(x = CV_WIDE + 20, y = 20)

x_text = Label(text = "x: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
x_text.place(x = CV_WIDE + 70, y = 70)

x_entry = Entry(font="-size 14", width = 9)
x_entry.place(x = CV_WIDE + 90, y = 70)

y_text = Label(text = "y: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
y_text.place(x = CV_WIDE + 330, y = 70)

y_entry = Entry(font="-size 14", width = 9)
y_entry.place(x = CV_WIDE + 350, y = 70)

add_dot_btn = Button(win, text = "Добавить точку", font="-size 14", command = lambda: point_io())
add_dot_btn.place(x = CV_WIDE + 200, y = 120)

make_figure_btn = Button(win, text = "Замкнуть фигуру", font="-size 14", command = lambda: close_lines())
make_figure_btn.place(x = CV_WIDE + 190, y = 190)

# Fill figure

color_text = Label(win, text = "Выбрать тип закраски", width = 43, font="-size 16", bg=WIN_COLOR, fg=FONT_COLOR)
color_text.place(x = CV_WIDE + 20, y = 370)

option_filling = IntVar()
option_filling.set(1)

draw_delay = Radiobutton(text = "С задержкой", font="-size 14", variable = option_filling, value = 1, indicatoron=True, bg = WIN_COLOR, fg=FONT_COLOR, activebackground=WIN_COLOR, highlightbackground = WIN_COLOR, command=lambda: config_rbs(0))
draw_delay.place(x = CV_WIDE + 25, y = 410)

draw_without_delay = Radiobutton(text = "Без задержки", font="-size 14", variable = option_filling, value = 2, indicatoron=True, bg = WIN_COLOR, fg=FONT_COLOR, activebackground=WIN_COLOR, highlightbackground = WIN_COLOR, command=lambda: config_rbs(1))
draw_without_delay.place(x = CV_WIDE + 350, y = 410)

fill_figure_btn = Button(win, text = "Закрасить выбранную область", font="-size 14", command = lambda: fill_io(color_cmbb.current(), option_filling.get()))
fill_figure_btn.place(x = CV_WIDE + 150, y = 460)

# Time and clear

time_label = Label(text = f"Время: {0.00} с", font="-size 16", bg = "lightgrey")
time_label.place(x = 20, y = CV_HEIGHT - 100)

clear_win_btn = Button(win, text = "Очистить экран", font="-size 15", command = lambda: reboot_prog(), width = 15, height=2)
clear_win_btn.place(x = CV_WIDE + 240, y = 540)

exit_btn = Button(win, text = "Выход", font="-size 15", command = lambda: win.quit(), width = 12, height=2)
exit_btn.place(x = CV_WIDE + 40, y = 540)

# back_btn = Button(win, text='Отмена действия', font='-size 15', command=lambda: cancel_action(), width=15, height=2)
# back_btn.place(x = CV_WIDE + 40, y = 620)

credits = Button(win, text = "Об авторе", font="-size 8", command = lambda: messagebox.showinfo(title='Об авторе', message='Золотухин Алексей ИУ7-44Б'), width = 15, height = 2)
credits.place(x = 10, y = 10)

about_prog = Button(win, text = "О программе", font="-size 8", command = lambda: aboutprog(), width = 15, height = 2)
about_prog.place(x = 10, y = 55)

canv.bind("<1>", add_dot_click)
canv.bind("<3>", close_dot_click)


win.mainloop()
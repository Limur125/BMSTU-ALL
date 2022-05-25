from re import I
from tkinter import *
from tkinter import messagebox, ttk
from math import sqrt
import numpy as np
import colorutils as cu

def rgb_to_hex(rgb):
    return f'#{rgb[0]:02x}{rgb[1]:02x}{rgb[2]:02x}'

WIN_WIDTH = 1600
WIN_HEIGHT = 900
WIN_COLOR = 'dark slate gray'

CV_WIDE = 900
CV_HEIGHT = 900
CV_COLOR = "#000000" #f3e6ff" #"#cce6ff"
MAIN_TEXT_COLOR = "#b566ff" #"lightblue" a94dff
FONT_COLOR = 'linen'

FILL_COLOR = "#ff6e41"

# Define

X_MIN = 0
X_MAX = 1
Y_MIN = 2
Y_MAX = 3

X_DOT = 0
Y_DOT = 1

win = Tk()
win['bg'] = WIN_COLOR
win.geometry("%dx%d" %(WIN_WIDTH, WIN_HEIGHT))
win.attributes("-fullscreen", True)

canv = Canvas(win, width = CV_WIDE, height = CV_HEIGHT, bg = CV_COLOR)
canv.place(x = 0, y = 0)

canv.delete("all")

def clear_canvas():
    canv.delete("all")


def reboot_prog():
    global lines
    global rect

    canv.delete("all")

    lines = [[]]
    rect = [-1, -1, -1, -1]


def parse_color(num_color):
    color = "orange"

    if (num_color == 1):
        color = "#ff6e41" #"orange"
    elif (num_color == 2):
        color = "#ff5733" #"red"
    elif (num_color == 3):
        color = "#0055ff" #"blue"
    elif (num_color == 4):
        color = "#45ff00" #"green"

    return color


def add_rect_click1(event):
    global is_set_rect

    is_set_rect = False


def add_rect_click(event):
    global rect
    global is_set_rect

    cutter_color = 'white'

    if (is_set_rect == False):
        rect[X_MIN] = event.x
        rect[Y_MAX] = event.y

        is_set_rect = True
    else:
        x_first = rect[X_MIN]
        y_first = rect[Y_MAX]

        x = event.x
        y = event.y

        canv.delete("all")
        canv.create_rectangle(x_first, y_first, x, y, outline = cutter_color)

        rect[X_MAX] = x
        rect[Y_MIN] = y

        draw_lines()

    
def add_rect():
    global rect

    try:
        x_min = int(xleft_cutter_entry.get())
        y_max = int(yleft_cutter_entry.get())
        x_max = int(xright_cutter_entry.get())
        y_min = int(yright_cutter_entry.get())
    except:
        messagebox.showinfo("Ошибка", "Неверно введены координаты")
        return

    cutter_color = 'white'

    canv.delete("all")
    canv.create_rectangle(x_min, y_max, x_max, y_min, outline = cutter_color)

    rect = [x_min, x_max, y_min, y_max]

    draw_lines()


def draw_lines():
    for line in lines:
        if (len(line) != 0):
            x1 = line[0][0]
            y1 = line[0][1]

            x2 = line[1][0]
            y2 = line[1][1]

            canv.create_line(x1, y1, x2, y2, fill = "#ff00ff")

def add_line_click(event):
    global lines
    line_color = '#ff00ff'
    
    x = event.x
    y = event.y

    if (len(lines[-1]) == 0):
        lines[-1].append((x, y))
    else:
        lines[-1].append((x, y))
        lines.append(list())

        x1 = lines[-2][0][0]
        y1 = lines[-2][0][1]

        x2 = lines[-2][1][0]
        y2 = lines[-2][1][1]

        canv.create_line(x1, y1, x2, y2, fill = line_color)


def add_line():
    global lines

    try:
        x1 = int(x_start_line_entry.get())
        y1 = int(y_start_line_entry.get())
        x2 = int(x_end_line_entry.get())
        y2 = int(y_end_line_entry.get())
    except:
        messagebox.showinfo("Ошибка", "Неверно введены координаты")
        return
    line_color = '#ff00ff'

    lines[-1].append((x1, y1))
    lines[-1].append((x2, y2))
    lines[-1].append(line_color)

    lines.append(list())
    
    canv.create_line(x1, y1, x2, y2, fill = line_color)


# Algorithm

def get_dot_bits(rect, dot):
    bits = 0b0000

    if (dot[X_DOT] < rect[X_MIN]):
        bits += 0b0001

    if (dot[X_DOT] > rect[X_MAX]):
        bits += 0b0010
    
    if (dot[Y_DOT] > rect[Y_MIN]):
        bits += 0b0100
        
    if (dot[Y_DOT] < rect[Y_MAX]):
        bits += 0b1000

    return bits

def get_bit(dot_bits, i):
    return (dot_bits >> i) & 1

def line_len(x1, y1, x2, y2):
    return sqrt((x1 - x2) ** 2 + (y1 - y2) ** 2)

def mid_point_clip(rect, line):
    x1, y1 = line[0][X_DOT], line[0][Y_DOT]
    x2, y2 = line[1][X_DOT], line[1][Y_DOT]
    i = 1
    while True:
        code_start = get_dot_bits(rect, (x1, y1))
        code_end = get_dot_bits(rect, (x2, y2))
        if (code_start | code_end) == 0:
            return (x1, y1), (x2, y2)

        if code_start & code_end:
            return
        
        if i > 2:
            return (x1, y1), (x2, y2)

        xr, yr = x1, y1

        if code_end == 0:
            x1, y1, x2, y2 = x2, y2, xr, yr
            i += 1
            continue
        
        while line_len(x1, y1, x2, y2) > 0.01:
            xm = (x1 + x2) / 2
            ym = (y1 + y2) / 2
            xt, yt = x1, y1
            x1, y1 = xm, ym
            code_start = get_dot_bits(rect, (x1, y1))
            code_end = get_dot_bits(rect, (x2, y2))
            if code_start & code_end:
                x1, y1 = xt, yt
                x2, y2 = xm, ym
        x1, y1, x2, y2 = x2, y2, xr ,yr
        i += 1


def cut_area():
    global rect

    if (rect[0] == -1):
        messagebox.showinfo("Ошибка", "Не задан отсекатель")

    rect = [min(rect[0], rect[1]), max(rect[0], rect[1]), max(rect[2], rect[3]), min(rect[2], rect[3])]

    canv.delete("all")

    canv.create_rectangle(rect[X_MIN] + 1, rect[Y_MAX] + 1, rect[X_MAX] - 1, rect[Y_MIN] - 1, outline = "white")
    
    for line in lines:
        if (line):
            res_line = mid_point_clip(rect, line)
            if res_line == None:
                canv.create_line(line[0][0], line[0][1], line[1][0], line[1][1], fill="#ffff00")
                continue
            canv.create_line(line[0][0], line[0][1], res_line[0][0], res_line[0][1], fill="#ffff00")
            canv.create_line(res_line[0][0], res_line[0][1], res_line[1][0], res_line[1][1], fill="#0000ff")
            canv.create_line(res_line[1][0], res_line[1][1], line[1][0], line[1][1], fill="#ffff00")




# Binds

lines = [[]]
canv.bind("<3>", add_line_click)

rect = [-1, -1, -1, -1]
canv.bind("<1>", add_rect_click1)
canv.bind('<B1-Motion>', add_rect_click)

# Add cutter

cutter_text = Label(win, text = "Координаты отсекателя", width = 43, font="-size 16")
cutter_text.place(x = CV_WIDE + 20, y = 10)

xleft_cutter_text = Label(text = "Левый верхний  x: ", font="-size 14")
xleft_cutter_text.place(x = CV_WIDE + 20, y = 50)

xleft_cutter_entry = Entry(font="-size 14", width = 9)
xleft_cutter_entry.place(x = CV_WIDE + 210, y = 50)

yleft_cutter_text = Label(text = "y: ", font="-size 14")
yleft_cutter_text.place(x = CV_WIDE + 360, y = 50)

yleft_cutter_entry = Entry(font="-size 14", width = 9)
yleft_cutter_entry.place(x = CV_WIDE + 390, y = 50)


xright_cutter_text = Label(text = "Правый нижний  x: ", font="-size 14")
xright_cutter_text.place(x = CV_WIDE + 20, y = 90)

xright_cutter_entry = Entry(font="-size 14", width = 9)
xright_cutter_entry.place(x = CV_WIDE + 210, y = 90)

yright_cutter_text = Label(text = "y: ", font="-size 14")
yright_cutter_text.place(x = CV_WIDE + 360, y = 90)

yright_cutter_entry = Entry(font="-size 14", width = 9)
yright_cutter_entry.place(x = CV_WIDE + 390, y = 90)


add_cutter_btn = Button(win, text = "Нарисовать отсекатель", font="-size 14", command = lambda: add_rect())
add_cutter_btn.place(x = CV_WIDE + 170, y = 130)

# Line

cutter_text = Label(win, text = "Добавить отрезок", width = 43, font="-size 16")
cutter_text.place(x = CV_WIDE + 20, y = 190)


x_start_line_text = Label(text = "Начало x: ", font="-size 14")
x_start_line_text.place(x = CV_WIDE + 20, y = 230)

x_start_line_entry = Entry(font="-size 14", width = 9)
x_start_line_entry.place(x = CV_WIDE + 130, y = 230)

y_start_line_text = Label(text = "y: ", font="-size 14")
y_start_line_text.place(x = CV_WIDE + 360, y = 230)

y_start_line_entry = Entry(font="-size 14", width = 9)
y_start_line_entry.place(x = CV_WIDE + 390, y = 230)


x_end_line_text = Label(text = "Конец  x: ", font="-size 14")
x_end_line_text.place(x = CV_WIDE + 20, y = 270)

x_end_line_entry = Entry(font="-size 14", width = 9)
x_end_line_entry.place(x = CV_WIDE + 130, y = 270)

y_end_line_text = Label(text = "y: ", font="-size 14")
y_end_line_text.place(x = CV_WIDE + 360, y = 270)

y_end_line_entry = Entry(font="-size 14", width = 9)
y_end_line_entry.place(x = CV_WIDE + 390, y = 270)


add_line_btn = Button(win, text = "Нарисовать отрезок", font="-size 14", command = lambda: add_line())
add_line_btn.place(x = CV_WIDE + 190, y = 305)

cut_btn = Button(win, text = "Отсечь", width = 18, height = 2, font="-size 14", command = lambda: cut_area())
cut_btn.place(x = CV_WIDE + 20, y = 630)

clear_btn = Button(win, text = "Очистить экран", width = 18, height = 2, font="-size 14", command = lambda: reboot_prog())
clear_btn.place(x = CV_WIDE + 390, y = 630)

exit_btn = Button(win, text = "Выход", font="-size 15", command = lambda: win.quit(), width = 12, height=2)
exit_btn.place(x = CV_WIDE + 240, y = 630)

win.mainloop()
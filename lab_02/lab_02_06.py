from functools import total_ordering
from tkinter import *
from math import atan, degrees, radians, sqrt, pi, cos, sin
import numpy
import tkinter.messagebox as mb
from numpy import linspace

OX = 725
OY = 400

Total_kx = 1.0
Total_ky = 1.0

PICTURE = []
WIN_WIDTH = 1400
WIN_HEIGHT = 900
WIN_COLOR = "#66b3ff"

CV_WIDE = 1000
CV_HEIGHT = 800
СV_COLOR = "#cce6ff"


TEXT_COLOR = "lightblue"


EPS = 0.14#0.1973

X_CENTER = 0
Y_CENTER = 0

win = Tk()
canv = Canvas(win, width=800, height=800, bg='gray')
canv.pack(expand=True, fill=BOTH)

OY_LINE = canv.create_line(OY - 1000000, OX, OY + 1000000, OX)
OX_LINE = canv.create_line(OY, OX - 1000000, OY, OX + 1000000)
ZERO = canv.create_oval(OY, OX, OY, OX, state=HIDDEN)

def init_picture():
    canv.delete(ALL)
    global PICTURE; PICTURE.clear()
    global OY_LINE; OY_LINE = canv.create_line(OY - 1000000, OX, OY + 1000000, OX)
    global OX_LINE; OX_LINE = canv.create_line(OY, OX - 1000000, OY, OX + 1000000)
    global ZERO; ZERO = canv.create_oval(OY, OX, OY, OX, state=HIDDEN)
    global Total_kx; Total_kx = 1.0
    global Total_ky; Total_ky = 1.0

    PICTURE.append(canv.create_oval(OY, OX - 340, OY, OX - 340, state=HIDDEN))
    PICTURE.append(canv.create_line(OY + 235, OX, OY + 215, OX - 40, OY + 95, OX - 40, OY + 75, OX, OY + 235, OX, OY + 215, OX))
    PICTURE.append(canv.create_line(OY - 235, OX, OY - 215, OX - 40, OY - 95, OX - 40, OY - 75, OX, OY - 235, OX, OY - 215, OX))
    PICTURE.append(canv.create_line(OY + 95, OX - 40, OY + 95, OX - 130, OY + 215, OX - 130, OY + 215, OX - 40))
    PICTURE.append(canv.create_line(OY - 95, OX - 40, OY - 95, OX - 130, OY - 215, OX - 130, OY - 215, OX - 40))
    PICTURE.append(canv.create_line(OY + 75, OX - 130, OY + 75, OX - 150, OY + 235, OX - 150, OY + 235, OX - 130, OY + 75, OX - 130))
    PICTURE.append(canv.create_line(OY - 75, OX - 130, OY - 75, OX - 150, OY - 235, OX - 150, OY - 235, OX - 130, OY - 75, OX - 130))
    PICTURE.append(canv.create_line(OY + 95, OX - 150, OY + 115, OX - 495, OY + 195, OX - 495, OY + 215, OX - 150))
    PICTURE.append(canv.create_line(OY - 95, OX - 150, OY - 115, OX - 495, OY - 195, OX - 495, OY - 215, OX - 150))
    PICTURE.append(canv.create_line(OY - 155, OX - 495, OY - 155, OX - 525, OY + 155, OX - 525, OY + 155, OX - 495, OY - 155, OX - 495))
    PICTURE.append(canv.create_line(OY - 155, OX - 510, OY + 155, OX - 510))
    PICTURE.append(canv.create_line(OY - 195, OX - 525, OY - 195, OX - 560, OY + 195, OX - 560, OY + 195, OX - 525, OY - 195, OX - 525))
    PICTURE.append(canv.create_line(OY - 195, OX - 560, OY, OX - 675, OY + 195, OX - 560))

    ellips = []
    for t in linspace(-pi, 0, 100):
        ellips.append(95 * cos(t) + OY)
        ellips.append(305 * sin(t) + OX - 150)
    PICTURE.append(canv.create_line(tuple(ellips)))
    ellips.clear()
    for t in linspace(-pi + atan(3/4), -atan(3/4), 100):
        ellips.append(135 * cos(t) + OY)
        ellips.append(345 * sin(t) + OX - 150)
    PICTURE.append(canv.create_line(tuple(ellips)))


def move_vert(dy, axes_move=False):
    global Total_ky
    pic_copy = PICTURE.copy()
    if axes_move == True:
        pic_copy.append(ZERO)
        pic_copy.append(OX_LINE)
        pic_copy.append(OY_LINE)
    for obj in pic_copy:
        canv.move(obj, 0, dy * Total_ky)

def move_horiz(dx, axes_move=False):
    global Total_kx
    pic_copy = PICTURE.copy()
    if axes_move == True:
        pic_copy.append(ZERO)
        pic_copy.append(OX_LINE)
        pic_copy.append(OY_LINE)
    for obj in pic_copy:
        canv.move(obj, dx * Total_kx, 0)

def _rot(xc, yc, x, y, angle):
    x -= xc
    y -= yc
    _x = x * cos(angle) + y * sin(angle)
    _y = -x * sin(angle) + y * cos(angle)
    return _x + xc, _y + yc

def rotate(angle, xc=None, yc=None):
    if xc == None or yc == None:
        center_coords = canv.coords(PICTURE[0])
        xc = center_coords[0]
        yc = center_coords[1]
    for obj in PICTURE:
        obj_coords = canv.coords(obj)
        _obj_coords = []
        for i in range(len(obj_coords) // 2):
            _x, _y = _rot(xc, yc, obj_coords[i * 2], obj_coords[i * 2 + 1], angle)
            _obj_coords.append(_x)
            _obj_coords.append(_y)
        canv.coords(obj, tuple(_obj_coords))

def zoom_event(event):
    global Total_ky, Total_kx
    if (event.delta > 0):
        zoom(1.1, 1.1, event.x, event.y)
        Total_kx *= 1.1
        Total_ky *= 1.1
    elif (event.delta < 0):
        zoom(0.9, 0.9, event.x, event.y)
        Total_kx *= 0.9
        Total_ky *= 0.9

def zoom(kx, ky, xc, yc):
    pic_copy = PICTURE.copy()
    pic_copy.append(ZERO)
    pic_copy.append(OX_LINE)
    pic_copy.append(OY_LINE)
    for obj in pic_copy:
        obj_coords = canv.coords(obj)
        _obj_coords = []
        for i in range(len(obj_coords) // 2):
            _x, _y = _zoom(xc, yc, obj_coords[i * 2], obj_coords[i * 2 + 1], kx, ky)
            _obj_coords.append(_x)
            _obj_coords.append(_y)
        canv.coords(obj, tuple(_obj_coords))

def _zoom(xc, yc, x, y, kx, ky):
    x -= xc
    y -= yc
    _x = x * kx
    _y = y * ky
    return _x + xc, _y + yc

def rotate_io():
    global Total_kx, Total_ky
    try:
        x_c = float(center_x.get()) * Total_kx 
        y_c = -float(center_y.get()) * Total_ky
    except:
        mb.showerror("Ошибка", "Неверно введены координаты центра поворота")
        center_x.delete(0, END)
        center_y.delete(0, END)
        return

    try:
        angle = radians(float(spin_angle.get()))
    except:
        mb.showerror("Ошибка", "Неверно введен угол поворота")
        spin_angle.delete(0, END)
        return

    rotate(angle, canv.coords(ZERO)[0] + x_c, canv.coords(ZERO)[1] + y_c)

def zoom_io():
    try:
        x_c = float(center_x.get()) * Total_kx
        y_c = -float(center_y.get()) * Total_ky
    except:
        mb.showerror("Ошибка", "Неверно введены координаты центра поворота и масштабирования")
        center_x.delete(0, END)
        center_y.delete(0, END)
        return

    try:
        kx = float(scale_x.get())  
        ky = float(scale_y.get()) 
    except:
        mb.showerror("Ошибка", "Неверно введены коэффициенты масштабирования")
        scale_x.delete(0, END)
        scale_y.delete(0, END)
        return

    zoom(kx, ky, canv.coords(ZERO)[0] + x_c, canv.coords(ZERO)[1] + y_c)

def move_io():
    try:
        dx = float(move_x.get())
        dy = float(move_y.get())
    except:
        mb.showerror("Ошибка", "Неверно введена величина смещения")
        return
    move_vert(dy)
    move_horiz(dx)

def focus(event):
    win.focus_set()

init_picture()

win['bg'] = WIN_COLOR
win.geometry("%dx%d" %(WIN_WIDTH, WIN_HEIGHT))
win.attributes("-fullscreen", True)


x_history = []
y_history = []

# Center
center_label = Label(win, text = "Центр(для масштабирования и поворота)", font="-family {Consolas} -size 16", bg = WIN_COLOR)
center_label.place (x = CV_WIDE + 15, y = 20)

center_x_label = Label(win, text = "X:", font="-family {Consolas} -size 14", bg = WIN_COLOR)
center_x_label.place(x = CV_WIDE + 70, y = 50)
center_x = Entry(win, font="-family {Consolas} -size 14", width = 9)
center_x.insert(END, "0")
center_x.place (x = CV_WIDE + 100, y = 50)

center_y_label = Label(win, text = "Y:", font="-family {Consolas} -size 14", bg = WIN_COLOR)
center_y_label.place(x = CV_WIDE + 270, y = 50)
center_y = Entry(win, font="-family {Consolas} -size 14", width = 9)
center_y.insert(END, "0")
center_y.place (x = CV_WIDE + 300, y = 50)

# Spin
spin_label = Label(win, text = "Поворот", width = 36, font="-family {Consolas} -size 18", bg = TEXT_COLOR)
spin_label.place(x = CV_WIDE + 1, y = 110)

spin_angle_label = Label(win, text = "Угол°: ", font="-family {Consolas} -size 16", bg = WIN_COLOR)
spin_angle_label.place(x = CV_WIDE + 160, y = 155)
spin_angle = Entry(win, font="-family {Consolas} -size 16", width = 9)
spin_angle.insert(END, "0")
spin_angle.place (x = CV_WIDE + 240, y = 155)

spin_btn = Button(win, text = "Повернуть", font="-family {Consolas} -size 14", command = lambda: rotate_io(), width = 15, height = 2, bg = TEXT_COLOR)
spin_btn.place(x = CV_WIDE + 160, y = 200)

# Scale
scale_label = Label(win, text = "Масштабирование", width = 36, font="-family {Consolas} -size 18", bg = TEXT_COLOR)
scale_label.place(x = CV_WIDE + 1, y = 300)

scale_x_label = Label(win, text = "kx: ", font="-family {Consolas} -size 16", bg = WIN_COLOR)
scale_x_label.place(x = CV_WIDE + 100, y = 360)
scale_x = Entry(win, font="-family {Consolas} -size 14", width = 9)
scale_x.insert(END, "1")
scale_x.place (x = CV_WIDE + 140, y = 360)

scale_y_label = Label(win, text = "ky: ", font="-family {Consolas} -size 16", bg = WIN_COLOR)
scale_y_label.place(x = CV_WIDE + 270, y = 360)
scale_y = Entry(win, font="-family {Consolas} -size 14", width = 9)
scale_y.insert(END, "1")
scale_y.place (x = CV_WIDE + 310, y = 360)

scale_btn = Button(win, text = "Масштабировать", font="-family {Consolas} -size 14", command = lambda: zoom_io(), width = 15, height = 2, bg = TEXT_COLOR)
scale_btn.place(x = CV_WIDE + 160, y = 420)

# Move
move_label = Label(win, text = "Перемещение", width = 36, font="-family {Consolas} -size 18", bg = TEXT_COLOR)
move_label.place(x = CV_WIDE + 1, y = 520)

move_x_label = Label(win, text = "dx: ", font="-family {Consolas} -size 16", bg = WIN_COLOR)
move_x_label.place(x = CV_WIDE + 100, y = 580)
move_x = Entry(win, font="-family {Consolas} -size 14", width = 9)
move_x.insert(END, "0")
move_x.place (x = CV_WIDE + 140, y = 580)

move_y_label = Label(win, text = "dy: ", font="-family {Consolas} -size 16", bg = WIN_COLOR)
move_y_label.place(x = CV_WIDE + 270, y = 580)
move_y = Entry(win, font="-family {Consolas} -size 14", width = 9)
move_y.insert(END, "0")
move_y.place (x = CV_WIDE + 310, y = 580)

move_btn = Button(win, text = "Передвинуть", font="-family {Consolas} -size 14", command = lambda: move_io(), width = 15, height = 2, bg = TEXT_COLOR)
move_btn.place(x = CV_WIDE + 160, y = 640)

quit_btn = Button(win, text="Выход",font="-family {Consolas} -size 14", command = lambda: win.quit(), width = 9, height = 2, bg = TEXT_COLOR )
quit_btn.place(x = CV_WIDE + 1, y = 760)


step_back = Button(win, text = "Шаг назад", font="-family {Consolas} -size 14", command = lambda: step_backing(), width = 15, height = 2, bg = TEXT_COLOR)
step_back.place(x = CV_WIDE + 120, y = 760)

clear = Button(win, text = "Сбросить", font="-family {Consolas} -size 14", command = lambda: init_picture(), width = 15, height = 2, bg = TEXT_COLOR)
clear.place(x = CV_WIDE + 300, y = 760)


win.bind('<Up>', lambda event: move_vert(-10))
win.bind('<Right>', lambda event: move_horiz(10))
win.bind('<Left>', lambda event: move_horiz(-10))
win.bind('<Down>', lambda event: move_vert(10))
win.bind('<w>', lambda event: move_vert(-10, True))
win.bind('<d>', lambda event: move_horiz(10, True))
win.bind('<a>', lambda event: move_horiz(-10, True))
win.bind('<s>', lambda event: move_vert(10, True))
win.bind('<q>', lambda event: rotate(0.1))
win.bind('<e>', lambda event: rotate(-0.1))
canv.bind('<Button-1>', focus)
canv.bind('<MouseWheel>', zoom_event)


win.mainloop()

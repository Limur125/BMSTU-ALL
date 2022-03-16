from tkinter import *
from math import atan, radians, pi, cos, sin
import tkinter.messagebox as mb
from numpy import linspace

OX = 725
OY = 400

Total_kx = 1.0
Total_ky = 1.0

ZOOM_IN_EVENT_K = 1.1
ZOOM_OUT_EVENT_K = 0.9
MOVE_EVENT_D = 10
ROT_EVENT_ANGLE = 0.02 * pi


PICTURE = []
PICTURE_LOG = []
PICTURE_LOG_MAX_LEN = 100

LABEL_COLOR = "old lace"

CV_WIDE = 1000
CV_HEIGHT = 800
CV_COLOUR = 'old lace'

TEXT_COLOR = "lightblue"

win = Tk()
canv = Canvas(win, width=CV_WIDE, height=CV_HEIGHT, bg=CV_COLOUR)
canv.pack(expand=True, fill=BOTH)

ZERO = canv.create_oval(OY, OX, OY, OX, state=HIDDEN)

def init_picture():
    canv.delete(ALL)
    global PICTURE; PICTURE.clear()
    global PICTURE_LOG; PICTURE_LOG.clear()
    global ZERO; ZERO = canv.create_oval(OY, OX, OY, OX, state=HIDDEN)
    global Total_kx; Total_kx = 1.0
    global Total_ky; Total_ky = 1.0
    x0 = 0
    y0 = 0
    global OY_LINE; OY_LINE = [x0 - 1000000, y0, x0 + 1000000, y0]
    global OX_LINE; OX_LINE = [x0, y0 - 1000000, x0, y0 + 1000000]
    
    PICTURE.append([x0, y0 - 340, x0, y0 - 340]) 
    PICTURE.append([x0 + 235, y0, x0 + 215, y0 - 40, x0 + 95, y0 - 40, x0 + 75, y0, x0 + 235, y0, x0 + 215, y0])
    PICTURE.append([x0 - 235, y0, x0 - 215, y0 - 40, x0 - 95, y0 - 40, x0 - 75, y0, x0 - 235, y0, x0 - 215, y0])
    PICTURE.append([x0 + 95, y0 - 40, x0 + 95, y0 - 130, x0 + 215, y0 - 130, x0 + 215, y0 - 40])
    PICTURE.append([x0 - 95, y0 - 40, x0 - 95, y0 - 130, x0 - 215, y0 - 130, x0 - 215, y0 - 40])
    PICTURE.append([x0 + 75, y0 - 130, x0 + 75, y0 - 150, x0 + 235, y0 - 150, x0 + 235, y0 - 130, x0 + 75, y0 - 130])
    PICTURE.append([x0 - 75, y0 - 130, x0 - 75, y0 - 150, x0 - 235, y0 - 150, x0 - 235, y0 - 130, x0 - 75, y0 - 130])
    PICTURE.append([x0 + 95, y0 - 150, x0 + 115, y0 - 495, x0 + 195, y0 - 495, x0 + 215, y0 - 150])
    PICTURE.append([x0 - 95, y0 - 150, x0 - 115, y0 - 495, x0 - 195, y0 - 495, x0 - 215, y0 - 150])
    PICTURE.append([x0 - 155, y0 - 495, x0 - 155, y0 - 525, x0 + 155, y0 - 525, x0 + 155, y0 - 495, x0 - 155, y0 - 495])
    PICTURE.append([x0 - 155, y0 - 510, x0 + 155, y0 - 510])
    PICTURE.append([x0 - 195, y0 - 525, x0 - 195, y0 - 560, x0 + 195, y0 - 560, x0 + 195, y0 - 525, x0 - 195, y0 - 525])
    PICTURE.append([x0 - 195, y0 - 560, x0, y0 - 675, x0 + 195, y0 - 560])
    ellips = []
    for t in linspace(-pi, 0, 100):
        ellips.append(95 * cos(t) + x0)
        ellips.append(305 * sin(t) + y0 - 150)
    PICTURE.append(ellips)
    ellips.clear()
    for t in linspace(-pi + atan(3/4), -atan(3/4), 100):
        ellips.append(135 * cos(t) + x0)
        ellips.append(345 * sin(t) + y0 - 150)
    PICTURE.append(ellips)
    draw_picture()

def draw_picture():
    global ZERO
    x0 = canv.coords(ZERO)[0]
    y0 = canv.coords(ZERO)[1]
    canv.delete(ALL)
    ZERO = canv.create_oval(x0, y0, x0, y0, state=HIDDEN)
    pic_copy = [OX_LINE, OY_LINE] + PICTURE.copy()
    for obj in pic_copy:
        _obj = []
        for i in range(len(obj) // 2):
            _obj.append(obj[i * 2] + x0)
            _obj.append(obj[i * 2 + 1] + y0)
        canv.create_line(tuple(_obj))

def _move(x, y, dx, dy):
    x += dx
    y += dy
    return x, y

def move(dx, dy, axes_move=False, text_move=False):
    global PICTURE_LOG, PICTURE
    if (not text_move and win.focus_get() != canv):
        return
    picture_record = [canv.coords(ZERO), OX_LINE, OY_LINE]
    if axes_move == True:
        for i in PICTURE:
            picture_record.append(i)
        if len(PICTURE_LOG) >= PICTURE_LOG_MAX_LEN:
            PICTURE_LOG = PICTURE_LOG[1:]
        PICTURE_LOG.append(picture_record)
        canv.move(ALL, dx, dy)
        return
    pic_copy = []
    for obj_coords in PICTURE:
        picture_record.append(obj_coords)
        _obj_coords = []
        for i in range(len(obj_coords) // 2):
            _x, _y = _move(obj_coords[i * 2], obj_coords[i * 2 + 1], dx, dy)
            _obj_coords.append(_x)
            _obj_coords.append(_y)
        pic_copy.append(_obj_coords)
    PICTURE = pic_copy
    print(PICTURE)
    draw_picture()
    if len(PICTURE_LOG) >= PICTURE_LOG_MAX_LEN:
        PICTURE_LOG = PICTURE_LOG[1:]
    PICTURE_LOG.append(picture_record)

def _rot(xc, yc, x, y, angle):
    x -= xc
    y -= yc
    _x = x * cos(angle) - y * sin(angle)
    _y = x * sin(angle) + y * cos(angle)
    return _x + xc, _y + yc

def rotate(angle, xc=None, yc=None, text_rot=False):
    global PICTURE_LOG, PICTURE
    if (not text_rot and win.focus_get() != canv):
        return
    if xc == None or yc == None:
        center_coords = PICTURE[0]
        xc = center_coords[0]
        yc = center_coords[1]
    picture_record = [canv.coords(ZERO), OX_LINE, OY_LINE]
    pic_copy = []
    for obj_coords in PICTURE:
        picture_record.append(obj_coords)
        _obj_coords = []
        for i in range(len(obj_coords) // 2):
            _x, _y = _rot(xc, yc, obj_coords[i * 2], obj_coords[i * 2 + 1], angle)
            _obj_coords.append(_x)
            _obj_coords.append(_y)
        pic_copy.append(_obj_coords)
    PICTURE = pic_copy
    draw_picture()
    if len(PICTURE_LOG) >= PICTURE_LOG_MAX_LEN:
        PICTURE_LOG = PICTURE_LOG[1:]
    PICTURE_LOG.append(picture_record)

def zoom_event(event):
    if (event.delta > 0):
        zoom(ZOOM_IN_EVENT_K, ZOOM_IN_EVENT_K, event.x, event.y, axes_scale=True)
    elif (event.delta < 0):
        zoom(ZOOM_OUT_EVENT_K, ZOOM_OUT_EVENT_K, event.x, event.y, axes_scale=True)

def zoom(kx, ky, xc=0, yc=0, text_scale=False, axes_scale=False):
    global PICTURE_LOG, PICTURE
    if (not text_scale and win.focus_get() != canv):
        return
    picture_record = [canv.coords(ZERO), OX_LINE, OY_LINE]
    if axes_scale == True:
        for i in PICTURE:
            picture_record.append(i)
        if len(PICTURE_LOG) >= PICTURE_LOG_MAX_LEN:
            PICTURE_LOG = PICTURE_LOG[1:]
        PICTURE_LOG.append(picture_record)
        canv.scale(ALL, xc, yc, kx, ky)
        return
    pic_copy = []
    for obj_coords in PICTURE:
        picture_record.append(obj_coords)
        _obj_coords = []
        for i in range(len(obj_coords) // 2):
            _x, _y = _zoom(xc, yc, obj_coords[i * 2], obj_coords[i * 2 + 1], kx, ky)
            _obj_coords.append(_x)
            _obj_coords.append(_y)
        pic_copy.append(_obj_coords)
    PICTURE = pic_copy
    print(PICTURE)
    draw_picture()
    if len(PICTURE_LOG) >= PICTURE_LOG_MAX_LEN:
         PICTURE_LOG = PICTURE_LOG[1:]
    PICTURE_LOG.append(picture_record)

def _zoom(xc, yc, x, y, kx, ky):
    x = x * kx + (1 - kx) * xc
    y = y * ky + (1 - ky) * yc
    return x, y

def rotate_io():
    global Total_kx, Total_ky
    try:
        x_c = float(center_x.get())
        y_c = float(center_y.get()) 
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

    rotate(angle, x_c, y_c, True)

def zoom_io():
    try:
        x_c = float(center_x.get())
        y_c = float(center_y.get())
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

    zoom(kx, ky, x_c, y_c, text_scale=True)

def move_io():
    try:
        dx = float(move_x.get())
        dy = float(move_y.get())
    except:
        mb.showerror("Ошибка", "Неверно введена величина смещения")
        return
    move(dx, dy, text_move=True)

def focus(event):
    canv.focus_set()

def step_backing(text_rot=False):
    global PICTURE
    if (not text_rot and win.focus_get() != canv):
        return
    if not len(PICTURE_LOG):
        return
    # print(PICTURE_LOG[-1], '\n', len(pic_copy), len(PICTURE_LOG[-1]))
    PICTURE = PICTURE_LOG[-1][3:]
    PICTURE_LOG.pop()
    draw_picture()

def aboutprog():
    mb.showinfo(title='О программе', message='Нарисовать исходный рисунок, затем его переместить,\
промасштабировать, повернуть')

init_picture()
draw_picture()
focus(None)

win.attributes("-fullscreen", True)

# Center
center_label = Label(win, text = "Центр(для масштабирования и поворота)", font="-size 16", bg = LABEL_COLOR)
center_label.place (x = CV_WIDE + 15, y = 20)

center_x_label = Label(win, text = "X:", font="-size 14", bg = LABEL_COLOR)
center_x_label.place(x = CV_WIDE + 70, y = 50)
center_x = Entry(win, font="-size 14", width = 9)
center_x.insert(END, "0")
center_x.place (x = CV_WIDE + 100, y = 50)

center_y_label = Label(win, text = "Y:", font=" -size 14", bg = LABEL_COLOR)
center_y_label.place(x = CV_WIDE + 270, y = 50)
center_y = Entry(win, font="-size 14", width = 9)
center_y.insert(END, "0")
center_y.place (x = CV_WIDE + 300, y = 50)

# Spin
spin_label = Label(win, text = "Поворот", font="-size 18", bg = LABEL_COLOR)
spin_label.place(x = CV_WIDE + 200, y = 110)

spin_angle_label = Label(win, text = "Угол°: ", font="-size 16", bg = LABEL_COLOR)
spin_angle_label.place(x = CV_WIDE + 160, y = 155)
spin_angle = Entry(win, font="-size 16", width = 9)
spin_angle.insert(END, "0")
spin_angle.place (x = CV_WIDE + 240, y = 155)

spin_btn = Button(win, text = "Повернуть", font="-size 14", command = lambda: rotate_io(), width = 15, height = 2)
spin_btn.place(x = CV_WIDE + 160, y = 200)

# Scale
scale_label = Label(win, text = "Масштабирование", font="-size 18", bg = LABEL_COLOR)
scale_label.place(x = CV_WIDE + 150, y = 300)

scale_x_label = Label(win, text = "kx: ", font="-size 16", bg = LABEL_COLOR)
scale_x_label.place(x = CV_WIDE + 100, y = 360)
scale_x = Entry(win, font="-size 14", width = 9)
scale_x.insert(END, "1")
scale_x.place (x = CV_WIDE + 140, y = 360)

scale_y_label = Label(win, text = "ky: ", font="-size 16", bg = LABEL_COLOR)
scale_y_label.place(x = CV_WIDE + 270, y = 360)
scale_y = Entry(win, font="-size 14", width = 9)
scale_y.insert(END, "1")
scale_y.place (x = CV_WIDE + 310, y = 360)

scale_btn = Button(win, text = "Масштабировать", font="-size 14", command = lambda: zoom_io(), width = 15, height = 2)
scale_btn.place(x = CV_WIDE + 160, y = 420)

# Move
move_label = Label(win, text = "Перемещение", font="-size 18", bg = LABEL_COLOR)
move_label.place(x = CV_WIDE + 170, y = 520)

move_x_label = Label(win, text = "dx: ", font="-size 16", bg = LABEL_COLOR)
move_x_label.place(x = CV_WIDE + 100, y = 580)
move_x = Entry(win, font="-size 14", width = 9)
move_x.insert(END, "0")
move_x.place (x = CV_WIDE + 140, y = 580)

move_y_label = Label(win, text = "dy: ", font="-size 16", bg = LABEL_COLOR)
move_y_label.place(x = CV_WIDE + 270, y = 580)
move_y = Entry(win, font="-size 14", width = 9)
move_y.insert(END, "0")
move_y.place (x = CV_WIDE + 310, y = 580)

move_btn = Button(win, text = "Передвинуть", font="-size 14", command = lambda: move_io(), width = 15, height = 2)
move_btn.place(x = CV_WIDE + 160, y = 640)

quit_btn = Button(win, text="Выход",font="-size 14", command = lambda: win.quit(), width = 9, height = 2)
quit_btn.place(x = CV_WIDE + 1, y = 760)

step_back = Button(win, text = "Шаг назад", font="-size 14", command = lambda: step_backing(True), width = 15, height = 2)
step_back.place(x = CV_WIDE + 120, y = 760)

clear = Button(win, text = "Сбросить", font="-size 14", command = lambda: init_picture(), width = 15, height = 2)
clear.place(x = CV_WIDE + 300, y = 760)

credits = Button(win, text = "Об авторе", font="-size 8", command = lambda: mb.showinfo(title='Об авторе', message='Золотухин Алексей ИУ7-44Б'), width = 15, height = 2)
credits.place(x = 10, y = 10)

about_prog = Button(win, text = "О программе", font="-size 8", command = lambda: aboutprog(), width = 15, height = 2)
about_prog.place(x = 10, y = 55)



win.bind('<Up>', lambda event: move(0, -MOVE_EVENT_D))
win.bind('<Right>', lambda event: move(MOVE_EVENT_D, 0))
win.bind('<Left>', lambda event: move(-MOVE_EVENT_D, 0))
win.bind('<Down>', lambda event: move(0, MOVE_EVENT_D))
win.bind('<w>', lambda event: move(0, -MOVE_EVENT_D, axes_move=True))
win.bind('<d>', lambda event: move(MOVE_EVENT_D, 0, axes_move=True))
win.bind('<a>', lambda event: move(-MOVE_EVENT_D, 0, axes_move=True))
win.bind('<s>', lambda event: move(0, MOVE_EVENT_D, axes_move=True))
win.bind('<q>', lambda event: rotate(ROT_EVENT_ANGLE))
win.bind('<e>', lambda event: rotate(-ROT_EVENT_ANGLE))
win.bind('<=>', lambda event: zoom(ZOOM_IN_EVENT_K, ZOOM_IN_EVENT_K))
win.bind('-', lambda event: zoom(ZOOM_OUT_EVENT_K, ZOOM_OUT_EVENT_K))
win.bind('<BackSpace>', lambda event: step_backing())
canv.bind('<Button-1>', focus)
canv.bind('<MouseWheel>', zoom_event)


win.mainloop()

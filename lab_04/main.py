from tkinter import Tk, Button, Label, Entry, Canvas, Radiobutton, HIDDEN, IntVar, ALL
from tkinter import messagebox, ttk
import numpy as np
import matplotlib.pyplot as plt
import time
from circle import draw_circle
from ellips import draw_ellips

SCALING = 1

WIN_WIDTH = 1600
WIN_HEIGHT = 900
WIN_COLOR = 'dark slate gray'

CV_WIDE = 900
CV_HEIGHT = 900
CV_COLOR = "old lace" #f3e6ff" #"#cce6ff"
MAIN_TEXT_COLOR = "#b566ff" #"lightblue" a94dff
FONT_COLOR = 'linen'

BTN_TEXT_COLOR = "#4d94ff"

BOX_COLOR = "#dab3ff"
BOX_WIDTH = 50

NUMBER_OF_RUNS = 20
MAX_RADIUS = 10000
STEP = 1000

win = Tk()
win['bg'] = WIN_COLOR
win.geometry("%dx%d" %(WIN_WIDTH, WIN_HEIGHT))
win.attributes("-fullscreen", True)

canv = Canvas(win, width = CV_WIDE, height = CV_HEIGHT, bg = CV_COLOR)
canv.place(x = 0, y = 0)

ZERO = canv.create_oval(CV_WIDE // 2, CV_HEIGHT // 2, CV_WIDE // 2, CV_HEIGHT // 2, state=HIDDEN)

def rgb_to_hex(rgb):
    return f'#{rgb[0]:02x}{rgb[1]:02x}{rgb[2]:02x}'

def render_figure(coords):
    x0 = canv.coords(ZERO)[0]
    y0 = canv.coords(ZERO)[1]
    for pixel in coords: 
        x = pixel[0] * SCALING + x0
        y = -pixel[1] * SCALING + y0
        canv.create_polygon(x, y , x + SCALING, y, x + SCALING, y + SCALING, x, y + SCALING, fill=rgb_to_hex(pixel[2]))
        x = pixel[0] * SCALING + x0
        y = pixel[1] * SCALING + y0
        canv.create_polygon(x, y , x + SCALING, y, x + SCALING, y + SCALING, x, y + SCALING, fill=rgb_to_hex(pixel[2]))
        x = -pixel[0] * SCALING + x0
        y = pixel[1] * SCALING + y0
        canv.create_polygon(x, y , x + SCALING, y, x + SCALING, y + SCALING, x, y + SCALING, fill=rgb_to_hex(pixel[2]))
        x = -pixel[0] * SCALING + x0
        y = -pixel[1] * SCALING + y0
        canv.create_polygon(x, y , x + SCALING, y, x + SCALING, y + SCALING, x, y + SCALING, fill=rgb_to_hex(pixel[2]))
        

def figure_io(method, color, figure):
    try:
        x_c = float(xc_fig_entry.get())
        y_c = float(yc_fig_entry.get())
        match figure:
            case 1:
                r = float(rad_circle_entry.get())
                coords = draw_circle(x_c, y_c, r, method, color)
                render_figure(coords)
            case 2:
                r_a = float(rad_a_ellips_entry.get())
                r_b = float(rad_b_ellips_entry.get())
                coords = draw_ellips(x_c, y_c, r_a, r_b, method, color)
                render_figure(coords)
    except:
        messagebox.showerror("Ошибка", "Неверно данные")

    return

def spektr_io(method, color, figure):
    try:
        x_c = float(xc_fig_entry.get())
        y_c = float(yc_fig_entry.get())
        match figure:
            case 1:
                r = float(rad_begin_crcl_entry.get())
                n = int(amount_crcl_entry.get())
                step = float(rad_step_crcl_entry.get())
                for i in range(n):
                    coords = draw_circle(x_c, y_c, r, method, color)
                    render_figure(coords)
                    r += step
            case 2:
                r_a = float(rad_a_elps_entry.get())
                r_b = float(rad_b_elps_entry.get())
                c = r_b / r_a
                n = int(amount_elps_entry.get())
                step = float(rad_step_elps_entry.get())
                for i in range(n):
                    coords = draw_ellips(x_c, y_c, r_a, r_b, method, color)
                    render_figure(coords)
                    r_b += step * c
                    r_a += step
    except:
        messagebox.showerror("Ошибка", "Неверно данные")
    
    return

def time_io(figure):
    time_res = []
    try:
        x_c = float(xc_fig_entry.get())
        y_c = float(yc_fig_entry.get())
        match figure:
            case 1:
                name = "окружность"
                _r = float(rad_begin_crcl_entry.get())
                n = int(amount_crcl_entry.get())
                step = float(rad_step_crcl_entry.get())
                for method in range(4):
                    time_start = [0] * n
                    time_end = [0] * n
                    for _ in range(NUMBER_OF_RUNS):
                        r = _r
                        for k in range(n):  
                            time_start[k] += time.time()
                            draw_circle(x_c, y_c, r, method, 0)
                            time_end[k] += time.time()
                            r += step
                    size = len(time_start)
                    res_time = list((time_end[j] - time_start[j]) / NUMBER_OF_RUNS for j in range(size))
                    time_res.append(res_time) 
            case 2:
                name = "эллипс"
                r_a = float(rad_a_elps_entry.get())
                _r = r_a
                r_b = float(rad_b_elps_entry.get())
                c = r_b / r_a
                n = int(amount_elps_entry.get())
                step = float(rad_step_elps_entry.get())
                for method in range(4):
                    time_start = [0] * n
                    time_end = [0] * n
                    for _ in range(NUMBER_OF_RUNS):
                        for k in range(n):  
                            time_start[k] += time.time()
                            draw_ellips(x_c, y_c, r_a, r_b, method, 0)
                            time_end[k] += time.time()
                            r_b += step * c
                            r_a += step  
                    size = len(time_start)
                    res_time = list((time_end[j] - time_start[j]) / NUMBER_OF_RUNS for j in range(size))
                    time_res.append(res_time)                
    except:
       messagebox.showerror("Ошибка", "Неверно данные")
    print(time_res)
    print(r)
    rad_arr = np.arange(_r, _r + n * step, step)
    plt.title("Замеры времени для различных методов\nФигура: " + name)
    plt.plot(rad_arr, time_res[0], label = "Каноническое\nуравнеие")
    plt.plot(rad_arr, time_res[1], label = "Параметрическое\nуравнение")
    plt.plot(rad_arr, time_res[2], label = "Брезенхем")
    plt.plot(rad_arr, time_res[3], label = "Алгоритм\nсредней точки")
    plt.xticks(np.arange(_r, _r + n * step, step))
    plt.legend()
    plt.ylabel("Время")
    plt.xlabel("Величина радиуса")
    plt.show()
    return

def change_figure(opt_figure):
    match opt_figure:
        case 1:
            rad_a_ellips_text.place_forget()
            rad_a_ellips_entry.place_forget()

            rad_b_ellips_text.place_forget()
            rad_b_ellips_entry.place_forget()

            rad_step_elps_text.place_forget()
            rad_step_elps_entry.place_forget()

            amount_elps_text.place_forget()
            amount_elps_entry.place_forget()

            rad_a_elps_text.place_forget()
            rad_a_elps_entry.place_forget()

            rad_b_elps_text.place_forget()
            rad_b_elps_entry.place_forget()

            rad_circle_text.place(x = CV_WIDE + 250, y = 340)
            rad_circle_entry.place(x = CV_WIDE + 290, y = 340)

            rad_step_crcl_text.place(x = CV_WIDE + 60, y = 500)
            rad_step_crcl_entry.place(x = CV_WIDE + 110, y = 500)

            amount_crcl_text.place(x = CV_WIDE + 210, y = 500)
            amount_crcl_entry.place(x = CV_WIDE + 330, y = 500)

            rad_begin_crcl_text.place(x = CV_WIDE + 430, y = 500)
            rad_begin_crcl_entry.place(x = CV_WIDE + 500, y = 500)
        case 2:
            rad_circle_text.place_forget()
            rad_circle_entry.place_forget()

            rad_step_crcl_text.place_forget()
            rad_step_crcl_entry.place_forget()

            amount_crcl_text.place_forget()
            amount_crcl_entry.place_forget()

            rad_begin_crcl_text.place_forget()
            rad_begin_crcl_entry.place_forget()

            rad_a_ellips_text.place(x = CV_WIDE + 90, y = 340)
            rad_a_ellips_entry.place(x = CV_WIDE + 150, y = 340)

            rad_b_ellips_text.place(x = CV_WIDE + 370, y = 340)
            rad_b_ellips_entry.place(x = CV_WIDE + 430, y = 340)

            rad_step_elps_text.place(x = CV_WIDE + 40, y = 500)
            rad_step_elps_entry.place(x = CV_WIDE + 160, y = 500)

            amount_elps_text.place(x = CV_WIDE + 340, y = 500)
            amount_elps_entry.place(x = CV_WIDE + 460, y = 500)

            rad_a_elps_text.place(x = CV_WIDE + 70, y = 550)
            rad_a_elps_entry.place(x = CV_WIDE + 160, y = 550)

            rad_b_elps_text.place(x = CV_WIDE + 370, y = 550)
            rad_b_elps_entry.place(x = CV_WIDE + 460, y = 550)

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

# Choose figure

color_text = Label(win, text = "Выбрать фигуру", width = BOX_WIDTH, font="-size 16", bg = WIN_COLOR, fg=FONT_COLOR)
color_text.place(x = CV_WIDE + 20, y = 20)

option_figure = IntVar()
option_figure.set(1)

figure_circle = Radiobutton(text = "Окружность", font="-size 14", variable = option_figure, value = 1, bg = WIN_COLOR, fg=FONT_COLOR, activebackground=WIN_COLOR, command = lambda: change_figure(option_figure.get()))
figure_circle.place(x = CV_WIDE + 25, y = 55)

figure_ellips = Radiobutton(text = "Эллипс", font="-size 14", variable = option_figure, value = 2, bg = WIN_COLOR, fg=FONT_COLOR, activebackground=WIN_COLOR, command = lambda: change_figure(option_figure.get()))
figure_ellips.place(x = CV_WIDE + 400, y = 55)

# Method

method_text = Label(win, text = "Алгоритм построения окружности", width = 43, font="-size 16", bg=WIN_COLOR, fg= FONT_COLOR, )
method_text.place(x = CV_WIDE + 60, y = 90)

method_cmbb = ttk.Combobox(win, state='readonly', width=40, font="-size 16")
win.option_add('*TCombobox*Listbox.font', '-size 14')
method_cmbb['values'] = ['Каноническое уравнение', 'Параметрическое уравнение', 'Алгоритм Брезенхема', 'Алгоритм средней точки']
method_cmbb.place(x = CV_WIDE + 60, y = 130)
method_cmbb.current(0)

# Color

color_text = Label(win, text = "Цвет отрезка", width = 43, font="-size 16", bg=WIN_COLOR, fg=FONT_COLOR)
color_text.place(x = CV_WIDE + 60, y = 170)

color_cmbb = ttk.Combobox(win, state='readonly', width=40, font="-size 16")
win.option_add('*TCombobox*Listbox.font', '-size 14')
color_cmbb['values'] = ['Черный', 'Синий', 'Красный', 'Фоновый']
color_cmbb.place(x = CV_WIDE + 60, y = 210)
color_cmbb.current(0)

# Render

line_text = Label(win, text = "Нарисовать фигуру", width=BOX_WIDTH, font="-size 14", bg=WIN_COLOR , fg=FONT_COLOR)
line_text.place(x = CV_WIDE + 40, y = 250)

xc_fig_text = Label(text = "x_с: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
xc_fig_text.place(x = CV_WIDE + 90, y = 290)

xc_fig_entry = Entry(font="-size 14", width = 9)
xc_fig_entry.place(x = CV_WIDE + 150, y = 290)

yc_fig_text = Label(text = "y_с: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
yc_fig_text.place(x = CV_WIDE + 370, y = 290)

yc_fig_entry = Entry(font="-size 14", width = 9)
yc_fig_entry.place(x = CV_WIDE + 430, y = 290)


# Radius

rad_circle_text = Label(text="R: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
rad_circle_entry = Entry(font="-size 14", width=9)

rad_a_ellips_text = Label(text="R_a: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
rad_a_ellips_entry = Entry(font="-size 14", width=9)

rad_b_ellips_text = Label(text = "R_b: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
rad_b_ellips_entry = Entry(font="-size 14", width=9)

draw_circle_btn = Button(win, text="Нарисовать", font="-size 14", command=lambda: figure_io(method_cmbb.current(), color_cmbb.current(), option_figure.get()), width = 20, bg = FONT_COLOR)
draw_circle_btn.place(x = CV_WIDE + 200, y = 390)

# Spektr

spektr_text = Label(win, text = "Нарисовать спектр", width = BOX_WIDTH, font="-size 16", bg=WIN_COLOR, fg=FONT_COLOR)
spektr_text.place(x = CV_WIDE + 20, y = 450)


# Elips
rad_step_elps_text = Label(text = "Шаг по R_a: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
rad_step_elps_entry = Entry(font="-size 14", width = 9)

rad_a_elps_text = Label(text = "R_a нач: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
rad_a_elps_entry = Entry(font="-size 14", width = 9)

rad_b_elps_text = Label(text = "R_b нач: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
rad_b_elps_entry = Entry(font="-size 14", width = 9)

amount_elps_text = Label(text = "Количество: ", font="-size 14", bg=WIN_COLOR, fg=FONT_COLOR)
amount_elps_entry = Entry(font="-size 14", width = 9)

# Circle

rad_step_crcl_text = Label(text = "Шаг: ", font="-size 14",bg=WIN_COLOR, fg=FONT_COLOR)
rad_step_crcl_entry = Entry(font="-size 14", width = 5)

amount_crcl_text = Label(text = "Количество:", font="-size 14",bg=WIN_COLOR, fg=FONT_COLOR)
amount_crcl_entry = Entry(font="-size 14", width = 5)

rad_begin_crcl_text = Label(text = "R нач:", font="-size 14",bg=WIN_COLOR, fg=FONT_COLOR)
rad_begin_crcl_entry = Entry(font="-size 14", width = 5)


draw_spektr_btn = Button(win, text = "Нарисовать", font="-size 14", command = lambda: spektr_io(method_cmbb.current(), color_cmbb.current(), option_figure.get()), width = 20)
draw_spektr_btn.place(x = CV_WIDE + 210, y = 600)

# Control buttons

compare_time_btn = Button(win, text = "Сравнить время", font="-size 15", command = lambda: time_io(option_figure.get()), width = 15, height = 2)
compare_time_btn.place(x = CV_WIDE + 210, y = 700)

clear_win_btn = Button(win, text = "Очистить экран", font="-size 15", command = lambda: clear_picture(), width = 15, height = 2)
clear_win_btn.place(x = CV_WIDE + 420, y = 700)

exit_btn = Button(win, text = "Выход", font="-size 15", command = lambda: win.quit(), width = 12, height=2)
exit_btn.place(x = CV_WIDE + 40, y = 700)

credits = Button(win, text = "Об авторе", font="-size 8", command = lambda: messagebox.showinfo(title='Об авторе', message='Золотухин Алексей ИУ7-44Б'), width = 15, height = 2)
credits.place(x = 10, y = 10)

about_prog = Button(win, text = "О программе", font="-size 8", command = lambda: aboutprog(), width = 15, height = 2)
about_prog.place(x = 10, y = 55)

change_figure(1) # set circle
canv.bind('<MouseWheel>', zoom)

win.mainloop()
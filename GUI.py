from tkinter import *
from math import sqrt
circles_list = []
points_list = []
circle_drawing = []
act_log = []
#==========================================================================================
def str_to_float(str):
    try:
        x = float(str)
        return x
    except:
        return str
#==========================================================================================
def create_circle(xc, yc, r):
    circle_id = canv.create_oval(xc - r, yc - r, xc + r, yc + r, width=3, outline='black')
    circle_drawing.append(r)
    circle_drawing.append(circle_id)
    act_log.append(circle_id)
    circle_drawing.append(-1)
#==========================================================================================
def create_circle_io():
    xc = str_to_float(entxc.get())
    yc = str_to_float(entyc.get())
    r = str_to_float(entr.get())
    lber.grid_forget()
    if type(r) != float or type(xc) != float or type(yc) != float:
        lber.grid(row=7)
        return
    create_circle(xc, yc, r)
#==========================================================================================
def xc_yc_coords(event):
    circle_drawing.clear()
    circle_drawing.append(event.x)
    circle_drawing.append(event.y)
    create_circle(circle_drawing[0], circle_drawing[1], 0.1)
#==========================================================================================
def point_coords(event):
    xp = event.x
    yp = event.y
    point_id = canv.create_oval(xp - 1, yp - 1, xp + 1, yp + 1, width=3, outline='black')
    act_log.append(point_id)
    points_list.append([xp, yp, point_id])
#==========================================================================================
def radius(event):
    x1, y1 = event.x, event.y
    x2, y2 = circle_drawing[0], circle_drawing[1]
    circle_drawing[2] = sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))
    canv.coords(circle_drawing[3], x2 - circle_drawing[2], y2 - circle_drawing[2], x2 + circle_drawing[2], y2 + circle_drawing[2])
#==========================================================================================
def max_intersection_circle():
    for i in range(len(circles_list)):
        canv.itemconfigure(circles_list[i][3], outline='black')
        circles_list[i][4] = -1
    for c1 in circles_list:
        for c2 in circles_list:
            outint = sqrt((c1[0] - c2[0]) ** 2 + (c1[1] - c2[1]) ** 2) > (c1[2] + c2[2])
            inint = sqrt((c1[0] - c2[0]) ** 2 + (c1[1] - c2[1]) ** 2) < abs(c1[2] - c2[2])
            if not (outint or inint):
                c1[4] += 1
    max_intersec = 1
    for i in range(len(circles_list)):
        if circles_list[i][4] > circles_list[max_intersec][4]:
            max_intersec = i
    canv.itemconfigure(circles_list[max_intersec][3], outline='green')
#==========================================================================================
def finish_circle_draw(event):
    circles_list.append(circle_drawing.copy())
    canv.unbind('<Button-1>')
    canv.unbind('<B1-Motion>')
    canv.unbind('<ButtonRelease-1>')

def start_circle_draw():
    circle_drawing.clear()
    canv.bind('<Button-1>', xc_yc_coords)
    canv.bind('<B1-Motion>', radius)
    canv.bind('<ButtonRelease-1>', finish_circle_draw)
#==========================================================================================
def finish_point_draw(event):
    canv.unbind('<Button-1>')
    canv.unbind('<ButtonRelease-1>')

def start_point_draw():
    circle_drawing.clear()
    canv.bind('<Button-1>', point_coords)
    canv.bind('<ButtonRelease-1>', finish_point_draw)
#==========================================================================================
def undo_last_act():
    if len(act_log) != 0:
        canv.delete(act_log[-1])
        if len(circles_list) != 0:
            if circles_list[-1][3] == act_log[-1]:
                circles_list.pop()
        elif len(points_list) != 0:
            if points_list[-1][2] == act_log[-1]:
                points_list.pop()
        act_log.pop()
#==========================================================================================
def zoomer(event):
        if (event.delta > 0):
            canv.scale("all", 0, 0, 2, 2)
        elif (event.delta < 0):
            canv.scale("all", 0, 0, 0.5, 0.5)
        canv.configure(scrollregion = canv.bbox("all"))        
#++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
win = Tk()
#==========================================================================================
canv = Canvas(win, width=600, height=600, bg='white')
canv.create_line(600, 0 , 600, 600, width=3, fill='black')
canv.grid(row=0, column=0)

#==========================================================================================
frm = Frame(win)
# lber = Label(frm, text='Ошибка ввода', fg='red')
# #------------------------------------------------------------------------------------------
# lbxc = Label(frm, text='Координата Х центра окружности')
# lbxc.grid(row=0)
# entxc = Entry(frm)
# entxc.grid(row=1)
# #------------------------------------------------------------------------------------------
# lbyc = Label(frm, text='Координата Y центра окружности')
# lbyc.grid(row=2)
# entyc = Entry(frm)
# entyc.grid(row=3)
# #------------------------------------------------------------------------------------------
# lbr = Label(frm, text='Радиус окружности')
# lbr.grid(row=4)
# entr = Entry(frm)
# entr.grid(row=5)
# #------------------------------------------------------------------------------------------
# input_circle_btn = Button(frm, text='Создать окружность', command=create_circle_io)
# input_circle_btn.grid(row=6)
#------------------------------------------------------------------------------------------
# task_btn = Button(frm, text='Найти окружность по заданию', command=max_intersection_circle)
# task_btn.grid(row=8)
circ_btn = Button(frm, text='◯',command=start_circle_draw)
circ_btn.grid(row=0, column=0)
circ_btn = Button(frm, text='◦',command=start_point_draw)
circ_btn.grid(row=0, column=1)
circ_btn = Button(frm, text='↺',command=undo_last_act)
circ_btn.grid(row=0, column=2)
frm.grid(row=0, column=2)
xsb = Scrollbar(win, orient="horizontal", command=canv.xview)
ysb = Scrollbar(win, orient="vertical", command=canv.yview)
canv.configure(yscrollcommand=ysb.set, xscrollcommand=xsb.set)
canv.configure(scrollregion=(0,0,600,600))
xsb.grid(row=1, column=0, sticky="ew")
ysb.grid(row=0, column=1, sticky="ns")
canv.bind("<MouseWheel>", zoomer)
#==========================================================================================
win.mainloop()

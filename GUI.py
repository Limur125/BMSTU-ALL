from tkinter import *
from math import sqrt
circles_list = []
points_list = []
circle_drawing = []
act_log = []
res_line = [] 
#==========================================================================================
def str_to_float(str):
    try:
        x = float(str)
        return x
    except:
        return str

def create_circle(xc, yc, r):
    circle_id = canv.create_oval(xc - r, yc - r, xc + r, yc + r, width=3, outline='black')
    circle_drawing.append(r)
    circle_drawing.append(circle_id)
    act_log.append(circle_id)
    circle_drawing.append(-1)

def xc_yc_coords(event):
    circle_drawing.clear()
    circle_drawing.append(event.x)
    circle_drawing.append(event.y)
    create_circle(circle_drawing[0], circle_drawing[1], 0.1)

def point_coords(event):
    xp = event.x
    yp = event.y
    point_id = canv.create_oval(xp - 0.1, yp - 0.1, xp + 0.1, yp + 0.1, width=3, outline='black')
    act_log.append(point_id)
    points_list.append([xp, yp, point_id])

def radius(event):
    x1, y1 = event.x, event.y
    x2, y2 = circle_drawing[0], circle_drawing[1]
    circle_drawing[2] = sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))
    canv.coords(circle_drawing[3], x2 - circle_drawing[2], y2 - circle_drawing[2], x2 + circle_drawing[2], y2 + circle_drawing[2])

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

def finish_point_draw(event):
    canv.unbind('<Button-1>')
    canv.unbind('<ButtonRelease-1>')

def start_point_draw():
    circle_drawing.clear()
    canv.bind('<Button-1>', point_coords)
    canv.bind('<ButtonRelease-1>', finish_point_draw)

def undo_last_act():
    if len(act_log) != 0:
        try:
            canv.delete(res_line[0])
        except:
            None
        res_line.clear()
        canv.delete(act_log[-1])
        if len(circles_list) != 0:
            if circles_list[-1][3] == act_log[-1]:
                circles_list.pop()
        elif len(points_list) != 0:
            if points_list[-1][2] == act_log[-1]:
                points_list.pop()
        act_log.pop()

def zoom(event):
    if (event.delta > 0):
        canv.scale("all", event.x, event.y, 1.1, 1.1)
    elif (event.delta < 0):
        canv.scale("all", event.x, event.y, 0.9, 0.9)

def calculate():
    points = points_list
    lines = []
    try:
        canv.delete(res_line[0])
    except:
        None
    res_line.clear()
    for i in range(len(points)):
        for j in range(i + 1, len(points)):
            line = [points[i], points[j], 0, 0]
            for point0 in points:
                cross_prod = (points[j][0] - points[i][0])*(point0[1] - points[i][1]) - (points[j][1] - points[i][1])*(point0[0] - points[i][0])
                if cross_prod > 0:
                    line[2] += 1
                elif cross_prod < 0:
                    line[2] -= 1
            if line[2] != 0:
                continue
            else:
                lines.append(line)
    for line in lines:
        for circle in circles_list:
            print(circle)
            print(line)
            d = abs((line[1][1] - line[0][1]) * circle[0] - (line[1][0] - line[0][0]) * circle[1] + line[1][0] * line[0][1] + line[1][1] * line[0][0]) / sqrt((line[1][1] - line[0][1]) * (line[1][1] - line[0][1]) + (line[1][0] - line[0][0]) * (line[1][0] - line[0][0]))
            print(d)
            if circle[2] > d:
                line[3] += 1
    max_line = lines[0]
    print(lines)
    for line in lines:
        if max_line[3] < line[3]:
            max_line = line
    x0 = (0 - max_line[0][1]) * (max_line[1][0] - max_line[0][0]) / (max_line[1][1] - max_line[0][1]) + max_line[0][0]
    x600 = (600 - max_line[0][1]) * (max_line[1][0] - max_line[0][0]) / (max_line[1][1] - max_line[0][1]) + max_line[0][0]
    res_line_id = canv.create_line(x0, 0, x600, 600, fill='blue')
    res_line.append(res_line_id)
#==========================================================================================
win = Tk()

canv = Canvas(win, width=600, height=600, bg='white')
canv.grid(row=0, column=0)

frm = Frame(win)
circ_btn = Button(frm, text='◯',command=start_circle_draw)
circ_btn.grid(row=0, column=0)

circ_btn = Button(frm, text='◦',command=start_point_draw)
circ_btn.grid(row=0, column=1)

circ_btn = Button(frm, text='↺',command=undo_last_act)
circ_btn.grid(row=0, column=2)

calc_btn = Button(frm, width=30, height=4, text='Найти прямую пересекающую\nнабольшее число окружностей\nи делящую множество\nточек пополам',command=calculate)
calc_btn.grid(row=1, column=0, columnspan=3)

frm.grid(row=0, column=2)

canv.bind("<MouseWheel>", zoom)

win.mainloop()

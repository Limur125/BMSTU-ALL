from math import sqrt, pi, sin, cos

def draw_circle(xc, yc, r, method_i, color_i):
    match color_i:
        case 0:
            color = (0, 0, 0)
        case 1:
            color = (0, 0, 255)
        case 2:
            color = (255, 0, 0)
        case 3:
            color = (253, 245, 230)

    match method_i:
        case 0:
            points_coords = canon_circle(xc, yc, r, color)
        case 1:
            points_coords = param_circle(xc, yc, r, color)
        case 2:
            points_coords = brez_circle(xc, yc, r, color)
        case 3:
            points_coords = mid_point_circle(xc, yc, r, color)

    return points_coords

def canon_circle(xc, yc, r, color):
    circle = []
    x = 0
    edge = r / sqrt(2)
    while (x <= edge):
        y = round(sqrt(r * r - (x - xc) * (x - xc)) + yc)
        circle.append([x, y, color])
        x += 1

    while (y >= 0):
        x = round(sqrt(r * r - (y - yc) * (y - yc)) + xc)
        circle.append([x, y, color])
        y -= 1
    return circle

def param_circle(xc, yc, r, color):
    step = 1 / r
    circle = []
    t = 0
    while (t < pi / 2 + step):
        x = round(r * cos(t))
        y = round(r * sin(t))
        circle.append([x, y, color])
        t += step
    return circle

def brez_circle(xc, yc, r, color):
    circle = []
    x = 0
    y = r
    delta = 1 - 2 * r
    error = 0
    while y >= 0:
        circle.append([x, y, color])
        error = 2 * (delta + y) - 1
        if delta < 0 and error < 0:
            x += 1
            delta = delta + (2 * x + 1)
            continue
        error = 2 * (delta - x) - 1
        if delta > 0 and error > 0:
            y -= 1
            delta = delta + (1 - 2 * y)
            continue
        x += 1
        delta = delta + (2 * (x - y))
        y -= 1
    return circle

def mid_point_circle(xc, yc, r, color):
    x = 0
    y = r

    x_edge = round(r / sqrt(2))

    f = 0
    circle = []

    while x <= x_edge:
        circle.append([x, y, color])
        df = r * r * (2 * x  + 1)
        x += 1

        if (f < 0):
            f = f + df
        else: 
            f = f + df - 2 * r * r * y
            y -= 1
    
    f = f + (r * r - r * r * r + r * r / 4)
    while y >= 0:
        circle.append([x, y, color])
        df = r * r * (-2 * y + 1)
        y -= 1

        if f > 0:
            f = f + df
        else: 
            f = f + df + 2 * r * r * x
            x += 1
    return circle
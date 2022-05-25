from math import sqrt, pi, sin, cos

def draw_ellips(xc, yc, a, b, method_i, color_i):
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
            points_coords = canon_ellips(xc, yc, a, b, color)
        case 1:
            points_coords = param_ellips(xc, yc, a, b, color)
        case 2:
            points_coords = brez_ellips(xc, yc, a, b, color)
        case 3:
            points_coords = mid_point_ellips(xc, yc, a, b, color)

    return points_coords

def canon_ellips(xc, yc, a, b, color):
    ellips = []
    x = 0
    edge = a * a / sqrt(a * a + b * b)
    while (x <= edge):
        y = round(sqrt(1 - (x - xc) * (x - xc) / (a * a)) * b + yc)
        ellips.append([x, y, color])
        x += 1

    while (y >= 0):
        x = round(sqrt(1 - (y - yc) * (y - yc) / (b * b)) * a + xc)
        ellips.append([x, y, color])
        y -= 1

    return ellips

def param_ellips(xc, yc, a, b, color):
    if a > b:
        step = 1 / a
    else:
        step = 1 / b
    ellips = []
    t = 0
    while (t < pi / 2 + step):
        x = round(a * cos(t))
        y = round(b * sin(t))
        ellips.append([x, y, color])
        t += step
    return ellips

def brez_ellips(xc, yc, a, b, color):
    ellips = []

    pow_a = a * a
    pow_b = b * b

    limit = round(a / sqrt(1 + pow_b / pow_a)) # производная для ограничения

    x = 0
    y = b
    ellips.append([x, y, color]) 

    func = pow_b - round(pow_a * (b - 1 / 4)) 
    
    # 1 участок
    while x <= limit:
        if func > 0: # диагональ
            y -= 1
            func -= pow_a * y * 2

        x += 1
        func += pow_b * (x + x + 1)
        ellips.append([x, y, color]) 

    limit = round(b / sqrt(1 + pow_a / pow_b)) # производная для ограничения

    x = a
    y = 0
    ellips.append([x, y, color]) 

    func = pow_a - round(pow_b * (x - 1 / 4))  

    # 2 участок
    while y <= limit:
        if func > 0: # диагональ
            x -= 1
            func -= 2 * pow_b * x

        y += 1
        func += pow_a * (y + y + 1)
        ellips.append([x, y, color]) 
    return ellips

def mid_point_ellips(xc, yc, a, b, color):
    x = 0
    y = b

    x_edge = a * a / sqrt(a * a + b * b)

    f = 0
    ellips = []

    while x < x_edge:
        ellips.append([x, y, color])
        df = b * b * (2 * x  + 1)
        x += 1

        if (f < 0):
            f = f + df
        else: 
            f = f + df - 2 * a * a * y
            y -= 1
    
    f = f + (b * b - a * a * b + a * a / 4)
    while y >= 0:
        ellips.append([x, y, color])
        df = a * a * (-2 * y + 1)
        y -= 1

        if f > 0:
            f = f + df
        else: 
            f = f + df + 2 * b * b * x
            x += 1
    return ellips
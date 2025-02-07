import matplotlib.pyplot as plt

x = []
y = []
with open("res.txt", 'r') as f:
    for line in f.readlines():
        x0, y0 = map(float, line.split())
        if (x0 < 1600):
            x.append(x0)
            y.append(y0)

x0s = []
y0s = []
with open("spectr.txt", 'r') as f:
    for line in f.readlines():
        _, _, x00, y00 = map(float, line.split())
        if (x00 < 1600):
            x0s.append(x00)
            y0s.append(y00)


plt.plot(x0s, y0s, 'r')
plt.plot(x, y, linewidth=0.7, linestyle='dashed', color='b')
plt.show()
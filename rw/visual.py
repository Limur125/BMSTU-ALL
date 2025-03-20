import matplotlib.pyplot as plt

x = []
y = []
with open("res.txt", 'r') as f:
    for line in f.readlines():
        x = list(map(float, line.split()))
        h = 0.35 / 20
        y = [0.35 - i * h for i in range(20, 0, -1)]

# x0s = []
# y0s = []
# with open("spectr.txt", 'r') as f:
#     for line in f.readlines():
#         _, _, x00, y00 = map(float, line.split())
#         if (x00 < 1600):
#             x0s.append(x00)
#             y0s.append(y00)


plt.plot(y, x, linewidth=0.7, linestyle='dashed', color='b')
plt.show()
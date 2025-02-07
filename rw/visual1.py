import matplotlib.pyplot as plt

x = []
with open("distr.txt", 'r') as f:
    for line in f.readlines():
        y = list(map(float, line.split()))

for i in range(len(y),0,-1):
    x.append(i * 1.0 / len(y))
plt.plot(x, y, linewidth=0.7, linestyle='dashed', color='b')
plt.show()
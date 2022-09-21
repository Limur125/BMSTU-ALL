from random import *
from sys import argv
x = int(argv[1])
r = int(argv[2])
f = open('i.txt', 'w')
f.write(str(x) + ' ' + str(x)+ '\n')
for i in range(0, x):
    for j in range(0, x):
        d = randint(0, 99)
        if d < r:
            h = randint(1, 9)
            f.write(str(h) + ' ')
        else:
            f.write('0 ')
    f.write('\n')
f.write(str(x) + ' ' + str(x)+ '\n')
for i in range(0, x):
    for j in range(0, x):
        d = randint(0, 99)
        if d < r:
            h = randint(1, 9)
            f.write(str(h) + ' ')
        else:
            f.write('0 ')
    f.write('\n')
f.close()
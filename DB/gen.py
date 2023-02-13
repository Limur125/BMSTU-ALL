from random import random, randrange, randint
rt = ["RP", "EC", "E", "E 10+", "T", "M", "A"]
with open("title3.txt", "r") as in1:
    with open("platf.txt", "r") as in2:
        with open("test.txt", "w") as out:
            p = in2.readlines()
            for line in in1.readlines():
                c = randint(0, 3)
                for i in range(c):
                    out.write(f"{line[:-1]}\t{p[randrange(0, len(p))]}")

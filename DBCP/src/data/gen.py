from random import random, randrange, randint, choice
import datetime
import csv
import io

def random_id(length):
    number = '0123456789'
    alpha = 'abcdefghijklmnopqrstuvwxyz'
    id = ''
    for i in range(0,length,2):
        id += choice(number)
        id += choice(alpha)
    return id


# gamesStr : list[str] = []
# with open("Windows_Games_List.csv", "r") as in1:
#     reader = csv.reader(in1)
#     for row in reader:
#         gamesStr.append(row)

# for game in gamesStr:
#     game[4] = ''.join(game[4].split(","))


# with open("games.csv", "w") as out:
#     for i in range(15):
#         c = randint(1, len(gamesStr))
#         out.write(';'.join(gamesStr[c - 1]))
#         out.write("\n")

users = ["asd", "limur", "limur1", "limur2", "limur3", "limur4"]
games = []
reviews = []
for user in users:
    u = []
    for i in range(15):
        u.append(i)
    games.append(u)

# with open("reviews.txt", "w") as out:
#     for _ in range(40):
#         u = randrange(0,len(users))
#         gI = randrange(0, len(games[u]))
#         while(games[u][gI] < 0):
#             gI = randrange(0, len(games[u]))
#         out.write(f"{games[u][gI]};{users[u]};{random_id(100)};{randint(1, 10)};{datetime.datetime.today().strftime('%Y-%m-%d')}\n")
#         games[u][gI] = -1

with open("times.txt", "w") as out:
    for _ in range(40):
        u = randrange(0,len(users))
        gI = randrange(0, len(games[u]))
        while(games[u][gI] < 0):
            gI = randrange(0, len(games[u]))
        t = randint(0, 1)
        if t == 0:
            out.write(f"{games[u][gI]};{users[u]};{randint(1, 5)};{randint(1, 60)};{t}\n")
        elif t == 1:
            out.write(f"{games[u][gI]};{users[u]};{randint(10, 15)};{randint(1, 60)};{t}\n")
        games[u][gI] = -1
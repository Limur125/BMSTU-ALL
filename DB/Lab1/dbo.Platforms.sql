drop table Platforms
create table Platforms
(
[name] varchar(100) primary key,
company varchar(100),
rel_year int,
processor int,
RAM int
)

bulk insert Platforms
from '\platforms.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)

drop table PlatformLink
create table PlatformLink
(
[game] varchar(200) references VideoGames(title),
plat varchar(100) references Platforms([name])
)

bulk insert PlatformLink
from '\test.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)
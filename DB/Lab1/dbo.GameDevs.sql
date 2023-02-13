drop table dbo.GameDevs
create table GameDevs
(
developer varchar(100) primary key,
city varchar(100) not null,
autonomous_area varchar(100) null,
country varchar(100) not null,
est_year int check(est_year > 0)
)

bulk insert dbo.GameDevs
from '\video-games-developers.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)

bulk insert dbo.GameDevs
from '\Test.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)
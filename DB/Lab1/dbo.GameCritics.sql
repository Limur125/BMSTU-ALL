drop table GameCritics
create table GameCritics
(
	title varchar(200) primary key,
	[platform] varchar(200),
	summary text,
	crit_score int,
	user_score numeric(2, 1),
)

bulk insert dbo.GameCritics
from '\test2.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)


bulk insert dbo.GameCritics
from '\test3.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)

delete from GameCritics
where title not in 
(select title from VideoGames)
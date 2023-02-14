drop table dbo.VideoGames

CREATE TABLE [dbo].[VideoGames] (
title varchar(200) NOT NULL primary key,
release INT NOT NULL CHECK (release > 0),
developer varchar(100) NOT NULL,
publisher TEXT NOT NULL,
genre varchar(200) NOT NULL, 
);

bulk insert dbo.VideoGames
from '\Windows_Games_list.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)

alter table dbo.VideoGames 
add constraint FK_Developers foreign key (developer)
references dbo.GameDevs (developer)

alter table dbo.VideoGames 
add constraint FK_GameTItle foreign key (title)
references dbo.GameCritics (title)

alter table dbo.VideoGames 
add constraint FK_GameTitleSales foreign key (title)
references dbo.GameSales (title)

alter table dbo.VideoGames 
add constraint FK_GameRating foreign key (title)
references dbo.GameRating (title)
create database GameTime COLLATE Cyrillic_General_CI_AS;
use GameTime;
drop table if exists Games;
create table Games
(
	id int primary key identity(1, 1),
	Title nvarchar(200) unique,
	ReleaseDate date,
	Developer nvarchar(200),
	Publisher nvarchar(200),
	[Platform] nvarchar(200)
);
set IDENTITY_INSERT Games on;
set IDENTITY_INSERT Games off;
drop table if exists Reviews;
create table Reviews
(
	GameId int,
	UserLogin nvarchar(200),
	[Text] nvarchar(1000),
	Score int,
	PublicationDate date,
	primary key(GameId, UserLogin)
);
drop table if exists TimeRecords;
create table TimeRecords
(
	GameId int,
	UserLogin nvarchar(200),
	[Hours] int,
	[Minutes] int,
	[Type] int,
	primary key(GameId, UserLogin, [Type])
);
drop table if exists Users;
create table Users
(
	[Login] nvarchar(200) primary key,
	[Password] nvarchar(200)
);

create login [AuthorizedUser] with password = 'UserPassword1'
create user AU for login [AuthorizedUser]
create role [AuthorizedUserRole]
alter role [AuthorizedUserRole] add member AU
deny delete on Reviews to [AuthorizedUserRole];
deny update on Reviews to [AuthorizedUserRole];
deny delete on TimeRecords to [AuthorizedUserRole];
deny update on TimeRecords to [AuthorizedUserRole];
deny insert on Games to [AuthorizedUserRole];
deny delete on Games to [AuthorizedUserRole];
deny update on Games to [AuthorizedUserRole];
deny delete on Users to [AuthorizedUserRole];
deny update on Users to [AuthorizedUserRole];
deny alter to [AuthorizedUserRole];
grant insert on Reviews to [AuthorizedUserRole];
grant insert on TimeRecords to [AuthorizedUserRole];
grant insert on Users to [AuthorizedUserRole];
grant select on Users to [AuthorizedUserRole];
grant select on Games to [AuthorizedUserRole];
grant select on Reviews to [AuthorizedUserRole];
grant select on TimeRecords to [AuthorizedUserRole];

create login [GuestUser] with password = 'GuestPassword1'
create user GU for login [GuestUser]
create role [GuestUserRole]
alter role [GuestUserRole] add member GU
deny delete on Reviews to [GuestUserRole];
deny update on Reviews to [GuestUserRole];
deny delete on TimeRecords to [GuestUserRole];
deny update on TimeRecords to [GuestUserRole];
deny insert on Games to [GuestUserRole];
deny delete on Games to [GuestUserRole];
deny update on Games to [GuestUserRole];
deny delete on Users to [GuestUserRole];
deny update on Users to [GuestUserRole];
deny alter to [GuestUserRole];
deny insert on Reviews to [GuestUserRole];
deny insert on TimeRecords to [GuestUserRole];
grant insert on Users to [GuestUserRole];
grant select on Users to [GuestUserRole];
grant select on Games to [GuestUserRole];
grant select on Reviews to [GuestUserRole];
grant select on TimeRecords to [GuestUserRole];

select * from Games
select * from TimeRecords
select * from Reviews
select * from Users

insert Games values('Crazy Jump', '03-09-2021', 'Gameslab', 'Gameslab', 'Shooter')
insert Users values('admin', 'admin')
delete from Games where 0 = 0
delete from TimeRecords where 0 = 0
delete from Reviews where 0 = 0
delete from Users where 0 = 0
go







bulk insert GamesRaw
from '\games.csv'
with 
(
FORMAT = 'CSV', 
FIELDQUOTE = '\',
FIELDTERMINATOR = ';',
ROWTERMINATOR = '0x0a'
)

bulk insert Reviews
from '\reviews.txt'
with 
(
FORMAT = 'CSV', 
FIELDQUOTE = '\',
FIELDTERMINATOR = ';',
ROWTERMINATOR = '0x0a'
)

bulk insert TimeRecords
from '\times.txt'
with 
(
FORMAT = 'CSV', 
FIELDQUOTE = '\',
FIELDTERMINATOR = ';',
ROWTERMINATOR = '0x0a'
)

drop table if exists GamesRaw;
create table GamesRaw
(
	Title nvarchar(200) unique,
	ReleaseDate date,
	Developer nvarchar(200),
	Publisher nvarchar(200),
	Genres nvarchar(200),
);
go

insert into Games(Title, ReleaseDate, Developer, Publisher, [Platform])
select * from GamesRaw

declare @title varchar(200);
declare @rd date;
declare @dev varchar(200);
declare @pub varchar(200);
declare @gen varchar(200);


drop table if exists GamesXML;
create table GamesXML
(
	id int primary key identity(1, 1),
	Title nvarchar(200) unique,
	ReleaseDate date,
	Developer nvarchar(200),
	Publisher nvarchar(200),
	Genres xml,
);
go

drop table if exists Platforms;
create table Platforms
(
	id int primary key identity(1, 1),
	[Name] nvarchar(200) unique,
	AvgPrice int,
);

drop table if exists GamePlatform;
create table GamePlatform
(
	GameId int,
	PlatformId int,
);

drop table if exists temp
create table temp
(
	title nvarchar(200),
	genre nvarchar(200),
)
go

create or alter procedure InsertGameProcedure
(
	@title nvarchar(200),
	@releaseDate date,
	@developer nvarchar(200),
	@publisher nvarchar(200),
	@genres nvarchar(200)
)
as
begin
	delete temp where 0 = 0
	insert into temp
	select @title, genre_split.value
	from string_split(@genres, ' ') as genre_split

	insert into Games
	values(@title, @releaseDate, @developer, @publisher,
		cast(
			(select temp.genre
			from temp
			for xml path(''), type, root('Genre')) as nvarchar(200)
		 ))
	delete temp where 0 = 0
end
go

delete from GamesXML where 0 = 0
exec InsertGameProcedure 'President Runner', '03-09-2021', 'Gameslab', 'Gameslab', 'Platformer 2D'

select * from temp
select * from GamesXML

select * from Games
select * from Users
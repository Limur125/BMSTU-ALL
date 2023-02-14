--------------------------------1-----------------------------------------------------
---- Для выгрузки в XML проверить все режимы конструкции FOR XML
-----1.1-----
--select *  from VideoGames
--for xml auto
--go
-------1.2-----
--select title, [platform]
--from GameCritics
--where user_score < 3
--for xml raw
--go
------1.3-----
--select title
--from VideoGames
--where release between 2000 and 2010
--for xml path
--go
------1.4------
--select [name] as 'Platform',
--(
--	select vg.* from PlatformLink as pl full join VideoGames as vg on game = vg.title
--	where pl.plat = p.[name]
--	for xml path(''), type) as 'Games'	
--from Platforms as p
----for xml path('Platfrom'), type
--go

------------------------------1-----------------------------------------------------
-- Для выгрузки в XML проверить все режимы конструкции FOR XML
---1.1-----
select *  from VideoGames
for xml auto
go
-----1.2-----
select title, [platform]
from GameCritics
where user_score < 3
for xml raw
go
----1.3-----
select *
from VideoGames
where release between 2000 and 2010
for xml path('Game'), type
go
----1.4------
select 1 as Tag,
    null as Parent,
    [name] as [Platforms!1!PlatformName],
    null as [GameCritics!2!Title],
    null as[GameCritics!2!CritScore]
from Platforms
union all
select 2 as Tag, 
    1 as Parent,
    p.[name],
    title,
    crit_score
from GameCritics as gc join Platforms as p on gc.[platform] = p.[name]
order by [Platforms!1!PlatformName], [GameCritics!2!Title], [GameCritics!2!CritScore]
for xml explicit
-----------------------------------2----------------------------
-- Загрузить в таблицу данные из xml`
drop table if exists #temp
create table #temp
(
    [title]     VARCHAR (200) NOT NULL,
    [release]   INT           NOT NULL,
    [developer] VARCHAR (100) NOT NULL,
    [publisher] varchar (100) NOT NULL,
    [genre]     VARCHAR (200) NOT NULL
)

declare @fileDataX xml
select @fileDataX = BulkColumn 
from openrowset
(
    bulk '\data.xml',
    single_blob
) x;

insert into #temp ([title], [release], [developer], [publisher], [genre])
select
    xData.value('title[1]','varchar(200)') title, -- 'xData' is our xml content alias
    xData.value('release[1]','int') [release],
    xData.value('developer[1]','varchar(100)') [developer],
    xData.value('publisher[1]','varchar(100)') [publisher],
    xData.value('genre[1]','varchar(200)') [genre]
from @fileDataX.nodes('Game') as x(xData)-- this is the xpath to the individual records we want to extract

select * from #temp
go
-----------------------------------3----------------------------
-- Создание XML файла
drop table if exists #temp2
create table #temp2
(
    title varchar(200),
    genre varchar(100)
)
insert into #temp2 
select title, genres.part
from VideoGames
cross apply dbo.someResult(genre, ' ') as genres

select * from #temp2

select title as [Game],
    release,
    developer,
    publisher,
(
	select evg.genre 
    from #temp2 as evg 
    where evg.title = vg.title
	for xml path(''), type
) as 'Genres'	
from VideoGames as vg
for xml path('Game'), type, root('Games')
go
-- Загрузка XML файла
drop table if exists XmlVideoGames
create table XmlVideoGames
(    
    [title]     VARCHAR (200) NOT NULL,
    [release]   INT           NOT NULL,
    [developer] VARCHAR (100) NOT NULL,
    [publisher] varchar (100) NOT NULL,
    [genre]     xml NOT NULL
)

declare @fileDataX xml
select @fileDataX = BulkColumn 
from openrowset
(
    bulk '\data2.xml',
    single_blob
) x;

insert into XmlVideoGames ([title], [release], [developer], [publisher], [genre])
select
    xData.value('Game[1]','varchar(200)') title,
    xData.value('release[1]','int') [release],
    xData.value('developer[1]','varchar(100)') [developer],
    xData.value('publisher[1]','varchar(100)') [publisher],
    xData.query('Genres[1]') [genre]
from @fileDataX.nodes('Games/Game') as x(xData)

select * from XmlVideoGames
go
-----------------------------------4----------------------------
drop table if exists #temp3
create table #temp3
(    
    [title]     VARCHAR (200) NOT NULL,
    [release]   INT           NOT NULL,
    [developer] VARCHAR (100) NOT NULL,
    [publisher] varchar (100) NOT NULL,
    [genre]     xml NOT NULL
)

declare @fileDataX xml
select @fileDataX = BulkColumn 
from openrowset
(
    bulk '\data2.xml',
    single_blob
) x;

insert into #temp3 ([title], [release], [developer], [publisher], [genre])
select
    xData.value('Game[1]','varchar(200)') title,
    xData.value('release[1]','int') [release],
    xData.value('developer[1]','varchar(100)') [developer],
    xData.value('publisher[1]','varchar(100)') [publisher],
    xData.query('Genres[1]') [genre]
from @fileDataX.nodes('Games/Game') as x(xData)


update #temp3
set genre.modify('delete (Genres/genre[1])')
where title = 'Advent Rising'

select * from XmlVideoGames
where title = 'Advent Rising'
select * from #temp3
where title = 'Advent Rising'

select * from XmlVideoGames
where genre.exist('Genres/genre[8]') = 1
go

-- def

drop table if exists #temp2
create table #temp2
(
    title varchar(200),
    genre varchar(100)
)
insert into #temp2 
select title, genres.part
from VideoGames
cross apply dbo.someResult(genre, ' ') as genres

select * from #temp2

with evg as (select distinct genre from #temp2)

select evg.genre as [Genre],
    (
        select title 
        from #temp2
        where evg.genre = #temp2.genre
        for xml path(''), type
    ) as 'Games'
from evg
for xml path('Genre'), type, root('Genres')
go

use VideoGames
go

sp_configure 'show advanced options', 1
go

sp_configure 'clr enabled', 1
reconfigure
go

alter database VideoGames set trustworthy on

drop function if exists GetRandomNumber
drop aggregate if exists Multiply
drop function if exists someResult 
drop procedure if exists SelectCritics
drop trigger DropTrigger
drop type if exists Email
drop assembly CLR

create assembly CLR
from '/ClassLibrary1.dll'
go




-- Скалярная функция
create function GetRandomNumber(@min int, @max int) returns int
as
external name CLR.[ClassLibrary1.Scalar].GetRandomFromGap
go

select dbo.GetRandomNumber(1,20) as Random
go

-- Агрегирующая функция

create aggregate Multiply (@score int) RETURNS int 
external name CLR.[ClassLibrary1.Multiply];
go  

select [platform], dbo.Multiply(crit_score) as mul_score
from GameCritics
group by [platform]
go

-- Табличная функция

create function someResult(@string nvarchar(200), @delimeter nchar(1))   
returns table (  
   part nvarchar(200),
   id_order int
)  
as external name CLR.[ClassLibrary1.Table].SplitString
go  

select * from someResult(
'ncijen,ceu9nwusn,efiuncn,csjinn'
, ',')
go

-- Процедура

create procedure SelectCritics(@type int)
as external name CLR.[ClassLibrary1.Procedure].SelectCritics;
go

exec SelectCritics 1
go
-- Триггер
create trigger DropTrigger
on database
for DROP_TABLE 
as external name CLR.[ClassLibrary1.Trigger].DropTableTrigger
go

create table test (id int)
go
drop table test
go

-- Новый тип

create type dbo.Email external name CLR.[ClassLibrary1.Email]
go

create table TestTable
(
    id int,
    mail dbo.Email
)
go

insert into dbo.TestTable
select 
row_number() over (order by [name]),  
cast(replace(lower([name]), ' ', '_' )+ '@inbox.com' as dbo.Email)
from Platforms
go

select id, cast(mail as varchar(100)) as mail, mail from TestTable
go

drop table TestTable
go
-- def

drop assembly CLRdef
create assembly CLRdef
from '/ClassLibrary2.dll'
go

create aggregate CountGames(@text nvarchar(100)) returns int
external name CLRdef.[ClassLibrary1.Def]
go

select Platforms.[name], dbo.CountGames(PlatformLink.[plat]) as [count]
from Platforms join PlatformLink on [name] = plat
group by Platforms.[name]
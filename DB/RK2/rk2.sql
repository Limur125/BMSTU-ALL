-- Задание 1
drop database if exists rk2
create database rk2

use  rk2

drop table if exists empl
create table empl 
(
	id int identity(1, 1) primary key,
	fio varchar(200),
	birth int,
	dolz varchar(100)
) 
go

drop table if exists trans
create table trans
(
	id int identity(1, 1) primary key,
	idEmp int references empl(id),
	valutaCursID int references valutaCurs(id),
	summa int
) 
go

drop table if exists valtaCurs
create table valutaCurs
(
	id int identity(1, 1) primary key,
	valutaId int references valuta(id),
	sell numeric(5, 2),
	buy numeric(5, 2)
) 
go

drop table if exists valuta
create table valuta
(
	id int identity(1, 1) primary key,
	[name] varchar(200)
) 
go

insert into empl(fio, birth, dolz) values
('sksksksksksk', 2000, 'qwqwqwqwqw'),
('owowowowowo', 1999, 'oooooooooo'),
('nwnwnwnwnwn', 2001, 'jjjjjjjjjjjj'),
('lrvnsdcn', 1800, ' uncjsa jca'),
('kdjcbnjw', 1999, 'ionsnvjds j j'),
('ciowm a', 1989, 'ddnjvmskcmksn '),
('bv jxnenew a', 1970, 'nvjs  jisacjn'),
(' je njn  k', 1963, 'sdhsnmc  cosd'),
('mv nwn m', 1920 ,'nisjdosa '),
('jiermignmdsk', 1998 ,'juhdsciakxoasn')

insert into trans(idEmp, valutaCursID, summa) values
(1, 4, 1000),
(4, 3, 111212),
(3, 3, 482528),
(1, 3, 54842),
(2, 3, 47117),
(1, 4, 45287),
(1, 8, 11111),
(9, 9, 28182),
(7, 2, 25181),
(10, 5, 48888)


insert into valutaCurs(valutaId, sell, buy) values
(1, 32.53, 234.43),
(2, 234.42, 123.43),
(3, 940.63, 84.84),
(4, 39.47, 84.27),
(5, 34.48, 948.34),
(6, 87.73, 16.45),
(7, 25.45, 25.86),
(8, 752.56, 405.55),
(9, 35.99, 44.44),
(10, 211.44, 753.21)


insert into valuta([name]) values
('jonsv'),
('oajsdi'),
('ksdaok'),
('uiqdm'),
('ijcew'),
('i0wjd'),
('iocw'),
('plvpe'),
('woc'),
('rkjen')

-- Задание 2
-- 1
select id, case  trans.valutaCursID
	when 1 then 'jonsv'
	when 2 then 'oajsdi'
	when 3 then 'ksdaok'
	when 4 then 'uiqdm'
	when 5 then 'ijcew'
	when 6 then 'i0wjd'
	when 7 then 'iocw'
	when 8 then 'plvpe'
	when 9 then 'woc'
	when 10 then 'rkjen'
	end as 'VALUTA'
from trans
-- 2
select id, summa, valutaCursID,
count(id) over (partition by valutaCursID order by valutaCursID) as valuta_trans_count
from trans
-- 3
select valutaCursID, count(valutaCursID)
from trans
group by valutaCursID
having count(valutaCursID) > 3
-- Задание 3

create or alter procedure backup_all as 
declare @name nvarchar(256)
declare @path nvarchar(512)
declare @fileName nvarchar(512)
declare @fileDate nvarchar(20)
set @path = '/'
select @fileDate = convert(nvarchar(20), getdate(), 112)
declare backup_cursor cursor read_only for 
select [name]
from sys.databases
where [name] not in ('master','model','msdb','tempdb')
open backup_cursor
fetch next from backup_cursor into @name
while @@fetch_status = 0
begin
	set @fileName = @path + @name + '_' + @fileDate + '.bak'
	backup database @name to disk = @fileName
	fetch next from backup_cursor into @name
end
close backup_cursor
deallocate backup_cursor
go

exec backup_all
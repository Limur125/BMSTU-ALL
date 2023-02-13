-- Скалярная функция

create function dbo.fraction(@region_sales numeric(4, 1), @global_sales numeric(4, 1))
returns numeric(4, 1)
as
begin	
	return @region_sales / @global_sales * 100
end
go

select title, jp_sales, global_sales, cast(dbo.fraction(jp_sales, global_sales) as varchar(5)) + '%' as [percentage]
from GameSales
where jp_sales > 6
go
-- Подставляемная табличная функция

create or alter function dbo.game_table(@esrb varchar(5))
returns table
as 
return
select gr.title
from GameRating as gr
where gr.esrb_rating = @esrb 
go

select *
from game_table('M')
go
-- Многооператорная табличная функция

create or alter function dbo.sales()
returns @rett table
(
	title varchar(200),
	na varchar(5),
	eu varchar(5),
	jp varchar(5),
	other varchar(5), 
	eu_sales numeric (4, 1),
    jp_sales numeric (4, 1),
    other_sales numeric (4, 1),
    global_sales numeric (4, 1)
)
as
begin
	insert @rett
	select title, cast(dbo.fraction(na_sales, global_sales) as varchar(5)) + '%', null, null, null, eu_sales, jp_sales, other_sales, global_sales
	from GameSales;

	with gs as
	(
		select title, cast(dbo.fraction(eu_sales, global_sales) as varchar(5)) + '%' as p
		from GameSales as gs
	)
	update @rett
	set eu = cast(dbo.fraction(eu_sales, global_sales) as varchar(5)) + '%';
	update @rett
	set jp = cast(dbo.fraction(jp_sales, global_sales) as varchar(5)) + '%';
	update @rett
	set other = cast(dbo.fraction(other_sales, global_sales) as varchar(5)) + '%';
	return 
end
go

select title, na, eu, jp, other 
from dbo.sales()
go

-- Рекурсивную функцию или функцию с рекурсивным ОТВ

create or alter function dbo.recursiv()
returns @rett table
(
	id int,
	title varchar(200),
	critscore int,
	mul numeric(30)
)
as
begin
	with idSales as
	(
		select gs.*, row_number() over (order by title asc) as id
		from GameCritics as gs
	),
	RecursiveCrits as
	(
		select id, title, crit_score, cast(crit_score as numeric(30)) as [mul]
		from idSales as l
		where id = 1
		union all
		select l.id, l.title, l.crit_score, cast([mul] * rec_l.crit_score as numeric(30))
		from idSales as l join RecursiveCrits as rec_l on l.id - 1 = rec_l.id
		where rec_l.id < 6
	)

	insert @rett 
	select *
	from RecursiveCrits
	return
end;
go

select *
from dbo.recursiv()
go

-- Хранимую процедуру без параметров или с параметрами

create or alter procedure raise_user_score(@raise_param numeric(2, 1))
as
begin
	update GameCritics
	set user_score = user_score + @raise_param
	where user_score + @raise_param < 10
end
go

exec raise_user_score 0.5
go
-- Рекурсивную хранимую процедуру или хранимую процедур с рекурсивным ОТВ

create or alter procedure dbo.recursivproc
as
begin
	with idSales as
	(
		select gs.*, row_number() over (order by title asc) as id
		from GameCritics as gs
	),
	RecursiveCrits as
	(
		select id, title, crit_score, cast(crit_score as numeric(30)) as [mul]
		from idSales as l
		where id = 1
		union all
		select l.id, l.title, l.crit_score, cast([mul] * rec_l.crit_score as numeric(30))
		from idSales as l join RecursiveCrits as rec_l on l.id - 1 = rec_l.id
		where rec_l.id < 6
	)

	select *
	from RecursiveCrits
end;
go

exec recursivproc
go
-- Хранимую процедуру с курсором

create or alter procedure find_rating(@rate varchar(5))
as
begin
	declare @title varchar(200);
	declare @esrb varchar(5);
	create table #temp
	(
		title varchar(200),
		esrb varchar(5)
	)
	declare rating_curs cursor  
    for select title, esrb_rating from GameRating 
	open rating_curs  
	fetch next from rating_curs
	into @title, @esrb
	while @@FETCH_STATUS is not null
	begin
		if @esrb = @rate
			insert into #temp values(@title, @esrb)
		fetch next from rating_curs;
	end
	close rating_curs
	deallocate rating_curs
	select * from #temp
end
go

exec find_rating 'M'
go


-- Хранимую процедуру доступа к метаданным

create or alter procedure cout_types(@type nvarchar)
as
	select count(*)
	from (sys.all_columns as c join sys.types as t on c.user_type_id = t.user_type_id) 
	join sys.objects as o on c.object_id = o.object_id
	where t.name = @type and type = 'U'
	group by t.user_type_id
go

exec cout_types 'numeric'
go

-- Триггер AFTER

create trigger on_update_GameSales
on GameSales
after update
as
select title, na, eu, jp, other 
from dbo.sales()
go

update GameSales
set other_sales = other_sales + 1, global_sales = global_sales + 1
go
-- Триггер INSTEAD OF

create trigger on_delete_GameSales
on GameSales
instead of delete
as
select title, na, eu, jp, other 
from dbo.sales()
go

delete GameSales
where eu_sales < 3
go

-- def
create trigger on_insert_GameCritics
on GameCritics
after insert
as
update GameCritics
set crit_score = 50 
where crit_score < 10
go

insert into GameCritics

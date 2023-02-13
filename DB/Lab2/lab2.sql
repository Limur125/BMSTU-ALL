with t as (select row_number() over (order by title) as id, *	
from VideoGames)
select title, release, developer, publisher, genre from t
where id between 100 and 200
-- 1.
select * from GameRating
where esrb_rating = 'M'
-- 2.
select * from GameSales
where global_sales between 15 and 30
-- 3.
select * from Platforms
where [name] like '%Xbox%'
-- 4.
select * from VideoGames
where developer in 
(
	select developer from GameDevs
	where city = 'Tokyo'
)
-- 5.
select * from VideoGames
where exists 
(
	select developer from GameDevs
	where GameDevs.developer = VideoGames.developer and city = 'Tokyo'
)
-- 6.
select * from GameCritics
where crit_score < all
(
	select crit_score from GameCritics
	where user_score > 9
)
-- 7.
select [platform], avg(crit_score) as avg_crit_score from GameCritics
where user_score < 5
group by [platform]
-- 8.
select 
	title,
	[platform],
	(select avg(crit_score) 
	from GameCritics
	where [platform] like 'Xbox%') as avg_crit_score 
from GameCritics
where [platform] like 'Xbox%'
-- 9.
select title, developer, 
	case release 
		when 2022 THEN 'This Year' 
		when 2021 THEN 'Last year' 
		else cast(2022 - release as varchar(5)) + ' years ago'
	end as 'When'
from VideoGames
-- 10.
select title, [platform],
	case
		when crit_score < 33 then 'Low'
		when crit_score < 66 then 'Average'
		else 'High'
	end as CriticsScore
from GameCritics
-- 11.
select [platform], avg(user_score) as us, avg(crit_score) as cs
into #avg_platform_score
from GameCritics
group by [platform]
-- 12.
select OD.title, developer, OD.crit_score 
from VideoGames as V join 
(
	select top 1000 GameCritics.title, crit_score
	from GameCritics 
	order by crit_score desc
) 
as OD on OD.title = V.title
-- 13.
select title, [platform], user_score
from GameCritics
where user_score >
(
	select avg(user_score) from GameCritics
	where [platform] in
	(
		select [name] from Platforms
		where [name] like 'Xbox%'
	)
) and [platform] like 'Xbox%'
-- 14.
select [platform], avg(user_score) as us, avg(crit_score) as cs
from GameCritics
group by [platform]
-- 15.
select [platform], avg(user_score) as us, avg(crit_score) as cs
from GameCritics
group by [platform]
having avg(user_score) > 5
-- 16.
insert into GameCritics(title, [platform], summary, user_score, crit_score)
values ('Atomic Heart', 'PC', 
'Prepare for the fight of your life. Choose optimal tactics for each unique opponent and use everything you can get your hands on, from the special abilities provided by your combat glove to heavy weapons of mass destruction.',
8.7,
90)
-- 17.
insert into Platforms([name], company, rel_year, processor, RAM)
select [platform], null, null ,null, null from GameCritics
where [platform] not in (select [name] from Platforms)
-- 18.
update GameCritics 
set crit_score = crit_score / 1.3 
WHERE [platform] like 'PlayStation%'
-- 19.
update GameCritics 
set crit_score = 
(
	select avg(crit_score) from GameCritics
)
WHERE [platform] = '3DS'
-- 20.
delete VideoGames
where release < 1980
-- 21.
delete VideoGames
where title in
(
	select game 
	from PlatformLink join Platforms on plat = Platforms.[name]
	where RAM < 3000
)
-- 22.
with Xbox(platname) as 
(
	select [name] from Platforms
	where [name] like 'Xbox%'
)

select avg(user_score) from GameCritics, Xbox
where [platform] in(platname)
go
-- 23.
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
-- 24.
select gc.*,
	max(gc.user_score) over (partition by [platform]) as max_user_Score,
	min(gc.crit_score) over (partition by [platform]) as min_crit_Score,
	avg(gc.user_score) over (partition by [platform]) as avg_user_Score
from GameCritics as gc
go
-- 25.
with triple_table as
(
	select *
	from VideoGames
	union all
	select *
	from VideoGames
	union all
	select *
	from VideoGames
),

delete_triple as
(
	select *, 
		row_number() over (partition by title order by title asc) as row_id
	from triple_table
)

select *
from delete_triple
where row_id = 1
--extra
drop table table_1;
drop table table_2;

create table table_1
(
	id int,
	var1 varchar(50),
	valid_from_dttm date,
	valid_to_dttm date
);

create table table_2
(
	id int,
	var2 varchar(50),
	valid_from_dttm date,
	valid_to_dttm date
);

insert into table_1(id, var1, valid_from_dttm, valid_to_dttm) values
(1, 'A', '2018-09-01', '2018-09-17'),
(1, 'B', '2018-09-18', '5999-12-31'),
(2, 'B', '2018-09-18', '5999-12-31');

insert into table_2(id, var2, valid_from_dttm, valid_to_dttm) values
(1, 'A', '2018-09-01', '2018-09-18'),
(1, 'B', '2018-09-19', '5999-12-31');


with dates as
(
	select t1.id, t1.var1, t2.var2, 
	case when t1.valid_from_dttm > t2.valid_from_dttm
		then t1.valid_from_dttm
		else t2.valid_from_dttm
	end as valid_from_dttm,
		case when t1.valid_to_dttm > t2.valid_to_dttm
		then t2.valid_to_dttm
		else t1.valid_to_dttm
	end as valid_to_dttm

	from table_1 as t1 join table_2 as t2 on t1.id = t2.id
)

select *
from dates
where valid_from_dttm <= valid_to_dttm
order by valid_from_dttm;
--def
select * from sys.all_columns
select * from sys.types
select * from sys.objects
select count(*)
from (sys.all_columns as c join sys.types as t on c.user_type_id = t.user_type_id) 
join sys.objects as o on c.object_id = o.object_id
where t.name = 'int' and type = 'U'
group by t.user_type_id

drop trigger on_delete_gamesales
select * from GameSales where title = 'Game123456'
delete GameSales where title like 'Game123456'
select * from GameSales where title = 'Game123456'
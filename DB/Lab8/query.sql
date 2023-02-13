create database VideoGames
use VideoGames
drop table  if exists labeight
create table labeight
(
	title varchar(200),
	release int,
	developer varchar(100),
	publisher varchar(100)
)
select * from labeight
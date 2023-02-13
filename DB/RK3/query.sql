create database rk3
use rk3;
SET DATEFORMAT dmy

drop table if exists logTable
drop table if exists employee
create table employee (
	id int not null primary key,
	fio varchar(70),
	birthdate date, 
	department varchar(70)
);


create table logTable(
	emp_id int references employee(id) not null,
	rdate date,
	dayweek varchar(15),
	arrtime time,
	arrtype int
);


insert into employee values 
	(1, 'aaaaaaaaaaaaaa', '25-09-1995', 'IT'),
	(2, 'bbbbbbbbbbbbbb', '30-09-1999', 'IT'),
	(3, 'cccccccccccccc', '25-09-1997', 'Fin'),
	(4, 'dddddddddddddd', '15-09-1990', 'Fin'),
	(5, 'eeeeeeeeeeeeee', '25-09-1996', 'IT'),
	(6, 'ffffffffffffff', '30-09-1991', 'IT');


insert into logTable values
	(1, '20-12-2018', 'Monday', '09:01', 1),
	(1, '20-12-2018', 'Monday', '09:12', 2),
	(1, '20-12-2018', 'Monday', '09:40', 1),
	(1, '20-12-2018', 'Monday', '12:01', 2),
	(1, '20-12-2018', 'Monday', '13:40', 1),
	(1, '20-12-2018', 'Monday', '20:40', 2),	
	(1, '21-12-2018', 'Monday', '09:01', 1),
	(1, '21-12-2018', 'Monday', '09:12', 2),
	(1, '21-12-2018', 'Monday', '09:40', 1),
	(1, '21-12-2018', 'Monday', '12:01', 2),
	(1, '21-12-2018', 'Monday', '13:40', 1),
	(1, '21-12-2018', 'Monday', '20:40', 2),
	(1, '22-12-2018', 'Monday', '09:01', 1),
	(1, '22-12-2018', 'Monday', '09:12', 2),
	(1, '22-12-2018', 'Monday', '09:40', 1),
	(1, '22-12-2018', 'Monday', '12:01', 2),
	(1, '22-12-2018', 'Monday', '13:40', 1),
	(1, '22-12-2018', 'Monday', '20:40', 2),
	(3, '21-12-2018', 'Monday', '09:15', 1),
	(3, '21-12-2018', 'Monday', '09:16', 2),
	(3, '21-12-2018', 'Monday', '09:40', 1),
	(3, '21-12-2018', 'Monday', '12:01', 2),
	(3, '21-12-2018', 'Monday', '13:40', 1),
	(3, '21-12-2018', 'Monday', '20:40', 2),
	(2, '21-12-2018', 'Monday', '08:51', 1),
	(2, '21-12-2018', 'Monday', '20:31', 2),
	(4, '21-12-2018', 'Monday', '09:51', 1),
	(4, '21-12-2018', 'Monday', '20:31', 2),
	(6, '21-12-2018', 'Monday', '09:51', 1),
	(6, '21-12-2018', 'Monday', '20:31', 2);
go

create function FindEmployee()
returns int
as
begin	
	declare @res int;
	with temp as (
	select min(lt.arrtime) as minartime, emp_id
	from logTable as lt
	where lt.arrtype = 1
	group by emp_id
	having DATEDIFF (MINUTE, '9:00', min(lt.arrtime) ) > 10),

	temp3 as (
	select DATEDIFF(year, e.birthdate, getdate()) as age
	from employee as e join temp as t on e.id = t.emp_id)

	select @res = min(temp3.age) from temp3
	return cast(@res as int)
end
go

select dbo.FindEmployee() as FIRE_HIM
go


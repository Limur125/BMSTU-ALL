SET DATEFORMAT dmy

drop table if exists Employee
create table Employee
(
    id int,
    FIO varchar(50),
    date_of_status date,
    [status] varchar(50)  
);

insert into Employee values
(1, 'Ivanov Ivan Ivanovich', '01-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '02-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '06-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '09-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '10-12-2022', 'Sick leave'),
(1, 'Ivanov Ivan Ivanovich', '11-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '12-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '16-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '19-12-2022', 'Work offline'),
(1, 'Ivanov Ivan Ivanovich', '20-12-2022', 'Sick leave'),
(1, 'Ivanov Ivan Ivanovich', '21-12-2022', 'Sick leave'),
(1, 'Ivanov Ivan Ivanovich', '22-12-2022', 'Distant work'),
(2, 'Petrov Petr Petrovich', '12-12-2022', 'Work offline'),
(2, 'Petrov Petr Petrovich', '13-12-2022', 'Work offline'),
(2, 'Petrov Petr Petrovich', '14-12-2022', 'Distant Work'),
(2, 'Petrov Petr Petrovich', '15-12-2022', 'Distant work'),
(2, 'Petrov Petr Petrovich', '16-12-2022', 'Work offline')
go

with temp1
as
(
    select *,
        lag(date_of_status) over(partition by id, [status] order by date_of_status) as prevDate,
        lead(date_of_status) over(partition by id, [status] order by date_of_status) as nextDate,
        count(id) over(partition by id, [status] order by date_of_status) as c
    from Employee
),
temp2 as 
(
    select T1.id, T1.FIO, T1.[status], T1.date_of_status as "fromdate", T2.date_of_status as "todate"
    from temp1 T1 join temp1 T2 on T1.id = T2.id and T1.[status] = T2.[status]
    where (T1.prevDate is null or day(T1.date_of_status) - day(T1.prevDate) > 1) and
        (T2.nextDate is null or day(T2.nextDate) - day(T2.date_of_status) > 1) and
        (day(T1.date_of_status) <= day(T2.date_of_status)) and 
        (day(T2.date_of_status) - day(T1.date_of_status) < T2.c)
),
temp3 as 
( 
    select * ,
    lag([status]) over (partition by id order by fromDate) as c2,
    lead([status]) over (partition by id order by fromDate) as c4
    from temp2
),
temp4 as
( 
    select *,
    min(fromDate) over (partition by id, [status]) as c3,
    max(toDate) over (partition by id, [status]) as c5
    from temp3
) select * from temp4
temp5 as (
select id , FIO, status, fromDate, 
    case when c4 = [status]
        then c5
        else toDate
    end as todate
from temp4
where c2 is null
),
temp6 as (
select id , FIO, status, todate, 
case when c2 = [status]
    then c3
    else fromdate
end as fromdate
from temp4
where c4 is null
)
select id, FIO, status, fromdate, todate
from temp4
where c2 is not null and c4 is not null and c2 <> status and c4 <> status 
union all
select * from temp5
union all 
select * from temp6
order by id, fromdate

drop table Rating
create table Rating
(
rate varchar(5) primary key,
[description] text
)

bulk insert Rating
from '\rate.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)
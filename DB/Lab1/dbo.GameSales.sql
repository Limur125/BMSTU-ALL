drop table GameSales;
CREATE TABLE [dbo].[GameSales] (
    [title]        VARCHAR (200)  NOT NULL,
    [na_sales]     NUMERIC (4, 1) NULL,
    [eu_sales]     NUMERIC (4, 1) NULL,
    [jp_sales]     NUMERIC (4, 1) NULL,
    [other_sales]  NUMERIC (4, 1) NULL,
    [global_sales] NUMERIC (4, 1) NULL,
    --PRIMARY KEY CLUSTERED ([title] ASC)
);

bulk insert GameSales
from '\sales2.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
)

--bulk insert GameSales
--from '\sales2.txt'
--with 
--(
--fieldterminator = '\t',
--rowterminator = '\n'
--)

--delete from GameSales
--where title not in
--(select title from VideoGames)


alter table GameSales
add constraint PK_Title primary key(title)
CREATE TABLE [dbo].[GameSales] (
    [title]        VARCHAR (200)  NOT NULL,
    [na_sales]     NUMERIC (4, 1) NULL,
    [eu_sales]     NUMERIC (4, 1) NULL,
    [jp_sales]     NUMERIC (4, 1) NULL,
    [other_sales]  NUMERIC (4, 1) NULL,
    [global_sales] NUMERIC (4, 1) NULL,
    CONSTRAINT [PK_Title] PRIMARY KEY CLUSTERED ([title] ASC)
);


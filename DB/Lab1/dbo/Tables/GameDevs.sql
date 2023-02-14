CREATE TABLE [dbo].[GameDevs] (
    [developer]       VARCHAR (100) NOT NULL,
    [city]            VARCHAR (100) NOT NULL,
    [autonomous_area] VARCHAR (100) NULL,
    [country]         VARCHAR (100) NOT NULL,
    [est_year]        INT           NULL,
    PRIMARY KEY CLUSTERED ([developer] ASC),
    CHECK ([est_year]>(0))
);


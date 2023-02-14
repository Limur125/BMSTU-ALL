CREATE TABLE [dbo].[VideoGames] (
    [title]     VARCHAR (200) NOT NULL,
    [release]   INT           NOT NULL,
    [developer] VARCHAR (100) NOT NULL,
    [publisher] TEXT          NOT NULL,
    [genre]     VARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([title] ASC),
    CHECK ([release]>(0)),
    CONSTRAINT [FK_Developers] FOREIGN KEY ([developer]) REFERENCES [dbo].[GameDevs] ([developer]),
    CONSTRAINT [FK_GameRating] FOREIGN KEY ([title]) REFERENCES [dbo].[GameRating] ([title]),
    CONSTRAINT [FK_GameTItle] FOREIGN KEY ([title]) REFERENCES [dbo].[GameCritics] ([title]),
    CONSTRAINT [FK_GameTitleSales] FOREIGN KEY ([title]) REFERENCES [dbo].[GameSales] ([title])
);


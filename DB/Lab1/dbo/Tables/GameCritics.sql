CREATE TABLE [dbo].[GameCritics] (
    [title]      VARCHAR (200)  NOT NULL,
    [platform]   VARCHAR (200)  NULL,
    [summary]    TEXT           NULL,
    [crit_score] INT            NULL,
    [user_score] NUMERIC (2, 1) NULL,
    PRIMARY KEY CLUSTERED ([title] ASC)
);


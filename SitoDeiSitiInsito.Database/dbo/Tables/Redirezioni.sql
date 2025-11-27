CREATE TABLE [dbo].[Redirezioni] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Url]    NVARCHAR (255) NOT NULL,
    [Attiva] BIT            NOT NULL,
    CONSTRAINT [PK_Redirezioni] PRIMARY KEY CLUSTERED ([Id] ASC)
);


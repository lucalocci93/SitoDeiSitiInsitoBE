CREATE TABLE [dbo].[Video] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Url]         NVARCHAR (255) NOT NULL,
    [Titolo]      NVARCHAR (255) NOT NULL,
    [Descrizione] NVARCHAR (MAX) NOT NULL,
    [Provider]    NVARCHAR (255) NULL,
    [Attivo]      BIT            NOT NULL,
    CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED ([Id] ASC)
);


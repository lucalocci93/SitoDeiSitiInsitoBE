CREATE TABLE [dbo].[Categoria] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Descrizione] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Categoria] PRIMARY KEY CLUSTERED ([Id] ASC)
);


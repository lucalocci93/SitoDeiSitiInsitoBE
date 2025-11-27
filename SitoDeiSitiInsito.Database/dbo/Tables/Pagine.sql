CREATE TABLE [dbo].[Pagine] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Pagina] VARCHAR (50) NULL,
    CONSTRAINT [PK_Pagine] PRIMARY KEY CLUSTERED ([Id] ASC)
);


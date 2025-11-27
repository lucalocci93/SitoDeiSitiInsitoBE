CREATE TABLE [dbo].[TipoDocumento] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Descrizione] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_TipoDocumento_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);


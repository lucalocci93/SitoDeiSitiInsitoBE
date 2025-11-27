CREATE TABLE [dbo].[Notifica] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Pagina]    INT              NOT NULL,
    [Testo]     NVARCHAR (MAX)   NULL,
    [Abilitata] BIT              NULL,
    CONSTRAINT [PK_Notifica] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notifica_Pagina] FOREIGN KEY ([Pagina]) REFERENCES [dbo].[Pagine] ([Id])
);


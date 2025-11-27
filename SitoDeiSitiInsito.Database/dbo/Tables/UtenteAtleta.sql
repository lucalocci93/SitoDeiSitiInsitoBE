CREATE TABLE [dbo].[UtenteAtleta] (
    [Rowguid]        UNIQUEIDENTIFIER NOT NULL,
    [Organizzazione] UNIQUEIDENTIFIER NULL,
    [Cintura]        INT              NULL,
    CONSTRAINT [PK_UtenteAtleta] PRIMARY KEY CLUSTERED ([Rowguid] ASC),
    CONSTRAINT [FK_UtenteAtleta_Cintura] FOREIGN KEY ([Cintura]) REFERENCES [dbo].[Cinture] ([Id]),
    CONSTRAINT [FK_UtenteAtleta_Organizzazione] FOREIGN KEY ([Organizzazione]) REFERENCES [dbo].[Organizzazioni] ([RowGuid]),
    CONSTRAINT [FK_UtenteAtleta_Utente] FOREIGN KEY ([Rowguid]) REFERENCES [dbo].[Utente] ([RowGuid])
);


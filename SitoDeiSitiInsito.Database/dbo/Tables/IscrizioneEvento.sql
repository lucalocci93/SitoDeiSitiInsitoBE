CREATE TABLE [dbo].[IscrizioneEvento] (
    [IdEvento]   UNIQUEIDENTIFIER NOT NULL,
    [IdUtente]   UNIQUEIDENTIFIER NOT NULL,
    [Note]       NVARCHAR (255)   NULL,
    [Gara]       UNIQUEIDENTIFIER NOT NULL,
    [Cancellata] BIT              NULL,
    CONSTRAINT [PK_IscrizioneEvento] PRIMARY KEY CLUSTERED ([IdEvento] ASC, [IdUtente] ASC, [Gara] ASC),
    CONSTRAINT [FK_IscrizioneEvento_Evento] FOREIGN KEY ([IdEvento]) REFERENCES [dbo].[Evento] ([Id]),
    CONSTRAINT [FK_IscrizioneEvento_Gare] FOREIGN KEY ([Gara]) REFERENCES [dbo].[Gare] ([Id]),
    CONSTRAINT [FK_IscrizioneEvento_Utente] FOREIGN KEY ([IdUtente]) REFERENCES [dbo].[Utente] ([RowGuid])
);


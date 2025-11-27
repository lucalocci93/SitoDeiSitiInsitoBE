CREATE TABLE [dbo].[UtenteInfo] (
    [RowGuid]     UNIQUEIDENTIFIER NOT NULL,
    [DataNascita] DATE             NULL,
    [Via]         NVARCHAR (50)    NULL,
    [Numero]      NVARCHAR (15)    NULL,
    [Citta]       NVARCHAR (50)    NULL,
    [Regione]     NVARCHAR (50)    NULL,
    [Nazione]     NVARCHAR (50)    NULL,
    CONSTRAINT [PK_UtenteInfo] PRIMARY KEY CLUSTERED ([RowGuid] ASC),
    CONSTRAINT [FK_UtenteInfoUtente] FOREIGN KEY ([RowGuid]) REFERENCES [dbo].[Utente] ([RowGuid]),
    CONSTRAINT [FK_UtentePrivacyUtente] FOREIGN KEY ([RowGuid]) REFERENCES [dbo].[Utente] ([RowGuid])
);


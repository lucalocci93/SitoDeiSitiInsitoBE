CREATE TABLE [dbo].[Abbonamento] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [TipoAbbonamento] INT              NOT NULL,
    [Utente]          UNIQUEIDENTIFIER NOT NULL,
    [DataIscrizione]  DATE             NULL,
    [DataScadenza]    DATE             NULL,
    [UrlPagamento]    VARCHAR (255)    NULL,
    [Attivo]          BIT              NULL,
    [Importo]         MONEY            NULL,
    [IdCheckout]      NVARCHAR (255)   NULL,
    [Pagato]          BIT              NULL,
    CONSTRAINT [PK_Abbonamento] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AbbonamentoTipoAbbonamento] FOREIGN KEY ([TipoAbbonamento]) REFERENCES [dbo].[TipoAbbonamento] ([Id]),
    CONSTRAINT [FK_AbbonamentoUtente] FOREIGN KEY ([Utente]) REFERENCES [dbo].[Utente] ([RowGuid]),
    CONSTRAINT [UC_Abbonamento] UNIQUE NONCLUSTERED ([Id] ASC, [TipoAbbonamento] ASC, [Utente] ASC)
);


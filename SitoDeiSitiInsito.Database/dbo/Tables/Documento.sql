CREATE TABLE [dbo].[Documento] (
    [IdDocumento]     UNIQUEIDENTIFIER CONSTRAINT [df_Documento_IdDocumento] DEFAULT (newid()) NOT NULL,
    [UtenteId]        UNIQUEIDENTIFIER NOT NULL,
    [TipoDocumentoId] INT              NOT NULL,
    [NomeDocumento]   VARCHAR (255)    NOT NULL,
    [DataCaricamento] DATE             NOT NULL,
    [DatiDocumento]   IMAGE            NOT NULL,
    CONSTRAINT [PK_Documento_IdDocumento] PRIMARY KEY CLUSTERED ([IdDocumento] ASC),
    CONSTRAINT [fk_Documento_TipoDocumento] FOREIGN KEY ([TipoDocumentoId]) REFERENCES [dbo].[TipoDocumento] ([Id]),
    CONSTRAINT [fk_Documento_Utente] FOREIGN KEY ([UtenteId]) REFERENCES [dbo].[Utente] ([RowGuid])
);


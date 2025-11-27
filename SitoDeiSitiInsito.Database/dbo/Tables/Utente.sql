CREATE TABLE [dbo].[Utente] (
    [Nome]       NVARCHAR (50)    NULL,
    [Cognome]    NVARCHAR (50)    NULL,
    [CodFiscale] NVARCHAR (16)    NULL,
    [EMail]      NVARCHAR (255)   NOT NULL,
    [Password]   NVARCHAR (255)   NULL,
    [IsAdmin]    BIT              NULL,
    [RowGuid]    UNIQUEIDENTIFIER CONSTRAINT [DF_DefaultRowGuid] DEFAULT (newid()) NOT NULL,
    [IsMaestro]  BIT              CONSTRAINT [Utente_IsMaestro_Default] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Utenti] PRIMARY KEY CLUSTERED ([EMail] ASC),
    CONSTRAINT [UC_Utente] UNIQUE NONCLUSTERED ([RowGuid] ASC)
);


CREATE TABLE [dbo].[Organizzazioni] (
    [RowGuid]     UNIQUEIDENTIFIER CONSTRAINT [Organizzazioni_Rowguid_Default] DEFAULT (newid()) NOT NULL,
    [Nome]        NVARCHAR (255)   NOT NULL,
    [Descrizione] NVARCHAR (1023)  NULL,
    [Indirizzo]   NVARCHAR (255)   NULL,
    [PartitaIva]  VARCHAR (11)     NULL,
    CONSTRAINT [PK_Organizzazioni] PRIMARY KEY CLUSTERED ([RowGuid] ASC)
);


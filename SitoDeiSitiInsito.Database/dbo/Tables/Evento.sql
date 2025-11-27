CREATE TABLE [dbo].[Evento] (
    [Id]                 UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [NomeEvento]         NVARCHAR (255)   NOT NULL,
    [DataInizioEvento]   DATE             NOT NULL,
    [DataFineEvento]     DATE             NOT NULL,
    [LuogoEvento]        NVARCHAR (255)   NOT NULL,
    [Categorie]          NVARCHAR (255)   NULL,
    [Descrizione]        NVARCHAR (1023)  NULL,
    [Link]               NVARCHAR (1023)  NULL,
    [Copertina]          IMAGE            NULL,
    [ImportoIscrizione]  MONEY            NULL,
    [ChiusuraIscrizioni] DATE             NULL,
    CONSTRAINT [PK_Evento] PRIMARY KEY CLUSTERED ([Id] ASC)
);


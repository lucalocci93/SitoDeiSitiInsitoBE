CREATE TABLE [dbo].[TipoAbbonamento] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Descrizione]     NVARCHAR (50) NULL,
    [GiorniDurata]    INT           NULL,
    [ScadMensile]     BIT           NULL,
    [ScadSettimanale] BIT           NULL,
    [ScadGiornaliera] BIT           NULL,
    CONSTRAINT [PK_TipoAbbonamento] PRIMARY KEY CLUSTERED ([Id] ASC)
);


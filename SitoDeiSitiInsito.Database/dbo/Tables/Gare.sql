CREATE TABLE [dbo].[Gare] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_Id] DEFAULT (newid()) NOT NULL,
    [Evento]            UNIQUEIDENTIFIER NOT NULL,
    [Nome]              NVARCHAR (1023)  NOT NULL,
    [ImportoIscrizione] MONEY            NULL,
    [Categoria]         INT              NULL,
    CONSTRAINT [PK_Gare] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Gare_Categoria] FOREIGN KEY ([Categoria]) REFERENCES [dbo].[Categoria] ([Id]),
    CONSTRAINT [FK_Gare_Evento] FOREIGN KEY ([Evento]) REFERENCES [dbo].[Evento] ([Id]),
    CONSTRAINT [UK_Gare] UNIQUE NONCLUSTERED ([Id] ASC, [Evento] ASC)
);


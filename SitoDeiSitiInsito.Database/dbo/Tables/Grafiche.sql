CREATE TABLE [dbo].[Grafiche] (
    [Id]                        INT             IDENTITY (1, 1) NOT NULL,
    [UrlImmagine]               NVARCHAR (1023) NULL,
    [Pagina]                    INT             NULL,
    [Sezione]                   INT             NULL,
    [UrlFromGoogleDrive]        BIT             NULL,
    [Titolo]                    NVARCHAR (255)  NULL,
    [Descrizione]               NVARCHAR (255)  NULL,
    [TestoAggiuntivo]           NVARCHAR (MAX)  NULL,
    [IsTestoAggiuntivoMarkdown] BIT             NULL,
    [Ordine]                    INT             NULL,
    [Attiva]                    BIT             NOT NULL,
    CONSTRAINT [PK_Immagini] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Immagini_Pagine] FOREIGN KEY ([Pagina]) REFERENCES [dbo].[Pagine] ([Id])
);


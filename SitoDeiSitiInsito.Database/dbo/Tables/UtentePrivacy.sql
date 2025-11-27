CREATE TABLE [dbo].[UtentePrivacy] (
    [RowGuid]           UNIQUEIDENTIFIER NOT NULL,
    [ConsensoInvioMail] BIT              NULL,
    CONSTRAINT [PK_UtentePrivacy] PRIMARY KEY CLUSTERED ([RowGuid] ASC)
);


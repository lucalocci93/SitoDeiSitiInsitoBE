CREATE TABLE [dbo].[Template] (
    [TemplateName]       NVARCHAR (255) NOT NULL,
    [TemplateBodyHtml]   NVARCHAR (MAX) NULL,
    [TemplateHeaderHtml] NVARCHAR (MAX) NULL,
    [TemplateFooterHtml] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([TemplateName] ASC)
);


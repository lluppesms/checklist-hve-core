CREATE TABLE [dbo].[TemplateCategories]
(
    [Id]             INT            IDENTITY(1,1) NOT NULL,
    [ListId]         INT            NOT NULL,
    [CategoryText]   NVARCHAR(200)  NOT NULL,
    [CategoryDscr]   NVARCHAR(500)  NULL,
    [ActiveInd]      NCHAR(1)       NOT NULL DEFAULT 'Y',
    [SortOrder]      INT            NOT NULL DEFAULT 0,
    [CreateDateTime] DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_TemplateCategories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TemplateCategories_TemplateLists] FOREIGN KEY ([ListId]) REFERENCES [dbo].[TemplateLists]([Id])
);

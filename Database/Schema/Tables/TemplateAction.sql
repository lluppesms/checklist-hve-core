CREATE TABLE [dbo].[TemplateActions]
(
    [Id]             INT            IDENTITY(1,1) NOT NULL,
    [CategoryId]     INT            NOT NULL,
    [ActionText]     NVARCHAR(200)  NOT NULL,
    [ActionDscr]     NVARCHAR(500)  NULL,
    [ActiveInd]      NCHAR(1)       NOT NULL DEFAULT 'Y',
    [SortOrder]      INT            NOT NULL DEFAULT 0,
    [CreateDateTime] DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_TemplateActions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TemplateActions_TemplateCategories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[TemplateCategories]([Id])
);

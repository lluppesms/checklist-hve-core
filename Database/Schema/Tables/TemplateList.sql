CREATE TABLE [dbo].[TemplateLists]
(
    [Id]             INT            IDENTITY(1,1) NOT NULL,
    [SetId]          INT            NOT NULL,
    [ListName]       NVARCHAR(100)  NOT NULL,
    [ListDscr]       NVARCHAR(500)  NULL,
    [ActiveInd]      NCHAR(1)       NOT NULL DEFAULT 'Y',
    [SortOrder]      INT            NOT NULL DEFAULT 0,
    [CreateDateTime] DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_TemplateLists] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TemplateLists_TemplateSets] FOREIGN KEY ([SetId]) REFERENCES [dbo].[TemplateSets]([Id])
);

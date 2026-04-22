CREATE TABLE [dbo].[CheckLists]
(
    [Id]               INT            IDENTITY(1,1) NOT NULL,
    [SetId]            INT            NOT NULL,
    [TemplateListId]   INT            NULL,
    [ListName]         NVARCHAR(100)  NOT NULL,
    [ListDscr]         NVARCHAR(500)  NULL,
    [ActiveInd]        NCHAR(1)       NOT NULL DEFAULT 'Y',
    [SortOrder]        INT            NOT NULL DEFAULT 0,
    [CreateDateTime]   DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_CheckLists] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CheckLists_CheckSets] FOREIGN KEY ([SetId]) REFERENCES [dbo].[CheckSets]([Id]),
    CONSTRAINT [FK_CheckLists_TemplateLists] FOREIGN KEY ([TemplateListId]) REFERENCES [dbo].[TemplateLists]([Id])
);

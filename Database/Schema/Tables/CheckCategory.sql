CREATE TABLE [dbo].[CheckCategories]
(
    [Id]                  INT            IDENTITY(1,1) NOT NULL,
    [ListId]              INT            NOT NULL,
    [TemplateCategoryId]  INT            NULL,
    [CategoryText]        NVARCHAR(200)  NOT NULL,
    [CategoryDscr]        NVARCHAR(500)  NULL,
    [ActiveInd]           NCHAR(1)       NOT NULL DEFAULT 'Y',
    [SortOrder]           INT            NOT NULL DEFAULT 0,
    [CreateDateTime]      DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_CheckCategories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CheckCategories_CheckLists] FOREIGN KEY ([ListId]) REFERENCES [dbo].[CheckLists]([Id])
);

CREATE TABLE [dbo].[CheckActions]
(
    [Id]             INT            IDENTITY(1,1) NOT NULL,
    [CategoryId]     INT            NOT NULL,
    [ListId]         INT            NOT NULL,
    [SetId]          INT            NOT NULL,
    [ActionText]     NVARCHAR(200)  NOT NULL,
    [ActionDscr]     NVARCHAR(500)  NULL,
    [CompleteInd]    NCHAR(1)       NOT NULL DEFAULT ' ',
    [CompletedBy]    NVARCHAR(100)  NULL,
    [CompletedAt]    DATETIME2      NULL,
    [SortOrder]      INT            NOT NULL DEFAULT 0,
    [CreateDateTime] DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_CheckActions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CheckActions_CheckCategories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[CheckCategories]([Id]),
    CONSTRAINT [FK_CheckActions_CheckLists] FOREIGN KEY ([ListId]) REFERENCES [dbo].[CheckLists]([Id]),
    CONSTRAINT [FK_CheckActions_CheckSets] FOREIGN KEY ([SetId]) REFERENCES [dbo].[CheckSets]([Id])
);

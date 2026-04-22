CREATE TABLE [dbo].[CheckSets]
(
    [Id]              INT            IDENTITY(1,1) NOT NULL,
    [TemplateSetId]   INT            NULL,
    [SetName]         NVARCHAR(100)  NOT NULL,
    [SetDscr]         NVARCHAR(500)  NULL,
    [OwnerName]       NVARCHAR(100)  NULL,
    [ActiveInd]       NCHAR(1)       NOT NULL DEFAULT 'Y',
    [SortOrder]       INT            NOT NULL DEFAULT 0,
    [CreateDateTime]  DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_CheckSets] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CheckSets_TemplateSets] FOREIGN KEY ([TemplateSetId]) REFERENCES [dbo].[TemplateSets]([Id])
);

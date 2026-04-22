CREATE TABLE [dbo].[TemplateSets]
(
    [Id]             INT            IDENTITY(1,1) NOT NULL,
    [SetName]        NVARCHAR(100)  NOT NULL,
    [SetDscr]        NVARCHAR(500)  NULL,
    [OwnerName]      NVARCHAR(100)  NULL,
    [ActiveInd]      NCHAR(1)       NOT NULL DEFAULT 'Y',
    [SortOrder]      INT            NOT NULL DEFAULT 0,
    [CreateDateTime] DATETIME2      NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_TemplateSets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

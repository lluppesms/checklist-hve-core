CREATE TABLE [dbo].[Customers]
(
    [Id]           INT            IDENTITY(1,1) NOT NULL,
    [UserId]       NVARCHAR(450)  NOT NULL,
    [CustomerName] NVARCHAR(100)  NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

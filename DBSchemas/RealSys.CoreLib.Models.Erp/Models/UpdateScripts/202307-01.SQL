------ Add IntTypes Table ------
Alter Table [dbo].[InvItems]
add [InvTypeId] int NULL;

CREATE TABLE [dbo].[InvTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(50)  NOT NULL,
    [Remarks] nvarchar(150)  NOT NULL,
    [SysCode] nvarchar(10)  NOT NULL
);
GO

ALTER TABLE [dbo].[InvTypes]
ADD CONSTRAINT [PK_InvTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [FK_InvTypeInvItem]
    FOREIGN KEY ([InvTypeId])
    REFERENCES [dbo].[InvTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_InvTypeInvItem]
ON [dbo].[InvItems]
    ([InvTypeId]);
GO
---------------------------------------

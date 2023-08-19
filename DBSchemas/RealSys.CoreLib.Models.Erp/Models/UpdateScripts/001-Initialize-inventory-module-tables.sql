------ Add IntTypes Table ------
Alter Table [dbo].[InvItems]
add [InvTypeId] int NULL;

--CREATE TABLE [dbo].[InvTypes] (
--    [Id] int IDENTITY(1,1) NOT NULL,
--    [Desc] nvarchar(50)  NOT NULL,
--    [Remarks] nvarchar(150)  NOT NULL,
--    [SysCode] nvarchar(10)  NOT NULL
--);
--GO

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

--CREATE INDEX [IX_FK_InvTypeInvItem]
--ON [dbo].[InvItems]
--    ([InvTypeId]);
--GO
---------------------------------------

----- Add Item Category and UOM -------

ALTER TABLE [dbo].[InvItems]
    ADD [InvCategoryId] int  NOT NULL DEFAULT(1),
    [InvUomId] int  NOT NULL  DEFAULT(1);



-- Creating table 'InvCategories'
--CREATE TABLE [dbo].[InvCategories] (
--    [Id] int IDENTITY(1,1) NOT NULL,
--    [Code] nvarchar(20)  NOT NULL,
--    [Description] nvarchar(50)  NOT NULL,
--    [Remarks] nvarchar(50)  NOT NULL
--);


-- Creating table 'InvUoms'
CREATE TABLE [dbo].[InvUoms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [uom] nvarchar(20)  NOT NULL
);


-- Creating primary key on [Id] in table 'InvCategories'
ALTER TABLE [dbo].[InvCategories]
ADD CONSTRAINT [PK_InvCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvUoms'
ALTER TABLE [dbo].[InvUoms]
ADD CONSTRAINT [PK_InvUoms]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating foreign key on [InvCategoryId] in table 'InvItems'
ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [FK_InvCategoryInvItem]
    FOREIGN KEY ([InvCategoryId])
    REFERENCES [dbo].[InvCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvCategoryInvItem'
--CREATE INDEX [IX_FK_InvCategoryInvItem]
--ON [dbo].[InvItems]
--    ([InvCategoryId]);



-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvItem'
--CREATE INDEX [IX_FK_InvUomInvItem]
--ON [dbo].[InvItems]
--    ([InvUomId]);


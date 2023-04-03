
CREATE TABLE "Reports" (
    "Id" INTEGER NOT NULL IDENTITY PRIMARY KEY,
    "Name" varchar(250) NULL,
    "DisplayName" varchar(250) NULL,
    "LayoutData" varbinary(max) NULL
);

CREATE TABLE "SqlDataConnections" (
    "Id" INTEGER NOT NULL IDENTITY PRIMARY KEY ,
    "Name" nvarchar(max) NULL,
    "DisplayName" nvarchar(max) NULL,
    "ConnectionString" nvarchar(max) NULL
);

CREATE TABLE "JsonDataConnections" (
    "Id" INTEGER NOT NULL IDENTITY PRIMARY KEY ,
    "Name" nvarchar(max) NULL,
    "DisplayName" nvarchar(max) NULL,
    "ConnectionString" nvarchar(max) NULL
);




--Real Sys added structure--
-- Creating table 'RptCategories'
CREATE TABLE [dbo].[RptCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NULL,
    [Description] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'RptReportCats'
CREATE TABLE [dbo].[RptReportCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RptCategoryId] int  NOT NULL,
    [ReportId] int  NOT NULL
);
GO

-- Creating table 'RptReportUsers'
CREATE TABLE [dbo].[RptReportUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ReportId] int  NOT NULL,
    [AspNetUserId] nvarchar(255)  NOT NULL,
    [RptAccessTypeId] int  NOT NULL
);
GO

-- Creating table 'RptReportRoles'
CREATE TABLE [dbo].[RptReportRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ReportId] int  NOT NULL,
    [AspNetRoleId] int  NOT NULL,
    [RptAccessTypeId] int  NOT NULL
);
GO

-- Creating table 'RptAccessTypes'
CREATE TABLE [dbo].[RptAccessTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL
);
GO


-- Creating primary key on [Id] in table 'RptCategories'
ALTER TABLE [dbo].[RptCategories]
ADD CONSTRAINT [PK_RptCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RptReportCats'
ALTER TABLE [dbo].[RptReportCats]
ADD CONSTRAINT [PK_RptReportCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RptReportUsers'
ALTER TABLE [dbo].[RptReportUsers]
ADD CONSTRAINT [PK_RptReportUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RptReportRoles'
ALTER TABLE [dbo].[RptReportRoles]
ADD CONSTRAINT [PK_RptReportRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RptAccessTypes'
ALTER TABLE [dbo].[RptAccessTypes]
ADD CONSTRAINT [PK_RptAccessTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating foreign key on [RptCategoryId] in table 'RptReportCats'
ALTER TABLE [dbo].[RptReportCats]
ADD CONSTRAINT [FK_RptCategoryRptReportCat]
    FOREIGN KEY ([RptCategoryId])
    REFERENCES [dbo].[RptCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RptCategoryRptReportCat'
CREATE INDEX [IX_FK_RptCategoryRptReportCat]
ON [dbo].[RptReportCats]
    ([RptCategoryId]);
GO

-- Creating foreign key on [RptAccessTypeId] in table 'RptReportRoles'
ALTER TABLE [dbo].[RptReportRoles]
ADD CONSTRAINT [FK_RptAccessTypeRptReportRoles]
    FOREIGN KEY ([RptAccessTypeId])
    REFERENCES [dbo].[RptAccessTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RptAccessTypeRptReportRoles'
CREATE INDEX [IX_FK_RptAccessTypeRptReportRoles]
ON [dbo].[RptReportRoles]
    ([RptAccessTypeId]);
GO

-- Creating foreign key on [RptAccessTypeId] in table 'RptReportUsers'
ALTER TABLE [dbo].[RptReportUsers]
ADD CONSTRAINT [FK_RptAccessTypeRptReportUser]
    FOREIGN KEY ([RptAccessTypeId])
    REFERENCES [dbo].[RptAccessTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RptAccessTypeRptReportUser'
CREATE INDEX [IX_FK_RptAccessTypeRptReportUser]
ON [dbo].[RptReportUsers]
    ([RptAccessTypeId]);
GO


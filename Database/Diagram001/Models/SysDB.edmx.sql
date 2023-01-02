
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/19/2022 09:56:04
-- Generated from EDMX file: C:\Users\Acer-PC\Documents\GitHub\eJob20\JobsV1\Models\SysDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-JobsV1-20160528101923];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SysServiceEntServices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntServices] DROP CONSTRAINT [FK_SysServiceEntServices];
GO
IF OBJECT_ID(N'[dbo].[FK_SysServiceSysServiceMenu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SysServiceMenus] DROP CONSTRAINT [FK_SysServiceSysServiceMenu];
GO
IF OBJECT_ID(N'[dbo].[FK_EntCompanyEntServices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntServices] DROP CONSTRAINT [FK_EntCompanyEntServices];
GO
IF OBJECT_ID(N'[dbo].[FK_EntCompanyEntSupportFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntSupportFiles] DROP CONSTRAINT [FK_EntCompanyEntSupportFile];
GO
IF OBJECT_ID(N'[dbo].[FK_EntCompanyEntAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntAddresses] DROP CONSTRAINT [FK_EntCompanyEntAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_EntCompanyEntContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntContacts] DROP CONSTRAINT [FK_EntCompanyEntContact];
GO
IF OBJECT_ID(N'[dbo].[FK_EntBusinessEntSetting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntSettings] DROP CONSTRAINT [FK_EntBusinessEntSetting];
GO
IF OBJECT_ID(N'[dbo].[FK_SysFileTypeEntSupportFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntSupportFiles] DROP CONSTRAINT [FK_SysFileTypeEntSupportFile];
GO
IF OBJECT_ID(N'[dbo].[FK_SysSetupTypeEntAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntAddresses] DROP CONSTRAINT [FK_SysSetupTypeEntAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_SysSetupTypeEntContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntContacts] DROP CONSTRAINT [FK_SysSetupTypeEntContact];
GO
IF OBJECT_ID(N'[dbo].[FK_SysSetupTypeEntSetting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EntSettings] DROP CONSTRAINT [FK_SysSetupTypeEntSetting];
GO
IF OBJECT_ID(N'[dbo].[FK_SysMenuSysServiceMenu]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SysServiceMenus] DROP CONSTRAINT [FK_SysMenuSysServiceMenu];
GO
IF OBJECT_ID(N'[dbo].[FK_SysMenuSysUserAccess]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SysAccessUsers] DROP CONSTRAINT [FK_SysMenuSysUserAccess];
GO
IF OBJECT_ID(N'[dbo].[FK_SysMenuSysRoleAccess]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SysAccessRoles] DROP CONSTRAINT [FK_SysMenuSysRoleAccess];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SysServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysServices];
GO
IF OBJECT_ID(N'[dbo].[EntBusinesses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntBusinesses];
GO
IF OBJECT_ID(N'[dbo].[EntServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntServices];
GO
IF OBJECT_ID(N'[dbo].[SysSetupTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysSetupTypes];
GO
IF OBJECT_ID(N'[dbo].[EntSupportFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntSupportFiles];
GO
IF OBJECT_ID(N'[dbo].[EntAddresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntAddresses];
GO
IF OBJECT_ID(N'[dbo].[EntContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntContacts];
GO
IF OBJECT_ID(N'[dbo].[SysMenus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysMenus];
GO
IF OBJECT_ID(N'[dbo].[SysServiceMenus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysServiceMenus];
GO
IF OBJECT_ID(N'[dbo].[SysAccessUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysAccessUsers];
GO
IF OBJECT_ID(N'[dbo].[SysAccessRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysAccessRoles];
GO
IF OBJECT_ID(N'[dbo].[SysCmdIdRefs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysCmdIdRefs];
GO
IF OBJECT_ID(N'[dbo].[EntSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntSettings];
GO
IF OBJECT_ID(N'[dbo].[SysSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SysSettings];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SysServices'
CREATE TABLE [dbo].[SysServices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SysCode] nvarchar(10)  NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [Status] nvarchar(5)  NOT NULL,
    [IconPath] nvarchar(max)  NOT NULL,
    [SeqNo] int  NOT NULL,
    [IconFA] nvarchar(max)  NULL
);
GO

-- Creating table 'EntBusinesses'
CREATE TABLE [dbo].[EntBusinesses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [ShortName] nvarchar(15)  NOT NULL,
    [BussRegNo] nvarchar(20)  NOT NULL,
    [User] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'EntServices'
CREATE TABLE [dbo].[EntServices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SysServiceId] int  NOT NULL,
    [EntCompanyId] int  NOT NULL,
    [Expiry] datetime  NOT NULL
);
GO

-- Creating table 'SysSetupTypes'
CREATE TABLE [dbo].[SysSetupTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [SysCode] nvarchar(10)  NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(250)  NOT NULL,
    [Status] nvarchar(3)  NOT NULL
);
GO

-- Creating table 'EntSupportFiles'
CREATE TABLE [dbo].[EntSupportFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SysFileTypeId] int  NOT NULL,
    [EntCompanyId] int  NOT NULL,
    [UrlPath] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'EntAddresses'
CREATE TABLE [dbo].[EntAddresses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EntCompanyId] int  NOT NULL,
    [SysSetupTypeId] int  NOT NULL,
    [add1] nvarchar(50)  NOT NULL,
    [Add2] nvarchar(50)  NOT NULL,
    [Add3] nvarchar(50)  NOT NULL,
    [Add4] nvarchar(50)  NOT NULL,
    [City] nvarchar(50)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [Telno1] nvarchar(20)  NOT NULL,
    [Telno2] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'EntContacts'
CREATE TABLE [dbo].[EntContacts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EntCompanyId] int  NOT NULL,
    [SysSetupTypeId] int  NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Position] nvarchar(80)  NOT NULL,
    [TelNo1] nvarchar(20)  NOT NULL,
    [TelNo2] nvarchar(20)  NOT NULL,
    [email] nvarchar(30)  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'SysMenus'
CREATE TABLE [dbo].[SysMenus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Menu] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [ParentId] int  NOT NULL,
    [Controller] nvarchar(30)  NULL,
    [Action] nvarchar(30)  NULL,
    [Params] nvarchar(80)  NULL,
    [CmdId] int  NULL,
    [Seqno] int  NOT NULL
);
GO

-- Creating table 'SysServiceMenus'
CREATE TABLE [dbo].[SysServiceMenus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SysMenuId] int  NOT NULL,
    [SysServiceId] int  NOT NULL
);
GO

-- Creating table 'SysAccessUsers'
CREATE TABLE [dbo].[SysAccessUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [SysMenuId] int  NOT NULL,
    [Seqno] int  NOT NULL
);
GO

-- Creating table 'SysAccessRoles'
CREATE TABLE [dbo].[SysAccessRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleId] nvarchar(max)  NOT NULL,
    [SysMenuId] int  NOT NULL
);
GO

-- Creating table 'SysCmdIdRefs'
CREATE TABLE [dbo].[SysCmdIdRefs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CmdId] int  NOT NULL,
    [Description] nvarchar(50)  NOT NULL,
    [Remarks] nvarchar(150)  NULL
);
GO

-- Creating table 'EntSettings'
CREATE TABLE [dbo].[EntSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SysSetupTypeId] int  NOT NULL,
    [EntBusinessId] int  NOT NULL,
    [Value1] nvarchar(max)  NOT NULL,
    [Value2] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SysSettings'
CREATE TABLE [dbo].[SysSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SysKey] nvarchar(max)  NOT NULL,
    [SysValue] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SysServices'
ALTER TABLE [dbo].[SysServices]
ADD CONSTRAINT [PK_SysServices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntBusinesses'
ALTER TABLE [dbo].[EntBusinesses]
ADD CONSTRAINT [PK_EntBusinesses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntServices'
ALTER TABLE [dbo].[EntServices]
ADD CONSTRAINT [PK_EntServices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysSetupTypes'
ALTER TABLE [dbo].[SysSetupTypes]
ADD CONSTRAINT [PK_SysSetupTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntSupportFiles'
ALTER TABLE [dbo].[EntSupportFiles]
ADD CONSTRAINT [PK_EntSupportFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntAddresses'
ALTER TABLE [dbo].[EntAddresses]
ADD CONSTRAINT [PK_EntAddresses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntContacts'
ALTER TABLE [dbo].[EntContacts]
ADD CONSTRAINT [PK_EntContacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysMenus'
ALTER TABLE [dbo].[SysMenus]
ADD CONSTRAINT [PK_SysMenus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysServiceMenus'
ALTER TABLE [dbo].[SysServiceMenus]
ADD CONSTRAINT [PK_SysServiceMenus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysAccessUsers'
ALTER TABLE [dbo].[SysAccessUsers]
ADD CONSTRAINT [PK_SysAccessUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysAccessRoles'
ALTER TABLE [dbo].[SysAccessRoles]
ADD CONSTRAINT [PK_SysAccessRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysCmdIdRefs'
ALTER TABLE [dbo].[SysCmdIdRefs]
ADD CONSTRAINT [PK_SysCmdIdRefs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EntSettings'
ALTER TABLE [dbo].[EntSettings]
ADD CONSTRAINT [PK_EntSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysSettings'
ALTER TABLE [dbo].[SysSettings]
ADD CONSTRAINT [PK_SysSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SysServiceId] in table 'EntServices'
ALTER TABLE [dbo].[EntServices]
ADD CONSTRAINT [FK_SysServiceEntServices]
    FOREIGN KEY ([SysServiceId])
    REFERENCES [dbo].[SysServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysServiceEntServices'
CREATE INDEX [IX_FK_SysServiceEntServices]
ON [dbo].[EntServices]
    ([SysServiceId]);
GO

-- Creating foreign key on [SysServiceId] in table 'SysServiceMenus'
ALTER TABLE [dbo].[SysServiceMenus]
ADD CONSTRAINT [FK_SysServiceSysServiceMenu]
    FOREIGN KEY ([SysServiceId])
    REFERENCES [dbo].[SysServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysServiceSysServiceMenu'
CREATE INDEX [IX_FK_SysServiceSysServiceMenu]
ON [dbo].[SysServiceMenus]
    ([SysServiceId]);
GO

-- Creating foreign key on [EntCompanyId] in table 'EntServices'
ALTER TABLE [dbo].[EntServices]
ADD CONSTRAINT [FK_EntCompanyEntServices]
    FOREIGN KEY ([EntCompanyId])
    REFERENCES [dbo].[EntBusinesses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EntCompanyEntServices'
CREATE INDEX [IX_FK_EntCompanyEntServices]
ON [dbo].[EntServices]
    ([EntCompanyId]);
GO

-- Creating foreign key on [EntCompanyId] in table 'EntSupportFiles'
ALTER TABLE [dbo].[EntSupportFiles]
ADD CONSTRAINT [FK_EntCompanyEntSupportFile]
    FOREIGN KEY ([EntCompanyId])
    REFERENCES [dbo].[EntBusinesses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EntCompanyEntSupportFile'
CREATE INDEX [IX_FK_EntCompanyEntSupportFile]
ON [dbo].[EntSupportFiles]
    ([EntCompanyId]);
GO

-- Creating foreign key on [EntCompanyId] in table 'EntAddresses'
ALTER TABLE [dbo].[EntAddresses]
ADD CONSTRAINT [FK_EntCompanyEntAddress]
    FOREIGN KEY ([EntCompanyId])
    REFERENCES [dbo].[EntBusinesses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EntCompanyEntAddress'
CREATE INDEX [IX_FK_EntCompanyEntAddress]
ON [dbo].[EntAddresses]
    ([EntCompanyId]);
GO

-- Creating foreign key on [EntCompanyId] in table 'EntContacts'
ALTER TABLE [dbo].[EntContacts]
ADD CONSTRAINT [FK_EntCompanyEntContact]
    FOREIGN KEY ([EntCompanyId])
    REFERENCES [dbo].[EntBusinesses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EntCompanyEntContact'
CREATE INDEX [IX_FK_EntCompanyEntContact]
ON [dbo].[EntContacts]
    ([EntCompanyId]);
GO

-- Creating foreign key on [EntBusinessId] in table 'EntSettings'
ALTER TABLE [dbo].[EntSettings]
ADD CONSTRAINT [FK_EntBusinessEntSetting]
    FOREIGN KEY ([EntBusinessId])
    REFERENCES [dbo].[EntBusinesses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EntBusinessEntSetting'
CREATE INDEX [IX_FK_EntBusinessEntSetting]
ON [dbo].[EntSettings]
    ([EntBusinessId]);
GO

-- Creating foreign key on [SysFileTypeId] in table 'EntSupportFiles'
ALTER TABLE [dbo].[EntSupportFiles]
ADD CONSTRAINT [FK_SysFileTypeEntSupportFile]
    FOREIGN KEY ([SysFileTypeId])
    REFERENCES [dbo].[SysSetupTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysFileTypeEntSupportFile'
CREATE INDEX [IX_FK_SysFileTypeEntSupportFile]
ON [dbo].[EntSupportFiles]
    ([SysFileTypeId]);
GO

-- Creating foreign key on [SysSetupTypeId] in table 'EntAddresses'
ALTER TABLE [dbo].[EntAddresses]
ADD CONSTRAINT [FK_SysSetupTypeEntAddress]
    FOREIGN KEY ([SysSetupTypeId])
    REFERENCES [dbo].[SysSetupTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysSetupTypeEntAddress'
CREATE INDEX [IX_FK_SysSetupTypeEntAddress]
ON [dbo].[EntAddresses]
    ([SysSetupTypeId]);
GO

-- Creating foreign key on [SysSetupTypeId] in table 'EntContacts'
ALTER TABLE [dbo].[EntContacts]
ADD CONSTRAINT [FK_SysSetupTypeEntContact]
    FOREIGN KEY ([SysSetupTypeId])
    REFERENCES [dbo].[SysSetupTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysSetupTypeEntContact'
CREATE INDEX [IX_FK_SysSetupTypeEntContact]
ON [dbo].[EntContacts]
    ([SysSetupTypeId]);
GO

-- Creating foreign key on [SysSetupTypeId] in table 'EntSettings'
ALTER TABLE [dbo].[EntSettings]
ADD CONSTRAINT [FK_SysSetupTypeEntSetting]
    FOREIGN KEY ([SysSetupTypeId])
    REFERENCES [dbo].[SysSetupTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysSetupTypeEntSetting'
CREATE INDEX [IX_FK_SysSetupTypeEntSetting]
ON [dbo].[EntSettings]
    ([SysSetupTypeId]);
GO

-- Creating foreign key on [SysMenuId] in table 'SysServiceMenus'
ALTER TABLE [dbo].[SysServiceMenus]
ADD CONSTRAINT [FK_SysMenuSysServiceMenu]
    FOREIGN KEY ([SysMenuId])
    REFERENCES [dbo].[SysMenus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysMenuSysServiceMenu'
CREATE INDEX [IX_FK_SysMenuSysServiceMenu]
ON [dbo].[SysServiceMenus]
    ([SysMenuId]);
GO

-- Creating foreign key on [SysMenuId] in table 'SysAccessUsers'
ALTER TABLE [dbo].[SysAccessUsers]
ADD CONSTRAINT [FK_SysMenuSysUserAccess]
    FOREIGN KEY ([SysMenuId])
    REFERENCES [dbo].[SysMenus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysMenuSysUserAccess'
CREATE INDEX [IX_FK_SysMenuSysUserAccess]
ON [dbo].[SysAccessUsers]
    ([SysMenuId]);
GO

-- Creating foreign key on [SysMenuId] in table 'SysAccessRoles'
ALTER TABLE [dbo].[SysAccessRoles]
ADD CONSTRAINT [FK_SysMenuSysRoleAccess]
    FOREIGN KEY ([SysMenuId])
    REFERENCES [dbo].[SysMenus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SysMenuSysRoleAccess'
CREATE INDEX [IX_FK_SysMenuSysRoleAccess]
ON [dbo].[SysAccessRoles]
    ([SysMenuId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
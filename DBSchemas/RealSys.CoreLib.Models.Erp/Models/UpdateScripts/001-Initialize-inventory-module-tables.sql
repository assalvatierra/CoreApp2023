---- update aspnetUser ----
update dbo.AspNetRoles set Name='ADMIN' where Name='Admin';

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


Alter Table [dbo].[InvItems]
ADD	[InvTypeId] int NULL;


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


alter table [dbo].[AspNetUsers]
ADD 
	[ConcurrencyStamp] [nvarchar](256) NULL,
	[LockoutEnd] [datetime] NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL
go

update [dbo].[AspNetUsers]
set [NormalizedEmail] = UPPER([Email])
,[NormalizedUserName] = UPPER([UserName])
go

alter table [dbo].[AspNetRoles]
add	
	[NormalizedName] [nvarchar](128) NULL
	,[ConcurrencyStamp] [nvarchar](128) NULL
go

update [dbo].[AspNetRoles]
set [NormalizedName] = UPPER([Name])
go


CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO
ALTER TABLE [dbo].[AspNetRoleClaims]
ADD CONSTRAINT [PK_AspNetRoleClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating foreign key on [RoleId] in table 'AspNetRoleClaims'
ALTER TABLE [dbo].[AspNetRoleClaims]
ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetRoleClaims_AspNetRoles_RoleId'
CREATE INDEX [IX_FK_AspNetRoleClaims_AspNetRoles_RoleId]
ON [dbo].[AspNetRoleClaims]
    ([RoleId]);
GO

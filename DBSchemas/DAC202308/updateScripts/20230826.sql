ALTER TABLE dbo.InvItems ALTER COLUMN ItemCode nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;
ALTER TABLE dbo.InvItemSpec_Steel ALTER COLUMN Description nvarchar(180) COLLATE SQL_Latin1_General_CP1_CI_AS NULL;

/****** Object:  Table [dbo].[SqlDataConnections]    Script Date: 8/28/2023 11:10:45 PM ******/

CREATE TABLE [dbo].[SqlDataConnections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[DisplayName] [nvarchar](max) NULL,
	[ConnectionString] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
;




---- Initialize Admin Login ----
insert into dbo.AspNetRoles([Id],[Name],[NormalizedName]) values(1,'Admin','Admin');
INSERT INTO dbo.AspNetUsers (Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,ConcurrencyStamp,LockoutEnd,NormalizedEmail,NormalizedUserName) VALUES
	 (N'5809ccd3-8917-4986-8911-8f03dbc615c4',N'admin@gmail.com',0,N'AQAAAAEAACcQAAAAEG1rjq1WkRgpOZkiwHmtxttOTT/Eqe3yC6dX+OGlGic8oVWWw29/yJVt2Lqs3goOeA==',N'AAZIYXEKBAHI2LT3UTQAXFDIU3BJ4NTC',NULL,0,0,NULL,1,0,N'admin@gmail.com',N'259d43dc-1ace-4d39-ac66-328bbef9a32b',NULL,N'ADMIN@GMAIL.COM',N'ADMIN@GMAIL.COM');

INSERT INTO dbo.AspNetUserRoles (UserId,RoleId) VALUES
	 (N'5809ccd3-8917-4986-8911-8f03dbc615c4',N'1');



---- Initialize Main Store/Warehosue ----
INSERT INTO dbo.InvStores (StoreName) VALUES
	 (N'MAIN');
	 
INSERT INTO dbo.InvTrxHdrStatus (Status,OrderNo) VALUES
	 (N'Request',1),
	 (N'Approved',2),
	 (N'Closed',5),
	 (N'Cancelled',6),
	 (N'Verified',3),
	 (N'Accepted',4);

INSERT INTO dbo.InvTrxTypes ([Type]) VALUES
	 (N'Receive'),
	 (N'Release'),
	 (N'Adjust');

INSERT INTO dbo.InvTrxDtlOperators (Description,Operator) VALUES
	 (N'Add',N'+'),
	 (N'Subtract',N'-');



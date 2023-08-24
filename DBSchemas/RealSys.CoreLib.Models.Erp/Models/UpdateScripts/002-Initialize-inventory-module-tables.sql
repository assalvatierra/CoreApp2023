
Alter Table [dbo].[InvItems]
ADD	
	[InvCategoryId] int NULL
    ,[InvUomId] int NULL
	,[Material] varchar(80)
	,[Weight] decimal(18,2)
	,[Code] nvarchar(40)
;
GO

-- Creating table 'InvItemClasses'
CREATE TABLE [dbo].[InvItemClasses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvClassificationId] int  NOT NULL
);
GO

-- Creating table 'InvClassifications'
CREATE TABLE [dbo].[InvClassifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Classification] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvSuppliers'
CREATE TABLE [dbo].[InvSuppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NULL
);
GO

-- Creating table 'InvSupplierItems'
CREATE TABLE [dbo].[InvSupplierItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvSupplierId] int  NOT NULL,
    [InvItemId] int  NOT NULL,
    [Remarks] nvarchar(max)  NULL,
    [Price] decimal(10,2)  NOT NULL,
    [LastUpdate] datetime  NOT NULL,
    [LeadTime] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvStores'
CREATE TABLE [dbo].[InvStores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StoreName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvPoHdrs'
CREATE TABLE [dbo].[InvPoHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvSupplierId] int  NOT NULL,
    [InvStoreId] int  NOT NULL,
    [DtPo] datetime  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [InvPoHdrStatusId] int  NOT NULL
);
GO

-- Creating table 'InvPoItems'
CREATE TABLE [dbo].[InvPoItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvPoHdrId] int  NOT NULL,
    [InvItemId] int  NOT NULL,
    [ItemQty] nvarchar(max)  NOT NULL,
    [InvUomId] int  NOT NULL
);
GO

-- Creating table 'InvRecHdrs'
CREATE TABLE [dbo].[InvRecHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreId] int  NOT NULL,
    [InvSupplierId] int  NOT NULL,
    [DtRec] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvRecItems'
CREATE TABLE [dbo].[InvRecItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvRecHdrId] int  NOT NULL,
    [InvUomId] int  NOT NULL,
    [ItemQty] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvRequestHdrs'
CREATE TABLE [dbo].[InvRequestHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreId] int  NOT NULL,
    [DtRec] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvRequestItems'
CREATE TABLE [dbo].[InvRequestItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvRequestHdrId] int  NOT NULL,
    [InvUomId] int  NOT NULL,
    [ItemQty] nvarchar(max)  NOT NULL,
    [InvRecItemId] int  NOT NULL
);
GO

-- Creating table 'InvAdjHdrs'
CREATE TABLE [dbo].[InvAdjHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreId] int  NOT NULL,
    [dtAdj] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvAdjItems'
CREATE TABLE [dbo].[InvAdjItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvAdjHdrId] int  NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvUomId] int  NOT NULL,
    [QtyAdded] nvarchar(max)  NOT NULL,
    [QtyDeduct] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvUoms'
CREATE TABLE [dbo].[InvUoms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [uom] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvTrxHdrs'
CREATE TABLE [dbo].[InvTrxHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreId] int  NOT NULL,
    [DtTrx] datetime  NOT NULL,
    [UserId] nvarchar(80)  NOT NULL,
    [Remarks] nvarchar(120)  NULL,
    [InvTrxTypeId] int  NOT NULL,
    [InvTrxHdrStatusId] int  NOT NULL
);
GO

-- Creating table 'InvTrxDtls'
CREATE TABLE [dbo].[InvTrxDtls] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvTrxHdrId] int  NOT NULL,
    [InvUomId] int  NOT NULL,
    [ItemQty] int  NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvTrxDtlOperatorId] int  NOT NULL,
    [LotNo] int  NULL,
    [BatchNo] nvarchar(20)  NULL
);
GO

-- Creating table 'InvTrxTypes'
CREATE TABLE [dbo].[InvTrxTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'InvPoHdrStatus'
CREATE TABLE [dbo].[InvPoHdrStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(20)  NOT NULL,
    [OrderNo] int  NOT NULL
);
GO

-- Creating table 'InvTrxHdrStatus'
CREATE TABLE [dbo].[InvTrxHdrStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(20)  NOT NULL,
    [OrderNo] int  NOT NULL
);
GO

-- Creating table 'InvTrxDtlOperators'
CREATE TABLE [dbo].[InvTrxDtlOperators] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(80)  NOT NULL,
    [Operator] nvarchar(5)  NOT NULL
);
GO

-- Creating table 'InvStoreUsers'
CREATE TABLE [dbo].[InvStoreUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreUserId] nvarchar(40)  NOT NULL,
    [InvStoreId] int  NOT NULL
);
GO

-- Creating table 'InvUomConversions'
CREATE TABLE [dbo].[InvUomConversions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvUomId_base] int  NOT NULL,
    [InvUomId_into] int  NOT NULL,
    [Factor] decimal(4,2)  NOT NULL,
    [Description] nvarchar(80)  NOT NULL
);
GO

-- Creating table 'InvUomConvItems'
CREATE TABLE [dbo].[InvUomConvItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvUomConversionId] int  NOT NULL,
    [InvClassificationId] int  NULL,
    [InvItemId] int  NULL
);
GO

-- Creating table 'InvWarningLevels'
CREATE TABLE [dbo].[InvWarningLevels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [Level1] decimal(5,2)  NOT NULL,
    [Level2] decimal(5,2)  NOT NULL,
    [InvWarningTypeId] int  NOT NULL,
    [InvUomId] int  NOT NULL
);
GO

-- Creating table 'InvWarningTypes'
CREATE TABLE [dbo].[InvWarningTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Desc] nvarchar(30)  NOT NULL
);
GO

-- Creating table 'InvCategories'
CREATE TABLE [dbo].[InvCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(20)  NOT NULL,
    [Description] nvarchar(50)  NOT NULL,
    [Remarks] nvarchar(50)  NULL
);
GO

-- Creating table 'InvItemSpec_Steel'
CREATE TABLE [dbo].[InvItemSpec_Steel] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(180)  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [InvItemId] int  NOT NULL,
    [SteelMainCatId] int  NOT NULL,
    [SteelSubCatId] int  NOT NULL,
    [SteelBrandId] int  NOT NULL,
    [SteelMaterialId] int  NOT NULL,
    [SteelOriginId] int  NOT NULL,
    [SteelMaterialGradeId] int  NOT NULL,
    [WtKgm] decimal(18,0)  NOT NULL,
    [WtKgpc] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'InvItemSysDefinedSpecs'
CREATE TABLE [dbo].[InvItemSysDefinedSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SpecName] nvarchar(30)  NULL,
    [SpecCode] nvarchar(10)  NOT NULL,
    [SpecGroup] nvarchar(10)  NULL
);
GO

-- Creating table 'InvCategorySpecDefs'
CREATE TABLE [dbo].[InvCategorySpecDefs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvCategoryId] int  NOT NULL,
    [InvItemSysDefinedSpecsId] int  NOT NULL
);
GO

-- Creating table 'InvCustomSpecs'
CREATE TABLE [dbo].[InvCustomSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SpecName] nvarchar(50)  NOT NULL,
    [InvCustomSpecTypeId] int  NOT NULL,
    [Order] int  NOT NULL,
    [Measurement] nvarchar(20)  NULL,
    [Remarks] nvarchar(50)  NULL
);
GO

-- Creating table 'InvItemCustomSpecs'
CREATE TABLE [dbo].[InvItemCustomSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvCustomSpecId] int  NOT NULL,
    [SpecValue] nvarchar(30)  NULL,
    [Remarks] nvarchar(50)  NULL,
    [Order] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvCatCustomSpecs'
CREATE TABLE [dbo].[InvCatCustomSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvCategoryId] int  NOT NULL,
    [Order] int  NOT NULL,
    [Remarks] nvarchar(50)  NULL,
    [InvCustomSpecId] int  NOT NULL
);
GO

-- Creating table 'InvCustomSpecTypes'
CREATE TABLE [dbo].[InvCustomSpecTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

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
    [AspNetUserId] nvarchar(255)  NOT NULL,
    [RptAccessTypeId] int  NOT NULL,
    [ReportId] int  NOT NULL
);
GO

-- Creating table 'RptReportRoles1'
CREATE TABLE [dbo].[RptReportRoles1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AspNetRoleId] int  NOT NULL,
    [RptAccessTypeId] int  NOT NULL,
    [ReportId] int  NOT NULL
);
GO

-- Creating table 'RptAccessTypes'
CREATE TABLE [dbo].[RptAccessTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(250)  NULL,
    [DisplayName] varchar(250)  NULL,
    [LayoutData] varbinary(max)  NULL
);
GO

-- Creating table 'SteelMainCats'
CREATE TABLE [dbo].[SteelMainCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelSubCats'
CREATE TABLE [dbo].[SteelSubCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelBrands'
CREATE TABLE [dbo].[SteelBrands] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelMaterials'
CREATE TABLE [dbo].[SteelMaterials] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelOrigins'
CREATE TABLE [dbo].[SteelOrigins] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelMaterialGrades'
CREATE TABLE [dbo].[SteelMaterialGrades] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(60)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SysLabels'
CREATE TABLE [dbo].[SysLabels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [DisplayText] nvarchar(50)  NOT NULL,
    [DisplayLang] nvarchar(2)  NOT NULL
);
GO

-- Creating table 'SysSettings'
--CREATE TABLE [dbo].[SysSettings] (
--    [Id] int IDENTITY(1,1) NOT NULL,
--    [Code] nvarchar(10)  NOT NULL,
--    [SysValue] nvarchar(255)  NOT NULL
--);
--GO

-- Creating table 'InvTrxApprovals'
CREATE TABLE [dbo].[InvTrxApprovals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApprovedBy] nvarchar(40)  NULL,
    [ApprovedDate] datetime  NULL,
    [VerifiedBy] nvarchar(40)  NULL,
    [VerifiedDate] datetime  NULL,
    [EncodedBy] nvarchar(40)  NOT NULL,
    [EncodedDate] datetime  NOT NULL,
    [InvTrxHdrId] int  NOT NULL
);
GO

-- Creating table 'InvPoApprovals'
CREATE TABLE [dbo].[InvPoApprovals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApprovedBy] nvarchar(40)  NULL,
    [ApprovedDate] datetime  NULL,
    [VerifiedBy] nvarchar(40)  NULL,
    [VerifiedDate] datetime  NULL,
    [EncodedBy] nvarchar(40)  NOT NULL,
    [EncodedDate] datetime  NOT NULL,
    [InvPoHdrId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'InvItems'
--ALTER TABLE [dbo].[InvItems]
--ADD CONSTRAINT [PK_InvItems]
--    PRIMARY KEY CLUSTERED ([Id] ASC);
--GO

-- Creating primary key on [Id] in table 'InvItemClasses'
ALTER TABLE [dbo].[InvItemClasses]
ADD CONSTRAINT [PK_InvItemClasses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvClassifications'
ALTER TABLE [dbo].[InvClassifications]
ADD CONSTRAINT [PK_InvClassifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvSuppliers'
ALTER TABLE [dbo].[InvSuppliers]
ADD CONSTRAINT [PK_InvSuppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvSupplierItems'
ALTER TABLE [dbo].[InvSupplierItems]
ADD CONSTRAINT [PK_InvSupplierItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvStores'
ALTER TABLE [dbo].[InvStores]
ADD CONSTRAINT [PK_InvStores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvPoHdrs'
ALTER TABLE [dbo].[InvPoHdrs]
ADD CONSTRAINT [PK_InvPoHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [PK_InvPoItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRecHdrs'
ALTER TABLE [dbo].[InvRecHdrs]
ADD CONSTRAINT [PK_InvRecHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [PK_InvRecItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRequestHdrs'
ALTER TABLE [dbo].[InvRequestHdrs]
ADD CONSTRAINT [PK_InvRequestHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [PK_InvRequestItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvAdjHdrs'
ALTER TABLE [dbo].[InvAdjHdrs]
ADD CONSTRAINT [PK_InvAdjHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [PK_InvAdjItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvUoms'
ALTER TABLE [dbo].[InvUoms]
ADD CONSTRAINT [PK_InvUoms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvTrxHdrs'
ALTER TABLE [dbo].[InvTrxHdrs]
ADD CONSTRAINT [PK_InvTrxHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvTrxDtls'
ALTER TABLE [dbo].[InvTrxDtls]
ADD CONSTRAINT [PK_InvTrxDtls]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvTrxTypes'
ALTER TABLE [dbo].[InvTrxTypes]
ADD CONSTRAINT [PK_InvTrxTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvPoHdrStatus'
ALTER TABLE [dbo].[InvPoHdrStatus]
ADD CONSTRAINT [PK_InvPoHdrStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvTrxHdrStatus'
ALTER TABLE [dbo].[InvTrxHdrStatus]
ADD CONSTRAINT [PK_InvTrxHdrStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvTrxDtlOperators'
ALTER TABLE [dbo].[InvTrxDtlOperators]
ADD CONSTRAINT [PK_InvTrxDtlOperators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvStoreUsers'
ALTER TABLE [dbo].[InvStoreUsers]
ADD CONSTRAINT [PK_InvStoreUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvUomConversions'
ALTER TABLE [dbo].[InvUomConversions]
ADD CONSTRAINT [PK_InvUomConversions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvUomConvItems'
ALTER TABLE [dbo].[InvUomConvItems]
ADD CONSTRAINT [PK_InvUomConvItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvWarningLevels'
ALTER TABLE [dbo].[InvWarningLevels]
ADD CONSTRAINT [PK_InvWarningLevels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvWarningTypes'
ALTER TABLE [dbo].[InvWarningTypes]
ADD CONSTRAINT [PK_InvWarningTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCategories'
ALTER TABLE [dbo].[InvCategories]
ADD CONSTRAINT [PK_InvCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [PK_InvItemSpec_Steel]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemSysDefinedSpecs'
ALTER TABLE [dbo].[InvItemSysDefinedSpecs]
ADD CONSTRAINT [PK_InvItemSysDefinedSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCategorySpecDefs'
ALTER TABLE [dbo].[InvCategorySpecDefs]
ADD CONSTRAINT [PK_InvCategorySpecDefs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCustomSpecs'
ALTER TABLE [dbo].[InvCustomSpecs]
ADD CONSTRAINT [PK_InvCustomSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemCustomSpecs'
ALTER TABLE [dbo].[InvItemCustomSpecs]
ADD CONSTRAINT [PK_InvItemCustomSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCatCustomSpecs'
ALTER TABLE [dbo].[InvCatCustomSpecs]
ADD CONSTRAINT [PK_InvCatCustomSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvCustomSpecTypes'
ALTER TABLE [dbo].[InvCustomSpecTypes]
ADD CONSTRAINT [PK_InvCustomSpecTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating primary key on [Id] in table 'RptReportRoles1'
ALTER TABLE [dbo].[RptReportRoles1]
ADD CONSTRAINT [PK_RptReportRoles1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RptAccessTypes'
ALTER TABLE [dbo].[RptAccessTypes]
ADD CONSTRAINT [PK_RptAccessTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [PK_Reports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelMainCats'
ALTER TABLE [dbo].[SteelMainCats]
ADD CONSTRAINT [PK_SteelMainCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelSubCats'
ALTER TABLE [dbo].[SteelSubCats]
ADD CONSTRAINT [PK_SteelSubCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelBrands'
ALTER TABLE [dbo].[SteelBrands]
ADD CONSTRAINT [PK_SteelBrands]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelMaterials'
ALTER TABLE [dbo].[SteelMaterials]
ADD CONSTRAINT [PK_SteelMaterials]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelOrigins'
ALTER TABLE [dbo].[SteelOrigins]
ADD CONSTRAINT [PK_SteelOrigins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelMaterialGrades'
ALTER TABLE [dbo].[SteelMaterialGrades]
ADD CONSTRAINT [PK_SteelMaterialGrades]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysLabels'
ALTER TABLE [dbo].[SysLabels]
ADD CONSTRAINT [PK_SysLabels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SysSettings'
--ALTER TABLE [dbo].[SysSettings]
--ADD CONSTRAINT [PK_SysSettings]
--    PRIMARY KEY CLUSTERED ([Id] ASC);
--GO

-- Creating primary key on [Id] in table 'InvTrxApprovals'
ALTER TABLE [dbo].[InvTrxApprovals]
ADD CONSTRAINT [PK_InvTrxApprovals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvPoApprovals'
ALTER TABLE [dbo].[InvPoApprovals]
ADD CONSTRAINT [PK_InvPoApprovals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [InvItemId] in table 'InvItemClasses'
ALTER TABLE [dbo].[InvItemClasses]
ADD CONSTRAINT [FK_InvItemInvItemClass]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemClass'
CREATE INDEX [IX_FK_InvItemInvItemClass]
ON [dbo].[InvItemClasses]
    ([InvItemId]);
GO

-- Creating foreign key on [InvClassificationId] in table 'InvItemClasses'
ALTER TABLE [dbo].[InvItemClasses]
ADD CONSTRAINT [FK_InvClassificationInvItemClass]
    FOREIGN KEY ([InvClassificationId])
    REFERENCES [dbo].[InvClassifications]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvClassificationInvItemClass'
CREATE INDEX [IX_FK_InvClassificationInvItemClass]
ON [dbo].[InvItemClasses]
    ([InvClassificationId]);
GO

-- Creating foreign key on [InvSupplierId] in table 'InvSupplierItems'
ALTER TABLE [dbo].[InvSupplierItems]
ADD CONSTRAINT [FK_InvSupplierInvSupplierItem]
    FOREIGN KEY ([InvSupplierId])
    REFERENCES [dbo].[InvSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvSupplierInvSupplierItem'
CREATE INDEX [IX_FK_InvSupplierInvSupplierItem]
ON [dbo].[InvSupplierItems]
    ([InvSupplierId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvSupplierItems'
ALTER TABLE [dbo].[InvSupplierItems]
ADD CONSTRAINT [FK_InvItemInvSupplierItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvSupplierItem'
CREATE INDEX [IX_FK_InvItemInvSupplierItem]
ON [dbo].[InvSupplierItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvSupplierId] in table 'InvPoHdrs'
ALTER TABLE [dbo].[InvPoHdrs]
ADD CONSTRAINT [FK_InvSupplierInvPoHdr]
    FOREIGN KEY ([InvSupplierId])
    REFERENCES [dbo].[InvSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvSupplierInvPoHdr'
CREATE INDEX [IX_FK_InvSupplierInvPoHdr]
ON [dbo].[InvPoHdrs]
    ([InvSupplierId]);
GO

-- Creating foreign key on [InvPoHdrId] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [FK_InvPoHdrInvPoItem]
    FOREIGN KEY ([InvPoHdrId])
    REFERENCES [dbo].[InvPoHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvPoHdrInvPoItem'
CREATE INDEX [IX_FK_InvPoHdrInvPoItem]
ON [dbo].[InvPoItems]
    ([InvPoHdrId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [FK_InvItemInvPoItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvPoItem'
CREATE INDEX [IX_FK_InvItemInvPoItem]
ON [dbo].[InvPoItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvRecHdrs'
ALTER TABLE [dbo].[InvRecHdrs]
ADD CONSTRAINT [FK_InvStoreInvRecHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvRecHdr'
CREATE INDEX [IX_FK_InvStoreInvRecHdr]
ON [dbo].[InvRecHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [FK_InvItemInvRecItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvRecItem'
CREATE INDEX [IX_FK_InvItemInvRecItem]
ON [dbo].[InvRecItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvRecHdrId] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [FK_InvRecHdrInvRecItem]
    FOREIGN KEY ([InvRecHdrId])
    REFERENCES [dbo].[InvRecHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvRecHdrInvRecItem'
CREATE INDEX [IX_FK_InvRecHdrInvRecItem]
ON [dbo].[InvRecItems]
    ([InvRecHdrId]);
GO

-- Creating foreign key on [InvSupplierId] in table 'InvRecHdrs'
ALTER TABLE [dbo].[InvRecHdrs]
ADD CONSTRAINT [FK_InvSupplierInvRecHdr]
    FOREIGN KEY ([InvSupplierId])
    REFERENCES [dbo].[InvSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvSupplierInvRecHdr'
CREATE INDEX [IX_FK_InvSupplierInvRecHdr]
ON [dbo].[InvRecHdrs]
    ([InvSupplierId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvRequestHdrs'
ALTER TABLE [dbo].[InvRequestHdrs]
ADD CONSTRAINT [FK_InvStoreInvRequestHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvRequestHdr'
CREATE INDEX [IX_FK_InvStoreInvRequestHdr]
ON [dbo].[InvRequestHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvItemInvRequestItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvRequestItem'
CREATE INDEX [IX_FK_InvItemInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvRequestHdrId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvRequestHdrInvRequestItem]
    FOREIGN KEY ([InvRequestHdrId])
    REFERENCES [dbo].[InvRequestHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvRequestHdrInvRequestItem'
CREATE INDEX [IX_FK_InvRequestHdrInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvRequestHdrId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvPoHdrs'
ALTER TABLE [dbo].[InvPoHdrs]
ADD CONSTRAINT [FK_InvStoreInvPoHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvPoHdr'
CREATE INDEX [IX_FK_InvStoreInvPoHdr]
ON [dbo].[InvPoHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvAdjHdrs'
ALTER TABLE [dbo].[InvAdjHdrs]
ADD CONSTRAINT [FK_InvStoreInvAdjHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvAdjHdr'
CREATE INDEX [IX_FK_InvStoreInvAdjHdr]
ON [dbo].[InvAdjHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvAdjHdrId] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [FK_InvAdjHdrInvAdjItem]
    FOREIGN KEY ([InvAdjHdrId])
    REFERENCES [dbo].[InvAdjHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvAdjHdrInvAdjItem'
CREATE INDEX [IX_FK_InvAdjHdrInvAdjItem]
ON [dbo].[InvAdjItems]
    ([InvAdjHdrId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [FK_InvItemInvAdjItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvAdjItem'
CREATE INDEX [IX_FK_InvItemInvAdjItem]
ON [dbo].[InvAdjItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvItems'
ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [FK_InvUomInvItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvItem'
CREATE INDEX [IX_FK_InvUomInvItem]
ON [dbo].[InvItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [FK_InvUomInvPoItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvPoItem'
CREATE INDEX [IX_FK_InvUomInvPoItem]
ON [dbo].[InvPoItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [FK_InvUomInvRecItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvRecItem'
CREATE INDEX [IX_FK_InvUomInvRecItem]
ON [dbo].[InvRecItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvUomInvRequestItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvRequestItem'
CREATE INDEX [IX_FK_InvUomInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [FK_InvUomInvAdjItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvAdjItem'
CREATE INDEX [IX_FK_InvUomInvAdjItem]
ON [dbo].[InvAdjItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvRecItemId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvRecItemInvRequestItem]
    FOREIGN KEY ([InvRecItemId])
    REFERENCES [dbo].[InvRecItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvRecItemInvRequestItem'
CREATE INDEX [IX_FK_InvRecItemInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvRecItemId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvTrxHdrs'
ALTER TABLE [dbo].[InvTrxHdrs]
ADD CONSTRAINT [FK_InvStoreInvTrxHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvTrxHdr'
CREATE INDEX [IX_FK_InvStoreInvTrxHdr]
ON [dbo].[InvTrxHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvTrxHdrId] in table 'InvTrxDtls'
ALTER TABLE [dbo].[InvTrxDtls]
ADD CONSTRAINT [FK_InvTrxHdrInvTrxDtl]
    FOREIGN KEY ([InvTrxHdrId])
    REFERENCES [dbo].[InvTrxHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvTrxHdrInvTrxDtl'
CREATE INDEX [IX_FK_InvTrxHdrInvTrxDtl]
ON [dbo].[InvTrxDtls]
    ([InvTrxHdrId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvTrxDtls'
ALTER TABLE [dbo].[InvTrxDtls]
ADD CONSTRAINT [FK_InvUomInvTrxDtl]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvTrxDtl'
CREATE INDEX [IX_FK_InvUomInvTrxDtl]
ON [dbo].[InvTrxDtls]
    ([InvUomId]);
GO

-- Creating foreign key on [InvTrxTypeId] in table 'InvTrxHdrs'
ALTER TABLE [dbo].[InvTrxHdrs]
ADD CONSTRAINT [FK_InvTrxTypeInvTrxHdr]
    FOREIGN KEY ([InvTrxTypeId])
    REFERENCES [dbo].[InvTrxTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvTrxTypeInvTrxHdr'
CREATE INDEX [IX_FK_InvTrxTypeInvTrxHdr]
ON [dbo].[InvTrxHdrs]
    ([InvTrxTypeId]);
GO

-- Creating foreign key on [InvPoHdrStatusId] in table 'InvPoHdrs'
ALTER TABLE [dbo].[InvPoHdrs]
ADD CONSTRAINT [FK_InvPoHdrStatusInvPoHdr]
    FOREIGN KEY ([InvPoHdrStatusId])
    REFERENCES [dbo].[InvPoHdrStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvPoHdrStatusInvPoHdr'
CREATE INDEX [IX_FK_InvPoHdrStatusInvPoHdr]
ON [dbo].[InvPoHdrs]
    ([InvPoHdrStatusId]);
GO

-- Creating foreign key on [InvTrxHdrStatusId] in table 'InvTrxHdrs'
ALTER TABLE [dbo].[InvTrxHdrs]
ADD CONSTRAINT [FK_InvTrxHdrStatusInvTrxHdr]
    FOREIGN KEY ([InvTrxHdrStatusId])
    REFERENCES [dbo].[InvTrxHdrStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvTrxHdrStatusInvTrxHdr'
CREATE INDEX [IX_FK_InvTrxHdrStatusInvTrxHdr]
ON [dbo].[InvTrxHdrs]
    ([InvTrxHdrStatusId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvTrxDtls'
ALTER TABLE [dbo].[InvTrxDtls]
ADD CONSTRAINT [FK_InvItemInvTrxDtl]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvTrxDtl'
CREATE INDEX [IX_FK_InvItemInvTrxDtl]
ON [dbo].[InvTrxDtls]
    ([InvItemId]);
GO

-- Creating foreign key on [InvTrxDtlOperatorId] in table 'InvTrxDtls'
ALTER TABLE [dbo].[InvTrxDtls]
ADD CONSTRAINT [FK_InvTrxDtlOperatorInvTrxDtl]
    FOREIGN KEY ([InvTrxDtlOperatorId])
    REFERENCES [dbo].[InvTrxDtlOperators]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvTrxDtlOperatorInvTrxDtl'
CREATE INDEX [IX_FK_InvTrxDtlOperatorInvTrxDtl]
ON [dbo].[InvTrxDtls]
    ([InvTrxDtlOperatorId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvStoreUsers'
ALTER TABLE [dbo].[InvStoreUsers]
ADD CONSTRAINT [FK_InvStoreInvStoreUser]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvStoreUser'
CREATE INDEX [IX_FK_InvStoreInvStoreUser]
ON [dbo].[InvStoreUsers]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvUomConversionId] in table 'InvUomConvItems'
ALTER TABLE [dbo].[InvUomConvItems]
ADD CONSTRAINT [FK_InvUomConversionInvUomConvItem]
    FOREIGN KEY ([InvUomConversionId])
    REFERENCES [dbo].[InvUomConversions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomConversionInvUomConvItem'
CREATE INDEX [IX_FK_InvUomConversionInvUomConvItem]
ON [dbo].[InvUomConvItems]
    ([InvUomConversionId]);
GO

-- Creating foreign key on [InvClassificationId] in table 'InvUomConvItems'
ALTER TABLE [dbo].[InvUomConvItems]
ADD CONSTRAINT [FK_InvClassificationInvUomConvItem]
    FOREIGN KEY ([InvClassificationId])
    REFERENCES [dbo].[InvClassifications]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvClassificationInvUomConvItem'
CREATE INDEX [IX_FK_InvClassificationInvUomConvItem]
ON [dbo].[InvUomConvItems]
    ([InvClassificationId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvUomConvItems'
ALTER TABLE [dbo].[InvUomConvItems]
ADD CONSTRAINT [FK_InvItemInvUomConvItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvUomConvItem'
CREATE INDEX [IX_FK_InvItemInvUomConvItem]
ON [dbo].[InvUomConvItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvWarningLevels'
ALTER TABLE [dbo].[InvWarningLevels]
ADD CONSTRAINT [FK_InvItemInvWarningLevel]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvWarningLevel'
CREATE INDEX [IX_FK_InvItemInvWarningLevel]
ON [dbo].[InvWarningLevels]
    ([InvItemId]);
GO

-- Creating foreign key on [InvWarningTypeId] in table 'InvWarningLevels'
ALTER TABLE [dbo].[InvWarningLevels]
ADD CONSTRAINT [FK_InvWarningTypeInvWarningLevel]
    FOREIGN KEY ([InvWarningTypeId])
    REFERENCES [dbo].[InvWarningTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvWarningTypeInvWarningLevel'
CREATE INDEX [IX_FK_InvWarningTypeInvWarningLevel]
ON [dbo].[InvWarningLevels]
    ([InvWarningTypeId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvWarningLevels'
ALTER TABLE [dbo].[InvWarningLevels]
ADD CONSTRAINT [FK_InvUomInvWarningLevel]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvWarningLevel'
CREATE INDEX [IX_FK_InvUomInvWarningLevel]
ON [dbo].[InvWarningLevels]
    ([InvUomId]);
GO

-- Creating foreign key on [InvCategoryId] in table 'InvItems'
ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [FK_InvCategoryInvItem]
    FOREIGN KEY ([InvCategoryId])
    REFERENCES [dbo].[InvCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCategoryInvItem'
CREATE INDEX [IX_FK_InvCategoryInvItem]
ON [dbo].[InvItems]
    ([InvCategoryId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_InvItemInvItemSpec_Steel]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemSpec_Steel'
CREATE INDEX [IX_FK_InvItemInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([InvItemId]);
GO

-- Creating foreign key on [InvCategoryId] in table 'InvCategorySpecDefs'
ALTER TABLE [dbo].[InvCategorySpecDefs]
ADD CONSTRAINT [FK_InvCategoryInvCategorySpecDef]
    FOREIGN KEY ([InvCategoryId])
    REFERENCES [dbo].[InvCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCategoryInvCategorySpecDef'
CREATE INDEX [IX_FK_InvCategoryInvCategorySpecDef]
ON [dbo].[InvCategorySpecDefs]
    ([InvCategoryId]);
GO

-- Creating foreign key on [InvItemSysDefinedSpecsId] in table 'InvCategorySpecDefs'
ALTER TABLE [dbo].[InvCategorySpecDefs]
ADD CONSTRAINT [FK_InvItemSysDefinedSpecsInvCategorySpecDef]
    FOREIGN KEY ([InvItemSysDefinedSpecsId])
    REFERENCES [dbo].[InvItemSysDefinedSpecs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemSysDefinedSpecsInvCategorySpecDef'
CREATE INDEX [IX_FK_InvItemSysDefinedSpecsInvCategorySpecDef]
ON [dbo].[InvCategorySpecDefs]
    ([InvItemSysDefinedSpecsId]);
GO

-- Creating foreign key on [InvCategoryId] in table 'InvCatCustomSpecs'
ALTER TABLE [dbo].[InvCatCustomSpecs]
ADD CONSTRAINT [FK_InvCategoryInvCatCustomSpec]
    FOREIGN KEY ([InvCategoryId])
    REFERENCES [dbo].[InvCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCategoryInvCatCustomSpec'
CREATE INDEX [IX_FK_InvCategoryInvCatCustomSpec]
ON [dbo].[InvCatCustomSpecs]
    ([InvCategoryId]);
GO

-- Creating foreign key on [InvCustomSpecTypeId] in table 'InvCustomSpecs'
ALTER TABLE [dbo].[InvCustomSpecs]
ADD CONSTRAINT [FK_InvCustomSpecTypeInvCustomSpec]
    FOREIGN KEY ([InvCustomSpecTypeId])
    REFERENCES [dbo].[InvCustomSpecTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCustomSpecTypeInvCustomSpec'
CREATE INDEX [IX_FK_InvCustomSpecTypeInvCustomSpec]
ON [dbo].[InvCustomSpecs]
    ([InvCustomSpecTypeId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvItemCustomSpecs'
ALTER TABLE [dbo].[InvItemCustomSpecs]
ADD CONSTRAINT [FK_InvItemInvItemCustomSpec]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemCustomSpec'
CREATE INDEX [IX_FK_InvItemInvItemCustomSpec]
ON [dbo].[InvItemCustomSpecs]
    ([InvItemId]);
GO

-- Creating foreign key on [InvCustomSpecId] in table 'InvItemCustomSpecs'
ALTER TABLE [dbo].[InvItemCustomSpecs]
ADD CONSTRAINT [FK_InvCustomSpecInvItemCustomSpec]
    FOREIGN KEY ([InvCustomSpecId])
    REFERENCES [dbo].[InvCustomSpecs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCustomSpecInvItemCustomSpec'
CREATE INDEX [IX_FK_InvCustomSpecInvItemCustomSpec]
ON [dbo].[InvItemCustomSpecs]
    ([InvCustomSpecId]);
GO

-- Creating foreign key on [InvCustomSpecId] in table 'InvCatCustomSpecs'
ALTER TABLE [dbo].[InvCatCustomSpecs]
ADD CONSTRAINT [FK_InvCustomSpecInvCatCustomSpec]
    FOREIGN KEY ([InvCustomSpecId])
    REFERENCES [dbo].[InvCustomSpecs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCustomSpecInvCatCustomSpec'
CREATE INDEX [IX_FK_InvCustomSpecInvCatCustomSpec]
ON [dbo].[InvCatCustomSpecs]
    ([InvCustomSpecId]);
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

-- Creating foreign key on [RptAccessTypeId] in table 'RptReportRoles1'
ALTER TABLE [dbo].[RptReportRoles1]
ADD CONSTRAINT [FK_RptAccessTypeRptReportRoles]
    FOREIGN KEY ([RptAccessTypeId])
    REFERENCES [dbo].[RptAccessTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RptAccessTypeRptReportRoles'
CREATE INDEX [IX_FK_RptAccessTypeRptReportRoles]
ON [dbo].[RptReportRoles1]
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

-- Creating foreign key on [ReportId] in table 'RptReportUsers'
ALTER TABLE [dbo].[RptReportUsers]
ADD CONSTRAINT [FK_ReportRptReportUser]
    FOREIGN KEY ([ReportId])
    REFERENCES [dbo].[Reports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportRptReportUser'
CREATE INDEX [IX_FK_ReportRptReportUser]
ON [dbo].[RptReportUsers]
    ([ReportId]);
GO

-- Creating foreign key on [ReportId] in table 'RptReportRoles1'
ALTER TABLE [dbo].[RptReportRoles1]
ADD CONSTRAINT [FK_ReportRptReportRoles]
    FOREIGN KEY ([ReportId])
    REFERENCES [dbo].[Reports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportRptReportRoles'
CREATE INDEX [IX_FK_ReportRptReportRoles]
ON [dbo].[RptReportRoles1]
    ([ReportId]);
GO

-- Creating foreign key on [ReportId] in table 'RptReportCats'
ALTER TABLE [dbo].[RptReportCats]
ADD CONSTRAINT [FK_ReportRptReportCat]
    FOREIGN KEY ([ReportId])
    REFERENCES [dbo].[Reports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportRptReportCat'
CREATE INDEX [IX_FK_ReportRptReportCat]
ON [dbo].[RptReportCats]
    ([ReportId]);
GO

-- Creating foreign key on [SteelMainCatId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelMainCatInvItemSpec_Steel]
    FOREIGN KEY ([SteelMainCatId])
    REFERENCES [dbo].[SteelMainCats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelMainCatInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelMainCatInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelMainCatId]);
GO

-- Creating foreign key on [SteelSubCatId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelSubCatInvItemSpec_Steel]
    FOREIGN KEY ([SteelSubCatId])
    REFERENCES [dbo].[SteelSubCats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelSubCatInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelSubCatInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelSubCatId]);
GO

-- Creating foreign key on [SteelBrandId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelBrandInvItemSpec_Steel]
    FOREIGN KEY ([SteelBrandId])
    REFERENCES [dbo].[SteelBrands]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelBrandInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelBrandInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelBrandId]);
GO

-- Creating foreign key on [SteelMaterialId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelMaterialInvItemSpec_Steel]
    FOREIGN KEY ([SteelMaterialId])
    REFERENCES [dbo].[SteelMaterials]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelMaterialInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelMaterialInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelMaterialId]);
GO

-- Creating foreign key on [SteelOriginId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelOriginInvItemSpec_Steel]
    FOREIGN KEY ([SteelOriginId])
    REFERENCES [dbo].[SteelOrigins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelOriginInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelOriginInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelOriginId]);
GO

-- Creating foreign key on [SteelMaterialGradeId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelMaterialGradeInvItemSpec_Steel]
    FOREIGN KEY ([SteelMaterialGradeId])
    REFERENCES [dbo].[SteelMaterialGrades]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelMaterialGradeInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelMaterialGradeInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelMaterialGradeId]);
GO

-- Creating foreign key on [InvTrxHdrId] in table 'InvTrxApprovals'
ALTER TABLE [dbo].[InvTrxApprovals]
ADD CONSTRAINT [FK_InvTrxHdrInvTrxApproval]
    FOREIGN KEY ([InvTrxHdrId])
    REFERENCES [dbo].[InvTrxHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvTrxHdrInvTrxApproval'
CREATE INDEX [IX_FK_InvTrxHdrInvTrxApproval]
ON [dbo].[InvTrxApprovals]
    ([InvTrxHdrId]);
GO

-- Creating foreign key on [InvPoHdrId] in table 'InvPoApprovals'
ALTER TABLE [dbo].[InvPoApprovals]
ADD CONSTRAINT [FK_InvPoHdrInvPoApproval]
    FOREIGN KEY ([InvPoHdrId])
    REFERENCES [dbo].[InvPoHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvPoHdrInvPoApproval'
CREATE INDEX [IX_FK_InvPoHdrInvPoApproval]
ON [dbo].[InvPoApprovals]
    ([InvPoHdrId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
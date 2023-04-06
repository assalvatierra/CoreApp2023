
----- Initialize RptCategories ----
if not exists (select 1 from RptCategories where Code='ACTIVE')
	insert into RptCategories values ('ACTIVE','Active Reports');
if not exists (select 1 from RptCategories where Code='SUBRPT')
	insert into RptCategories values ('SUBRPT','Sub Reports');
if not exists (select 1 from RptCategories where Code='RECORD')
	insert into RptCategories values ('RECORD','Record Reports');
if not exists (select 1 from RptCategories where Code='DRAFT')
	insert into RptCategories values ('DRAFT','Draft Reports');
if not exists (select 1 from RptCategories where Code='INACTIVE')
	insert into RptCategories values ('INACTIVE','Inactive Reports');
--select * from RptCategories;

insert into dbo.RptAccessTypes values('NA');
--select * from dbo.RptAccessTypes;


----- Initialize Sample Data -------------------------------
delete from dbo.RptReportCats;
insert into RptReportCats values 
	('1','1'),('4','2'),('5','3') ;
--select * from dbo.RptReportCats;

insert into dbo.RptReportUsers([ReportId],[AspNetUserId],[RptAccessTypeId])
values(1,'admin@gmail.com',1);



----- Check Data --------------------------------
select * from dbo.Reports report
left join RptReportCats rptXcats on rptXcats.ReportId=report.Id
left join RptCategories cats on cats.Id = rptXcats.RptCategoryId
;



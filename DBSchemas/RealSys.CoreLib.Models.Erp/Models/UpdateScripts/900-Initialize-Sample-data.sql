
---------- ADD INITIAL VALUES ---------------
INSERT INTO dbo.InvStores (StoreName) VALUES
	 (N'Main');

INSERT INTO [InvTypes]([Desc],[Remarks],[SysCode])
VALUES ('Merchandise','Merchandise Items','MERC'),
       ('Services','Services','SERV');

INSERT INTO [InvCategories]([Code],[Description],[Remarks])
VALUES  ('STL', 'Steel Plate',''),
        ('BM', 'Beams',''),
        ('PC', 'Plate Carbon',''),
        ('PSTL', 'Plate Stainless',''),
        ('PPC', 'Seamless Pipe Carbon',''),
        ('AB', 'Angle Bar','');

INSERT INTO [InvUoms]([uom])
VALUES ('PC'),('Meters'),('Feet'),('Box');
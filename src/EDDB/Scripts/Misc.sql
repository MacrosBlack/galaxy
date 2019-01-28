select count(*) from tblEDSystemsWithCoordinates

SELECT TOP 10 [Date], DATENAME(dw, [Date]), COUNT(Date) FROM tblEDSystemsWithCoordinates GROUP BY [Date] ORDER BY COUNT([Date]) DESC

select top 1 max([Date]) from tblEDSystemsWithCoordinates

-- truncate table [dbo].[tblEDSystemsWithCoordinates]
-- truncate table [dbo].[tblSystemsProgress]

select count(*) from tblEDStations

select * from tblEDStations where Name = 'Levchenko Enterprise'

select * from tblEDStations where Name = 'Barnard Hub'

select * from tblEDStations where Id = 49242

-- truncate table [dbo].[tblSystemsProgress]
-- truncate table [dbo].[tblEDSystemsWithCoordinates]

ALTER DATABASE EDSystems
SET RECOVERY SIMPLE
GO
DBCC SHRINKFILE (EDSystems, 1)
GO

CREATE INDEX [IX_tblEDSystemsWithCoordinates_Id] ON [dbo].[tblEDSystemsWithCoordinates] ([Id])
GO

CREATE INDEX [IX_tblEDSystemsWithCoordinates_Name] ON [dbo].[tblEDSystemsWithCoordinates] ([Name])
GO
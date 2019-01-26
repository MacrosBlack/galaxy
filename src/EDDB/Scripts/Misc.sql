select count(*) from tblEDSystemsWithCoordinates

SELECT TOP 10 [Date], DATENAME(dw, [Date]), COUNT(Date) FROM tblEDSystemsWithCoordinates GROUP BY [Date] ORDER BY COUNT([Date]) DESC

select top 1 max([Date]) from tblEDSystemsWithCoordinates

-- truncate table [dbo].[tblEDSystemsWithCoordinates]

-- truncate table [dbo].[tblSystemsProgress]
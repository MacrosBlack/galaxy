CREATE VIEW [dbo].[vwStationSystem]
SELECT        dbo.tblEDStations.*, dbo.tblEDSystemsWithCoordinates.X, dbo.tblEDSystemsWithCoordinates.Y, dbo.tblEDSystemsWithCoordinates.Z
FROM            dbo.tblEDStations INNER JOIN
                         dbo.tblEDSystemsWithCoordinates ON dbo.tblEDStations.Id = dbo.tblEDSystemsWithCoordinates.Id
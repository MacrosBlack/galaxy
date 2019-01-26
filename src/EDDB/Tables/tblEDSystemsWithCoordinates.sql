CREATE TABLE [dbo].[tblEDSystemsWithCoordinates]
(
    [Id] INT NOT NULL, 
    [Id64] BIGINT NULL, 
    [Name] VARCHAR(80) NOT NULL, 
    [X] DECIMAL(18, 5) NULL, 
    [Y] DECIMAL(18, 5) NULL, 
    [Z] DECIMAL(18, 5) NULL, 
    [Date] DATE NOT NULL
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_EDSystemsWithCoordinates_Name] ON [dbo].[tblEDSystemsWithCoordinates] ([Name])

GO
CREATE TABLE [dbo].[tblEDBody]
(
	[Id] INT NOT NULL, 
    [Id64] BIGINT NULL, 
    [BodyId] INT NULL, 
    [Name] VARCHAR(60) NULL, 
    [Type] VARCHAR(60) NULL, 
    [SubType] VARCHAR(60) NULL, 
    [SystemId] INT NOT NULL, 
    [SystemId64] BIGINT NULL, 
    [SystemName] VARCHAR(60) NOT NULL 
)

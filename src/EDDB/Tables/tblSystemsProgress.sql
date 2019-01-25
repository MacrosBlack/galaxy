﻿CREATE TABLE [dbo].[tblSystemsProgress]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [FileName] VARCHAR(250) NOT NULL, 
    [RowsProcessed] BIGINT NOT NULL, 
    [Date] DATETIME NOT NULL DEFAULT GETDATE()
)

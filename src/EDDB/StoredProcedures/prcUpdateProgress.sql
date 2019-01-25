CREATE PROCEDURE [dbo].[prcUpdateProgress]
	@FileName varchar(250),
	@RowsProcessed bigint
AS
	INSERT INTO [dbo].[tblSystemsProgress] ([FileName], [RowsProcessed]) VALUES (@FileName, @RowsProcessed)
RETURN 
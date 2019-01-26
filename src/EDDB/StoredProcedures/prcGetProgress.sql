CREATE PROCEDURE [dbo].[prcGetProgress]
	@FileName varchar(250),
	@RowsProcessed int OUTPUT
AS
	SET @RowsProcessed = ISNULL((SELECT TOP 1 [RowsProcessed] FROM [dbo].[tblSystemsProgress] WHERE [FileName] = @FileName ORDER BY [Date] DESC), 0)
RETURN


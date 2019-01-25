CREATE PROCEDURE [dbo].[prcAddSystemWithCoordinates]
	@Id int = 0, @Id64 bigint, @Name char(80), @X decimal(18), @Y decimal(18), @Z decimal(18), @Date DateTime
	AS
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id OR a.Name = @Name)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id, @Id64, @Name, @X, @Y, @Z, @Date)
	END
RETURN 0


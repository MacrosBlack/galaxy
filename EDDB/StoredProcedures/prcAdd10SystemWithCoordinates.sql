CREATE PROCEDURE [dbo].[Add10SystemWithCoordinates]
	@Id0 int = 0, @Id640 bigint, @Name0 char(80), @X0 int, @Y0 int, @Z0 int, @Date0 DateTime
	AS
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id0 OR a.Name = @Name0)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id0, @Id640, @Name0, @X0, @Y0, @Z0, @Date0)
	END
RETURN 0

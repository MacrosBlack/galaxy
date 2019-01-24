CREATE PROCEDURE [dbo].[AddSystemWithCoordinates]
	@Id0 int = 0, @Id640 bigint, @Name0 char(80), @X0 int, @Y0 int, @Z0 int, @Date0 DateTime,
	@Id1 int = 0, @Id641 bigint, @Name1 char(80), @X1 int, @Y1 int, @Z1 int, @Date1 DateTime,
	@Id2 int = 0, @Id642 bigint, @Name2 char(80), @X2 int, @Y2 int, @Z2 int, @Date2 DateTime,
	@Id3 int = 0, @Id643 bigint, @Name3 char(80), @X3 int, @Y3 int, @Z3 int, @Date3 DateTime,
	@Id4 int = 0, @Id644 bigint, @Name4 char(80), @X4 int, @Y4 int, @Z4 int, @Date4 DateTime,
	@Id5 int = 0, @Id645 bigint, @Name5 char(80), @X5 int, @Y5 int, @Z5 int, @Date5 DateTime,
	@Id6 int = 0, @Id646 bigint, @Name6 char(80), @X6 int, @Y6 int, @Z6 int, @Date6 DateTime,
	@Id7 int = 0, @Id647 bigint, @Name7 char(80), @X7 int, @Y7 int, @Z7 int, @Date7 DateTime,
	@Id8 int = 0, @Id648 bigint, @Name8 char(80), @X8 int, @Y8 int, @Z8 int, @Date8 DateTime,
	@Id9 int = 0, @Id649 bigint, @Name9 char(80), @X9 int, @Y9 int, @Z9 int, @Date9 DateTime
	AS
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id0 OR a.Name = @Name0)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id0, @Id640, @Name0, @X0, @Y0, @Z0, @Date0)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id1 OR a.Name = @Name1)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id1, @Id641, @Name1, @X1, @Y1, @Z1, @Date1)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id2 OR a.Name = @Name2)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id2, @Id642, @Name2, @X2, @Y2, @Z2, @Date2)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id3 OR a.Name = @Name3)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id3, @Id643, @Name3, @X3, @Y3, @Z3, @Date3)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id4 OR a.Name = @Name4)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id4, @Id644, @Name4, @X4, @Y4, @Z4, @Date4)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id5 OR a.Name = @Name5)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id5, @Id645, @Name5, @X5, @Y5, @Z5, @Date5)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id6 OR a.Name = @Name6)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id6, @Id646, @Name6, @X6, @Y6, @Z6, @Date6)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id7 OR a.Name = @Name7)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id7, @Id647, @Name7, @X7, @Y7, @Z7, @Date7)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id8 OR a.Name = @Name8)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id8, @Id648, @Name8, @X8, @Y8, @Z8, @Date8)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[EDSystemsWithCoordinates] a WHERE a.Id = @Id9 OR a.Name = @Name9)
	BEGIN
		INSERT INTO [dbo].[EDSystemsWithCoordinates] VALUES(@Id0, @Id640, @Name0, @X9, @Y9, @Z9, @Date9)
	END
RETURN 0

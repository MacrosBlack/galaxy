CREATE PROCEDURE [dbo].[prcAdd10SystemsWithCoordinates]
	@Id0 int = 0, @Id640 bigint, @Name0 char(80), @X0 decimal(18), @Y0 decimal(18), @Z0 decimal(18), @Date0 DateTime,
	@Id1 int = 0, @Id641 bigint, @Name1 char(80), @X1 decimal(18), @Y1 decimal(18), @Z1 decimal(18), @Date1 DateTime,
	@Id2 int = 0, @Id642 bigint, @Name2 char(80), @X2 decimal(18), @Y2 decimal(18), @Z2 decimal(18), @Date2 DateTime,
	@Id3 int = 0, @Id643 bigint, @Name3 char(80), @X3 decimal(18), @Y3 decimal(18), @Z3 decimal(18), @Date3 DateTime,
	@Id4 int = 0, @Id644 bigint, @Name4 char(80), @X4 decimal(18), @Y4 decimal(18), @Z4 decimal(18), @Date4 DateTime,
	@Id5 int = 0, @Id645 bigint, @Name5 char(80), @X5 decimal(18), @Y5 decimal(18), @Z5 decimal(18), @Date5 DateTime,
	@Id6 int = 0, @Id646 bigint, @Name6 char(80), @X6 decimal(18), @Y6 decimal(18), @Z6 decimal(18), @Date6 DateTime,
	@Id7 int = 0, @Id647 bigint, @Name7 char(80), @X7 decimal(18), @Y7 decimal(18), @Z7 decimal(18), @Date7 DateTime,
	@Id8 int = 0, @Id648 bigint, @Name8 char(80), @X8 decimal(18), @Y8 decimal(18), @Z8 decimal(18), @Date8 DateTime,
	@Id9 int = 0, @Id649 bigint, @Name9 char(80), @X9 decimal(18), @Y9 decimal(18), @Z9 decimal(18), @Date9 DateTime
	AS
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id0 OR a.Name = @Name0)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id0, @Id640, @Name0, @X0, @Y0, @Z0, @Date0)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id1 OR a.Name = @Name1)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id1, @Id641, @Name1, @X1, @Y1, @Z1, @Date1)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id2 OR a.Name = @Name2)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id2, @Id642, @Name2, @X2, @Y2, @Z2, @Date2)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id3 OR a.Name = @Name3)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id3, @Id643, @Name3, @X3, @Y3, @Z3, @Date3)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id4 OR a.Name = @Name4)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id4, @Id644, @Name4, @X4, @Y4, @Z4, @Date4)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id5 OR a.Name = @Name5)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id5, @Id645, @Name5, @X5, @Y5, @Z5, @Date5)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id6 OR a.Name = @Name6)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id6, @Id646, @Name6, @X6, @Y6, @Z6, @Date6)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id7 OR a.Name = @Name7)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id7, @Id647, @Name7, @X7, @Y7, @Z7, @Date7)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id8 OR a.Name = @Name8)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id8, @Id648, @Name8, @X8, @Y8, @Z8, @Date8)
	END
	IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEDSystemsWithCoordinates] a WHERE a.Id = @Id9 OR a.Name = @Name9)
	BEGIN
		INSERT INTO [dbo].[tblEDSystemsWithCoordinates] VALUES(@Id9, @Id649, @Name9, @X9, @Y9, @Z9, @Date9)
	END
RETURN 0


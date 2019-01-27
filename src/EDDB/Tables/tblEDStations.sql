CREATE TABLE [dbo].[tblEDStations]
(
	[Id] INT NOT NULL, 
    [MarketId] BIGINT NULL, 
    [Type] VARCHAR(20) NOT NULL, 
    [Name] VARCHAR(40) NOT NULL, 
    [DistanceToArrival] DECIMAL(18, 4) NOT NULL, 
    [Allegiance] VARCHAR(20) NOT NULL, 
    [Goverment] VARCHAR(20) NULL, 
    [Economy] VARCHAR(20) NULL, 
    [SecondEconomy] VARCHAR(20) NULL, 
    [HaveMarket] BIT NOT NULL, 
    [HaveShipyard] BIT NOT NULL, 
    [HaveOutfitting] BIT NOT NULL, 
    [OtherServices] VARCHAR(50) NOT NULL, 
    [SystemId] INT NOT NULL, 
    [SystemId64] BIGINT NOT NULL, 
    [SystemName] VARCHAR(50) NOT NULL 
)

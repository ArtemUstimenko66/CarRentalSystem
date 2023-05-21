CREATE TABLE [dbo].[CarData] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Color]         NVARCHAR (50) DEFAULT ('') NOT NULL,
    [Brand]         NVARCHAR (50) NOT NULL,
    [Model]         NVARCHAR (50) NOT NULL,
    [Year]          INT           NOT NULL,
    [FuelType]      NVARCHAR (50) DEFAULT ('') NOT NULL,
    [LicensePlate]  NVARCHAR (50) DEFAULT ('') NOT NULL,
    [NumberOfSeats] INT           DEFAULT ('') NOT NULL,
    [Mileage]       FLOAT (53)    NOT NULL,
    [Availability]  NVARCHAR (50) NOT NULL,
	 [Photo]         VARBINARY (MAX) DEFAULT (0x00) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


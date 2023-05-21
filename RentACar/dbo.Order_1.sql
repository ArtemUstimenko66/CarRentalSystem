CREATE TABLE [dbo].[Order] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Users]       NVARCHAR (50) DEFAULT ('') NOT NULL,
    [RentalDate]  DATE          NOT NULL,
    [ReturnDate]  DATE          NOT NULL,
    [Days]        INT           DEFAULT ('') NOT NULL,
    [RatePerDay]  INT           DEFAULT ('') NOT NULL,
    [TotalAmount] INT           DEFAULT ('') NOT NULL,
    [Model]       NVARCHAR (50) DEFAULT ('') NOT NULL,
    [Brand]       NVARCHAR (50) DEFAULT ('') NOT NULL,
    [Selected] BIT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


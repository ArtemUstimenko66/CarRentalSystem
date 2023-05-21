CREATE TABLE [dbo].[Order] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Users]       NVARCHAR (50) NOT NULL,
    [RentalDate]  DATE          NOT NULL,
    [ReturnDate]  DATE          NOT NULL,
    [Days]        INT           NOT NULL DEFAULT '',
    [RatePerDay]  INT           NOT NULL DEFAULT '',
    [TotalAmount] INT           NOT NULL DEFAULT '',
    [Model]       NVARCHAR (50) NOT NULL DEFAULT '',
    [Brand]       NVARCHAR (50) NOT NULL DEFAULT '',
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


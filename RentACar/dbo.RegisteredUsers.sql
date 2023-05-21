CREATE TABLE [dbo].[RegisteredUsers] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Users]         NVARCHAR (50) NOT NULL,
    [Password]      NVARCHAR (50) NOT NULL,
    [UserTypeUsers] INT           NOT NULL,
    [Phone]         NVARCHAR (50) NOT NULL,
    [Email]         NVARCHAR (50) NOT NULL, 
    [Address] NVARCHAR(50) NOT NULL, 
    [Gender] NVARCHAR(50) NOT NULL, 
    [DateofBirth] DATE NOT NULL
);


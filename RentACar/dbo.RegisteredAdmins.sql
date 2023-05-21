CREATE TABLE [dbo].[RegisteredAdmins] (
    [RoleId]    INT           IDENTITY (1, 1) NOT NULL,
    [AdminName] NVARCHAR (50) NOT NULL,
    [Password]  NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC), 
    CONSTRAINT [FK_RegisteredAdmins_ToTableRole] FOREIGN KEY ([RoleId]) REFERENCES [Role]([RoleId])

);


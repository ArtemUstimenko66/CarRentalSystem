CREATE TABLE [dbo].[User_Profile] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [user_id] INT           NOT NULL,
    [phone]   VARCHAR (20)  NOT NULL,
    [address] VARCHAR (100) NOT NULL
	CONSTRAINT [FK_User_Profile_ToTableUser] FOREIGN KEY ([user_id]) REFERENCES [User]([Id])
);


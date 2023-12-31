CREATE TABLE [dbo].[Commande]
(
	CommandNumber INT NOT NULL PRIMARY KEY IDENTITY,
	Id_User int NOT NULL,
	CONSTRAINT [FK_User] FOREIGN KEY (Id_User) REFERENCES [Users]([Id])	
)

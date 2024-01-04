CREATE TABLE [dbo].[Recommandations]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Id_User] INT NOT NULL, 
    [Id_Category] INT NOT NULL, 
    CONSTRAINT [FK_Recommandations_Users] FOREIGN KEY ([Id_User]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Recommandations_Category] FOREIGN KEY ([Id_Category]) REFERENCES [Category]([Id])

)

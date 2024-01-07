CREATE TABLE [dbo].[CommandList]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[IdProduct] INT NOT NULL, 
    [CommandNumber] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [FK_Article_CommandList] FOREIGN KEY (IdProduct) REFERENCES Articles(Id),
	CONSTRAINT [FK_Commande_CommandList] FOREIGN KEY (CommandNumber) REFERENCES Commande(CommandNumber)
)

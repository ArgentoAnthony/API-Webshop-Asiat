CREATE TABLE [dbo].[Articles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	Name varchar(50) not null,
	Quantity int,
	Price FLOAT,
	Description varchar(max),
	Image varchar(max),
	Id_Category int, 
    [Id_Vendeur] INT NOT NULL, 
    CONSTRAINT [CK_Articles_Price] CHECK (Price >= 0), 
	CONSTRAINT [CK_Articles_Quantity] CHECK (Quantity >= 0),
    CONSTRAINT [FK_Articles_Category] FOREIGN KEY ([Id_Category]) REFERENCES [Category]([Id]),	
    CONSTRAINT [FK_Vendeur] FOREIGN KEY ([Id_Vendeur]) REFERENCES [Users]([Id])	
)

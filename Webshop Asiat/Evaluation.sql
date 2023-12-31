CREATE TABLE [dbo].[Evaluation]
(
	[Id_User] INT NOT NULL, 
    [Id_Produit] INT NOT NULL, 
    [Etoile] INT NULL,
    CONSTRAINT [PK_Etoile] PRIMARY KEY (Id_User,Id_Produit),
    CONSTRAINT [PK_Rating] CHECK (Etoile >= 0 AND Etoile <= 5)
)

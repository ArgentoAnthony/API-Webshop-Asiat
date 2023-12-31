CREATE TABLE [dbo].[Commentaires]
(
	[Id_User] INT NOT NULL, 
    [Id_Produit] INT NOT NULL, 
    [Commentaire] nvarchar(max) NULL,
    CONSTRAINT [PK_Commentaire] PRIMARY KEY (Id_User,Id_Produit)
)

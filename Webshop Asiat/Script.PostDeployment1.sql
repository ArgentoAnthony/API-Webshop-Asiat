/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Insertion des utilisateurs
INSERT INTO [dbo].[Users] ([Username], [Mail], [Password], [Address], [BirthDate], [IBAN], [TVA], [Role])
VALUES 
('Alice123', 'alice@email.com', 'motdepasse1', 'Adresse d''Alice', '1990-05-15', 'BE123456789', NULL, 1),
('Bob456', 'bob@email.com', 'motdepasse2', 'Adresse de Bob', '1985-09-20', 'BE987654321', NULL, 1),
('Eva789', 'eva@email.com', 'motdepasse3', 'Adresse d''Eva', '2000-02-10', NULL, NULL, 2),
('Daniel99', 'daniel@email.com', 'motdepasse4', 'Adresse de Daniel', '1978-12-03', NULL, NULL, 3),
('Sophie77', 'sophie@email.com', 'motdepasse5', 'Adresse de Sophie', '1995-08-28', NULL, 123456789, 4),
('Marc00', 'marc@email.com', 'motdepasse6', 'Adresse de Marc', '1982-04-12', NULL, 987654321, 4);

-- Insertion des catégories asiatiques
INSERT INTO [dbo].[Category] ([Name])
VALUES 
('Vêtements traditionnels'),
('Cuisine asiatique'),
('Décoration d''intérieur'),
('Art et artisanat'),
('Accessoires de mode'),
('Instruments de musique'),
('Jardin zen'),
('Bijoux et pierres précieuses'),
('Littérature et poésie');

-- Insertion de produits liés aux catégories asiatiques
INSERT INTO [dbo].[Articles] ([Name], [Quantity], [Price], [Description], [Image], [Id_Category], [Id_Vendeur])
VALUES 
('Kimono en soie', 15, 50, NULL, 'kimono.jpg', 1,5),
('Théière en fonte', 20, 35, 'Théière traditionnelle japonaise en fonte', NULL, 2,5),
('Statue de Bouddha', 10, 80, 'Statue en bronze représentant Bouddha', NULL, 3,5),
('Paravent peint à la main', 8, 120,NULL, 'paravent.jpg', 4,5),
('Collier en jade', 30, 25, NULL, 'collier.jpg', 5,5),
('Flûte traditionnelle', 12, 70, 'Flûte traditionnelle chinoise en bambou', 'flute.jpg', 6,5),
('Bonsaï', 25, 55, 'Arbre bonsaï japonais miniature', 'bonsai.jpg', 7,5),
('Livres de poésie chinoise', 18, 20, 'Recueils de poèmes classiques chinois', 'livres.jpg', 9,6),
('Robe Hanbok', 18, 65, 'Robe traditionnelle coréenne Hanbok', 'hanbok.jpg', 1,6),
('Service à sushi en céramique', 22, 40, NULL,NULL, 2,6),
('Calligraphie encadrée', 14, 90, 'Peinture de calligraphie chinoise encadrée', 'calligraphie.jpg', 4,6),
('Éventail décoratif', 30, 15, 'Éventail décoratif avec motif de fleurs de cerisier', 'eventail.jpg', 3,6),
('Bracelet en perles de bois', 25, 18, 'Bracelet fait de perles de bois naturel', 'bracelet.jpg', 5,6),
('Guzheng (instrument à cordes)', 7, 300, NULL,NULL, 6,6),
('Lanterne en papier', 40, 12, 'Lanterne traditionnelle en papier pour décoration', 'lanterne.jpg', 3,6),
('Tasse à thé en porcelaine', 50, 8, 'Tasse à thé en porcelaine avec motif floral', NULL, 2,6),
('Masque de théâtre Noh', 11, 75, 'Masque utilisé dans le théâtre traditionnel japonais Noh', 'masque.jpg', 4,1),
('Kimono pour enfants', 20, 40, NULL, 'kimono_enfant.jpg', 1,1),
('Encens parfumé', 35, 6, 'Encens parfumé japonais pour méditation', 'encens.jpg', 8,1),
('Épée samouraï décorative', 9, 150, 'Réplique d''une épée de samouraï décorative', 'epee.jpg', 4,1);

INSERT INTO Recommandations (Id_User, Id_Category)
VALUES (1, 3),
       (1, 4),
       (1, 2),
       (2, 1),
       (2, 2),
       (2, 5),
       (3, 2),
       (3, 1);

﻿CREATE TABLE [dbo].[Users]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] NVARCHAR(50) NOT NULL UNIQUE, 
    [Mail] NVARCHAR(100) NOT NULL UNIQUE, 
    [Password] NVARCHAR(MAX) NOT NULL, 
    [Address] NVARCHAR(100) NOT NULL, 
    [BirthDate] DATETIME2 NULL, 
    [IBAN] NVARCHAR(100) NULL, 
    [TVA] INT NULL, 
    [Role] INT NOT NULL
)
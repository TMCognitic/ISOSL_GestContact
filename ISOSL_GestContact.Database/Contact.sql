﻿CREATE TABLE [dbo].[Contact]
(
	[Id] INT NOT NULL IDENTITY, 
    [Nom] NVARCHAR(75) NOT NULL, 
    [Prenom] NVARCHAR(75) NOT NULL, 
    [Pays] NVARCHAR(75) NOT NULL, 
    CONSTRAINT [PK_Contact] PRIMARY KEY ([Id]) 
)

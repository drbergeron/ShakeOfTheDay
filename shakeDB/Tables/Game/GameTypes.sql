﻿CREATE TABLE [dbo].[GameTypes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [MaxPlaysPerDay] INT NOT NULL DEFAULT 1
)

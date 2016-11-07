CREATE TABLE [dbo].[Games]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [TypeId] INT NOT NULL, 
    [UserId] NCHAR(10) NOT NULL, 
    [Year] INT NOT NULL, 
    [Day] INT NOT NULL, 
    [isClosed] BIT NOT NULL DEFAULT 0
)

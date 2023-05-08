CREATE TABLE [dbo].[Picture] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    [Width]  INT           NOT NULL,
    [Height] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Pixel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [X] INT NOT NULL, 
    [Y] INT NOT NULL, 
    [Color] INT NOT NULL, 
    [PictureId] INT NOT NULL, 
    CONSTRAINT [FK_Pixel_Picture] FOREIGN KEY ([PictureId]) REFERENCES [Picture]([Id])
)
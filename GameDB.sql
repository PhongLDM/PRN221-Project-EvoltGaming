IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Game] (
    [gameId] int NOT NULL IDENTITY,
    [name] varchar(150) NOT NULL,
    [description] text NOT NULL,
    [publisher] varchar(100) NOT NULL,
    [releaseDate] date NOT NULL,
    [updateDate] date NOT NULL,
    [platform] varchar(50) NOT NULL,
    [pirateLink] text NULL,
    [officialLink] text NULL,
    [image] text NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY ([gameId])
);
GO

CREATE TABLE [Genre] (
    [genreId] int NOT NULL IDENTITY,
    [genreName] varchar(50) NOT NULL,
    CONSTRAINT [PK_Genre] PRIMARY KEY ([genreId])
);
GO

CREATE TABLE [Role] (
    [roleId] int NOT NULL IDENTITY,
    [roleName] varchar(50) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([roleId])
);
GO

CREATE TABLE [GameRequirement] (
    [gameId] int NOT NULL,
    [type] varchar(50) NOT NULL,
    [os] text NOT NULL,
    [processor] text NOT NULL,
    [memory] float NOT NULL,
    [storage] float NOT NULL,
    [directX] int NULL,
    [graphic] text NULL,
    [other] text NULL,
    CONSTRAINT [PK_GameRequirement] PRIMARY KEY ([gameId], [type]),
    CONSTRAINT [FK_GameRequirement_Game] FOREIGN KEY ([gameId]) REFERENCES [Game] ([gameId])
);
GO

CREATE TABLE [GameGenre] (
    [gameId] int NOT NULL,
    [genreId] int NOT NULL,
    CONSTRAINT [PK_GameGenre] PRIMARY KEY ([gameId], [genreId]),
    CONSTRAINT [FK_GameGenre_Game] FOREIGN KEY ([gameId]) REFERENCES [Game] ([gameId]),
    CONSTRAINT [FK_GameGenre_Genre] FOREIGN KEY ([genreId]) REFERENCES [Genre] ([genreId])
);
GO

CREATE TABLE [User] (
    [userId] int NOT NULL IDENTITY,
    [username] varchar(50) NOT NULL,
    [password] varchar(30) NOT NULL,
    [roleId] int NOT NULL,
    [isActive] bit NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([userId]),
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([roleId]) REFERENCES [Role] ([roleId])
);
GO

CREATE TABLE [Comment] (
    [commentId] int NOT NULL IDENTITY,
    [userId] int NOT NULL,
    [gameId] int NOT NULL,
    [content] text NOT NULL,
    [postTime] datetime NOT NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY ([commentId]),
    CONSTRAINT [FK_Comment_Game] FOREIGN KEY ([gameId]) REFERENCES [Game] ([gameId]),
    CONSTRAINT [FK_Comment_User] FOREIGN KEY ([userId]) REFERENCES [User] ([userId])
);
GO

CREATE TABLE [Favourite] (
    [gameId] int NOT NULL,
    [userId] int NOT NULL,
    CONSTRAINT [PK_Favourite] PRIMARY KEY ([gameId], [userId]),
    CONSTRAINT [FK_Favourite_Game] FOREIGN KEY ([gameId]) REFERENCES [Game] ([gameId]),
    CONSTRAINT [FK_Favourite_User] FOREIGN KEY ([userId]) REFERENCES [User] ([userId])
);
GO

CREATE TABLE [UserDetail] (
    [userId] int NOT NULL,
    [firstName] varchar(50) NOT NULL,
    [lastName] varchar(50) NOT NULL,
    [email] varchar(50) NOT NULL,
    [createdDate] date NOT NULL,
    [image] text NULL,
    CONSTRAINT [PK_UserDetail] PRIMARY KEY ([userId]),
    CONSTRAINT [FK_UserDetail_User] FOREIGN KEY ([userId]) REFERENCES [User] ([userId])
);
GO

CREATE INDEX [IX_Comment_gameId] ON [Comment] ([gameId]);
GO

CREATE INDEX [IX_Comment_userId] ON [Comment] ([userId]);
GO

CREATE INDEX [IX_Favourite_userId] ON [Favourite] ([userId]);
GO

CREATE INDEX [IX_GameGenre_genreId] ON [GameGenre] ([genreId]);
GO

CREATE INDEX [IX_User_roleId] ON [User] ([roleId]);
GO

CREATE UNIQUE INDEX [Unique_username] ON [User] ([username]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240614214553_InitialCreate', N'6.0.10');
GO

COMMIT;
GO


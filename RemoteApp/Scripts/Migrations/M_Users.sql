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

CREATE TABLE [User] (
    [UserId] int NOT NULL IDENTITY,
    [UserLogin] nvarchar(max) NOT NULL,
    [UserPassword] nvarchar(max) NOT NULL,
    [UserName] nvarchar(max) NOT NULL,
    [UserPrenom] nvarchar(max) NOT NULL,
    [UserStatus] int NOT NULL,
    [UserPhone] nvarchar(max) NOT NULL,
    [UserEmail] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([UserId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221213213300_M_Users', N'7.0.1');
GO

COMMIT;
GO


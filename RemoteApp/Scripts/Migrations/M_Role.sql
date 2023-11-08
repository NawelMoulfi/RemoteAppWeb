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

ALTER TABLE [User] ADD [RoleId] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [Role] (
    [RoleId] int NOT NULL IDENTITY,
    [RoleName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([RoleId])
);
GO

CREATE INDEX [IX_User_RoleId] ON [User] ([RoleId]);
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_User_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221214091525_M_Role', N'7.0.1');
GO

COMMIT;
GO


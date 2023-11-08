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

CREATE TABLE [Entry] (
    [EntryId] bigint NOT NULL IDENTITY,
    [EntryName] nvarchar(max) NOT NULL,
    [EntryType] int NOT NULL,
    [ID] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [UserId] int NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [URL] nvarchar(max) NOT NULL,
    [FolderId] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [EntryStatus] int NOT NULL,
    [Command] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Entry] PRIMARY KEY ([EntryId]),
    CONSTRAINT [FK_Entry_Folder_FolderId] FOREIGN KEY ([FolderId]) REFERENCES [Folder] ([FolderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Entry_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([UserId]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Entry_FolderId] ON [Entry] ([FolderId]);
GO

CREATE INDEX [IX_Entry_UserId] ON [Entry] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221214093954_M_Entry', N'7.0.1');
GO

COMMIT;
GO


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

CREATE TABLE [Folder] (
    [FolderId] int NOT NULL IDENTITY,
    [FolderName] nvarchar(max) NOT NULL,
    [ParentFolderId] int NULL,
    [FolderDescription] nvarchar(max) NOT NULL,
    [FolderStatus] int NOT NULL,
    CONSTRAINT [PK_Folder] PRIMARY KEY ([FolderId]),
    CONSTRAINT [FK_Folder_Folder_ParentFolderId] FOREIGN KEY ([ParentFolderId]) REFERENCES [Folder] ([FolderId])
);
GO

CREATE INDEX [IX_Folder_ParentFolderId] ON [Folder] ([ParentFolderId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221214092730_M_Folder', N'7.0.1');
GO

COMMIT;
GO


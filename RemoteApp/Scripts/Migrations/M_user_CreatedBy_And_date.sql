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

ALTER TABLE [Entry] DROP CONSTRAINT [FK_Entry_User_UserId];
GO

EXEC sp_rename N'[Entry].[UserId]', N'CreatedByUserId', N'COLUMN';
GO

EXEC sp_rename N'[Entry].[IX_Entry_UserId]', N'IX_Entry_CreatedByUserId', N'INDEX';
GO

ALTER TABLE [Entry] ADD [CreatedDate] datetime2 NULL;
GO

ALTER TABLE [Entry] ADD CONSTRAINT [FK_Entry_User_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [User] ([UserId]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221219124518_M_user_CreatedBy_And_date', N'7.0.1');
GO

COMMIT;
GO


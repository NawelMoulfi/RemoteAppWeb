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

EXEC sp_rename N'[User].[UserName]', N'UserNom', N'COLUMN';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ModuleAction]') AND [c].[name] = N'ModuleActionNom');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ModuleAction] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [ModuleAction] ALTER COLUMN [ModuleActionNom] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221216103021_M_UserNom', N'7.0.1');
GO

COMMIT;
GO


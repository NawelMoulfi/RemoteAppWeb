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

ALTER TABLE [User] ADD [UserMaxCapacity] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [Module] (
    [ModuleID] int NOT NULL IDENTITY,
    [ModuleNom] nvarchar(max) NOT NULL,
    [ModuleGroup] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Module] PRIMARY KEY ([ModuleID])
);
GO

CREATE TABLE [ModuleAction] (
    [ModuleActionID] int NOT NULL IDENTITY,
    [ModuleID] int NULL,
    [Resource] int NOT NULL,
    [Action] int NOT NULL,
    [ModuleActionNom] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ModuleAction] PRIMARY KEY ([ModuleActionID]),
    CONSTRAINT [FK_ModuleAction_Module_ModuleID] FOREIGN KEY ([ModuleID]) REFERENCES [Module] ([ModuleID])
);
GO

CREATE TABLE [ModuleActionRole] (
    [ModuleActionRoleId] int NOT NULL IDENTITY,
    [Role_RoleID] int NOT NULL,
    [ModuleAction_ModuleActionID] int NOT NULL,
    CONSTRAINT [PK_ModuleActionRole] PRIMARY KEY ([ModuleActionRoleId]),
    CONSTRAINT [FK_ModuleActionRole_ModuleAction_ModuleAction_ModuleActionID] FOREIGN KEY ([ModuleAction_ModuleActionID]) REFERENCES [ModuleAction] ([ModuleActionID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ModuleActionRole_Role_Role_RoleID] FOREIGN KEY ([Role_RoleID]) REFERENCES [Role] ([RoleId]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_ModuleAction_ModuleID] ON [ModuleAction] ([ModuleID]);
GO

CREATE INDEX [IX_ModuleActionRole_ModuleAction_ModuleActionID] ON [ModuleActionRole] ([ModuleAction_ModuleActionID]);
GO

CREATE INDEX [IX_ModuleActionRole_Role_RoleID] ON [ModuleActionRole] ([Role_RoleID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221214103027_M_ModuleActions', N'7.0.1');
GO

COMMIT;
GO


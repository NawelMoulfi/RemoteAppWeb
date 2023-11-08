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

CREATE TABLE [Client] (
    [ClientId] bigint NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Wilaya] nvarchar(max) NOT NULL,
    [Adresse] nvarchar(max) NOT NULL,
    [PID] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY ([ClientId])
    );
GO

CREATE TABLE [Materiel] (
    [MaterielId] bigint NOT NULL IDENTITY,
    [Code] nvarchar(max) NOT NULL,
    [Piece] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Materiel] PRIMARY KEY ([MaterielId])
    );
GO

CREATE TABLE [RapportIntervention] (
    [RapportId] bigint NOT NULL IDENTITY,
    [CreatedDate] datetime2 NULL,
    [Operation] int NOT NULL,
    [CommentaireTraveaux] nvarchar(max) NOT NULL,
    [AutreInformation] nvarchar(max) NOT NULL,
    [Num] bigint NOT NULL,
    [Logiciel] int NOT NULL,
    [ClientId] bigint NOT NULL,
    CONSTRAINT [PK_RapportIntervention] PRIMARY KEY ([RapportId]),
    CONSTRAINT [FK_RapportIntervention_Client_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Client] ([ClientId]) ON DELETE NO ACTION
    );
GO

CREATE TABLE [MaterielRapport] (
    [MaterielRapportId] bigint NOT NULL,
    [MaterielId] bigint NOT NULL,
    [RapportInterventionId] bigint NOT NULL,
    [Nombre] bigint NOT NULL,
     CONSTRAINT [PK_MaterielRapport] PRIMARY KEY ([MaterielRapportId]),
     CONSTRAINT [FK_MaterielRapport_Materiel_MaterielId] FOREIGN KEY ([MaterielId]) REFERENCES [Materiel] ([MaterielId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MaterielRapport_RapportIntervention_RapportInterventionId] FOREIGN KEY ([RapportInterventionId]) REFERENCES [RapportIntervention] ([RapportId]) ON DELETE NO ACTION
    );
GO

CREATE INDEX [IX_MaterielRapport_MaterielId] ON [MaterielRapport] ([MaterielId]);
GO

CREATE INDEX [IX_MaterielRapport_RapportInterventionId] ON [MaterielRapport] ([RapportInterventionId]);
GO

CREATE INDEX [IX_RapportIntervention_ClientId] ON [RapportIntervention] ([ClientId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230206103041_RapportIntervention', N'7.0.1');
GO

COMMIT;
GO


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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250323135413_InitialCreate'
)
BEGIN
    CREATE TABLE [Contributions] (
        [ContributionID] int NOT NULL IDENTITY,
        [MemberID] int NOT NULL,
        [ContributionType] int NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [ContributionDate] datetime2 NOT NULL,
        [IsValidated] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Contributions] PRIMARY KEY ([ContributionID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250323135413_InitialCreate'
)
BEGIN
    CREATE TABLE [Employers] (
        [EmployerID] int NOT NULL IDENTITY,
        [CompanyName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Employers] PRIMARY KEY ([EmployerID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250323135413_InitialCreate'
)
BEGIN
    CREATE TABLE [Members] (
        [MemberID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [Email] nvarchar(max) NULL,
        [Phone] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [IsEligibleForBenefits] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Members] PRIMARY KEY ([MemberID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250323135413_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250323135413_InitialCreate', N'9.0.3');
END;

COMMIT;
GO


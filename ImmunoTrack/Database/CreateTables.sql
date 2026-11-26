-- ============================================================
-- IMMUNOTRACK - 2 of 4: CreateTables.sql
-- Run SECOND. Creates the six tables with keys and constraints.
--
-- WARNING (development convenience): this script DROPS the
-- tables first, so running it again ERASES all data. Run
-- InsertSampleData.sql afterwards to reload the sample rows.
-- ============================================================

USE ImmunoTrackDB;
GO

-- Drop in reverse dependency order (children of foreign keys first)
IF OBJECT_ID(N'dbo.VaccinationRecords', N'U') IS NOT NULL DROP TABLE dbo.VaccinationRecords;
IF OBJECT_ID(N'dbo.VaccineSchedule',    N'U') IS NOT NULL DROP TABLE dbo.VaccineSchedule;
IF OBJECT_ID(N'dbo.Vaccines',           N'U') IS NOT NULL DROP TABLE dbo.Vaccines;
IF OBJECT_ID(N'dbo.Children',           N'U') IS NOT NULL DROP TABLE dbo.Children;
IF OBJECT_ID(N'dbo.Guardians',          N'U') IS NOT NULL DROP TABLE dbo.Guardians;
IF OBJECT_ID(N'dbo.HealthWorkers',      N'U') IS NOT NULL DROP TABLE dbo.HealthWorkers;
GO

-- ------------------------------------------------------------
-- TABLE 1: Guardians
-- ------------------------------------------------------------
CREATE TABLE dbo.Guardians
(
    GuardianID          INT IDENTITY(1,1)  NOT NULL,
    FirstName           NVARCHAR(50)       NOT NULL,
    MiddleName          NVARCHAR(50)       NULL,
    LastName            NVARCHAR(50)       NOT NULL,
    ContactNumber       VARCHAR(20)        NOT NULL,
    Address             NVARCHAR(200)      NOT NULL,
    RelationshipToChild NVARCHAR(30)       NOT NULL,

    CONSTRAINT PK_Guardians PRIMARY KEY (GuardianID),
    CONSTRAINT CK_Guardians_FirstName CHECK (LEN(LTRIM(RTRIM(FirstName))) > 0),
    CONSTRAINT CK_Guardians_LastName  CHECK (LEN(LTRIM(RTRIM(LastName))) > 0),
    -- digits, +, space and dash only
    CONSTRAINT CK_Guardians_Contact   CHECK (ContactNumber NOT LIKE '%[^0-9+ -]%')
);
GO

-- ------------------------------------------------------------
-- TABLE 2: HealthWorkers (also the system users who log in)
-- Passwords are NEVER stored as plain text: a random salt and
-- a SHA-256 hash of (salt + password) are stored instead.
-- ------------------------------------------------------------
CREATE TABLE dbo.HealthWorkers
(
    HealthWorkerID INT IDENTITY(1,1)  NOT NULL,
    FirstName      NVARCHAR(50)       NOT NULL,
    MiddleName     NVARCHAR(50)       NULL,
    LastName       NVARCHAR(50)       NOT NULL,
    ContactNumber  VARCHAR(20)        NOT NULL,
    Position       NVARCHAR(50)       NOT NULL,
    LicenseNo      NVARCHAR(30)       NULL,
    Username       NVARCHAR(30)       NOT NULL,
    PasswordSalt   VARCHAR(32)        NOT NULL,
    PasswordHash   VARBINARY(32)      NOT NULL,
    -- 'Admin' accounts cannot be deleted or demoted and can manage the
    -- Health Workers screen. Everyone else is 'Staff'.
    Role           VARCHAR(10)        NOT NULL CONSTRAINT DF_HealthWorkers_Role DEFAULT 'Staff',

    CONSTRAINT PK_HealthWorkers PRIMARY KEY (HealthWorkerID),
    CONSTRAINT CK_HealthWorkers_Role CHECK (Role IN ('Admin', 'Staff')),
    CONSTRAINT UQ_HealthWorkers_Username UNIQUE (Username),
    CONSTRAINT CK_HealthWorkers_FirstName CHECK (LEN(LTRIM(RTRIM(FirstName))) > 0),
    CONSTRAINT CK_HealthWorkers_LastName  CHECK (LEN(LTRIM(RTRIM(LastName))) > 0),
    CONSTRAINT CK_HealthWorkers_Contact   CHECK (ContactNumber NOT LIKE '%[^0-9+ -]%'),
    CONSTRAINT CK_HealthWorkers_Username  CHECK (LEN(LTRIM(RTRIM(Username))) >= 3)
);
GO

-- ------------------------------------------------------------
-- TABLE 3: Children
-- ------------------------------------------------------------
CREATE TABLE dbo.Children
(
    ChildID     INT IDENTITY(1,1)  NOT NULL,
    FirstName   NVARCHAR(50)       NOT NULL,
    MiddleName  NVARCHAR(50)       NULL,
    LastName    NVARCHAR(50)       NOT NULL,
    DateOfBirth DATE               NOT NULL,
    Sex         VARCHAR(6)         NOT NULL,
    BloodType   VARCHAR(3)         NULL,
    Address     NVARCHAR(200)      NOT NULL,
    GuardianID  INT                NOT NULL,

    CONSTRAINT PK_Children PRIMARY KEY (ChildID),
    CONSTRAINT FK_Children_Guardians FOREIGN KEY (GuardianID)
        REFERENCES dbo.Guardians (GuardianID),
    CONSTRAINT CK_Children_FirstName CHECK (LEN(LTRIM(RTRIM(FirstName))) > 0),
    CONSTRAINT CK_Children_LastName  CHECK (LEN(LTRIM(RTRIM(LastName))) > 0),
    CONSTRAINT CK_Children_Sex       CHECK (Sex IN ('Male', 'Female')),
    CONSTRAINT CK_Children_BloodType CHECK (BloodType IS NULL
        OR BloodType IN ('A+','A-','B+','B-','AB+','AB-','O+','O-')),
    CONSTRAINT CK_Children_DOB       CHECK (DateOfBirth <= CAST(GETDATE() AS DATE))
);
GO

CREATE INDEX IX_Children_Name     ON dbo.Children (LastName, FirstName);
CREATE INDEX IX_Children_Guardian ON dbo.Children (GuardianID);
GO

-- ------------------------------------------------------------
-- TABLE 4: Vaccines (reference list only - NOT inventory)
-- ------------------------------------------------------------
CREATE TABLE dbo.Vaccines
(
    VaccineID   INT IDENTITY(1,1)  NOT NULL,
    VaccineName NVARCHAR(100)      NOT NULL,
    Description NVARCHAR(250)      NULL,
    IsActive    BIT                NOT NULL CONSTRAINT DF_Vaccines_IsActive DEFAULT (1),

    CONSTRAINT PK_Vaccines PRIMARY KEY (VaccineID),
    CONSTRAINT UQ_Vaccines_Name UNIQUE (VaccineName),
    CONSTRAINT CK_Vaccines_Name CHECK (LEN(LTRIM(RTRIM(VaccineName))) > 0)
);
GO

-- ------------------------------------------------------------
-- TABLE 5: VaccineSchedule (one row per dose of a vaccine)
-- RecommendedAgeDays : child's age (in days) when the dose is due
-- MinimumIntervalDays: minimum days after the PREVIOUS dose
--                      (NULL for dose 1 or when only age applies)
-- ------------------------------------------------------------
CREATE TABLE dbo.VaccineSchedule
(
    ScheduleID          INT IDENTITY(1,1)  NOT NULL,
    VaccineID           INT                NOT NULL,
    DoseNumber          INT                NOT NULL,
    RecommendedAgeDays  INT                NOT NULL,
    MinimumIntervalDays INT                NULL,
    IsActive            BIT                NOT NULL CONSTRAINT DF_VaccineSchedule_IsActive DEFAULT (1),

    CONSTRAINT PK_VaccineSchedule PRIMARY KEY (ScheduleID),
    CONSTRAINT FK_VaccineSchedule_Vaccines FOREIGN KEY (VaccineID)
        REFERENCES dbo.Vaccines (VaccineID),
    CONSTRAINT UQ_VaccineSchedule_Dose UNIQUE (VaccineID, DoseNumber),
    CONSTRAINT CK_VaccineSchedule_Dose     CHECK (DoseNumber >= 1),
    CONSTRAINT CK_VaccineSchedule_Age      CHECK (RecommendedAgeDays >= 0),
    CONSTRAINT CK_VaccineSchedule_Interval CHECK (MinimumIntervalDays IS NULL OR MinimumIntervalDays >= 0)
);
GO

-- ------------------------------------------------------------
-- TABLE 6: VaccinationRecords (the transaction log)
-- UNIQUE (ChildID, ScheduleID) = a child can receive each dose once.
-- ------------------------------------------------------------
CREATE TABLE dbo.VaccinationRecords
(
    RecordID         INT IDENTITY(1,1)  NOT NULL,
    ChildID          INT                NOT NULL,
    ScheduleID       INT                NOT NULL,
    DateAdministered DATE               NOT NULL,
    HealthWorkerID   INT                NOT NULL,
    Remarks          NVARCHAR(250)      NULL,

    CONSTRAINT PK_VaccinationRecords PRIMARY KEY (RecordID),
    CONSTRAINT FK_VaccinationRecords_Children FOREIGN KEY (ChildID)
        REFERENCES dbo.Children (ChildID),
    CONSTRAINT FK_VaccinationRecords_Schedule FOREIGN KEY (ScheduleID)
        REFERENCES dbo.VaccineSchedule (ScheduleID),
    CONSTRAINT FK_VaccinationRecords_HealthWorkers FOREIGN KEY (HealthWorkerID)
        REFERENCES dbo.HealthWorkers (HealthWorkerID),
    CONSTRAINT UQ_VaccinationRecords_ChildDose UNIQUE (ChildID, ScheduleID),
    CONSTRAINT CK_VaccinationRecords_Date CHECK (DateAdministered <= CAST(GETDATE() AS DATE))
);
GO

CREATE INDEX IX_VaccinationRecords_Child ON dbo.VaccinationRecords (ChildID);
GO

PRINT 'All 6 tables created.';
GO
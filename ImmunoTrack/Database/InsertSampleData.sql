-- ============================================================
-- IMMUNOTRACK - 3 of 4: InsertSampleData.sql
-- Run THIRD (after CreateTables.sql).
--
-- *** ALL DATA BELOW IS MADE UP (SAMPLE ONLY) ***
-- The vaccines, doses, ages and intervals are NOT a real or
-- medically accurate schedule. Replace them with an approved
-- schedule before any real use.
--
-- Child ages are relative to today's date (GETDATE), so the
-- statuses below stay meaningful whenever you run the script.
-- Guarded: it does nothing if Guardians already has rows.
-- ============================================================

USE ImmunoTrackDB;
GO

SET NOCOUNT ON;

IF EXISTS (SELECT 1 FROM dbo.Guardians)
BEGIN
    PRINT 'Sample data skipped: tables already contain data. Re-run CreateTables.sql to reset.';
    RETURN;
END

DECLARE @today DATE = CAST(GETDATE() AS DATE);

-- ---------- Health workers (the two system administrators) ----------
-- Logins:  admin1 / 1234567890   and   admin2 / 112233445566
-- Hash = SHA-256 of (salt + password). The C# login code must use the
-- same formula: SHA256( UTF8( salt + password ) ).
-- Role 'Admin' = cannot be deleted or demoted; can open Health Workers.
INSERT INTO dbo.HealthWorkers
    (FirstName, MiddleName, LastName, ContactNumber, Position, LicenseNo, Username, PasswordSalt, PasswordHash, Role)
VALUES
    ('John Michael', 'B.', 'Matiga', '09123456789', 'Barangay Health Officer', 'ADMIN-0001',
        'admin1', 'S4lt-AD-01', HASHBYTES('SHA2_256', 'S4lt-AD-01' + '1234567890'), 'Admin'),
    ('Kent', NULL, 'Espelita', '09876543211', 'Barangay Health Officer', 'ADMIN-0002',
        'admin2', 'S4lt-AD-02', HASHBYTES('SHA2_256', 'S4lt-AD-02' + '112233445566'), 'Admin');

-- ---------- Guardians ----------
INSERT INTO dbo.Guardians (FirstName, MiddleName, LastName, ContactNumber, Address, RelationshipToChild)
VALUES
    ('Gehlee', NULL, 'Dangca',  '09171110001', 'Sample St. 1, Sample Barangay', 'Mother'),
    ('Nicole', NULL, 'Fortuna', '09171110002', 'Sample St. 2, Sample Barangay', 'Mother'),
    ('Bela',   NULL, 'Arcal',   '09171110003', 'Sample St. 3, Sample Barangay', 'Mother'),
    ('Wynwyn', NULL, 'Marquez', '09171110004', 'Sample St. 4, Sample Barangay', 'Mother');

-- ---------- Vaccines (SAMPLE, made up) ----------
INSERT INTO dbo.Vaccines (VaccineName, Description, IsActive)
VALUES
    ('SAMPLE Vaccine A', 'Made-up sample vaccine, 3 doses. Not a real schedule.', 1),
    ('SAMPLE Vaccine B', 'Made-up sample vaccine, 2 doses. Not a real schedule.', 1),
    ('SAMPLE Vaccine C', 'Made-up sample vaccine, 1 dose at birth. Not a real schedule.', 1);

-- ---------- VaccineSchedule (SAMPLE, made up) ----------
INSERT INTO dbo.VaccineSchedule (VaccineID, DoseNumber, RecommendedAgeDays, MinimumIntervalDays, IsActive)
SELECT v.VaccineID, s.DoseNumber, s.AgeDays, s.IntervalDays, 1
FROM (VALUES
    ('SAMPLE Vaccine A', 1,  45, NULL),
    ('SAMPLE Vaccine A', 2,  75, 28),
    ('SAMPLE Vaccine A', 3, 105, 28),
    ('SAMPLE Vaccine B', 1, 270, NULL),
    ('SAMPLE Vaccine B', 2, 365, 90),
    ('SAMPLE Vaccine C', 1,   0, NULL)
) AS s (VaccineName, DoseNumber, AgeDays, IntervalDays)
INNER JOIN dbo.Vaccines v ON v.VaccineName = s.VaccineName;

-- ---------- Children (first names are unique here; date of birth is fixed) ----------
INSERT INTO dbo.Children (FirstName, MiddleName, LastName, DateOfBirth, Sex, BloodType, Address, GuardianID)
SELECT c.FirstName, c.MiddleName, c.LastName, c.DateOfBirth,
       c.Sex, c.BloodType, g.Address, g.GuardianID
FROM (VALUES
    ('Thirdy',  'Dangca', 'Dangca',      CAST('2026-05-10' AS DATE), 'Male',   'O+',  'Dangca'),
    ('Cianne',  NULL,     'Dominguez',   CAST('2026-07-08' AS DATE), 'Female', 'AB-', 'Fortuna'),
    ('Anthony', NULL,     'Constantino', CAST('2026-01-30' AS DATE), 'Male',   'O-',  'Fortuna'),
    ('Race',    NULL,     'Matias',      CAST('2026-10-12' AS DATE), 'Male',   'B+',  'Arcal'),
    ('Primo',   NULL,     'Alterado',    CAST('2026-03-19' AS DATE), 'Male',   'A+',  'Arcal'),
    ('Nathan',  NULL,     'Garcia',      CAST('2025-10-22' AS DATE), 'Male',   NULL,  'Marquez')
) AS c (FirstName, MiddleName, LastName, DateOfBirth, Sex, BloodType, GuardianLastName)
INNER JOIN dbo.Guardians g ON g.LastName = c.GuardianLastName;

-- ---------- Vaccination records (days after birth the dose was given) ----------
DECLARE @worker INT = (SELECT HealthWorkerID FROM dbo.HealthWorkers WHERE Username = 'admin1');

INSERT INTO dbo.VaccinationRecords (ChildID, ScheduleID, DateAdministered, HealthWorkerID, Remarks)
SELECT c.ChildID, sch.ScheduleID, DATEADD(DAY, r.DaysAfterBirth, c.DateOfBirth), @worker, 'Sample record'
FROM (VALUES
    ('Thirdy',  'SAMPLE Vaccine C', 1,   1),
    ('Thirdy',  'SAMPLE Vaccine A', 1,  46),
    ('Cianne', 'SAMPLE Vaccine C', 1,   1),
    ('Anthony', 'SAMPLE Vaccine C', 1,   2),
    ('Race', 'SAMPLE Vaccine C', 1,   0),
    ('Nathan', 'SAMPLE Vaccine C', 1,   1),
    ('Nathan', 'SAMPLE Vaccine A', 1,  45),
    ('Nathan', 'SAMPLE Vaccine A', 2,  76),
    ('Nathan', 'SAMPLE Vaccine A', 3, 106),
    ('Nathan', 'SAMPLE Vaccine B', 1, 272),
    ('Nathan', 'SAMPLE Vaccine B', 2, 366)
) AS r (FirstName, VaccineName, DoseNumber, DaysAfterBirth)
INNER JOIN dbo.Children c        ON c.FirstName = r.FirstName
INNER JOIN dbo.Vaccines vac      ON vac.VaccineName = r.VaccineName
INNER JOIN dbo.VaccineSchedule sch ON sch.VaccineID = vac.VaccineID AND sch.DoseNumber = r.DoseNumber;

PRINT 'Sample data inserted.';
GO

-- Quick check (expected: Guardians 4, HealthWorkers 2, Vaccines 3,
-- VaccineSchedule 6, Children 6, VaccinationRecords 11)
SELECT 'Guardians' AS TableName, COUNT(*) AS Rows FROM dbo.Guardians
UNION ALL SELECT 'HealthWorkers',      COUNT(*) FROM dbo.HealthWorkers
UNION ALL SELECT 'Vaccines',           COUNT(*) FROM dbo.Vaccines
UNION ALL SELECT 'VaccineSchedule',    COUNT(*) FROM dbo.VaccineSchedule
UNION ALL SELECT 'Children',           COUNT(*) FROM dbo.Children
UNION ALL SELECT 'VaccinationRecords', COUNT(*) FROM dbo.VaccinationRecords;
GO
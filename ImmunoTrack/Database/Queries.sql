-- ============================================================
-- IMMUNOTRACK - 4 of 4: Queries.sql
-- Reference queries for the Data Access layer, plus a view that
-- calculates each child's next required dose.
--
-- Run the CREATE VIEW part once. The queries that use @Parameters
-- are templates: in C# they are sent with SqlCommand.Parameters,
-- so they will NOT run as-is in the query window.
-- ============================================================

USE ImmunoTrackDB;
GO

-- ============================================================
-- A. VIEW: next required dose per child per vaccine
-- Rule: next dose = lowest ACTIVE dose number NOT yet recorded.
-- DueDate = the LATER of:
--     (1) DateOfBirth + RecommendedAgeDays
--     (2) previous dose date + MinimumIntervalDays (if configured)
-- ============================================================
CREATE OR ALTER VIEW dbo.vw_ChildNextDoses
AS
WITH Pending AS
(
    SELECT c.ChildID, c.DateOfBirth, s.ScheduleID, s.VaccineID, s.DoseNumber,
           s.RecommendedAgeDays, s.MinimumIntervalDays,
           ROW_NUMBER() OVER (PARTITION BY c.ChildID, s.VaccineID
                              ORDER BY s.DoseNumber) AS rn
    FROM dbo.Children c
    CROSS JOIN dbo.VaccineSchedule s
    INNER JOIN dbo.Vaccines v ON v.VaccineID = s.VaccineID
    WHERE s.IsActive = 1
      AND v.IsActive = 1
      AND NOT EXISTS (SELECT 1
                      FROM dbo.VaccinationRecords r
                      WHERE r.ChildID = c.ChildID AND r.ScheduleID = s.ScheduleID)
)
SELECT p.ChildID, p.VaccineID, v.VaccineName, p.ScheduleID, p.DoseNumber,
       CASE WHEN prev.DateAdministered IS NOT NULL
                 AND p.MinimumIntervalDays IS NOT NULL
                 AND DATEADD(DAY, p.MinimumIntervalDays, prev.DateAdministered)
                     > DATEADD(DAY, p.RecommendedAgeDays, p.DateOfBirth)
            THEN DATEADD(DAY, p.MinimumIntervalDays, prev.DateAdministered)
            ELSE DATEADD(DAY, p.RecommendedAgeDays, p.DateOfBirth)
       END AS DueDate
FROM Pending p
INNER JOIN dbo.Vaccines v ON v.VaccineID = p.VaccineID
OUTER APPLY
(
    SELECT TOP 1 r.DateAdministered
    FROM dbo.VaccinationRecords r
    INNER JOIN dbo.VaccineSchedule ps ON ps.ScheduleID = r.ScheduleID
    WHERE r.ChildID = p.ChildID
      AND ps.VaccineID = p.VaccineID
      AND ps.DoseNumber < p.DoseNumber
    ORDER BY ps.DoseNumber DESC
) AS prev
WHERE p.rn = 1;
GO

-- ============================================================
-- B. VERIFY THE VIEW (runs as-is). With the sample data expect:
--    Juan  : Vaccine A dose 2 OVERDUE      Maria : A dose 1 Upcoming
--    Carla : A dose 1 Upcoming             Pedro : A dose 1 DUE TODAY
--    Luis  : C dose 1 + A dose 1 OVERDUE   Ana   : no rows (fully vaccinated)
-- ============================================================
SELECT c.ChildID,
       c.FirstName + ' ' + c.LastName AS ChildName,
       n.VaccineName, n.DoseNumber, n.DueDate,
       CASE WHEN n.DueDate < CAST(GETDATE() AS DATE) THEN 'Overdue'
            WHEN n.DueDate = CAST(GETDATE() AS DATE) THEN 'Due Today'
            ELSE 'Upcoming' END AS Status
FROM dbo.vw_ChildNextDoses n
INNER JOIN dbo.Children c ON c.ChildID = n.ChildID
ORDER BY n.DueDate, ChildName;
GO

-- ============================================================
-- C. DEFAULTERS: every missed next dose (due date already passed)
-- ============================================================
SELECT c.ChildID,
       c.FirstName + ' ' + c.LastName AS ChildName,
       g.FirstName + ' ' + g.LastName AS GuardianName, g.ContactNumber,
       n.VaccineName, n.DoseNumber, n.DueDate,
       DATEDIFF(DAY, n.DueDate, CAST(GETDATE() AS DATE)) AS DaysOverdue
FROM dbo.vw_ChildNextDoses n
INNER JOIN dbo.Children c ON c.ChildID = n.ChildID
INNER JOIN dbo.Guardians g ON g.GuardianID = c.GuardianID
WHERE n.DueDate < CAST(GETDATE() AS DATE)
ORDER BY DaysOverdue DESC;
GO

-- ============================================================
-- D. OVERALL STATUS PER CHILD (most urgent dose decides)
--    Earliest next due date < today -> Overdue; = today -> Due Today;
--    later -> Upcoming; no pending dose -> Fully Vaccinated
-- ============================================================
SELECT c.ChildID, c.FirstName + ' ' + c.LastName AS ChildName,
       CASE WHEN MIN(n.DueDate) IS NULL THEN 'Fully Vaccinated'
            WHEN MIN(n.DueDate) < CAST(GETDATE() AS DATE) THEN 'Overdue'
            WHEN MIN(n.DueDate) = CAST(GETDATE() AS DATE) THEN 'Due Today'
            ELSE 'Upcoming' END AS Status
FROM dbo.Children c
LEFT JOIN dbo.vw_ChildNextDoses n ON n.ChildID = c.ChildID
GROUP BY c.ChildID, c.FirstName, c.LastName
ORDER BY c.ChildID;
GO

-- ============================================================
-- E. DASHBOARD COUNTS
-- ============================================================
SELECT COUNT(*) AS TotalChildren       FROM dbo.Children;
SELECT COUNT(*) AS TotalVaccinations   FROM dbo.VaccinationRecords;

WITH ChildStatus AS
(
    SELECT c.ChildID,
           CASE WHEN MIN(n.DueDate) IS NULL THEN 'Fully Vaccinated'
                WHEN MIN(n.DueDate) < CAST(GETDATE() AS DATE) THEN 'Overdue'
                WHEN MIN(n.DueDate) = CAST(GETDATE() AS DATE) THEN 'Due Today'
                ELSE 'Upcoming' END AS Status
    FROM dbo.Children c
    LEFT JOIN dbo.vw_ChildNextDoses n ON n.ChildID = c.ChildID
    GROUP BY c.ChildID
)
SELECT Status, COUNT(*) AS Total FROM ChildStatus GROUP BY Status;
GO

-- ============================================================
-- F. IMMUNIZATION HISTORY of one child  (parameter: @ChildID)
-- ============================================================
/*
SELECT r.RecordID, v.VaccineName, s.DoseNumber, r.DateAdministered,
       hw.FirstName + ' ' + hw.LastName AS AdministeredBy, r.Remarks
FROM dbo.VaccinationRecords r
INNER JOIN dbo.VaccineSchedule s ON s.ScheduleID = r.ScheduleID
INNER JOIN dbo.Vaccines v        ON v.VaccineID = s.VaccineID
INNER JOIN dbo.HealthWorkers hw  ON hw.HealthWorkerID = r.HealthWorkerID
WHERE r.ChildID = @ChildID
ORDER BY r.DateAdministered, s.DoseNumber;
*/

-- ============================================================
-- G. SEARCH CHILDREN (templates - one per search type)
-- ============================================================
/*
-- By Child ID
SELECT c.*, g.FirstName + ' ' + g.LastName AS GuardianName
FROM dbo.Children c INNER JOIN dbo.Guardians g ON g.GuardianID = c.GuardianID
WHERE c.ChildID = @ChildID;

-- By child's name (first + last, or first + middle + last)
SELECT c.*, g.FirstName + ' ' + g.LastName AS GuardianName
FROM dbo.Children c INNER JOIN dbo.Guardians g ON g.GuardianID = c.GuardianID
WHERE (c.FirstName + ' ' + c.LastName) LIKE '%' + @Name + '%'
   OR (c.FirstName + ' ' + ISNULL(c.MiddleName + ' ', '') + c.LastName) LIKE '%' + @Name + '%';

-- By guardian's name
SELECT c.*, g.FirstName + ' ' + g.LastName AS GuardianName
FROM dbo.Children c INNER JOIN dbo.Guardians g ON g.GuardianID = c.GuardianID
WHERE (g.FirstName + ' ' + g.LastName) LIKE '%' + @Name + '%';
*/

-- ============================================================
-- H. CRUD TEMPLATES (parameterized - used from C# via ADO.NET)
-- ============================================================
/*
-- ---- CHILDREN ----
INSERT INTO dbo.Children (FirstName, MiddleName, LastName, DateOfBirth, Sex, BloodType, Address, GuardianID)
VALUES (@FirstName, @MiddleName, @LastName, @DateOfBirth, @Sex, @BloodType, @Address, @GuardianID);
SELECT CAST(SCOPE_IDENTITY() AS INT);

SELECT c.*, g.FirstName + ' ' + g.LastName AS GuardianName
FROM dbo.Children c INNER JOIN dbo.Guardians g ON g.GuardianID = c.GuardianID
ORDER BY c.LastName, c.FirstName;

UPDATE dbo.Children
SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName,
    DateOfBirth = @DateOfBirth, Sex = @Sex, BloodType = @BloodType,
    Address = @Address, GuardianID = @GuardianID
WHERE ChildID = @ChildID;

DELETE FROM dbo.Children WHERE ChildID = @ChildID;   -- fails (error 547) if the child has vaccination records

-- ---- GUARDIANS ----
INSERT INTO dbo.Guardians (FirstName, MiddleName, LastName, ContactNumber, Address, RelationshipToChild)
VALUES (@FirstName, @MiddleName, @LastName, @ContactNumber, @Address, @RelationshipToChild);

UPDATE dbo.Guardians
SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName,
    ContactNumber = @ContactNumber, Address = @Address, RelationshipToChild = @RelationshipToChild
WHERE GuardianID = @GuardianID;

DELETE FROM dbo.Guardians WHERE GuardianID = @GuardianID;   -- fails (547) if the guardian still has children

-- ---- HEALTH WORKERS ----
INSERT INTO dbo.HealthWorkers (FirstName, MiddleName, LastName, ContactNumber, Position, LicenseNo, Username, PasswordSalt, PasswordHash)
VALUES (@FirstName, @MiddleName, @LastName, @ContactNumber, @Position, @LicenseNo, @Username, @PasswordSalt, @PasswordHash);

UPDATE dbo.HealthWorkers
SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName,
    ContactNumber = @ContactNumber, Position = @Position, LicenseNo = @LicenseNo
WHERE HealthWorkerID = @HealthWorkerID;

DELETE FROM dbo.HealthWorkers WHERE HealthWorkerID = @HealthWorkerID;   -- fails (547) if they recorded vaccinations

-- ---- LOGIN: fetch salt + hash by username, then compare the hash in C# ----
SELECT HealthWorkerID, FirstName, LastName, PasswordSalt, PasswordHash
FROM dbo.HealthWorkers WHERE Username = @Username;

-- ---- VACCINES / SCHEDULE ----
INSERT INTO dbo.Vaccines (VaccineName, Description, IsActive) VALUES (@VaccineName, @Description, @IsActive);
INSERT INTO dbo.VaccineSchedule (VaccineID, DoseNumber, RecommendedAgeDays, MinimumIntervalDays, IsActive)
VALUES (@VaccineID, @DoseNumber, @RecommendedAgeDays, @MinimumIntervalDays, @IsActive);

-- ---- VACCINATION TRANSACTION (the C# service checks dose order first) ----
INSERT INTO dbo.VaccinationRecords (ChildID, ScheduleID, DateAdministered, HealthWorkerID, Remarks)
VALUES (@ChildID, @ScheduleID, @DateAdministered, @HealthWorkerID, @Remarks);
*/

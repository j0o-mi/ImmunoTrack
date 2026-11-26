-- ============================================================
-- IMMUNOTRACK - 1 of 4: CreateDatabase.sql
-- Run this FIRST, connected to (localdb)\MSSQLLocalDB.
-- Safe to run more than once: it only creates the database
-- if it does not exist yet.
-- ============================================================

IF DB_ID(N'ImmunoTrackDB') IS NULL
BEGIN
    CREATE DATABASE ImmunoTrackDB;
    PRINT 'ImmunoTrackDB created.';
END
ELSE
BEGIN
    PRINT 'ImmunoTrackDB already exists.';
END
GO

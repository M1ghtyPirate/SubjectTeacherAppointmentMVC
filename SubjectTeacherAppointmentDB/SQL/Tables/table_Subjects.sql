USE [SubjectTeacherAppointment]

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Subjects')
	CREATE TABLE [dbo].[Subjects]
	(
		[SubjectID] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY, 
		[Name] NVARCHAR(256) NOT NULL,
			--CONSTRAINT [UC_Name] UNIQUE,
		[CreationDate] DATETIME
			DEFAULT (GETDATE())
	)
--DROP TABLE [dbo].[Subjects]
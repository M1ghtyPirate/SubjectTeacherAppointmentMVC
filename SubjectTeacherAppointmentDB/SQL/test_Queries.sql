DROP TABLE [Subjects]
DROP TABLE [Teacher]
DROP TABLE [SubjectTeachers]


DECLARE @SubjectID UNIQUEIDENTIFIER
EXEC [dbo].[sp_Subject] N'TEST_SUBJECT', @ForestryID OUT
SELECT @SubjectID


DECLARE @SubjectID UNIQUEIDENTIFIER
EXEC [dbo].[sp_Subject] N'TEST_SUBJECT', @ForestryID OUT
SELECT @SubjectID

SELECT *
FROM Users

USE [SubjectTeacherAppointment]

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Teachers')
	CREATE TABLE [dbo].[Teachers]
	(
		[TeacherID] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY, 
		[LastName] NVARCHAR(256) NOT NULL,
		[Surname] NVARCHAR(256),
		[Name] NVARCHAR(256) NOT NULL,
		[Birthday] DATETIME,
		[Sex] NVARCHAR(10) CHECK ([Sex] IN(N'Мужской', N'Женский')),
		[Photo] VARBINARY(MAX),
		[Notes] NVARCHAR(MAX),
		[CreationDate] DATETIME
			DEFAULT (GETDATE())
	)
--DROP TABLE [dbo].[Teachers]

USE [SubjectTeacherAppointment]

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SubjectTeachers')
	CREATE TABLE [dbo].[SubjectTeachers]
	(
		[SubjectTeacherID] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY, 
		[SubjectID] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[Subjects] ([SubjectID]),
		[TeacherID] UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES [dbo].[Teachers] ([TeacherID]),
		[HoursPerWeek] INT
	)
--DROP TABLE [dbo].[SubjectTeachers]

SELECT *
FROM Teachers
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
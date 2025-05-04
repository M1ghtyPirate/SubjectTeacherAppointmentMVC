USE [SubjectTeacherAppointment]

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Teachers')
	CREATE TABLE [dbo].[Teachers]
	(
		[TeacherID] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY, 
		[LastName] NVARCHAR(256) NOT NULL,
		[Surname] NVARCHAR(256),
		[Name] NVARCHAR(256) NOT NULL,
		[Birthday] DATETIME,
		[Sex] NVARCHAR(10), --CHECK ([Sex] IN(N'Мужской', N'Женский')),
		[Photo] VARBINARY(MAX),
		[Notes] NVARCHAR(MAX),
		[CreationDate] DATETIME
			DEFAULT (GETDATE())
	)
--DROP TABLE [dbo].[Teachers]
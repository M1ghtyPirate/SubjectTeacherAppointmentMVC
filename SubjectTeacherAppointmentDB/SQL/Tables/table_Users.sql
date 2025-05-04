USE [SubjectTeacherAppointment]

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users')
	CREATE TABLE [dbo].[Users]
	(
		[UserID] UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY, 
		[Login] NVARCHAR(256) NOT NULL
			CONSTRAINT [UC_Login] UNIQUE,
		[PasswordHashed] BINARY(32) NOT NULL,
		[IsAdmin] BIT NOT NULL
			DEFAULT 0
	)
--DROP TABLE [dbo].[Users]
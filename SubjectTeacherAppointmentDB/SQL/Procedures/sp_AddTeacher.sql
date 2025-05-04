USE [SubjectTeacherAppointment]

GO
CREATE PROCEDURE [dbo].[sp_AddTeacher] (@LastName NVARCHAR(256), @Surname NVARCHAR(256), @Name NVARCHAR(256), @Birthday DATETIME, @Sex NVARCHAR(10), @Photo BINARY, @Notes NVARCHAR(MAX), @TeacherID UNIQUEIDENTIFIER OUT)
AS
BEGIN
	DECLARE @TeacherIDs TABLE ([ID] UNIQUEIDENTIFIER)

	INSERT INTO [dbo].[Teachers] ([LastName], [Surname], [Name], [Birthday], [Sex], [Photo], [Notes])
	OUTPUT inserted.[TeacherID] INTO @TeacherIDs
	VALUES (@LastName, @Surname, @Name, @Birthday, @Sex, @Photo, @Notes)

	SELECT @TeacherID = [ID] FROM @TeacherIDs
END

--DROP PROCEDURE [dbo].[sp_AddTeacher]
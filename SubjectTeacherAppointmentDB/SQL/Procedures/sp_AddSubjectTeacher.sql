USE [SubjectTeacherAppointment]

GO
CREATE PROCEDURE [dbo].[sp_AddSubjectTeacher] (@SubjectID UNIQUEIDENTIFIER, @TeacherID UNIQUEIDENTIFIER, @HoursPerWeek INT, @SubjectTeacherID UNIQUEIDENTIFIER OUT)
AS
BEGIN
	DECLARE @SubjectTeacherIDs TABLE ([ID] UNIQUEIDENTIFIER)

	INSERT INTO [dbo].[SubjectTeachers] ([SubjectID], [TeacherID], [HoursPerWeek])
	OUTPUT inserted.[SubjectTeacherID] INTO @SubjectTeacherIDs
	VALUES (@SubjectID, @TeacherID)

	SELECT @SubjectTeacherID = [ID] FROM @SubjectTeacherIDs
END

--DROP PROCEDURE [dbo].[sp_AddTeacher]
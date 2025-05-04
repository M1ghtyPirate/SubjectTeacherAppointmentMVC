USE [SubjectTeacherAppointment]

GO
CREATE PROCEDURE [dbo].[sp_AddSubject] (@SubjectName NVARCHAR(256), @AuthorID UNIQUEIDENTIFIER, @SubjectID UNIQUEIDENTIFIER OUT)
AS
BEGIN
	DECLARE @SubjectIDs TABLE ([ID] UNIQUEIDENTIFIER)

	INSERT INTO [dbo].[Subjects] ([Name])
	OUTPUT inserted.[SubjectID] INTO @SubjectIDs
	VALUES (@SubjectName)

	SELECT @@SubjectID = [ID] FROM @SubjectIDs
END

--DROP PROCEDURE [dbo].[sp_AddSubject]
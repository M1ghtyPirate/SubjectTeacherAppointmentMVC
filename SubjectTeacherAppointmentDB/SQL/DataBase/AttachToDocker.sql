IF NOT EXISTS (SELECT * FROM master.sys.databases WHERE name='SubjectTeacherAppointment')
CREATE DATABASE SubjectTeacherAppointment;

--EXEC sp_detach_db 'SubjectTeacherAppointment', 'true';
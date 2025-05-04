IF NOT EXISTS (SELECT * FROM master.sys.databases WHERE name='SubjectTeacherAppointment')
CREATE DATABASE SubjectTeacherAppointment   
    ON (FILENAME = N'/var/opt/mssql/data/SubjectTeacherAppointment.mdf'),   
    (FILENAME = N'/var/opt/mssql/data/SubjectTeacherAppointment_Log.ldf')   
    FOR ATTACH;

--EXEC sp_detach_db 'SubjectTeacherAppointment', 'true';
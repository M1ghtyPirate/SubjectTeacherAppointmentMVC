IF NOT EXISTS (SELECT * FROM master.sys.databases WHERE name='SubjectTeacherAppointment')
CREATE DATABASE SubjectTeacherAppointment
    ON (FILENAME = 'D:\git\SubjectTeacherAppointmentMVC\SubjectTeacherAppointmentDB\SQL\DataBase\SubjectTeacherAppointment.mdf'),   
    (FILENAME = 'D:\git\SubjectTeacherAppointmentMVC\SubjectTeacherAppointmentDB\SQL\DataBase\SubjectTeacherAppointment_log.ldf')   
    FOR ATTACH;

--EXEC sp_detach_db 'SubjectTeacherAppointment', 'true';
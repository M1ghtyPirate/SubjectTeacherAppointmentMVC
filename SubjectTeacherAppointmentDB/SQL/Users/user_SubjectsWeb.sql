USE [SubjectTeacherAppointment]

CREATE LOGIN [SubjectsWeb] WITH PASSWORD = 'Password1';
CREATE USER [SubjectsWeb] FOR LOGIN [SubjectsWeb]; 

GRANT SELECT TO [SubjectsWeb];

GRANT INSERT TO [SubjectsWeb];

GRANT UPDATE TO [SubjectsWeb];

GRANT DELETE TO [SubjectsWeb];

GRANT EXECUTE TO [SubjectsWeb];
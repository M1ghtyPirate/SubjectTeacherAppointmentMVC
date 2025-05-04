# Wait to be sure that SQL Server came up
sleep 30s

# Run the setup script to create the DB and the schema in the DB
# Note: make sure that your password matches what is in the Dockerfile
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d master -i /app/SQL/DataBase/AttachToDocker.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d master -i /app/SQL/Users/user_SubjectsWeb.sql

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d master -i /app/SQL/Tables/table_Subjects.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d master -i /app/SQL/Tables/table_Teachers.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d master -i /app/SQL/Tables/table_Users.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P password123! -d master -i /app/SQL/Tables/table_SubjectTeachers.sql
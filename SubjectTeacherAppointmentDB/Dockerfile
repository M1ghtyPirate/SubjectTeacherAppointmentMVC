FROM mcr.microsoft.com/mssql/server:2019-CU13-ubuntu-20.04
WORKDIR /app
COPY . .

ENV SA_PASSWORD password123!
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh
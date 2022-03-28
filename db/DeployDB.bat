@echo off

set ConfigFile=config.ini
SET DatabaseName=AMERIA_LOAN_SCORING
for /F "usebackq eol=;" %%s in ("%ConfigFile%") do set %%s

SET SQLEXEC=sqlcmd 

IF DEFINED DB_ServerName SET SQLEXEC=%SQLEXEC% -S %DB_ServerName%
IF DEFINED DB_Username SET SQLEXEC=%SQLEXEC% -U %DB_Username%
IF DEFINED DB_Password SET SQLEXEC=%SQLEXEC% -P %DB_Password%
IF DEFINED DB_Name SET SQLEXEC=%SQLEXEC% -v DatabaseName=%DB_Name%

ECHO %SQLEXEC%

IF DEFINED RecreateDB %SQLEXEC% -Q "DROP DATABASE %DB_Name%"
IF DEFINED RecreateDB %SQLEXEC% -i Database.sql

for %%G in (Common/Types/*.sql) do %SQLEXEC% -d %DB_Name% -i "Common/Types/%%G"
for %%G in (Common/Tables/*.sql) do %SQLEXEC% -d %DB_Name% -i "Common/Tables/%%G"
for %%G in (Common/Functions/*.sql) do %SQLEXEC% -d %DB_Name% -i "Common/Functions/%%G"
for %%G in (Common/Procedures/*.sql) do %SQLEXEC% -d %DB_Name% -i "Common/Procedures/%%G"
for %%G in (Common/Values/*.sql) do %SQLEXEC% -d %DB_Name% -i "Common/Values/%%G"

for %%G in (GeneralLoan/Types/*.sql) do %SQLEXEC% -d %DB_Name% -i "GeneralLoan/Types/%%G"
for %%G in (GeneralLoan/Tables/*.sql) do %SQLEXEC% -d %DB_Name% -i "GeneralLoan/Tables/%%G"
for %%G in (GeneralLoan/Functions/*.sql) do %SQLEXEC% -d %DB_Name% -i "GeneralLoan/Functions/%%G"
for %%G in (GeneralLoan/Procedures/*.sql) do %SQLEXEC% -d %DB_Name% -i "GeneralLoan/Procedures/%%G"
for %%G in (GeneralLoan/Values/*.sql) do %SQLEXEC% -d %DB_Name% -i "GeneralLoan/Values/%%G"

for %%G in (InstallationLoan/Types/*.sql) do %SQLEXEC% -d %DB_Name% -i "InstallationLoan/Types/%%G"
for %%G in (InstallationLoan/Tables/*.sql) do %SQLEXEC% -d %DB_Name% -i "InstallationLoan/Tables/%%G"
for %%G in (InstallationLoan/Functions/*.sql) do %SQLEXEC% -d %DB_Name% -i "InstallationLoan/Functions/%%G"
for %%G in (InstallationLoan/Procedures/*.sql) do %SQLEXEC% -d %DB_Name% -i "InstallationLoan/Procedures/%%G"
for %%G in (InstallationLoan/Values/*.sql) do %SQLEXEC% -d %DB_Name% -i "InstallationLoan/Values/%%G"

for %%G in (Misc.Dev/Functions/*.sql) do %SQLEXEC% -d %DB_Name% -i "Misc.Dev/Functions/%%G"
for %%G in (Misc.Dev/Procedures/*.sql) do %SQLEXEC% -d %DB_Name% -i "Misc.Dev/Procedures/%%G"
for %%G in (Misc.Dev/Values/*.sql) do %SQLEXEC% -d %DB_Name% -i "Misc.Dev/Values/%%G"
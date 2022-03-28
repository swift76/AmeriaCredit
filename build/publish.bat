@ECHO OFF
SETLOCAL
SET Configuration=%1
SET DIR=%~dp0
SET ProjectsFolder=%DIR%../backend/Scoring/
SET OutputFolder=%DIR%../pub/
IF "%Configuration%"=="" (SET Configuration=Release)

dotnet publish %ProjectsFolder%IdentityServer -c %Configuration% -f netcoreapp2.2 -o %OutputFolder%IdentityServer
dotnet publish %ProjectsFolder%IntelART.Ameria.BankRestApi -c %Configuration% -f netcoreapp2.2 -o %OutputFolder%BankRestApi
dotnet publish %ProjectsFolder%IntelART.Ameria.LoanApplicationRestApi -c %Configuration% -f netcoreapp2.2 -o %OutputFolder%LoanRestApi
dotnet publish %ProjectsFolder%IntelART.Ameria.CustomerRestApi -c %Configuration% -f netcoreapp2.2 -o %OutputFolder%CustomerRestApi
dotnet publish %ProjectsFolder%IntelART.Ameria.BankModuleWebApp -c %Configuration% -f netcoreapp2.2 -o %OutputFolder%BankWebApp
dotnet publish %ProjectsFolder%IntelART.Ameria.ShopModuleWebApp -c %Configuration% -f netcoreapp2.2 -o %OutputFolder%ShopWebApp
dotnet publish %ProjectsFolder%IntelART.Ameria.CustomerModuleWebApp -c %Configuration% -f netcoreapp2.2 -o %OutputFolder%CustomerWebApp

ENDLOCAL
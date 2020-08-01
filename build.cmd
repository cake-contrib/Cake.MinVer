@echo on
@cd %~dp0

dotnet tool restore
@if %ERRORLEVEL% neq 0 goto :eof

dotnet cake --verbosity=diagnostic %*

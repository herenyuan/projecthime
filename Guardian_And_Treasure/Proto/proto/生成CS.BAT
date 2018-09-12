@echo off
for /f "delims=" %%i in ('dir /b/a ".\*.proto"') do ..\protogen.exe -i:.\%%~ni.proto -o:..\..\ProtoCmd\%%~ni.cs
pause
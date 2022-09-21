echo off
cls
set AsmDir=c:\masm32\bin

%AsmDir%\ml.exe /c /coff main.asm

%AsmDir%\link.exe /subsystem:windows main.obj

:ScriptEnd
pause
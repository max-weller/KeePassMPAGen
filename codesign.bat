@echo off
echo Signing DLLs...

signtool sign /t http://timestamp.digicert.com /a  KeeMasterPasswordApp\bin\Debug\MasterPasswordLib.dll
signtool sign /t http://timestamp.digicert.com /a  KeeMasterPasswordApp\bin\Debug\MPAGen.dll

echo Run setup builder now...
pause

echo Signing setup...
signtool sign /t http://timestamp.digicert.com /a  KeePassMPAGenPluginSetup.exe

echo Done
pause


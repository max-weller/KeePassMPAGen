;NSIS Modern User Interface
;Basic Example Script
;Written by Joost Verburg

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "MasterPasswordApp PwGenerator for KeePass 2.x"
  OutFile "KeePassMPAGenPluginSetup.exe"

  ;Default installation folder
  InstallDir "C:\Program Files (x86)\KeePass Password Safe 2\"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\KeePassPasswordSafe2_is1" "InstallLocation"

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

!define MUI_PAGE_CUSTOMFUNCTION_LEAVE DirectoryPageLeave
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section

  SetOutPath "$INSTDIR"
  
  File KeeMasterPasswordApp\bin\Debug\MPAGen.dll
  File KeeMasterPasswordApp\bin\Debug\MasterPasswordLib.dll
  

SectionEnd

Function DirectoryPageLeave
   IfFileExists "$INSTDIR\KeePass.exe" end
   IfFileExists "$INSTDIR\..\KeePass.exe" end
   IfFileExists "$INSTDIR\..\..\KeePass.exe" end
   
   messagebox mb_iconstop "Couldn't find KeePass.exe in the selected folder. The plugin must be installed in the KeePass program directory. Please select the directory where KeePass is installed."
         abort
   end:
Functionend



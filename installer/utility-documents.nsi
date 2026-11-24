Unicode true
!include "MUI2.nsh"

!ifndef PRODUCT_VERSION
  !define PRODUCT_VERSION "1.0.0"
!endif
!ifndef PUBLISH_DIR
  !define PUBLISH_DIR "artifacts\\publish"
!endif

Name "Utility Documents"
OutFile "..\\artifacts\\UtilityDocuments-Setup-${PRODUCT_VERSION}-x64.exe"
InstallDir "$PROGRAMFILES64\Utility Documents"
InstallDirRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\UtilityDocuments" "InstallLocation"
RequestExecutionLevel admin
SetCompressor /SOLID lzma

!define UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\UtilityDocuments"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"

Section "Install"
  SetOutPath "$INSTDIR"
  File "${PUBLISH_DIR}\UtilityDocuments.exe"
  File "${PUBLISH_DIR}\placeholder.html"
  File "${PUBLISH_DIR}\icon.ico"

  CreateDirectory "$SMPROGRAMS\Utility Documents"
  CreateShortcut "$SMPROGRAMS\Utility Documents\Utility Documents.lnk" "$INSTDIR\UtilityDocuments.exe" "" "$INSTDIR\icon.ico"
  CreateShortcut "$DESKTOP\Utility Documents.lnk" "$INSTDIR\UtilityDocuments.exe" "" "$INSTDIR\icon.ico"

  WriteUninstaller "$INSTDIR\Uninstall.exe"
  WriteRegStr HKLM "${UNINST_KEY}" "DisplayName" "Utility Documents"
  WriteRegStr HKLM "${UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr HKLM "${UNINST_KEY}" "Publisher" "herotux"
  WriteRegStr HKLM "${UNINST_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "${UNINST_KEY}" "UninstallString" '"$INSTDIR\Uninstall.exe"'
  WriteRegDWORD HKLM "${UNINST_KEY}" "NoModify" 1
  WriteRegDWORD HKLM "${UNINST_KEY}" "NoRepair" 1
SectionEnd

Section "Uninstall"
  Delete "$SMPROGRAMS\Utility Documents\Utility Documents.lnk"
  RMDir "$SMPROGRAMS\Utility Documents"
  Delete "$DESKTOP\Utility Documents.lnk"
  Delete "$INSTDIR\UtilityDocuments.exe"
  Delete "$INSTDIR\placeholder.html"
  Delete "$INSTDIR\icon.ico"
  Delete "$INSTDIR\Uninstall.exe"
  RMDir "$INSTDIR"
  DeleteRegKey HKLM "${UNINST_KEY}"
SectionEnd

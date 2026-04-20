@echo off
set GODOT_PID=%1
set ADDON_DIR=%2
set STAGING_DIR=%3
set OLD_DIR=%4
set GODOT_BIN=%5
set PROJECT_DIR=%6
set SCRIPT_PATH=%7
set TEMP_ZIP=%~8

:wait
tasklist /fi "PID eq %GODOT_PID%" 2>nul | find "%GODOT_PID%" >nul && (timeout /t 1 /nobreak >nul & goto wait)

move "%ADDON_DIR%" "%OLD_DIR%"
move "%STAGING_DIR%\addons\ziva_agent" "%ADDON_DIR%"
if errorlevel 1 (move "%OLD_DIR%" "%ADDON_DIR%") else (rmdir /s /q "%OLD_DIR%")

rmdir /s /q "%STAGING_DIR%"
if exist "%TEMP_ZIP%" del "%TEMP_ZIP%"
start "" "%GODOT_BIN%" -e --path "%PROJECT_DIR%"
del "%SCRIPT_PATH%"

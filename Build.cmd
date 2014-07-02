@echo off
set msbuild="C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe"

%msbuild% Build.msbuild /t:Full /maxcpucount /verbosity:normal /flp:verbosity=normal;logfile=Build.log /p:Version=0.0.0.1;Configurations=Debug;Platforms=x86;SkipTests=False
if not %ERRORLEVEL%==0 goto build_failed
goto build_succeeded

:build_failed
pause
exit /b 1

:build_succeeded
exit /b 0

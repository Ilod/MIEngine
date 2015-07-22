@echo off
setlocal

REM Copyright (c) Microsoft. All rights reserved.
REM Licensed under the MIT license. See LICENSE file in the project root for full license information.

if "%~4"=="" goto help
if NOT "%~5"=="" goto help
goto Run
:Help
echo syntax: ValidateSerializer.cmd ^<path_to_input_assembly^> ^<path_to_designer_file^> ^<intermediate_directory^> ^<sgen_path^>
echo.
echo ValidateSerializer.cmd will use sgen.exe to generate a serializer for the 
echo given input assembly, it will then strip out the parts of this which should be 
echo removed and compare it to the baseline designer file. Updating the baseline if 
echo there are meaningful differences.
echo.
exit /b -1

:Run
if exist "%~3\ValidateSerializer" rmdir /s /q "%~3\ValidateSerializer"
if exist "%~3\ValidateSerializer" echo ERROR: Unable to remove "%~3\ValidateSerializer". >&2 & exit /b -1

mkdir "%~3\ValidateSerializer"
if NOT %ERRORLEVEL%==0 echo ERROR: Failed to create '%~3\ValidateSerializer' directory. >&2 & exit /b -1

echo %4 /compiler:/nowarn: /assembly:%1 /k /out:"%~3\ValidateSerializer"
%4 /assembly:%1 /k /out:"%~3\ValidateSerializer"
if NOT %ERRORLEVEL%==0 echo ERROR: sgen failed with error %ERRORLEVEL%. >&2 & exit /b -1

pushd "%~3\ValidateSerializer"
set SerializerFile=
set Error=
for %%f in (*.cs) do call :ProcessFile %%f
if "%SerializerFile%"=="" echo ERROR: sgen did not create a .cs file >&2 & exit /b -1
if NOT "%Error%"=="" exit /b -1

echo // Copyright (c) Microsoft. All rights reserved.> generated-serializer.cs
echo // Licensed under the MIT license. See LICENSE file in the project root for full license information.>> generated-serializer.cs
echo.>> generated-serializer.cs
echo //------------------------------------------------------------------------------ >> generated-serializer.cs
echo // ^<auto-generated^> >> generated-serializer.cs
echo //     This code was generated by a tool. >> generated-serializer.cs
echo //     Changes to this file may cause incorrect behavior and will be lost if >> generated-serializer.cs
echo //     the code is regenerated. >> generated-serializer.cs
echo // ^</auto-generated^> >> generated-serializer.cs
echo //------------------------------------------------------------------------------ >> generated-serializer.cs
echo.>> generated-serializer.cs
echo #pragma warning disable 219>> generated-serializer.cs
echo.>> generated-serializer.cs
findstr /v /r /c:"^\[assembly\:" %SerializerFile% >> generated-serializer.cs
popd

call "%~dp0ValidateDesignerFile.cmd" %2 "%~3\ValidateSerializer\generated-serializer.cs" %3
exit /b %ERRORLEVEL%

:ProcessFile
    if NOT "%SerializerFile%"=="" echo ERROR: sgen created multiple .cs files (%CD%\%SerializerFile% and %CD%\%1). >&2 & set Error= & goto eof
    set SerializerFile=%1

:eof
: # This is a special script which intermixes both sh
: # and cmd code. It is written this way because it is
: # used in system() shell-outs directly in otherwise
: # portable code. See http://stackoverflow.com/questions/17510688
: # for details.
: #
: # This is particularly interesting for Languages and compilers as the script
: # is an example of a program that is syntactically correct in two languages!
: #
:<<"::ENDOFWINDOWS"
@ECHO off
REM Batch file to Run the Compiler testing Makefile
REM Brian Tompsett Sep 2013 
REM
:: Usage
:: RunCompiler <source directory> ["BATCH=yes"] ["SOURCE=name"] ["TESTS=test list"]
:: defaults <current directory>
:: 
::  When "BATCH=yes" is specified, the script does not pause for input
::  When "SOURCE=name" is specified the scripts looks for files called name.l name.y and name.c
::  When "TESTS=test list" the compiler is only run of the tests named in the list.
::
:: History:
::    Ver 1 : Sep 2013 Cloned from 08241 test scripts
::        2 : Dec 2014 Made multi-platform
::          : Oct 2015 Fixed Visual Studio Compiler function
:: ===============================================================================
::
:: We can check how the script was called %cmdcmdline% can be the null string 
:: or sometime "C: (unpaired quote!)
:: or the string  cmd /c ""T:\lab1test.bat" "
:: We have to discover which
:: There is magic space at the end of the following line to remove a null string
set invoker=%cmdcmdline% 
:: Now remove any troublesome quotes
REM %invoker%
set invoker=%invoker:"=`%
REM %invoker%
:: Here we must quote the line as it might contain meta characters
IF "%invoker:~0,3%" == "cmd" GOTO :Clicked
:: For some reason, sometime the full path is used - and the case varies
IF /I "%invoker:~0,27%" == "C:\WINDOWS\system32\cmd.exe" GOTO :Clicked
GOTO :DOSLaunch
:Clicked
@color 4e
echo Launched from Windows: %CD%
echo.
echo This will make basic tests of the compiler for 08348 ACW
echo.
echo.
echo.
echo.
echo.
echo.
echo Normally %~n0 is called from the Command Prompt 
echo                    in the directory of the compiler location...
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo Type the full path to the compiler sources on the next line...
set /P Ans=[or hit enter to close] : 
if "%Ans%" == "" EXIT /b 1
set cmdcmdline= 
Call "%~0" "%Ans%"
PAUSE
EXIT /b
:DOSLaunch
echo Starting tests %~n0
:: -------------------------------- Variable Initialisation --------
REM set standard strings
set TESTNAME=initialisation
::
:: --------------------------------  Argument Processing --------------
::
REM
::
:: --------------------------------  End Argument Processing --------------
::
:: Check for gcc?

set OBJECTFLAG=-o
set LINKERFLAG=-lfl
set CC=gcc
set gccpath=
CALL :which gcc gccpath
IF NOT "%gccpath%x"=="x" GOTO :HaveGCC
echo GCC Not found, looking for visual studio...
:: get visual studio environment
if EXIST "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" GOTO :VS12CS
if EXIST "C:\Program Files\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" GOTO :VS12
if EXIST "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" GOTO :VS11CS
if EXIST "C:\Program Files\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" GOTO :VS11
if EXIST "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" GOTO :VS10CS
if EXIST "C:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" GOTO :VS10
if EXIST "C:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat" GOTO :VS9
echo Unable to find Visual Studio ...
GOTO :Failure
:VS9
call ^"C:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat^" x86 >nul <nul
GOTO :VS
:VS10
call ^"C:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat^" x86 >nul <nul
set theValue=
for /f "delims=" %%a in ('where csc 2^>nul') do @set theValue=%%a 
if NOT "%theValue%."=="." GOTO :VS
echo *** Stupid machine in Fenner has a setup error, but I fixed it! ****
set CC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\cl
set compile=%CC% /Fe:%TEMP%\program.exe
GOTO :CompilerReady
GOTO :VS
:VS10CS
call ^"C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat^" x86 >nul <nul
set theValue=
for /f "delims=" %%a in ('where csc 2^>nul') do @set theValue=%%a 
if NOT "%theValue%."=="." GOTO :VS
echo *** Stupid machine in RB-321 has a setup error, but I fixed it! ****
set CC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\cl
set compile=%CC% /Fe:%TEMP%\program.exe

GOTO :CompilerReady
:VS11
call ^"C:\Program Files\Microsoft Visual Studio 11.0\VC\vcvarsall.bat^" x86 >nul <nul
GOTO :VS
:VS11CS
call ^"C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat^" x86 >nul <nul
GOTO :VS
:VS12
call ^"C:\Program Files\Microsoft Visual Studio 12.0\VC\vcvarsall.bat^" x86 >nul <nul
GOTO :VS
:VS12CS
call ^"C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat^" x86 >nul <nul
set theValue=
for /f "delims=" %%a in ('where cl 2^>nul') do @set theValue=%%a 
if NOT "%theValue%."=="." GOTO :VS
echo *** Stupid machine in RB-321 has a setup error, but I fixed it! ****
set CC="C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\bin\cl"
set compile=%CC% /Fe:%TEMP%\program.exe 
set LIBPATH="C:\Windows\Microsoft.NET\Framework\v4.0.30319";"C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\lib";"C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x86";%LIBPATH%
set INCLUDE="C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\include"
set _CL_=/link "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\lib\LIBCMT.lib" "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\lib\OLDNAMES.lib" "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x86\kernel32.lib"
:: set CL=/Fo%TEMP%
GOTO :CompilerReady
:VS
set CC=cl

set compile=%CC% /Fe:%TEMP%\program.exe
:CompilerReady
set CL=/nologo -DYY_MAIN
set OBJECTFLAG=/Fe:
set LINKERFLAG=/link C:\GnuWin32\lib\libfl.a
if NOT EXIST C:\GnuWin32\lib\libfl.a SET LINKERFLAG=/link T:\GnuWin32\lib\libfl.a

:HaveGCC

:: look for nmake buried in visual studio
:: Paths courtesy of http://bojan-komazec.blogspot.co.uk/2011/10/nmake-and-its-environment.html
::
set MAKE="C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\bin\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files\Microsoft Visual Studio 12.0\VC\bin\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\bin\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files\Microsoft Visual Studio 11.0\VC\bin\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\bin\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files\Microsoft Visual Studio 10.0\VC\bin\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files\Microsoft Visual Studio 9.0\VC\bin\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files\Microsoft Visual Studio 10.0\VC\bin\amd64\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\Program Files\Microsoft Visual Studio 9.0\VC\bin\amd64\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\WinDDK\6001.18001\bin\x86\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\WinDDK\6001.18001\bin\ia64\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
:: Now look for standalone nmake
set MAKE="C:\dmake\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
set MAKE="C:\nmake\nmake.exe"
if EXIST %MAKE% GOTO :HaveMake
echo **** Nmake program not found. You can get it from http://guest.engelschall.com/~sb/download/win32/ ****
GOTO :Failure
:HaveMake

:: Now check for Makefile
IF EXIST "T:\" GOTO :GOTDIR
SET myhome=%~dp0
:: Chop off trailing backslash in subst
SUBST T: "%myhome:~0,-1%"
:GOTDIR
IF "%~1" == "clean" IF "%~2" == "" GOTO :CleanUp
:: Can not subst an equal - needs a for loop workaround:
:: http://www.experts-exchange.com/Programming/Languages/Scripting/Shell/Batch/Q_26868786.html
set _one=%~1
set _two=%~1
for /f "tokens=1* delims==" %%a in ("%_one%") do (
  if "%%b"=="" (
     set %_one%=%%a
	 ) else (
	 set %_one%=%%aX%%b
	 )
)
IF "%_two%" == "%_one%" GOTO :NOOPT
echo Parameter 1 is a directory name, not an option
GOTO :Failure
:CleanUp
CALL %MAKE% -f T:Makefile /NOLOGO clean
GOTO :Complete

:NOOPT
set OCD=%CD%
IF "%~1"=="" GOTO :NODIR
IF %1==""  GOTO :NODIR
IF EXIST %1 GOTO :Found
echo ***** Source directory does not exist ******
GOTO :Failure
:Found
CD /d %1
:NODIR

:: Quick check for bison path
set BISON="C:\GnuWin32\bin\bison.exe" 
set BISONPATH=C:\GnuWin32\bin\
if EXIST %BISON% GOTO :GotBison
set BISON="C:\Program Files (x86)\GnuWin32\bin\bison.exe"
set BISONPATH=
if NOT EXIST %BISON% GOTO :GotBison
echo Bison cannot be installed in "Program Files" - no space in directory permitted
:GotBison
set theValue=
for /f "delims=" %%a in ('where bison 2^>nul') do @set theValue=%%a 
if NOT "%theValue%."=="." GOTO :BisonPath
:: Look for bison in some places
echo Bison not on the command prompt path
:: Fix the Path
IF "%BISONPATH%"=="" GOTO :NoBison
SET PATH=%BISONPATH%;%PATH%
GOTO :StartTests
:NoBison
:: Look for bison in the hidden place
IF NOT EXIST "T:\GnuWin32\bin\bison.exe" GOTO :BisonPath
SET BISON="T:\GnuWin32\bin\bison.exe"
set FLEX="T:\GnuWin32\bin\flex.exe"
set FLEXLIB=T:\GnuWin32\lib
IF NOT "%LINKERFLAG%" == "-lfl" SET LINKERFLAG=/link T:\GnuWin32\lib\libfl.a
SET BISONPATH=T:\GnuWin32\bin
SET PATH=%BISONPATH%;%PATH%
GOTO :StartTests
:BisonPath
set BISON=bison

:: Start tests
:StartTests
:: The %CC% variable may contain a quoted path to the compiler, so the quotes need escaping
CALL %MAKE% -f T:Makefile /NOLOGO "CC=%CC:"=\"%" "OBJECTFLAG=%OBJECTFLAG%" "LINKERFLAG=%LINKERFLAG:"=\"%" %2 %3 %4 %5 %6 %7 %8 %9
:: Special code to check for Makefile abort - output error files
IF EXIST "%TEMP%\lex-errors.txt" (
	echo -- FLEX Errors -------------------------------- 
	TYPE "%TEMP%\lex-errors.txt" 
	DEL "%TEMP%\lex-errors.txt"
)
IF EXIST "%TEMP%\lexer-errors.txt" (
	echo -- GCC -DPRINT Errors -------------------------------- 
	TYPE "%TEMP%\lexer-errors.txt" 
	DEL "%TEMP%\lexer-errors.txt"
)
IF EXIST "%TEMP%\debug-errors.txt" (
	echo -- BISON -t Errors -------------------------------- 
	TYPE "%TEMP%\debug-errors.txt" 
	DEL "%TEMP%\debug-errors.txt"
)
IF EXIST "%TEMP%\parser-errors.txt" (
	echo -- GCC -DYYDEBUG Errors -------------------------------- 
	TYPE "%TEMP%\parser-errors.txt" 
	DEL "%TEMP%\parser-errors.txt"
)
IF EXIST "%TEMP%\tree-errors.txt" (
	echo -- GCC -DDEBUG Errors -------------------------------- 
	TYPE "%TEMP%\tree-errors.txt" 
	DEL "%TEMP%\tree-errors.txt"
)
IF EXIST "%TEMP%\compiler-errors.txt" (
	echo -- GCC Errors -------------------------------- 
	TYPE "%TEMP%\compiler-errors.txt" 
	DEL "%TEMP%\compiler-errors.txt"
)
CD /d "%OCD%"

GOTO :Complete

:: -------------------------  Subroutines  -------------------------
:: Source: http://www.dostips.com/DtTutoFunctions.php

:: ===========================================================================
::
:which
setlocal enableextensions enabledelayedexpansion
:: Needs an argument.

if "x%1"=="x" (
    echo Usage: which ^<progName^>
    goto :endsub
)

:: First try the unadorned filenmame.

set pathis=
set fullspec=
call :find_it %1 pathis

:: Then try all adorned filenames in order.

set mypathext=!pathext!
:loop1
    :: Stop if found or out of extensions.

    if "x!mypathext!"=="x" goto :loop1end

    :: Get the next extension and try it.

    for /f "delims=;" %%j in ("!mypathext!") do set myext=%%j
    call :find_it %1!myext! pathis

:: Remove the extension (not overly efficient but it works).

:loop2
    if not "x!myext!"=="x" (
        set myext=!myext:~1!
        set mypathext=!mypathext:~1!
        goto :loop2
    )
    if not "x!mypathext!"=="x" set mypathext=!mypathext:~1!

    goto :loop1
:loop1end

:endsub
  ( endlocal
   set %2=%pathis%
  )
goto :End

:: Function to find and print a file in the path.
::
:: http://stackoverflow.com/questions/304319/is-there-an-equivalent-of-which-on-the-windows-command-line
::
:find_it
    for %%i in (%1) do set fullspec=%%~$PATH:i
      if not "x!fullspec!"=="x" set %2=!fullspec!
    goto :End

:: --------------- Standard tidy up tasks --------------
:Complete
:: echo All test completed: %passCount% passed, %nonfatalCount% nonfatal, %failCount% failed.
IF "%failCount%"=="" GOTO :End
IF /I %failCount% GTR 0 GOTO :Fatal
GOTO :End
:Failure
:: Tidy missed tempfiles when aborted
set /a failCount=%failCount% + 1
echo Test failed at %TESTNAME% with %ERRORLEVEL%
echo Output: %theValue%
echo All test completed: %passCount% passed, %nonfatalCount% nonfatal, %failCount% failed.
:: Exit the batch file when fatal - do not return from subroutine
:Fatal
exit /b 1
:End
exit /b
::ENDOFWINDOWS
# Batch file to Run the Compiler testing Makefile on a bash shell
# Brian Tompsett Nov 2014
#
# Usage
# RunCompiler <source directory> ["BATCH=yes"] ["SOURCE=name"] ["TESTS=test list"]
# defaults <current directory>
# 
#  When "BATCH=yes" is specified, the script does not pause for input
#  When "SOURCE=name" is specified the scripts looks for files called name.l name.y and name.c
#  When "TESTS=test list" the compiler is only run of the tests named in the list.
#
# History:
#    Ver 1 : Nov 2014 Cloned from DOS/Windows test scripts
#
# --------------------------------------------------
# First find the directory with the test scripts and test programs
#  They could be mounted in a OS/X or cygwin format
#
# http://stackoverflow.com/questions/59895/can-a-bash-script-tell-what-directory-its-stored-in
#
# The file is used for both bash and windows and has windowss line breaks of \r\n
# Each line ends in a comments to protect the \r character which bash does not like
# Some of the lines are very long because we can't have \r in continuations in bash
#
SOURCE="${BASH_SOURCE[0]}" #
while [ -h "$SOURCE" ]; do # resolve $SOURCE until the file is no longer a symlink
  DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )" #
  SOURCE="$(readlink "$SOURCE")" #
  [[ $SOURCE != /* ]] && SOURCE="$DIR/$SOURCE" # if $SOURCE was a relative symlink, we need to resolve it relative to the path where the symlink file was located
done #
DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )" #
SystemName="$(uname)" #
if [ "$SystemName" == "Darwin" ] #
then #
   # We are on a Mac
   TESTLOC=$DIR #
   LEXLIB=l #
elif [ "$(expr substr $SystemName 1 5)" == "Linux" ] #
then #
   # We are on linux
   TESTLOC=$DIR #
   LEXLIB=fl #
elif [ "$(expr substr $SystemName 1 10)" == "MINGW32_NT" ] #
then #
   # We are on MinGW
   TESTLOC=$DIR #
   LEXLIB=fl #
elif [ "$(expr substr $(uname -s) 1 6)" == "CYGWIN" ] #
then #
   # We are on cygwin
   TESTLOC=$DIR #
   LEXLIB=fl #
elif [ -n "$COMSPEC" -a -x "$COMPSEC" ] #
then #
   # We are on cygwin under Windows
   TESTLOC=$DIR #
   LEXLIB=fl #
else #
   # Not sure
   echo Unidentified system $(uname -s) #
   exit #
fi #
if [ ! -d "$TESTLOC" ] #
then #
    echo Expected test location "$TESTLOC" does not exist
    exit #
fi #
# Now check we can see the Makefile
if [ ! -r "$TESTLOC/Makefile" ] #
then #
   echo Makefile cannot be found
   exit #
fi #
if [ "$1" != "" ] #
then  #
# Now check for the directory in $1
if [ ! -d "$1" ] #
then #
  echo Directory $1 not found
  exit #
fi #
pushd $1 #
fi #
# Removes spaces in paths for make.....
echo Tests are located in $TESTLOC
# Now convert the MS nmake syntax to Gnu make syntax
#  Careful: Gnu sed has extensions. This is not gnu
sed -e 's/!IFDEF/ifdef/' -e 's/!ENDIF/endif/' -e 's/!IF \(.*\)==\(.*\)/ifeq \1 \2/' -e 's/!IF \(.*\)!=\(.*\)/ifneq \1 \2/' -e 's/!MESSAGE/#message/' -e 's/!IFNDEF/ifndef/' -e 's/!ELSE/else/' -e 's/\$(TESTS:\(.*\)=\(.*\))/$(subst \1,\2,$(TESTS))/' -e 's/\([_A-Z][_A-Z]*\)=\$(\1)/\1 +=/g' -e 's/!IF EXIST *("\(.*\)")/ifneq ("$(wildcard \1))","")/' -e 's/!UNDEF/undefine/' "$TESTLOC/Makefile" |  make -f - "TESTLOC=${TESTLOC/ /?}" "COMSPEC=" "LEXLIB=$LEXLIB" $2 $3 $4 $5 $6 $7 $8 #
#   In the case of make abort we need to output some error files	 
if [ -f "/tmp/lex-errors.txt" ] #
 then #
	echo "-- FLEX Errors --------------------------------" #
	cat "/tmp/lex-errors.txt" #
	rm -f "/tmp/lex-errors.txt" #
fi #
if [ -f "/tmp/lexer-errors.txt" ] #
 then #
	echo "-- $(CC) -DPRINT Errors --------------------------------" #
	cat "/tmp/lexer-errors.txt" #
	rm -f "/tmp/lexer-errors.txt" #
fi #
if [ -f "/tmp/debug-errors.txt" ] #
 then #
	echo "-- BISON -t Errors --------------------------------" #
	cat "/tmp/debug-errors.txt" #
	rm -f "/tmp/debug-errors.txt" #
fi #
if [ -f "/tmp/parser-errors.txt" ] #
 then #
	echo "-- GCC -DYYDEBUG Errors --------------------------------" #
	cat "/tmp/parser-errors.txt" #
	rm -f "/tmp/parser-errors.txt" #
fi #
if [ -f "/tmp/tree-errors.txt" ] #
 then #
	echo "-- GCC -DDEBUG Errors --------------------------------" #
	cat "/tmp/tree-errors.txt" #
	rm -f "/tmp/tree-errors.txt" #
fi #
if [ -f "/tmp/compiler-errors.txt" ] #
 then #
	echo "-- GCC Errors --------------------------------" #
	cat "/tmp/compiler-errors.txt" #
	rm -f "/tmp/compiler-errors.txt" #
fi #
test "$1" != "" && popd #
exit #
exit #
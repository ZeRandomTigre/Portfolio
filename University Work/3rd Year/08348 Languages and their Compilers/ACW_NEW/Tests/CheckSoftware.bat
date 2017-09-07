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
@echo off
REM Batch file to test that the software for compilers is installed OK
REM Brian Tompsett Oct 2014 
REM
:: Usage
:: CheckSoftware <source directory> ["BATCH=yes"]
:: defaults <current directory>
:: 
::  When "BATCH=yes" is specified, the script does not pause for input
::
:: History:
::    Ver 1 : Oct 2014 Cloned from RunCompiler script
::          : Aug 2015 Fixed bug in path names
::          : Oct 2015 Fixed to work with Visual Studio
:: ``````````````````````````````````````````````````````````````````
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
IF /I "%invoker:~0,3%" == "cmd" GOTO :Clicked
:: For some reason, sometime the full path is used - and the case varies
IF /I "%invoker:~0,27%" == "C:\WINDOWS\system32\cmd.exe" GOTO :Clicked
GOTO :DOSLaunch
:Clicked
@color 4e
echo Launched from Windows: %CD%
echo.
echo This will make basic tests of the software installation for 08348 
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
:DOSLaunch
echo Starting tests %~n0
:: -------------------------------- Variable Initialisation --------
REM set standard strings
set TESTNAME=initialisation
set failCount=0
set passCount=0
set nonfatalCount=0
::
:: --------------------------------  Argument Processing --------------
::
REM
::
:: --------------------------------  End Argument Processing --------------
::
:: change to a safe location - in case directory is read only etc
:CheckSource
:: Now check for Source directory
SET myhome=%~dp0
:: Use B: as T: may be already in use
:: Chop off trailing backslash in subst, as causes problems on Subst
SUBST B: "%myhome:~0,-1%"
:GOTDIR
set OCD=%CD%
set ORIGIN=B:
cd /d "%TEMP%"
IF "%1"=="" GOTO :NODIR
IF %1==""  GOTO :NODIR
IF EXIST %1 GOTO :Found
echo ***** Source directory does not exist ******
GOTO :Failure
:Found
CD /d %1
set ORIGIN=%1
:NODIR


:: Now check for flex and bison availability
set FLEX="C:\GnuWin32\bin\flex.exe" 
set FLEXLIB=C:\GnuWin32\lib
if EXIST %FLEX% GOTO :GotFlex
set FLEX="C:\Program Files (x86)\GnuWin32\bin\flex.exe" 
if NOT EXIST %FLEX% GOTO :GotFlex
echo Flex cannot be installed in "Program Files" - no space in directory permitted
set /a failCount += 1
:GotFlex

set theValue=
for /f "delims=" %%a in ('where flex 2^>nul') do @set theValue=%%a 
if NOT "%theValue%."=="." GOTO :FlexPath
:: Look for flex in some places
echo Flex not on the command prompt path
set /a failCount += 1
GOTO :DoneFlex
:FlexPath
set FLEX=flex
:DoneFlex
IF EXIST "%FLEXLIB%\libfl.a" GOTO :FindBison
set FLEXLIB=B:\GnuWin32\lib

:FindBison
set BISON="C:\GnuWin32\bin\bison.exe" 
set BISONPATH=C:\GnuWin32\bin\
if EXIST %BISON% GOTO :GotBison
set BISON="C:\Program Files (x86)\GnuWin32\bin\bison.exe"
set BISONPATH=
if NOT EXIST %BISON% GOTO :GotBison
echo Bison cannot be installed in "Program Files" - no space in directory permitted
set /a failCount += 1
:GotBison
set theValue=
for /f "delims=" %%a in ('where bison 2^>nul') do @set theValue=%%a 
if NOT "%theValue%."=="." GOTO :BisonPath
:NoBisonPath
:: Look for bison in some places
echo Bison not on the command prompt path
set /a failCount += 1
:: Fix the Path - If we have a bison
IF "%BISONPATH%"=="" GOTO :NoBison
echo bison found in %BISONPATH%
SET PATH=%BISONPATH%;%PATH%
GOTO :CheckBison
:BisonPath
set BISON=bison
:CheckBison
:: We have a bison, so we need to invoke it to ensure the libraries are found
:: echo %BISON% "%ORIGIN%\arith.y"
:: pause
%BISON% "%ORIGIN%\arith.y"  > "%TEMP%\bisonout.txt" 2>&1
:: more %TEMP%\bisonout.txt
:: pause
@echo off
IF ERRORLEVEL 1 GOTO :BisonFault
IF NOT EXIST arith.tab.c GOTO :BisonFault
DEL arith.tab.c
GOTO :BisonDone
:NoBison
:: Look for bison in the hidden place
IF NOT EXIST "%ORIGIN%\GnuWin32\bin\bison.exe" GOTO :BisonDone
SET BISON="%ORIGIN%\GnuWin32\bin\bison.exe"
set FLEX="%ORIGIN%\GnuWin32\bin\flex.exe"
set FLEXLIB=%ORIGIN%\GnuWin32\lib
SET BISONPATH=%ORIGIN%\GnuWin32\bin
SET PATH=%BISONPATH%;%PATH%
set /a nonfatalCount += 1
GOTO :CheckBison
:BisonFault
echo Bison is installed but does not operate correctly
set /a failCount += 1
:BisonDone
:: Check for gcc or other C compiler ?
set OBJECTFLAG=-o
set LINKERFLAG=-lfl
set CC=gcc
set gccpath=
CALL :which gcc gccpath
IF NOT "%gccpath%x"=="x" GOTO :HaveGCC
echo GCC Not found, looking for visual studio...
set /a nonfatalCount  =+ 1
:: get visual studio environment
if EXIST "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" GOTO :VS12CS
if EXIST "C:\Program Files\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" GOTO :VS12
if EXIST "C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" GOTO :VS11CS
if EXIST "C:\Program Files\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" GOTO :VS11
if EXIST "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" GOTO :VS10CS
if EXIST "C:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" GOTO :VS10
if EXIST "C:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat" GOTO :VS9
echo Unable to find any Visual Studio..
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
set CL=/nologo
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
set CL=/nologo
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
set CL=/nologo %CL%
set compile=%CC% /Fe%TEMP%\program.exe 
:CompilerReady
set OBJECTFLAG=/Fe

set LINKERFLAG=/link "%FLEXLIB%\libfl.a"

:HaveGCC
:: Check that GCC runs on "Hello World"
echo #include ^<stdio.h^> > %TEMP%\hello.c
echo int main(void) {printf("Hello World\n");} >> %TEMP%\hello.c
%CC% "%OBJECTFLAG%%TEMP%\hello.exe" %TEMP%\hello.c %LINKERFLAG%
if exist "%TEMP%\hello.c" DEL "%TEMP%\hello.c"
if exist "%TEMP%\hello.obj" DEL "%TEMP%\hello.obj"
if exist "%TEMP%\hello.exe" GOTO :TestCompile
echo C Compiler not functioning
set /a failCount += 1
GOTO :Complete
:TestCompile
for /f tokens^=*^ delims^=^ eol^= %%a in ('call "%TEMP%\hello.exe" ^< nul 2^>^&1') do (set theValue=%%a)
if "%theValue%" == "Hello World" GOTO :TestedBin
echo C compiler not correct
SET /a failCount += 1
:TestedBin
DEL %TEMP%\hello.exe
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


:: Start tests
cd /d %TEMP%
:: Create a Makefile for the calculator
echo SOURCE = arith > "%TEMP%\Makefile"
echo LEX = %FLEX%  >> "%TEMP%\Makefile"
echo YACC = %BISON%  >> "%TEMP%\Makefile"
echo CC = gcc  >> "%TEMP%\Makefile"
echo TYPE = type  >> "%TEMP%\Makefile"
echo RENAME = rename  >> "%TEMP%\Makefile"
echo DEL = DEL  >> "%TEMP%\Makefile"
echo CFLAGS =   >> "%TEMP%\Makefile"
echo OBJECTFLAG = -o  >> "%TEMP%\Makefile" 
echo LINKERFLAG = -lfl >> "%TEMP%\Makefile" 
echo ORIGIN=%ORIGIN%  >> "%TEMP%\Makefile"
echo all: $(TEMP)\$(SOURCE).exe >> "%TEMP%\Makefile"
echo lex.yy.c : "$(ORIGIN)\$(SOURCE).l" >> "%TEMP%\Makefile"
echo     $(LEX) "$(ORIGIN)\$(SOURCE).l" >> "%TEMP%\Makefile"
echo $(SOURCE).tab.c: "$(ORIGIN)\$(SOURCE).y" lex.yy.c >> "%TEMP%\Makefile"
echo        $(YACC) "$(ORIGIN)\$(SOURCE).y" >> "%TEMP%\Makefile"
echo $(TEMP)\$(SOURCE).exe: $(SOURCE).tab.c "$(ORIGIN)\$(SOURCE).c" >> "%TEMP%\Makefile"
echo       $(CC) $(OBJECTFLAG)$@ "$(ORIGIN)\$(SOURCE).c" $(SOURCE).tab.c $(CFLAGS) $(LINKERFLAG) >> "%TEMP%\Makefile"
echo       $(DEL) lex.yy.c $(SOURCE).tab.c >> "%TEMP%\Makefile"
:: type "%TEMP%\Makefile"
:: echo %MAKE% -f "%TEMP%\Makefile" /NOLOGO "CC=%CC:"=\"%" "OBJECTFLAG=%OBJECTFLAG%" "LINKERFLAG=%LINKERFLAG:"=\"%" %2 %3 %4 %5 %6 %7 %8 %9
:: The %CC% variable may contain a quoted path to the compiler, so the quotes need escaping
CALL %MAKE% -f "%TEMP%\Makefile" /NOLOGO "CC=%CC:"=\"%" "OBJECTFLAG=%OBJECTFLAG%" "LINKERFLAG=%LINKERFLAG:"=\"%" %2 %3 %4 %5 %6 %7 %8 %9
IF ERRORLEVEL 1 SET /a failCount += 1
IF EXIST "%TEMP%\Makefile" DEL "%TEMP%\Makefile"
IF EXIST "%TEMP%\arith.exe" GOTO :Built
echo Failed to make program
SET /a failCount += 1
GOTO :Complete
:Built
echo 1+2 > "%TEMP%\plus.data"

for /f tokens^=*^ delims^=^ eol^= %%a in ('call "%TEMP%\arith" ^< "%TEMP%\plus.data" 2^>^&1') do (set theValue=%%a)
if NOT "%theValue%" == "value : 3" SET /a failCount += 1
IF EXIST "%TEMP%\plus.data" DEL "%TEMP%\plus.data"
echo 3*2 > "%TEMP%\mult.data"
for /f tokens^=*^ delims^=^ eol^= %%a in ('call "%TEMP%\arith" ^< "%TEMP%\mult.data" 2^>^&1') do (set theValue=%%a)
if NOT "%theValue%" == "value : 6" SET /a failCount += 1
IF EXIST "%TEMP%\mult.data" DEL "%TEMP%\mult.data"
IF EXIST "%TEMP%\arith" DEL "%TEMP%\arith"



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
IF NOT "%OCD%" == "" CD /d "%OCD%"
SUBST B: /D
echo All test completed: %passCount% passed, %nonfatalCount% nonfatal, %failCount% failed.
IF "%failCount%"=="" GOTO :DoneIt
IF /I %failCount% GTR 0 GOTO :Fatal
IF "%nonfatalCount%"=="" GOTO :DoneIt
IF /I %nonfatalCount% GTR 0 GOTO :Fatal
echo Looks like the Software is installed OK
IF /I "%invoker:~0,3%" == "cmd" GOTO :ThePause
:: For some reason, sometime the full path is used - and the case varies
IF /I "%invoker:~0,27%" == "C:\WINDOWS\system32\cmd.exe" GOTO :ThePause
GOTO :DoneIt
:ThePause
PAUSE
:DoneIt
exit /b

:Failure
:: Tidy missed tempfiles when aborted
set /a failCount=%failCount% + 1
echo Test failed at %TESTNAME% with %ERRORLEVEL%
echo Output: %theValue%
echo All test completed: %passCount% passed, %nonfatalCount% nonfatal, %failCount% failed.
:: Exit the batch file when fatal - do not return from subroutine
:Fatal
IF NOT "%OCD%" == "" CD /d "%OCD%"
:: set to a blank
set aliased= 
:: Check if alias exists - delete if it does, so new one replaces old
for /f "tokens=2 delims==" %%i in ('subst ^| find "B:" ') do set aliased=%%i
if "%aliased:~2%" == "" GOTO :NoAlias
:: echo Deleting old mapping of T: to %aliased:~2%
SUBST B: /D
:NoAlias
IF /I "%invoker:~0,3%" == "cmd" GOTO :apause
:: For some reason, sometime the full path is used - and the case varies
IF /I "%invoker:~0,27%" == "C:\WINDOWS\system32\cmd.exe" GOTO :apause
GOTO :TheExit



:apause
PAUSE
:TheExit
exit /b 1
:End
exit /b
::ENDOFWINDOWS
# Batch file to test that the software for compilers is installed OK for bash shell
# Brian Tompsett Dec 2014
#
# Usage
# CheckSoftware.bat <source directory> ["BATCH=yes"]
# defaults <current directory>
# 
#  When "BATCH=yes" is specified, the script does not pause for input
#
# History:
#    Ver 1 : Dec 2014 Cloned from Windows script
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
echo Starting tests $SOURCE #
# -------------------------------- Variable Initialisation --------
# set standard strings
TESTNAME=initialisation #
failCount=0 #
passCount=0 #
nonfatalCount=0 #
#
# --------------------------------  Argument Processing --------------
#
#
#
# --------------------------------  End Argument Processing --------------
#
# Check for source directory and move to a safe place
#
ORIGIN="$( cd -P "$( dirname "$SOURCE" )" && pwd )" #
if [ "$T" == "" ] ; then #
   # Set a local T: if global is missing
   T=$ORIGIN #
fi #
ocd=`pwd` #
if [ "$1" != "" ] ; then #
  if [ ! -e $1 ] ; then #
    echo '***** Source directory does not exist ******' #
    exit #
  else #
    pushd $1 #
	ORIGIN=$1  #
  fi #
  else #
  pushd /tmp > /dev/null #
fi #
# Check for gcc?
#
OBJECTFLAG="-o " #
CC=gcc #
gccpath=`which gcc` #
if [ "$gccpath" == "" ] ; then #
  echo GCC Not found # 
  failCount = `expr $failCount + 1` #
else #
  # Check that GCC runs on "Hello World"
  echo '#include <stdio.h>' > /tmp/hello.c #
  echo 'int main(void) {printf("Hello World\n");}' >> /tmp/hello.c #
  $CC $OBJECTFLAG/tmp/hello.exe /tmp/hello.c #
  if [ -e "/tmp/hello.c" ] ; then #
    rm "/tmp/hello.c" #
  fi #
  if [ -e "/tmp/hello.obj" ] ; then #
   rm "/tmp/hello.obj" #
   fi #
  if [ -x "/tmp/hello.exe" ] ; then #
   result=`/tmp/hello.exe < /dev/null` #
   if [ "$result" != "Hello World" ] ; then #
   echo C compiler not correct #
     failCount=`expr $failCount + 1` #
	 fi #
  else #
  echo C Compiler not functioning #
       failCount=`expr $failCount + 1` #
  fi #
  rm -f /tmp/hello.exe #
fi #
# check we have make
makepath=`which make` #
if [ "$makepath" == "" ] ; then #
  echo Make not found #
    failCount=`expr $failCount + 1` #
fi #
# Now check for flex and bison
flexpath=`which flex` #
FLEX=flex #
if [ "$flexpath" == "" ] ; then #
  echo Flex not found #
    failCount=`expr $failCount + 1` #
fi #
bisonpath=`which bison` #
BISON=bison #
if [ "$bisonpath" == "" ] ; then #
  echo Bison not found #
    failCount=`expr $failCount + 1` #
fi #
# Gnu Make cannot handlespaces in paths. Have to glob
ORIGINSHELL="$ORIGIN"  #
ORIGIN="${ORIGIN// /?}"  #
# Copy the source files to avoid spaces in names
cp ${ORIGIN}/arith.l . #
cp ${ORIGIN}/arith.y . #
cp ${ORIGIN}/arith.c . #
# Create a Makefile for the calculator
echo SOURCE = arith > "/tmp/Makefile" #
echo LEX = $FLEX  >> "/tmp/Makefile" #
echo YACC = $BISON  >> "/tmp/Makefile" #
echo LEXLIB = $LEXLIB  >> "/tmp/Makefile" #
echo CC = gcc  >> "/tmp/Makefile" #
echo TYPE = cat  >> "/tmp/Makefile" #
echo RENAME = mv   >> "/tmp/Makefile" #
echo DEL = rm -f  >> "/tmp/Makefile" #
echo CFLAGS =   >> "/tmp/Makefile" #
echo OBJECTFLAG = "-o  " >> "/tmp/Makefile" # 
echo 'empty :='   >> "/tmp/Makefile" #
echo 'space := $(empty) $(empty)'  >> "/tmp/Makefile" #
echo ORIGIN:="$ORIGIN"  >> "/tmp/Makefile" #
echo ORIGINSHELL:="${ORIGINSHELL// /\\ }"  >> "/tmp/Makefile" #
echo all: '/tmp/$(SOURCE).exe' >> "/tmp/Makefile" #
echo lex.yy.c : '$(SOURCE).l' >> "/tmp/Makefile" #
echo '	$(LEX) $(SOURCE).l' >> "/tmp/Makefile" #
echo '$(SOURCE).tab.c:' '$(SOURCE).y' lex.yy.c >> "/tmp/Makefile" #
echo '	 $(YACC) $(SOURCE).y -o /tmp/arith.tab.c' >> "/tmp/Makefile" #
echo '/tmp/$(SOURCE).exe: $(SOURCE).tab.c $(SOURCE).c' >> "/tmp/Makefile" #
echo '	$(CC) $(OBJECTFLAG)$@ $(SOURCE).c $(SOURCE).tab.c -l$(LEXLIB) -ansi -Wno-return-type $(CFLAGS)' >> "/tmp/Makefile" #
echo '	$(DEL) lex.yy.c $(SOURCE).tab.c' >> "/tmp/Makefile" #
#
# cat /tmp/Makefile #
make -f /tmp/Makefile #
rc=$? #
if [ -e /tmp/Makefile ] ; then #
   rm -f /tmp/Makefile #
fi #
# remove temp links
rm -f /tmp/arith.[lyc] #
if [ $rc != 0 -o ! -x /tmp/arith.exe ] ; then #
   echo Failed to make program # 
   failCount=`expr $failCount + 1` #
fi #
echo '1+2' > /tmp/plus.data #
result=`/tmp/arith.exe < /tmp/plus.data` #
if [ "$result" != "value : 3" ] ; then #
     failCount=`expr $failCount + 1` #
fi #
rm -f /tmp/plus.data #
echo '3*2' > /tmp/mult.data #
result=`/tmp/arith.exe < /tmp/mult.data` #
if [ "$result" != "value : 6" ] ; then #
     failCount=`expr $failCount + 1` #
fi #
rm -f /tmp/mult.data #
rm -f /tmp/arith.exe #
popd > /dev/null #
echo All tests completed: $passCount passed, $nonfatalCount nonfatal, $failCount failed #
if [ "$failCount" -gt 0 ] ; then #
   exit -1 #
else #
  echo Looks like the Software is installed OK #
fi #
exit #


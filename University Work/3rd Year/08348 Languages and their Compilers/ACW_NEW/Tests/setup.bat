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
REM Batch file to map the T: drive for quick testing
REM Brian Tompsett Feb 2013
::
:: History:
::    Ver 1 : Feb 2013
::    Ver 2 : Apr 2013 Added interactive version
::            Sep 2013 08238 version
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
:: set to a blank
set aliased= 
:: Check if alias exists - delete if it does, so new one replaces old
for /f "tokens=2 delims==" %%i in ('subst ^| find "T:" ') do set aliased=%%i
if "%aliased:~2%" == "" GOTO :NoAlias
echo Deleting old mapping of T: to %aliased:~2%
SUBST T: /D
:NoAlias
:: Do the Subst
::   Find containing folder
SET myhome=%~dp0
:: Chop off trailing backslash in subst
SUBST T: "%myhome:~0,-1%"
:: Here we must quote the line as it might contain meta characters
IF /I "%invoker:~0,3%" == "cmd" GOTO :Clicked
:: For some reason, sometime the full path is used - and the case varies
IF /I "%invoker:~0,27%" == "C:\WINDOWS\system32\cmd.exe" GOTO :Clicked
GOTO :DOSLaunch
:Clicked
@color 4e
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo The drive alias T: has been created to run the tests more easily.
echo You can access the tests as t:a.SPL and t:Makefile etc
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
subst
pause
:DOSLaunch
Subst
EXIT /b
echo MELON, MELON - Out of cheese Error - Please Reboot Universe!
::ENDOFWINDOWS
# Bash script file to map the T variable to the current directory for quick testing
# As this modifies the environemnt it must be executed by using the '. setup.at' method
#
# Brian Tompsett Dec 204
#
# History:
#    Ver 1 : Dec 2014 Cloned from windows version
#
# First find the directory with the test scripts and test programs
#  They could be mounted in a OS/X or cygwin format
#
# http://stackoverflow.com/questions/59895/can-a-bash-script-tell-what-directory-its-stored-in
#
# The file is used for both bash and windows and has windows line breaks of \r\n
# Each line ends in a comments to protect the \r character which bash does not like
# Some of the lines are very long because we can't have \r in continuations in bash
#
SOURCE="${BASH_SOURCE[0]}" #
while [ -h "$SOURCE" ]; do # resolve $SOURCE until the file is no longer a symlink
  DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )" #
  SOURCE="$(readlink "$SOURCE")" #
  [[ $SOURCE != /* ]] && SOURCE="$DIR/$SOURCE" # if $SOURCE was a relative symlink, we need to resolve it relative to the path where the symlink file was located
done #
T="$( cd -P "$( dirname "$SOURCE" )" && pwd )" #
echo Variable T set to $T
export T #
 #

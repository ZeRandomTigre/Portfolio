Supporting Files for 08348 (Language and their Compilers) Laboratory and Assessed Coursework
--------------------------------------------------------------------------------------------

Eur Ing Brian C Tompsett, Department of Computer Science.  B.C.Tompsett@hull.ac.uk  x5222

Introduction
------------

The files in this directory are supplied to support students doing the laboratory work for
the 08348 module and the assessed coursework for that module. If you copy the files you
should not assume that you always have the correct version. The files in this directory may 
be updated to fix bugs or include further features at any time. You are reminded to check 
for updates (by looking at the file modify date and time) at frequent intervals.

Inventory
---------

README.txt        -- This file

a.SPL             -- example test files for the SPL language
b.SPL             --  (These are not Shockwave Flash files-
c.SPL             --    You cannot open them by left clicking 
d.SPL             --     You need to right-click and select "Edit with Notepad++"
e.SPL

arith.bnf         -- A text file containing the BNF of the calculator used in the initial Lab
                  -- This is useful as an example of the BNF notation and learning about the grammar
arith.l           -- This is the lex/flex file example from the initial lab
arith.y           -- This is the bison/yacc file example from the initial lab
arith.c           -- This is the C main program used in compiling the calculator
                  -- In the initial lab
				 
Additional Tests  -- This is a folder of 160 tests used to test your SPL compiler
                  -- This folder is only uploaded towards the end of the laboratory
				  -- sessions in semester one and be removed once the assessment is
				  -- complete
				 
CheckSoftware.bat -- This is a Batch file script for checking the lab software
                  -- It is described below
setup.bat         -- This is a Batch file script for a shortcut to these files
                  -- It is described below
RunCompiler.bat   -- This is a script for running the ACW on most systems
                  -- It works on most versions on Microsoft Windows, MAC OS/X and linux
                  -- It is described below
Makefile          -- This is a Makefile for running the ACW called from the above script
                  -- It is described below

SPL01skeleton.l   -- This is a lex/flex skeleton file for use in the ACW
SPL01skeleton.y   -- This is a bison/yacc skeleton file for use in the ACW

Files.zip         -- This is a zip of files useful for the ACW
                  -- It contains a.SPL, b.SPL, c.SPL, d.SPL, e.SPL and
				  -- SPL01skeleton.l, SPL01skeleton.y
				  
Instructions for using the files
--------------------------------

Accessing from Home

You do not need to copy any of these files to your own directory as you can use
them directly from this location either in the laboratory or at home. At home you
can access this directory by mounting the university drives over the VPN. For
more details see the instructional web page at:

http://www2.hull.ac.uk/ICT/get_connected/network_drives/off_campus_using_a_pc.aspx

SPL Programs

 You can use the SPL programs in several ways. You can just read them to see 
example of SPL programs, which will help you understand the SPL language as 
you look at the syntax diagrams in the ACW specification, and they will also
help you write the BNF in the initial part of the assessment. You could also
look at the arith.bnf file as a guide and there are other examples of BNF 
available from sharepoint with the lecture notes.

 The principle use of the SPL programs is for you to test your compiler as
you build it in stages. You can use them to check that your lexical analyser
recognises the lexemes of the language and returns the appropriate tokens.
You can use them to check that your parser built using bison/yacc recognises
simple SPL programs, and you can use them to check that your compiler generates
appropriate code and perhaps also optimises some of that code. The examples a-e
are graduated to show more features of SPL so you can start with a simple program
and work towards more complicated ones.

 There will sometimes be a folder called "Additional Tests". This folder, when
available, will contain hundreds of small tests programs in SPL that can be used
to further diagnose faults or confirm the functionality of an SPL compiler. They
are not published early in the semester as they could confuse someone only working 
on the early stages of the compiler or BNF. We make announcements when these test
programs become available.

  You can input these programs into your SPL compiler using input redirection.
Input and output re-direction is explained further in the ACW FAQ file on 
sharepoint at:

http://intra.net.dcs.hull.ac.uk/student/modules/08348/WebPages/ACWhelp.html

 In summary you use the "<" symbol in the command prompt to supply a test file 
to your compiler:

  G:\08348\> spl < "U:\Computer Science\08348\Labs\a.spl"
  
Using the Batch Files

 Several batch files of scripts are available to assist in building, running and testing
your compiler. The assessment of your program will be made by similar script files so it 
might be useful to use them at some stage to check how your compiler runs. They assist in
collecting the results of testing in files that can later be submitted with your work.

 The first batch file to use is called setup.bat. The purpose of this is to create a
virtual drive in windows called T:. Using T: is easier than typing the full path
"U:\Computer Science\08348\Labs\" every time. So, in the above example of using input
redirection you could simply type:

 G:\08348\> spl < T:a.spl
 
You can execute the setup.bat script by either clicking on its name in a File Explorer
view, or typing its full name in a command prompt. Both have the same effect.

 G:\08348\> "U:\Computer Science\08348\Labs\setup"
 

 The script file called CheckSoftware.bat can be used to check if the software necessary 
for the compiler lab and coursework is running properly. Sometimes you may encounter a 
machine in the laboratory which may not appear to be running properly or the software
does not appear to be working as described. You can click this by running this batch file, 
either by clicking on its name or typing its name in a command prompt. This script is
also useful if you have installed the software on your own machine to check that it all
works properly.

 The batch file called RunCompiler.bat is used to build and run your compiler with all the 
test files. This can be used either by calling it from the command prompt or by clicking
on its name in the File Explorer view. If you click on the name the File Explorer view it 
opens a window (with a red background) and asks you to type the path of the directory which
contains the compiler source files (spl.l, sply, and spl.c). More normally you would first 
execute the setup script (as described above) and then use the T: drive in the command
prompt to access the RunCompiler script. The best way to open a command prompt screen in
the correct directory is to open the directory in the File Explorer, left click in the 
background (to ensure no file is selected) and then depress the shift key whilst pressing
the right mouse button. Select "Open Command Prompt here.." from the pop-up menu.

 In that command prompt you can then call the RunCompiler scripts:
 
 G:\08348\> T:RunCompiler
 
 This will then build your compiler from your source files and then run each stage through
the test programs a-e It will display a window with the output for you to inspect. You then
need to close that window and press enter to continue to the next test. If you do not wish 
to continue you can use a double Control-C (two Control-C in quick succession) to abort the
run of the script.

 The RunCompiler script uses the Makefile to facilitate the work of building of the compiler
and running the tests. The Makefile is not normally used directly. Those who know about 
Makefiles are welcome to read the source of the Makefile.

The RunCompiler Script has several optional parameters than can be used. These are outlined 
below:

 RunCompiler <source directory> "SOURCE=spl" "TESTS=a.SPL" "SILENT=yes" "BATCH=yes" clean
 
The first parameter is used to change the directory holding the source files for the compiler
(spl.l, spl.y and spl.c), If not specified or specified as the null string "" then the
current directory is used. The remaining parameters are all optional and operate as follows:

SOURCE=name      This parameter changes the file name used for the source code of the compiler.
                The default is spl. If name is used then the script looks for name.l, name.y and name.c
				
TESTS=a.spl      This parameter is a list of tests to be run. By default this is a.spl, a.spl, c.spl
                 d.spl and e.spl. This parameter can be used to supply a subset of that list.
                 Other files cannot be used, even if they are provided in this parameter. The
                 tests are hard-wired into the script.
				
SILENT=yes       This parameter turns off any output from the script. All output is sent into
                files that can be consulted later
				
BATCH=yes        This parameter turns off the interactivity of the script. It does not then
                wait for an operator to click a key to continue. It also disables the pop-up
				windows showing the output.
				
clean			This parameter causes the script to delete all the intermediate files it created
				whilst testing the compiler. It does not remove the file of final results. This
				parameter can be used in place of the optional source directory.
				
Using Linux, Apple Mac etc
--------------------------

It is possible to use the test and script files from other system types. The script files
setup.bat, CheckSoftware.bat and RunCompiler.bat are also designed to run from the bash
command prompt. The script assumes the test programs are stored in the same directory as the
script. It has been tested in the following environments: Apple Mac OS/X (both terminal 
window and XQuartz), MinGW MSYS (an optional addition to MinGW accessed from the download 
manager).

 To use the script open the appropriate command prompt window for the bash shell (by clicking 
on the appropriate icon for Mac terminal, Mac XQuartz, Windows MinGW MSYS) and then change to
the directory containing you source files. These files could be in your university network 
drives mounted over the net, for example:

bash-3.2$ cd /Volumes/801234/08348/ACW
bash-3.2$ . "/Volumes/studentshare/Computer Science/08348/Lab/setup.bat"
bash-3.2$ $T/CheckSoftware.bat
bash-3.2$ $T/RunCompiler.bat 
...

(Note: To create the environmental variable $T the setup script must be run using the "."
operation.

 The operation of these scripts will therein be the same as in a windows command prompt.


# A makefile to run compiler tests for 08348
#
# Brian Tompsett September 2013
#                Nov 2014 BCT Portability and manual links added
#                         Changed DOS commands to Nmake commands
#
# This can be executed within the Visual Studio Command Prompt as:
# nmake /f t:Makefile
#
# You can specify the tests to run a,b,c,d,e etc as an optional argument
#  When BATCH=yes is specified, it does not pause for input.
#
# This assumes: 
#    flex rules in spl.l
#      compile flag PRINT enable prints, no PRINT returns token
#    bison rules in spl.y
#      compile flag YYDEUG enables parse only
#      compile flag DEBUG enables parse tree print - no code generator
#    main program in spl.c
#
# The tests are found on the T: drive which is created by the setup batch file
#
# Variables
#  Set for name of command 
#    - allows this to be ported between Windows and Linux and Mac
#
# Can override file name spl.l and spl.y using SOURCE=
# The set of tests can be changed using TESTS=
#
# -------------------------------------------------------------------------------------
# This is written for MS NMAKE: http://msdn.microsoft.com/en-us/library/dd9y37ha.aspx
#                  http://read.pudn.com/downloads6/ebook/18630/Managing%20Projects%20with%20NMAKE.pdf
#
# Portability Improvements:
#          http://nmake.alcatel-lucent.com/faq/gmake.html
#          https://docs.google.com/document/d/1oUR7iMnaNzkeT3TTOS-Gwul6_V3TE8caIDAd1FwPyNc/edit
#          http://www.oreilly.com/openbook/make3/book/ch07.pdf
#          http://skramm.blogspot.co.uk/2013/04/writing-portable-makefiles.html
#  - Does not work on MS NMAKE we are using
!IFDEF COMSPEC
# If Command Interpreter does not exist, then not windows
!IF EXIST ("$(COMSPEC)")
# !MESSAGE COMSPEC EXIST 
!ELSE
!MESSAGE COMSPEC $(COMSPEC) does not exist
!UNDEF COMPSEC
!ENDIF
!ENDIF
!IFDEF COMSPEC
!MESSAGE Windows: $(COMSPEC)
# File path separator and command separator meta-characters
s2=\\
cmd=;
# Meta-character quote
q=
lt=^<
gt=^>
# remove embedded spaces and double backslash from path separator
s3=$(s2: =)
s=$(s3:\\=\)
!ELSE
s2=/
cmd=&
# Meta-character quote
q=\\
q:=$(subst \\\\,\\,$(q))
lt='<'
gt='>'
# remove embedded spaces and double backslash from path separator
s:=$(s2)
!ENDIF
!IFNDEF LOG
LOG=no
!ENDIF
!IF "$(LOG)"=="no"
L=@
!ENDIF
# Names of tools - normally same for most systems:
LEX=flex
YACC=bison
CC=gcc
# Names of System Commands & syntax symbols - Is system dependent
!IFDEF COMSPEC
TYPE=type
RENAME=rename
DEL=DEL
ANNOUNCE=REM
DISPLAY=START notepad $@
COPY=COPY
NUL=nul
PAUSE=PAUSE
# See note below about spaces. In Windows we can use drive subst to handle this
TESTLOC=T:
TESTLOC_SHELL=$(TESTLOC)
LEXLIB=fl
DATE=%DATE%
TIME=%TIME%
DIFF=fc /w $(TEMP)$(s)sample.txt %s | FIND "FC: no dif"
LINECOUNT=FOR /F "delims=" %%l IN ( ' find /c /v "~~~" < %s ') DO set lines=%l
TESTNAME=%|fF
!ELSE
TYPE=cat
RENAME=mv
DEL=rm -f
ANNOUNCE=echo
# NB MinGW does not have many commands... 
DISPLAY=less
PAUSE=bash -c "read -s -n1 -p 'Press any key to continue....'< /dev/tty" ; echo
COPY=cp
NUL=/dev/null
# TESTLOC cannot have any embedded spaces to make the make portable
# So we have to have a shell version with spaces. Make can Glob files.
TESTLOC=/Volumes/studentshare/Computer?Science/08348/Lab
TESTLOC_SHELL=$(subst ?, ,$(TESTLOC))
LEXLIB=l
DATE=`date +%d-%m-%Y`
TIME=`date +%k:%M:%S`
USERNAME ?=`whoami`
TEMP ?=/tmp
DIFF=diff -b "$(TEMP)$(s)sample.txt" > $(NUL) $<
LINECOUNT=test `wc -l < $<`  
TESTNAME=$(<F)
!ENDIF
CFLAGS = 
OBJECTFLAG = -o
LINKERFLAG = -l$(LEXLIB) 
TESTS = a.spl b.spl c.spl d.spl e.spl
SOURCE = spl
SILENT=
BATCH= 
# These macros are set by pre-processor to the list of results to be collected
# They are derived from the names of the tests in TESTS

TOKEN_FILES=
PARSE_FILES="$(USERNAME)-tokens.txt"
TREE_FILES="$(USERNAME)-parse.txt"
CODE_FILES="$(USERNAME)-tree.txt"
RESULTS_FILES="$(USERNAME)-code.txt"
TIDY_FILES=

!IF "$(TESTS)" != "$(TESTS:a.spl=)"
TOKEN_FILES=$(TOKEN_FILES) "$(TEMP)$(s)a-tokens.txt"
PARSE_FILES=$(PARSE_FILES) "$(TEMP)$(s)a-parse.txt"
TREE_FILES=$(TREE_FILES) "$(TEMP)$(s)a-tree.txt"
CODE_FILES=$(CODE_FILES) "$(TEMP)$(s)a-code.txt"
RESULTS_FILES=$(RESULTS_FILES) "$(TEMP)$(s)a-results.txt"
TIDY_FILES=$(TIDY_FILES) a.exe a.c
!ENDIF
!IF "$(TESTS)" != "$(TESTS:b.spl=)"
TOKEN_FILES=$(TOKEN_FILES) "$(TEMP)$(s)b-tokens.txt"
PARSE_FILES=$(PARSE_FILES) "$(TEMP)$(s)b-parse.txt"
TREE_FILES=$(TREE_FILES) "$(TEMP)$(s)b-tree.txt"
CODE_FILES=$(CODE_FILES) "$(TEMP)$(s)b-code.txt"
RESULTS_FILES=$(RESULTS_FILES) "$(TEMP)$(s)b-results.txt"
TIDY_FILES=$(TIDY_FILES) b.exe b.c
!ENDIF
!IF "$(TESTS)" != "$(TESTS:c.spl=)"
TOKEN_FILES=$(TOKEN_FILES) "$(TEMP)$(s)c-tokens.txt"
PARSE_FILES=$(PARSE_FILES) "$(TEMP)$(s)c-parse.txt"
TREE_FILES=$(TREE_FILES) "$(TEMP)$(s)c-tree.txt"
CODE_FILES=$(CODE_FILES) "$(TEMP)$(s)c-code.txt"
RESULTS_FILES=$(RESULTS_FILES) "$(TEMP)$(s)c-results.txt"
TIDY_FILES=$(TIDY_FILES) c.exe c.c
!ENDIF
!IF "$(TESTS)" != "$(TESTS:d.spl=)"
TOKEN_FILES=$(TOKEN_FILES) "$(TEMP)$(s)d-tokens.txt"
PARSE_FILES=$(PARSE_FILES) "$(TEMP)$(s)d-parse.txt"
TREE_FILES=$(TREE_FILES) "$(TEMP)$(s)d-tree.txt"
CODE_FILES=$(CODE_FILES) "$(TEMP)$(s)d-code.txt"
RESULTS_FILES=$(RESULTS_FILES) "$(TEMP)$(s)d-results.txt"
TIDY_FILES=$(TIDY_FILES) d.exe d.c
!ENDIF
!IF "$(TESTS)" != "$(TESTS:e.spl=)"
TOKEN_FILES=$(TOKEN_FILES) "$(TEMP)$(s)e-tokens.txt"
PARSE_FILES=$(PARSE_FILES) "$(TEMP)$(s)e-parse.txt"
TREE_FILES=$(TREE_FILES) "$(TEMP)$(s)e-tree.txt"
CODE_FILES=$(CODE_FILES) "$(TEMP)$(s)e-code.txt"
RESULTS_FILES=$(RESULTS_FILES) "$(TEMP)$(s)e-results.txt"
TIDY_FILES=$(TIDY_FILES) e.exe e.c
!ENDIF

# all -> run all tests
all : "$(USERNAME)-results.txt"
	$(L)echo All tests completed - The results are in: $?

"$(USERNAME)-results.txt" : $(RESULTS_FILES)
# Add in any compilation error files and delete them
#  Makefile pre-processing commands happen before execution so cannot be used here.
# !IF EXIST ("$(TEMP)$(s)lex-errors.txt")
	$(L)echo -- $(LEX) Errors -------------------------------- > $@
	-$(L)$(TYPE) "$(TEMP)$(s)lex-errors.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)lex-errors.txt"
# !ENDIF
# !IF EXIST ("$(TEMP)$(s)lexer-errors.txt")
	$(L)echo -- $(CC) -DPRINT Errors -------------------------------- >> $@
	-$(L)$(TYPE) "$(TEMP)$(s)lexer-errors.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)lexer-errors.txt"
# !ENDIF
# !IF EXIST ("$(TEMP)$(s)debug-errors.txt")
	$(L)echo -- $(YACC) -t Errors -------------------------------- >> $@
	-$(L)$(TYPE) "$(TEMP)$(s)debug-errors.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)debug-errors.txt"
# !ENDIF
#!IF EXIST ("$(TEMP)$(s)parser-errors.txt")
	$(L)echo -- $(CC) -DYYDEBUG Errors -------------------------------- >> $@
	-$(L)$(TYPE) "$(TEMP)$(s)parser-errors.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)parser-errors.txt"
# !ENDIF
# !IF EXIST ("$(TEMP)$(s)tree-errors.txt")
	$(L)echo -- $(CC) -DDEBUG Errors -------------------------------- >> $@
	-$(L)$(TYPE) "$(TEMP)$(s)tree-errors.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)tree-errors.txt"
# !ENDIF
# !IF EXIST ("$(TEMP)$(s)compiler-errors.txt")
	$(L)echo -- $(CC) Errors -------------------------------- >> $@
	-$(L)$(TYPE) "$(TEMP)$(s)compiler-errors.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)compiler-errors.txt"
# !ENDIF
	$(L)echo -- Output -------------------------------- >> $@
	$(L)$(TYPE) $(RESULTS_FILES) >> $@ 2> $(NUL)

"$(TEMP)$(s)a-results.txt" : a-output.txt
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo hello > "$(TEMP)$(s)sample.txt"
!IFDEF COMSPEC
	-$(L)fc /w "$(TEMP)$(s)sample.txt" %s | FIND "FC: no dif" > nul && echo Correct Execution output for a.spl >> $@ || echo Program A output incorrect >> $@
!ELSE
	-$(L)$(DIFF) && echo Correct Execution output for a.spl: >> $@ || echo Program a.spl output incorrect: >> $@
!ENDIF
	-$(L)$(DEL)  "$(TEMP)$(s)sample.txt"
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	-$(L)$(TYPE) "$(TEMP)$(s)a-error.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)a-error.txt"
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) $@
!ENDIF
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)b-results.txt" : b-output.txt
# B has a floating point number which can be to any precision
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo B >  "$(TEMP)$(s)sample.txt"
	$(L)echo 7.82 >>  "$(TEMP)$(s)sample.txt"
	$(L)echo x >>  "$(TEMP)$(s)sample.txt"
!IFDEF COMSPEC
	$(L)@fc /w "$(TEMP)$(s)sample.txt" %s | FIND "FC: no dif" > nul && echo Correct Execution output for b.spl >> $@ || FOR /F "delims=" %%l IN ( ' find /c /v "~~~" ^< %s ') DO @IF /i %l EQU 3 (echo Program B needs a visual inspection of floating point numbers >> $@) else (echo Program B output incorrect - %l >> $@)
!ELSE
	-$(L)lines=`wc -l < $<`; diff -b "$(TEMP)$(s)sample.txt" $< > $(NUL)  && echo Correct Execution output for b.spl >> $@ && test $$lines == 3 && echo Program B needs a visual inspection of floating point numbers >> $@ || echo Program B output incorrect - $$lines >> $@ 
!ENDIF
	-$(L)$(DEL)  "$(TEMP)$(s)sample.txt"
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	-$(L)$(TYPE) "$(TEMP)$(s)b-error.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)b-error.txt"
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) $@
!ENDIF
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)c-results.txt" : c-output.txt
# Program C has a boolean expression with two results depending on operator binding,
# so two set of results have to be checked. 
	$(L)echo --$(DATE)----------------------------------------------------- > $@
# First test for 25 line version
	$(L)echo 1 > "$(TEMP)$(s)sample.txt"
	$(L)echo 2 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 3 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 4 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 5 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 6 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 8 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 9 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 10 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 11 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 12 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 13 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 1 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 2 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 3 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 4 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 5 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 7 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 9 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 10 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 11 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 12 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 13 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 14 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 01234567891011 >> "$(TEMP)$(s)sample.txt"
!IFDEF COMSPEC
	-$(L)fc /w "$(TEMP)$(s)sample.txt" %s | FIND "FC: no dif" > nul && echo Correct Alternate Execution output for c.spl >> $@ || FOR /F "delims=" %%l IN ( ' find /c /v "~~~" ^< %s ') DO @IF /i %l EQU 25 echo Program C needs a visual inspection to confirm result  >> $@ 
!ELSE
	$(L)lines=`wc -l < $<` ; diff -b "$(TEMP)$(s)sample.txt" $< > $(NUL)  && echo Correct Alternate Execution output for c.spl >> $@ || test \$lines == 25 && echo Program C needs a visual inspection to confirm result  >> $@
!ENDIF
# Now do the test for the 26 line version
	$(L)echo 1 > "$(TEMP)$(s)sample.txt"
	$(L)echo 2 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 3 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 4 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 5 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 6 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 8 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 9 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 10 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 11 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 12 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 13 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 1 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 2 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 3 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 4 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 5 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 7 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 8 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 9 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 10 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 11 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 12 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 13 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 14 >> "$(TEMP)$(s)sample.txt"
	$(L)echo 01234567891011 >> "$(TEMP)$(s)sample.txt"
!IFDEF COMSPEC
	-$(L)fc /w "$(TEMP)$(s)sample.txt" %s | FIND "FC: no dif" > nul && echo Correct Alternate Execution output for c.spl >> $@ || FOR /F "delims=" %%l IN ( ' find /c /v "~~~" ^< %s ') DO @IF /i %l EQU 26 echo Program C needs a visual inspection to confirm result  >> $@ 
	$(L)FOR /F "delims=" %%l IN ( ' find /c /v "~~~" ^< %s ') DO @IF /i %l NEQ 26 IF /i %l NEQ 25 echo Incorrect Execution output for c.spl - %l >> $@
!ELSE
	-$(L)diff -b "$(TEMP)$(s)sample.txt" $< > $(NUL)  && echo Correct Alternate Execution output for c.spl >> $@ || test `wc -l < $<` == 26 && echo Program C needs a visual inspection to confirm result  >> $@ 
	$(L)lines=`wc -l < $<`; test \( \$lines != 26 \) -a \( \$lines != 25 \) && echo Incorrect Execution output for c.spl - \$lines >> $@
!ENDIF
	-$(L)$(DEL)  "$(TEMP)$(s)sample.txt"
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	-$(L)$(TYPE) "$(TEMP)$(s)c-error.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)c-error.txt"
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) $@
!ENDIF
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)d-results.txt" : d-output.txt
# D has two floating point results which can be to any precision
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo 11.34778378378378 > "$(TEMP)$(s)sample.txt"
	$(L)echo 17.14778378378378 >> "$(TEMP)$(s)sample.txt"
!IFDEF COMSPEC
	-@fc /w "$(TEMP)$(s)sample.txt" %s | FIND "FC: no dif" > nul && echo Correct Execution output for d.spl >> $@ || FOR /F "delims=" %%l IN ( ' find /c /v "~~~" ^< %s ') DO @IF /i %l EQU 2 (echo Program D needs a visual inspection of floating point numbers  >> $@) else (echo Program D output incorrect - %l >> $@)
!ELSE
	-$(L)lines=`wc -l < $<`; diff -b "$(TEMP)$(s)sample.txt" $< > $(NUL) && echo Correct Execution output for d.spl >> $@ || test $$lines == 2 && echo Program D needs a visual inspection of floating point numbers  >> $@ || echo Program D output incorrect - $$lines >> $@
!ENDIF
	-$(L)$(DEL)  "$(TEMP)$(s)sample.txt"
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	-$(L)$(TYPE) "$(TEMP)$(s)d-error.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)d-error.txt"
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) $@
!ENDIF
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)e-results.txt" : e-output.txt
# E has a floating point result which can be to any precision
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo 1234535.0 > "$(TEMP)$(s)sample.txt"
	$(L)echo -1 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -2 >>"$(TEMP)$(s)sample.txt"
	$(L)echo -3 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -4 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -5 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -1 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -2 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -3 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -4 >> "$(TEMP)$(s)sample.txt"
	$(L)echo -5 >> "$(TEMP)$(s)sample.txt"
!IFDEF COMSPEC
	-@fc /w "$(TEMP)$(s)sample.txt" %s | FIND "FC: no dif" > nul && echo Correct Execution output for e.spl >> $@ || FOR /F "delims=" %%l IN ( ' find /c /v "~~~" ^< %s ') DO @IF /i %l EQU 11 (echo Program E needs a visual inspection of floating point numbers  >> $@) else (echo Program E output incorrect - %l >> $@)
!ELSE
	-$(L)lines=`wc -l < $<`; diff -b "$(TEMP)$(s)sample.txt" $< > $(NUL) && echo Correct Execution output for e.spl >> $@ || test $$lines == 11 && echo Program E needs a visual inspection of floating point numbers  >> $@ || echo Program E output incorrect - $$lines >> $@
!ENDIF
	-$(L)$(DEL)  "$(TEMP)$(s)sample.txt"
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	-$(L)$(TYPE) "$(TEMP)$(s)e-error.txt" >> $@
	-$(L)$(DEL) "$(TEMP)$(s)e-error.txt"
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) $@
!ENDIF
	$(L)$(TYPE) $? >> $@

a-output.txt : a.exe
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)a.exe ^> $@
!ELSE
	$(L)echo	.$(s)a.exe '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)a.exe < $(NUL) > $@ 2>&1
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

# Caution - float could be in any format or precision
b-output.txt : b.exe
	$(L)echo 	1 2 3.4 x > "$(TEMP)$(s)sample.txt"
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)b.exe ^> $@
!ELSE
	$(L)echo	.$(s)b.exe '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)b.exe <  "$(TEMP)$(s)sample.txt"  > $@ 2>&1
	-$(L)$(DEL) "$(TEMP)$(s)sample.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

c-output.txt : c.exe
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)c.exe ^> $@
!ELSE
	$(L)echo	.$(s)c.exe '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)c.exe < $(NUL)  > $@ 2>&1
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

# Caution - float could be in any format or precision
d-output.txt : d.exe
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)d.exe ^> $@
!ELSE
	$(L)echo	.$(s)d.exe '>' $@
!ENDIF
!ENDIF
	$(L)echo 	5.8 > "$(TEMP)$(s)sample.txt"
	-$(L).$(s)d.exe < "$(TEMP)$(s)sample.txt"  > $@
	-$(L)$(DEL) "$(TEMP)$(s)sample.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

# Caution - float could be in any format or precision
e-output.txt : e.exe
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)e.exe ^> $@
!ELSE
	$(L)echo	.$(s)e.exe '>' $@
!ENDIF
!ENDIF
	$(L)echo 	12345 > "$(TEMP)$(s)sample.txt"
	-$(L).$(s)e.exe < "$(TEMP)$(s)sample.txt" > $@
	-$(L)$(DEL) "$(TEMP)$(s)sample.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)a-code.txt" : a.c
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo C Code output for a.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)b-code.txt" : b.c
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo C Code output for b.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)c-code.txt" : c.c
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo C Code output for c.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)d-code.txt" : d.c
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo C Code output for d.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	$(L)$(TYPE) $? >> $@

"$(TEMP)$(s)e-code.txt" : e.c
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo C Code output for e.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
	$(L)$(TYPE) $? >> $@

"$(USERNAME)-code.txt" : $(CODE_FILES)
	$(L)$(TYPE) $(CODE_FILES) > $@ 2> $(NUL) 
!IF "$(BATCH)."=="." 
	$(L)echo Code Generation complete - Continue to code execution?
	$(L)$(PAUSE)
!ENDIF

a.exe : a.c
# Fake a gcc line for clean output!
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS)
!ENDIF
	-$(L)$(DEL) a.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS)  > "$(TEMP)$(s)a-error.txt" 2>&1
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) "$(TEMP)$(s)a-error.txt"
!ENDIF

a.c : $(TESTLOC)$(s)a.spl compiler.exe
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)compiler ^< a.spl
!ELSE
	$(L)echo	.$(s)compiler '<' a.spl
!ENDIF
!ENDIF
# We should separate stdout from sterr in compiler output!
	-$(L).$(s)compiler.exe < "$(TESTLOC_SHELL)$(s)a.spl" > $@ 2>> "$(TEMP)$(s)a-error.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

b.exe : b.c
# Fake a gcc line for clean output!
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS)
!ENDIF
	-$(L)$(DEL) b.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS) > "$(TEMP)$(s)b-error.txt" 2>&1
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) "$(TEMP)$(s)b-error.txt"
!ENDIF

b.c : $(TESTLOC)$(s)b.spl compiler.exe 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)compiler ^< b.spl
!ELSE
	$(L)echo	.$(s)compiler '<' b.spl
!ENDIF
!ENDIF
# We should separate stdout from sterr in compiler output!
	-$(L).$(s)compiler.exe < "$(TESTLOC_SHELL)$(s)b.spl" > $@ 2>> "$(TEMP)$(s)b-error.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

c.exe : c.c
# Fake a gcc line for clean output!
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS)
!ENDIF
	-$(L)$(DEL) c.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS) > "$(TEMP)$(s)c-error.txt" 2>&1
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) "$(TEMP)$(s)c-error.txt"
!ENDIF

c.c : $(TESTLOC)$(s)c.spl compiler.exe 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)compiler ^< c.spl
!ELSE
	$(L)echo	.$(s)compiler '<' c.spl
!ENDIF
!ENDIF
# We should separate stdout from sterr in compiler output!
	-$(L).$(s)compiler.exe < "$(TESTLOC_SHELL)$(s)c.spl" > $@ 2>> "$(TEMP)$(s)c-error.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

d.exe : d.c
# Fake a gcc line for clean output!
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS)
!ENDIF
	-$(L)$(DEL) d.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS) > "$(TEMP)$(s)d-error.txt" 2>&1
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) "$(TEMP)$(s)d-error.txt"
!ENDIF

d.c : $(TESTLOC)$(s)d.spl compiler.exe 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)compiler ^< d.spl
!ELSE
	$(L)echo	.$(s)compiler '<' d.spl
!ENDIF
!ENDIF
# We should separate stdout from sterr in compiler output!
	-$(L).$(s)compiler.exe < "$(TESTLOC_SHELL)$(s)d.spl" > $@ 2>> "$(TEMP)$(s)d-error.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

e.exe : e.c
# Fake a gcc line for clean output!
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS)
!ENDIF
	-$(L)$(DEL) e.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ $? $(CFLAGS) > "$(TEMP)$(s)e-error.txt" 2>&1
!IF "$(BATCH)."=="." 
	-$(L)$(TYPE) "$(TEMP)$(s)e-error.txt"
!ENDIF

e.c : $(TESTLOC)$(s)e.spl compiler.exe 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)compiler ^< e.spl
!ELSE
	$(L)echo	.$(s)compiler '<' e.spl
!ENDIF
!ENDIF
# We should separate stdout from sterr in compiler output!
	-$(L).$(s)compiler.exe < "$(TESTLOC_SHELL)$(s)e.spl" > $@ 2>> "$(TEMP)$(s)e-error.txt"
!IF "$(BATCH)."=="." 
	-$(TYPE) $@
	$(L)$(PAUSE)
!ENDIF

compiler.exe : "$(USERNAME)-tree.txt" $(SOURCE)tab.c "$(TEMP)$(s)$(SOURCE).c" 
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ $(SOURCE).c $(SOURCE)tab.c $(LINKERFLAG) $(CFLAGS)
!ENDIF
	-$(L)$(DEL) compiler.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ -UYY_MAIN "$(TEMP)$(s)$(SOURCE).c" $(SOURCE)tab.c $(LINKERFLAG) $(CFLAGS) > "$(TEMP)$(s)compiler-errors.txt" 2>&1
!IF "$(SILENT)."=="."
	$(L)$(TYPE) "$(TEMP)$(s)compiler-errors.txt" 
!ENDIF

lex.yy.c: $(SOURCE).l
!IF "$(SILENT)."=="."
	$(L)echo	$(LEX) $(SOURCE).l
!ENDIF
	-$(L)$(LEX) $(SOURCE).l > "$(TEMP)$(s)lex-errors.txt" 2>&1
!IF "$(SILENT)."=="."
	$(L)$(TYPE) "$(TEMP)$(s)lex-errors.txt" 
!ENDIF	

$(SOURCE)tab.c:: "$(USERNAME)-parse.txt" $(SOURCE).y lex.yy.c 
!IF "$(SILENT)."=="."
	$(L)echo	$(YACC) $(SOURCE).y -o $@
!ENDIF
	-$(L)$(YACC) $(SOURCE).y -o $@ > "$(TEMP)$(s)bison-errors.txt" 2>&1
!IF "$(SILENT)."=="."
	$(L)$(TYPE) "$(TEMP)$(s)bison-errors.txt" 
!ENDIF	

"$(USERNAME)-parse.txt" : $(PARSE_FILES)
	$(L)$(TYPE) $(PARSE_FILES) > $@ 2> $(NUL)
!IF "$(BATCH)."=="." 
	$(L)echo Parser Complete - Continue to Tree Build test?
	$(L)$(PAUSE)
!ENDIF

"$(USERNAME)-tree.txt" : $(TREE_FILES)
	$(L)$(TYPE) $(TREE_FILES) > $@ 2> $(NUL)
!IF "$(BATCH)."=="." 
	$(L)echo Tree Test Complete - Continue to Code generation test?
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)a-tree.txt" : $(TESTLOC)$(s)a.spl tree.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse Tree output for a.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)tree ^< a.spl ^> $@
!ELSE
	$(L)echo	.$(s)tree '<' a.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)tree.exe < "$(TESTLOC_SHELL)$(s)a.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if tree is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)b-tree.txt" : $(TESTLOC)$(s)b.spl tree.exe 
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse Tree output for b.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)tree ^< b.spl ^> $@
!ELSE
	$(L)echo	.$(s)tree '<' b.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)tree.exe < "$(TESTLOC_SHELL)$(s)b.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if tree is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)c-tree.txt" : $(TESTLOC)$(s)c.spl tree.exe 
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse Tree output for c.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)tree ^< c.spl ^> $@
!ELSE
	$(L)echo	.$(s)tree '<' c.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)tree.exe < "$(TESTLOC_SHELL)$(s)c.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if tree is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)d-tree.txt" : $(TESTLOC)$(s)d.spl tree.exe 
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse Tree output for d.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)tree ^< d.spl ^> $@
!ELSE
	$(L)echo	.$(s)tree '<' d.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)tree.exe < "$(TESTLOC_SHELL)$(s)d.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if tree is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)e-tree.txt" : $(TESTLOC)$(s)e.spl tree.exe 
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse Tree output for e.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@ 
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)tree ^< e.spl ^> $@
!ELSE
	$(L)echo	.$(s)tree '<' e.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)tree.exe < "$(TESTLOC_SHELL)$(s)e.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if tree is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

tree.exe: "$(USERNAME)-parse.txt" $(SOURCE)tab.c "$(TEMP)$(s)$(SOURCE).c" lex.yy.c 
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ -DDEBUG -UYY_MAIN $(SOURCE).c $(SOURCE)tab.c $(LINKERFLAG) $(CFLAGS)
!ENDIF 
	-$(L)$(DEL) tree.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ -DDEBUG -UYY_MAIN "$(TEMP)$(s)$(SOURCE).c" "$(SOURCE)tab.c" $(LINKERFLAG) $(CFLAGS) > "$(TEMP)$(s)tree-errors.txt" 2>&1
!IF "$(SILENT)."=="."
	$(L)$(TYPE) "$(TEMP)$(s)tree-errors.txt"
!ENDIF

"$(TEMP)$(s)a-parse.txt" : $(TESTLOC)$(s)a.spl parser.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse debug output for a.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)parser ^< a.spl ^> $@
!ELSE
	$(L)echo	.$(s)parser '<' a.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)parser.exe < "$(TESTLOC_SHELL)$(s)a.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if parse is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)b-parse.txt" : $(TESTLOC)$(s)b.spl parser.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse debug output for b.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)parser ^< b.spl ^> $@
!ELSE
	$(L)echo	.$(s)parser '<' b.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)parser.exe < "$(TESTLOC_SHELL)$(s)b.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if parse is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)c-parse.txt" : $(TESTLOC)$(s)c.spl parser.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse debug output for c.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)parser ^< c.spl ^> $@
!ELSE
	$(L)echo	.$(s)parser '<' c.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)parser.exe < "$(TESTLOC_SHELL)$(s)c.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if parse is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)d-parse.txt" : $(TESTLOC)$(s)d.spl parser.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse debug output for d.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)parser ^< d.spl ^> $@
!ELSE
	$(L)echo	.$(s)parser '<' d.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)parser.exe < "$(TESTLOC_SHELL)$(s)d.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if parse is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)e-parse.txt" : $(TESTLOC)$(s)e.spl parser.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Parse debug output for e.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)parser ^< e.spl ^> $@
!ELSE
	$(L)echo	.$(s)parser '<' e.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)parser.exe < "$(TESTLOC_SHELL)$(s)e.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if parse is OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

parser.exe: "$(USERNAME)-tokens.txt" $(SOURCE)debugtab.c "$(TEMP)$(s)$(SOURCE).c" lex.yy.c 
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ -DYYDEBUG -UYY_MAIN "$(SOURCE).c" "$(SOURCE)debugtab.c" $(LINKERFLAG) $(CFLAGS) 
!ENDIF
	-$(L)$(DEL) parser.exe > $(NUL) 2>&1
	-$(L)$(CC) $(OBJECTFLAG)$@ -DYYDEBUG -UYY_MAIN "$(TEMP)$(s)$(SOURCE).c" "$(SOURCE)debugtab.c" $(LINKERFLAG) $(CFLAGS) > "$(TEMP)$(s)parser-errors.txt" 2>&1
!IF "$(SILENT)."=="."
	$(L)$(TYPE) "$(TEMP)$(s)parser-errors.txt"
!ENDIF

$(SOURCE)debugtab.c:: lex.yy.c $(SOURCE).y "$(USERNAME)-tokens.txt"
!IF "$(SILENT)."=="."
	$(L)echo	$(YACC) -t $(SOURCE).y  -o $@
!ENDIF
	-$(L)$(YACC) -t $(SOURCE).y -o $@ > "$(TEMP)$(s)debug-errors.txt" 2>&1
!IF "$(SILENT)."=="."
	$(L)$(TYPE) "$(TEMP)$(s)debug-errors.txt"
!ENDIF

"$(USERNAME)-tokens.txt": $(TOKEN_FILES)
	$(L)$(TYPE) $(TOKEN_FILES) > $@ 2> $(NUL)
!IF "$(BATCH)."=="." 
	$(L)echo Lexical Testing Complete - Continue to Parser Testing?
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)a-tokens.txt" : $(TESTLOC)$(s)a.spl lexer.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Lexical Tokens for a.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)lexer ^< a.spl ^> $@
!ELSE
	$(L)echo	.$(s)lexer '<' a.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)lexer.exe < "$(TESTLOC_SHELL)$(s)a.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if Tokens are OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

lexer.exe: lex.yy.c
	-$(L)$(DEL) lexer.exe > $(NUL) 2>&1
!IF "$(SILENT)."=="."
	$(L)echo	$(CC) $(OBJECTFLAG)$@ -DPRINT -DYY_MAIN $? $(LINKERFLAG) $(CFLAGS)
!ENDIF
	-$(L)$(CC) $(OBJECTFLAG)$@ -DPRINT -DYY_MAIN $? $(LINKERFLAG) > "$(TEMP)$(s)lexer-error.txt" 2>&1
!IF "$(SILENT)."=="."
	$(L)$(TYPE) "$(TEMP)$(s)lexer-error.txt"
!ENDIF

"$(TEMP)$(s)b-tokens.txt" : $(TESTLOC)$(s)b.spl lexer.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Lexical Tokens for b.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)lexer ^< b.spl ^> $@
!ELSE
	$(L)echo	.$(s)lexer '<' b.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)lexer.exe < "$(TESTLOC_SHELL)$(s)b.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if Tokens are OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)c-tokens.txt" : $(TESTLOC)$(s)c.spl lexer.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Lexical Tokens for c.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)lexer ^< c.spl ^> $@
!ELSE
	$(L)echo	.$(s)lexer '<' c.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)lexer.exe < "$(TESTLOC_SHELL)$(s)c.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if Tokens are OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)d-tokens.txt" : $(TESTLOC)$(s)d.spl lexer.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Lexical Tokens for d.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)lexer ^< d.spl ^> $@
!ELSE
	$(L)echo	.$(s)lexer '<' d.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)lexer.exe < "$(TESTLOC_SHELL)$(s)d.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if Tokens are OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

"$(TEMP)$(s)e-tokens.txt" : $(TESTLOC)$(s)e.spl lexer.exe
	$(L)echo --$(DATE)----------------------------------------------------- > $@
	$(L)echo Lexical Tokens for e.spl >> $@
	$(L)echo --$(TIME)----------------------------------------------------- >> $@
!IF "$(SILENT)."=="."
!IFDEF COMSPEC
	$(L)echo	.$(s)lexer ^< e.spl ^> $@
!ELSE
	$(L)echo	.$(s)lexer '<' e.spl '>' $@
!ENDIF
!ENDIF
	-$(L).$(s)lexer.exe < "$(TESTLOC_SHELL)$(s)e.spl" >> $@ 2>&1
!IF "$(BATCH)."=="." 
	$(L)$(ANNOUNCE) Displaying $@
	$(L)$(DISPLAY) $@
	$(L)echo Continue if Tokens are OK or interrupt to fix any errors
	$(L)$(PAUSE)
!ENDIF

# Just in case the spl.c file is missing, make a default one
"$(TEMP)$(s)$(SOURCE).c": $(SOURCE).c
	-$(L)$(COPY) $? $@ > $(NUL)

$(SOURCE).c:
!IFDEF COMSPEC
	$(L)echo You$(q)'ve forgotton to make a $(SOURCE).c file
	$(L)echo $(q)#include ^<stdio.h^>  >"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo int main$(q)(void$(q)) >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo {  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo $(q)#if YYDEBUG == 1  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo extern int yydebug$(q);  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo yydebug=1$(q);  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo $(q)#endif  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo return$(q)(yyparse$(q)($(q))$(q))$(q);  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo }  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo void yyerror$(q)(char $(q)*s$(q))  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo {  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo fprintf$(q)(stderr$(q), $(q)"Error : Exiting $(q)%s$(q)$(q)$(q)$(q)n$(q)"$(q), s$(q))$(q);  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo }  >>"$(TEMP)$(s)$(SOURCE).c" 
!ELSE
	$(L)echo You\'ve forgotton to make a $(SOURCE).c file
	$(L)echo \#include '<'stdio.h'>'  >"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo int main\(void\) >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo {  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo \#if YYDEBUG == 1  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo extern int yydebug\;  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo yydebug=1\;  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo \#endif  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo return\(yyparse\(\)\)\;  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo }  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo void yyerror\(char \*s\)  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo {  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo fprintf\(stderr\, \"Error : Exiting \%s\\\\n\"\, s\)\;  >>"$(TEMP)$(s)$(SOURCE).c" 
	$(L)echo }  >>"$(TEMP)$(s)$(SOURCE).c" 
!ENDIF
#	$(TYPE) "$(TEMP)$(s)$(SOURCE).c" 

clean:
	-$(DEL) $(SOURCE).tab.c 
	-$(L)$(DEL) "$(TEMP)$(s)$(SOURCE).c"
	-$(DEL) $(SOURCE)debug.tab.c lex.yy.c compiler.exe tree.exe parser.exe lexer.exe
	-$(DEL) $(SOURCE).obj $(SOURCE).tab.obj $(SOURCE)debug.tab.obj
	-$(DEL) *-output.txt *-tokens.txt *-parse.txt *-tree.txt *-code.txt
	-$(DEL) "$(TEMP)$(s)*-output.txt" "$(TEMP)$(s)*-tokens.txt" "$(TEMP)$(s)*-tree.txt"
	-$(DEL) "$(TEMP)$(s)*-code.txt" "$(TEMP)$(s)*-results.txt" "$(TEMP)$(s)*-parse.txt"
	-$(DEL) $(TIDY_FILES)


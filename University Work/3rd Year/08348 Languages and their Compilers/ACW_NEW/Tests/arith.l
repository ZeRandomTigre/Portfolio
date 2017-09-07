delim           [ \t\r]
ws              {delim}+
digit           [0-9]
number          {digit}+

%%

{ws}            ; /* Do Nothing */

\n              return(NEWLINE);

"+"             return(PLUS);
"*"             return(TIMES);

"("             return(BRA);
")"             return(KET);

{number}        {
                  /* call the library function to convert ascii to int */
                  /* assign the integer value of the text in yytext to 
                      the iVal part of the yylVal object */
                  yylval.iVal = atoi(yytext) ;
                  return(NUMBER);
                }


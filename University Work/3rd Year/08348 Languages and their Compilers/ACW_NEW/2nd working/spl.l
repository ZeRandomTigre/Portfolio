%{
#ifdef PRINT
#define TOKEN(token) printf("Token: " #token "\n")
#define TOKEN_NUMBER(token) printf("Number %d Token: " #token "\n", atoi(yytext));
#define TOKEN_ID(token) printf("ID %s Token: " #token "\n", yytext);
#else
int k;
#define TOKEN(token) return(token)
#define TOKEN_NUMBER(token) k = atoi(yytext); return(token)
#define TOKEN_ID(token) yylval.iVal = installId(yytext); return(token);

#include <string.h>
extern SYMTABNODEPTR symTab[SYMTABSIZE];
extern int currentSymTabSize;

#endif
%}

delim 			[ \t\r\n]
ws				{delim}+
digit			[0-9]
character		[a-zA-Z]
number			{digit}+
id				{character}({character}|{digit})*
char			'{character}'

%%
				
{ws} 			; /* white space, do nothing */ 
 
":"				TOKEN(COLON);
"."				TOKEN(FULLSTOP);
";"				TOKEN(SEMICOLON);
","				TOKEN(COMMA);

"->"			TOKEN(ASSIGNMENT);
"("				TOKEN(OPEN_BRACKET);
")"				TOKEN(CLOSE_BRACKET);

"="				TOKEN(EQUAL);
"<>"			TOKEN(LESS_GREATER_THAN);
"<"				TOKEN(LESS_THAN);
">"				TOKEN(GREATER_THAN);
"<="			TOKEN(LESS_THAN_EQUAL);
">="			TOKEN(GREATER_THAN_EQUAL);

"+"				TOKEN(PLUS);
"-"				TOKEN(MINUS);
"*"				TOKEN(MULTIPLY);
"/"				TOKEN(DIVIDE);

NEWLINE			TOKEN(NEWLINE);
CHARACTER		TOKEN(CHARACTER);
INTEGER			TOKEN(INTEGER);
REAL			TOKEN(REAL);

IF				TOKEN(IF);
THEN			TOKEN(THEN);
ELSE			TOKEN(ELSE);
DO				TOKEN(DO);
WHILE			TOKEN(WHILE);
FOR				TOKEN(FOR);
IS				TOKEN(IS);
BY				TOKEN(BY);
TO              TOKEN(TO);

NOT				TOKEN(NOT);
AND				TOKEN(AND);
OR				TOKEN(OR);

ENDP			TOKEN(ENDP);
ENDIF			TOKEN(ENDIF);
ENDFOR			TOKEN(ENDFOR);
ENDDO			TOKEN(ENDDO);
ENDWHILE		TOKEN(ENDWHILE);
WRITE			TOKEN(WRITE);
READ			TOKEN(READ);
DECLARATIONS	TOKEN(DECLARATIONS);
CODE			TOKEN(CODE);
OF 				TOKEN(OF);
TYPE			TOKEN(TYPE);

{id} 			TOKEN_ID(ID);
					
{number} 		TOKEN_NUMBER(NUMBER);
				
{char}          TOKEN(CHAR);

%%

#ifndef PRINT

SYMTABNODEPTR newSymTabNode()
{
    return ((SYMTABNODEPTR)malloc(sizeof(SYMTABNODE)));
}

int installId(char *id) 
{
    extern SYMTABNODEPTR symTab[SYMTABSIZE]; 
    extern int currentSymTabSize;
    int index;

    index = lookup(id);
    if (index >= 0)
    {
        return (index);
    }
    else 
       if (currentSymTabSize >= SYMTABSIZE) 
          /* SYMTAB is full */
          return (NOTHING) ;
    else
    {
       symTab[currentSymTabSize] = newSymTabNode();
       /* Recommended code for preventing buffer overrun on bounded strings */
       strncpy(symTab[currentSymTabSize]->ID,id,IDLENGTH);
       symTab[currentSymTabSize]->ID[IDLENGTH-1] = '\0';
       return(currentSymTabSize++);
    }
}

int lookup(char *s)
{
    extern SYMTABNODEPTR symTab[SYMTABSIZE];
    extern int currentSymTabSize;
    int i;

    for(i=0; i<currentSymTabSize; i++)
    {
        if(strncmp(s,symTab[i]->ID,IDLENGTH) == 0)
        {
            return (i);
        }
    }
    return (-1);    
}

#endif




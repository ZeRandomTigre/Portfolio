%{
#ifdef PRINT
#define TOKEN(token) printf("Token: " #token "\n")
#define TOKEN_NO(token) printf("Token: " #token "\n")
#define TOKEN_ID(token) printf("Token: " #token "\n")
#else
int k;
#define TOKEN(token) return(token)
#define TOKEN_NO(token) k = atoi(yytext); return(token)
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
"."				TOKEN(DOT);
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

{id} 			TOKEN(ID);
{number} 		TOKEN_NO(NUMBER);
{char}          TOKEN(CHAR);

%token COLON DOT COMMA SEMICOLON ASSIGNMENT OPEN_BRACKET CLOSE_BRACKET 
		LESS_THAN GREATER_THAN LESS_THAN_EQUAL GREATER_THAN_EQUAL PLUS MINUS EQUAL
		MULTIPLY DIVIDE OPEN_QUOTE CLOSE_QUOTE LESS_GREATER_THAN
		DECLARATIONS CODE OF TYPE CHARACTER INTEGER REAL IF ENDIF 
		THEN ELSE DO WHILE ENDDO ENDWHILE FOR IS BY TO ENDFOR
		NEWLINE READ WRITE NOT AND OR ENDP
%token	ID NUMBER CHAR
%%
program : ID COLON block ENDP ID DOT
			;
block : DECLARATIONS declaration_block CODE statement_list
			| CODE statement_list
			;
declaration_block : identifier_list OF TYPE type SEMICOLON
						| identifier_list OF TYPE type SEMICOLON declaration_block
						;
identifier_list : ID 
                | ID COMMA identifier_list
				;
type : CHARACTER
			| INTEGER
			| REAL
			;
statement_list : statement 
					| statement SEMICOLON statement_list
					;
statement : assignment_statement
				| if_statement
				| do_statement
				| while_statement
				| for_statement
				| write_statement
				| read_statment
				;
assignment_statement : expression ASSIGNMENT ID
						;
if_statement : IF conditional THEN statement_list ENDIF
					| IF conditional THEN statement_list ELSE statement_list ENDIF
					;
do_statement : DO statement_list WHILE conditional ENDDO
				;
while_statement : WHILE conditional DO statement_list ENDWHILE
				;
for_statement : FOR ID IS expression BY expression TO expression DO statement_list ENDFOR
				;
write_statement : WRITE OPEN_BRACKET output_list CLOSE_BRACKET
					| NEWLINE
					;
read_statment : READ OPEN_BRACKET ID CLOSE_BRACKET
				;
output_list : value
				| value COMMA output_list
				;
conditional : NOT conditional
				| expression comparator expression AND conditional
				| expression comparator expression OR conditional
				| expression comparator expression
				;
comparator : EQUAL
				| LESS_GREATER_THAN
				| LESS_THAN
				| GREATER_THAN
				| LESS_THAN_EQUAL
				| GREATER_THAN_EQUAL
				;
expression : term
				| term PLUS expression
				| term MINUS expression
				;
term : value
		| value MULTIPLY term
		| value DIVIDE term
		;
value : ID
			| constant
			| OPEN_BRACKET expression CLOSE_BRACKET
			;
constant : number_constant
			| CHAR
			;

number_constant : NUMBER
					| MINUS NUMBER 
					| NUMBER DOT NUMBER
					| MINUS NUMBER DOT NUMBER 
					;
%%
#include "lex.yy.c"

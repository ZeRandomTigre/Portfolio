%{

#include <stdio.h>
#include <stdlib.h>

#define SYMTABSIZE     50
#define IDLENGTH       15
#define NOTHING        -1
#define INDENTOFFSET    2

enum ParseTreeNodeType { PROGRAM, BLOCK, DECLARATION_BLOCK, IDENTIFIER_LIST, TYPE_T, STATEMENT_LIST, STATEMENT, ASSIGNMENT_STATEMENT,
							IF_STATEMENT, DO_STATEMENT, WHILE_STATEMENT,
							FOR_STATEMENT, WRITE_STATEMENT, READ_STATMENT, OUTPUT_LIST, CONDITIONAL, COMPARATOR, EXPRESSION, 
							TERM, VALUE, CONSTANT, NUMBER_CONSTANT};
							
char *NodeName[] = { "PROGRAM", "BLOCK", "DECLARATION_BLOCK", "IDENTIFIER_LIST", "TYPE_T", "STATEMENT_LIST", "STATEMENT", "ASSIGNMENT_STATEMENT",
							"IF_STATEMENT", "DO_STATEMENT", "WHILE_STATEMENT",
							"FOR_STATEMENT", "WRITE_STATEMENT", "READ_STATMENT", "OUTPUT_LIST", "CONDITIONAL", "COMPARATOR", "EXPRESSION", 
							"TERM", "VALUE", "CONSTANT", "NUMBER_CONSTANT"};
							
/* 
   Some constants.
*/
										   
#ifndef TRUE
#define TRUE 1
#endif

#ifndef FALSE
#define FALSE 0
#endif

#ifndef NULL
#define NULL 0
#endif

/* ------------- parse tree definition --------------------------- */

struct treeNode {
    int  item;
    int  nodeIdentifier;
    struct treeNode *first;
    struct treeNode *second;
    struct treeNode *third;
  };

typedef  struct treeNode TREE_NODE;
typedef  TREE_NODE        *TERNARY_TREE;

/* ------------- forward declarations --------------------------- */

TERNARY_TREE create_node(int,int,TERNARY_TREE,TERNARY_TREE,TERNARY_TREE);
void PrintTree(TERNARY_TREE t);

/* ------------- symbol table definition --------------------------- */

struct symTabNode {
    char ID[IDLENGTH];
};

typedef  struct symTabNode SYMTABNODE;
typedef  SYMTABNODE        *SYMTABNODEPTR;

SYMTABNODEPTR  symTab[SYMTABSIZE]; 

int currentSymTabSize = 0;

%}

/****************/
/* Start symbol */
/****************/

%start  program

/**********************/
/* Action value types */
/**********************/

%union {
    int iVal;
    TERNARY_TREE  tVal;
}

%token<iVal> ID NUMBER CHAR

%token COLON FULLSTOP COMMA SEMICOLON ASSIGNMENT OPEN_BRACKET CLOSE_BRACKET 
		LESS_THAN GREATER_THAN LESS_THAN_EQUAL GREATER_THAN_EQUAL PLUS MINUS EQUAL
		MULTIPLY DIVIDE OPEN_QUOTE CLOSE_QUOTE LESS_GREATER_THAN
		DECLARATIONS CODE OF TYPE CHARACTER INTEGER REAL IF ENDIF 
		THEN ELSE DO WHILE ENDDO ENDWHILE FOR IS BY TO ENDFOR
		NEWLINE READ WRITE NOT AND OR ENDP
		
%type<tVal> program block declaration_block identifier_list type statement_list
		statement assignment_statement if_statement do_statement for_statement while_statement 
		write_statement read_statment output_list conditional comparator 
		expression term value constant number_constant	

%%
program : ID COLON block ENDP ID FULLSTOP
			{ 
				TERNARY_TREE ParseTree;
				ParseTree = create_node($1, PROGRAM, $3, NULL, NULL);
				PrintTree(ParseTree);
			}
			;
block : DECLARATIONS declaration_block CODE statement_list
		{			
			$$ = create_node(NOTHING, BLOCK, $2, $4, NULL);
		}
		| CODE statement_list
		{			
			$$ = create_node(NOTHING, BLOCK, $2, NULL, NULL);
		}
		;
declaration_block : identifier_list OF TYPE type SEMICOLON
					{			
						$$ = create_node(NOTHING, DECLARATION_BLOCK, $1, $4, NULL);
					}
					| identifier_list OF TYPE type SEMICOLON declaration_block
					{			
						$$ = create_node(NOTHING, DECLARATION_BLOCK, $1, $4, $6);
					}						
					;
identifier_list : ID
				{			
					$$ = create_node($1, IDENTIFIER_LIST, NULL, NULL, NULL);
				}
                | ID COMMA identifier_list
				{
					$$ = create_node($1, IDENTIFIER_LIST, $3, NULL, NULL);
				}
				;
type : CHARACTER
		{
			$$ = create_node(NOTHING, TYPE_T, NULL, NULL, NULL);
		}
		| INTEGER
		{
			$$ = create_node(NOTHING, TYPE_T, NULL, NULL, NULL);
		}
		| REAL
		{
			$$ = create_node(NOTHING, TYPE_T, NULL, NULL, NULL);
		}
		;
statement_list : statement
				{
					$$ = create_node(NOTHING, STATEMENT_LIST, $1, NULL, NULL);
				}
				| statement SEMICOLON statement_list
				{
					$$ = create_node(NOTHING, STATEMENT_LIST, $1, $3, NULL);
				}
				;
statement : assignment_statement
			{
				$$ = create_node(NOTHING, STATEMENT, $1, NULL, NULL);
			}
			| if_statement
			{
				$$ = create_node(NOTHING, STATEMENT, $1, NULL, NULL);
			}
			| do_statement
			{
				$$ = create_node(NOTHING, STATEMENT, $1, NULL, NULL);
			}
			| while_statement
			{
				$$ = create_node(NOTHING, STATEMENT, $1, NULL, NULL);
			}
			| for_statement
			{
				$$ = create_node(NOTHING, STATEMENT, $1, NULL, NULL);
			}
			| write_statement
			{
				$$ = create_node(NOTHING, STATEMENT, $1, NULL, NULL);
			}
			| read_statment
			{
				$$ = create_node(NOTHING, STATEMENT, $1, NULL, NULL);
			}
			;
assignment_statement : expression ASSIGNMENT ID
						{
							$$ = create_node($3, ASSIGNMENT_STATEMENT, $1, NULL, NULL);
						}
						;
if_statement : IF conditional THEN statement_list ENDIF
				{
					$$ = create_node(NOTHING, IF_STATEMENT, $2, $4, NULL);
				}
				| IF conditional THEN statement_list ELSE statement_list ENDIF
				{
					$$ = create_node(NOTHING, IF_STATEMENT, $2, $4, $6);
				}
				;
do_statement : DO statement_list WHILE conditional ENDDO
				{
					$$ = create_node(NOTHING, DO_STATEMENT, $2, $4, NULL);
				}
				;
while_statement : WHILE conditional DO statement_list ENDWHILE
				{
					$$ = create_node(NOTHING, WHILE_STATEMENT, $2, $4, NULL);
				}
				;
for_statement : FOR ID IS expression BY expression TO expression DO statement_list ENDFOR
				{
					$$ = create_node($2, FOR_STATEMENT, $4, $10, NULL);
				}
				;
write_statement : WRITE OPEN_BRACKET output_list CLOSE_BRACKET
					{
						$$ = create_node(NOTHING, WRITE_STATEMENT, $3, NULL, NULL);
					}
					| NEWLINE
					{
						$$ = create_node(NOTHING, WRITE_STATEMENT, NULL, NULL, NULL);
					}
					;
read_statment : READ OPEN_BRACKET ID CLOSE_BRACKET
				{
					$$ = create_node($3, READ_STATMENT, NULL, NULL, NULL);
				}
				;
output_list : value				
				{
					$$ = create_node(NOTHING, OUTPUT_LIST, $1, NULL, NULL);
				}
				| value COMMA output_list				
				{
					$$ = create_node(NOTHING, OUTPUT_LIST, $1, $3, NULL);
				}
				;
conditional : NOT conditional								
				{
					$$ = create_node(NOTHING, CONDITIONAL, $2, NULL, NULL);
				}
				| expression comparator expression AND conditional												
				{
					$$ = create_node(NOTHING, CONDITIONAL, $1, $2, $5);
				}
				| expression comparator expression OR conditional												
				{
					$$ = create_node(NOTHING, CONDITIONAL, $1, $2, $5);
				}
				| expression comparator expression												
				{
					$$ = create_node(NOTHING, CONDITIONAL, $1, NULL, NULL);
				}
				;
comparator : EQUAL
				{
					$$ = create_node(NOTHING, COMPARATOR, NULL, NULL, NULL);
				}
				| LESS_GREATER_THAN				
				{
					$$ = create_node(NOTHING, COMPARATOR, NULL, NULL, NULL);
				}
				| LESS_THAN
				{
					$$ = create_node(NOTHING, COMPARATOR, NULL, NULL, NULL);
				}
				| GREATER_THAN
				{
					$$ = create_node(NOTHING, COMPARATOR, NULL, NULL, NULL);
				}
				| LESS_THAN_EQUAL
				{
					$$ = create_node(NOTHING, COMPARATOR, NULL, NULL, NULL);
				}
				| GREATER_THAN_EQUAL
				{
					$$ = create_node(NOTHING, COMPARATOR, NULL, NULL, NULL);
				}
				;
expression : term
				{
					$$ = create_node(NOTHING, EXPRESSION, $1, NULL, NULL);
				}
				| term PLUS expression
				{
					$$ = create_node(NOTHING, EXPRESSION, $1, $3, NULL);
				}
				| term MINUS expression
				{
					$$ = create_node(NOTHING, EXPRESSION, $1, $3, NULL);
				}
				;
term : value
		{
			$$ = create_node(NOTHING, TERM, $1, NULL, NULL);
		}
		| value MULTIPLY term		
		{
			$$ = create_node(NOTHING, TERM, $1, $3, NULL);
		}
		| value DIVIDE term		
		{
			$$ = create_node(NOTHING, TERM, $1, $3, NULL);
		}		;
value : ID
		{
			$$ = create_node($1, VALUE, NULL, NULL, NULL);
		}
		| constant
		{
			$$ = create_node(NOTHING, VALUE, $1, NULL, NULL);
		}
		| OPEN_BRACKET expression CLOSE_BRACKET
		{
			$$ = create_node(NOTHING, VALUE, $2, NULL, NULL);
		}
		;
constant : number_constant
			{
				$$ = create_node(NOTHING, CONSTANT, $1, NULL, NULL);
			}
			| CHAR
			{
				$$ = create_node(NOTHING, CONSTANT, NULL, NULL, NULL);
			}
			;

number_constant : NUMBER
					{
						$$ = create_node(NOTHING, NUMBER_CONSTANT, NULL, NULL, NULL);
					}
					| MINUS NUMBER 
					{
						$$ = create_node(NOTHING, NUMBER_CONSTANT, NULL, NULL, NULL);
					}
					| NUMBER FULLSTOP NUMBER
					{
						$$ = create_node(NOTHING, NUMBER_CONSTANT, NULL, NULL, NULL);
					}
					| MINUS NUMBER FULLSTOP NUMBER 
					{
						$$ = create_node(NOTHING, NUMBER_CONSTANT, NULL, NULL, NULL);
					}
					;
%%

/* Code for routines for managing the Parse Tree */

TERNARY_TREE create_node(int ival, int case_identifier, TERNARY_TREE p1,
			 TERNARY_TREE  p2, TERNARY_TREE  p3)
{
    TERNARY_TREE t;
    t = (TERNARY_TREE)malloc(sizeof(TREE_NODE));
    t->item = ival;
    t->nodeIdentifier = case_identifier;
    t->first = p1;
    t->second = p2;
    t->third = p3;
    return (t);
}


/* Put other auxiliary functions here */

void PrintTree(TERNARY_TREE t)
{
	if (t == NULL) return;
	if (t->item != NOTHING)
	{
		if (t->nodeIdentifier == NUMBER)
			printf("Number: %d ", t->item);
		else if (t->nodeIdentifier == ID)
			if (t->item > 0 && t->item < SYMTABSIZE)
				printf("ID: %s ", symTab[t->item]->ID);
			else
				printf("Unknown ID: %d ",t->item);
	}
	if (t->nodeIdentifier < 0 || t->nodeIdentifier > sizeof(NodeName))
		printf("Unknown NodeIdentifier: %d\n", t->nodeIdentifier);
	else
		printf("NodeIdentifier: %s\n",NodeName[t->nodeIdentifier]);
	PrintTree(t->first);
	PrintTree(t->second);
	PrintTree(t->third);
}

#include "lex.yy.c"

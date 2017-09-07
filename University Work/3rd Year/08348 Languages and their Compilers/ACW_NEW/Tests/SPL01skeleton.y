/* SPL01.y - SPL01 parser */
/* Author: Peter Parsons */
/* Revision: Oct 08 BCT */

/* Skeleton yacc file that can be used */
/* This file shows how a parser could build up a parse tree

   Do not use this until you need to, and understand some
   of the material on tree building and walking.             */

/* declare some standard headers to be used to import declarations
   and libraries into the parser. */
%{

#include <stdio.h>
#include <stdlib.h>

/* 
   Some constants.
*/

  /* These constants are used later in the code */
#define SYMTABSIZE     50
#define IDLENGTH       15
#define NOTHING        -1
#define INDENTOFFSET    2

  enum ParseTreeNodeType { PROGRAM, BLOCK } ;  
                          /* Add more types here, as more nodes
                                           added to tree */

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

/* ------------- symbol table definition --------------------------- */

struct symTabNode {
    char identifier[IDLENGTH];
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

/* We can declare types of tree nodes */

/* These are the types of lexical tokens -> iVal */
%token<iVal> IDENTIFIER 
/* Some tokens do not return a value */
%token COLON ENDP FULLSTOP

/* Whereas Rules return a tVal type (Tree) */
%type<tVal> program block

%%
/* ----------------------- grammar rules --------------------------- */

program      : identifier COLON block ENDP identifier FULLSTOP
               { TERNARY_TREE ParseTree;
	       /* This is an example, but the types are wrong 
		  ParseTree = create_node(NOTHING,PROGRAM,$1,$3,$5); */

	       /* We should probably check that $1 has the same name as $5 */
                                             IDENTIFIER             IDENTIFIER

               /* A better way would be
                  ParseTree = create_node($1, PROGRAM, $3, NULL, NULL) ; */
               }
             ;

     /* Now define block and all the other rules .... */

block       : ....
              {  /* here we continue building the tree by assigning to
                    $$ which is the LHS and thus adds a branch */
		$$ = create_node(NOTHING, BLOCK, .... ) ;
              }

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

#include "lex.yy.c"


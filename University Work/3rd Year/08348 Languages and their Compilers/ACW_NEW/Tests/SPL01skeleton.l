/*    file:     SPL01skeleton.l */
/*    author:   Peter Parsons   */
/*    date:     February 1997   */
/*    Revision: BCT Nov 13  - Added CR   */

/* Skeleton lex file that can be used */
/* This file shows how a symbol table can be linked to a lexer 

   Do not use this until you need to, and understand some
   of the material on the Symbol Table                      */

/* Declare Symbol Table Type and Array as imported types */
%{
#include <string.h>
extern SYMTABNODEPTR symTab[SYMTABSIZE];
extern int currentSymTabSize;
%}

/* Place your Regular expressions for lexemes here */
/* The following lines are examples */

delim		[ \t\n\r]
ws		{delim}+
letter		[A-Za-z]
digit		[0-9]
  :
  :
  :

%%

{ws}             ; /* Place your Token actions here, like this example */

  :
  :
  :

{identifier}	{
                  /* This is an example of putting an identifier
                     in a symbol table */
                  yylval.iVal = installId(yytext);
                  /* and return to the parser the lexer token type */
                  return (IDENTIFIER);
		};

{number}         {
                  /* This is an example of handling a number conversion
                     to preserve all the bits by storing as a negative */
                   yylval.iVal = 0l - atol(yytext);
				   return (NUMBER);
                  }

%%

/* Here is the code for the library of symbol table routines */

/* code for a simple symbol table, which is an array of pointers to
   structs, each of which contains an identifier.
*/


SYMTABNODEPTR newSymTabNode()
{
    return ((SYMTABNODEPTR)malloc(sizeof(SYMTABNODE)));
}


/* Look up an identifier in the symbol table, if its there return
   its index.  If its not there, put it in the end position,
   as long as the table isn't full, and return its index.
*/

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
       strncpy(symTab[currentSymTabSize]->identifier,id,IDLENGTH);
       symTab[currentSymTabSize]->identifier[IDLENGTH-1] = '\0';
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
        if(strncmp(s,symTab[i]->identifier,IDLENGTH) == 0)
        {
            return (i);
        }
    }
    return (-1);    
}


%{
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
extern int yylineno;

#define YYDEBUG 1

int yylex();
int yyerror(char *s);

%}

%locations

%expect 34

%token LET
%token IF
%token ELSE
%token WHILE
%token INT
%token BOOL
%token CHAR
%token PRINT
%token READ
%token COMPOSED_OPERATOR
%token IDENTIFIER
%token CONSTANT

%left '+' '-' '*' '/' '%' '<' '>'

%start program

%%
program : statement_list //{printf("program 0\n");}
	;
statement_list : statement statement_list // {printf("statement_list 0\n");}
	| statement //{printf("statement_list 1\n");}
	;
statement : declare_statement //{printf("statement 0\n");}
	| expression_statement //{printf("statement 1\n");}
	| ';' //{printf("statement 2\n");}
	;
declare_statement : LET IDENTIFIER ':' type //{printf("declare_statement 0\n");}
	;
expression_statement : expression ';' //{printf("expression_statement 2\n");}
	;
expression : CONSTANT
	| IDENTIFIER
	| assign_expression
	| if_expression
	| while_expression
	| print_expression
	| read_expression
	| binary_operator_expression
	| unary_operator_expression
	| group_expression
	| index_expression
	| array_expression
	;
block : '{' block_content '}'
	;
block_content : statement_list
	| statement_list expression
	;
assign_expression : IDENTIFIER '=' expression
	;
if_expression : IF '(' expression ')' block
	| IF '(' expression ')' block ELSE block
	;
while_expression : WHILE '(' expression ')' block
	;
print_expression : PRINT '(' expression ')'
	;
read_expression : READ
	;
index_expression : IDENTIFIER '[' expression ']'
	;
array_expression : '[' array_elements ']'
	;
array_elements : CONSTANT
	| CONSTANT ',' array_elements
	;
unary_operator_expression : unary_operator expression
	;
unary_operator : '-'
	| '!'
	;
binary_operator_expression : expression binary_operator expression
	;
binary_operator : COMPOSED_OPERATOR
	| arithmetic_operator
	| '>'
	| '<'
	;
arithmetic_operator : '+'
	| '-'
	| '*'
	| '/'
	| '%'
	;
group_expression : '(' expression ')'
;
type : basic_type
	| array_type
	;
basic_type : INT
	| CHAR
	| BOOL
	;
array_type : '[' type ';' CONSTANT ']'
;

%%

int yyerror(char *s)
{
	fprintf(stderr,"%s | line: %d\n\n", s, yylineno);
}


extern FILE *yyin;

int main(int argc, char **argv)
{
	yyparse ();
	printf("done");

	return 0;
}

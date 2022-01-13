%{
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define YYDEBUG 1

int yylex();
int yyerror(char *s);

%}

%expect 34

%token LET
%token IF
%token ELSE
%token WHILE
%token I32
%token BOOL
%token CHAR
%token PRINT
%token READ
%token COMPOSED_OPERATOR
%token IDENTIFIER
%token CONSTANT

%left "+" "-" "*" "/" "%" "<" ">" "<=" ">=" "==" "!="

%start program

%%
program : statement_list
;
statement_list : statement statement_list | statement
;
statement : declare_statement | expression_statement | ";"
;
declare_statement : LET IDENTIFIER ":" type ";"
;
expression_statement : expression ";"
;
expression : CONSTANT | IDENTIFIER | assign_expression | if_expression | while_expression | print_expression | read_expression | binary_operator_expression | unary_operator_expression | group_expression | index_expression | array_expression
;
block : "{" block_content "}"
;
block_content : statement_list | statement_list expression
;
assign_expression : IDENTIFIER "=" expression
;
if_expression : IF "(" expression ")" block | IF "(" expression ")" block ELSE block
;
while_expression : WHILE "(" expression ")" block
;
print_expression : PRINT "(" expression ")"
;
read_expression : READ
;
index_expression : IDENTIFIER "[" expression "]"
;
array_expression : "[" array_elements "]"
;
array_elements : CONSTANT | CONSTANT "," array_elements
;
unary_operator_expression : unary_operator expression
;
unary_operator : "-" | "!"
;
binary_operator_expression : expression binary_operator expression
;
binary_operator : COMPOSED_OPERATOR | arithmetic_operator | ">" | "<"
;
arithmetic_operator : "+" | "-" | "*" | "/" | "%"
;
group_expression : "(" expression ")"
;
type : basic_type | array_type
;
basic_type : I32 | CHAR | BOOL
;
array_type : "[" type ";" CONSTANT "]"
;

%%

int yyerror(char *s)
{
	printf("%s\n",s);
}


extern FILE *yyin;

int main(int argc, char **argv)
{
	if(argc>1) yyin :  fopen(argv[1],"r");
	if(argc>2 && !strcmp(argv[2],"-d")) yydebug: 1;
	if(!yyparse()) fprintf(stderr, "\tO.K.\n");

	return 0;
}

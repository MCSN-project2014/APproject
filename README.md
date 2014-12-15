Advanced Programming Project 2014
======================================================
FunW@P - An Imperative-Functional Programming Language
======================================================

####The grammar
#####Token
FUN 						= "fun"
RET							= "return"
COMMA 						= ","
OBRA						= "("
CBRA						= ")"
VAR 						= "var"
EQ							= "="
SEMICOL						= ";"
IF 							= "if"
ELSE 						= "else"
MAINT						= "main"
PRINT						= "println"
FOR 						= "for"
BOOLT						= "bool"
BOOLV 						= <<true|false>>
INTT						= "int"
ASYNC						= "async"
ADD                         = "+"
SUB                        	= "-"
MUL                         = "*"
DIV                         = "/"
OCURL		                = "{"
CCURL                       = "}"
ISEQ						= "=="
ISDIFF						= "!="
LT 							= "<"
LTE 						= "<="
GT 							= ">"
GTE 					    = ">="
NOT							= "!"
NUMBER                      = <<[0-9]+>>
IDEN			    = <<[a-zA-Z][a-zA-Z0-9]*>>
WHITESPACE                  = <<[ \t\n\x0B\f\r| \t\n\r]+>> %ignore%


#####Productions
Program = [FDecList] Main ;
FDecList = FDec [FDecList] ;
FDec    =  "fun" Ide "(" [DParamList] ")" RType Block ;

DParamList = Ide Type [DParamList1] ;
DParamList1 = "," Ide Type [DParamList1] ;


Main = "fun" "main" "(" ")" Block ;
Block = "{" [StmtList] "}"  ;


RType = BOOLT | "int" | "fun" "(" TypeList ")" RType ;
Type  = BOOLT | "int" | "fun" ;



StmtList = Stmt [StmtList] | Decl ";" [StmtList] ;
Decl = "var" Ide Type ;

Stmt = "var" Ide Type  "=" AExp ";"
       | Ide "=" AExp ";"
       | "println" "(" Exp ")" ";"
       | "if" Exp Block ["else" Block]
       | "for" Ide "=" Exp ";" Exp ";" Ide "=" Exp Block
       | "return" FExp ";" ;



TypeList = Type [TypeList1];
TypeList1 = "," Type [TypeList1];


AExp  = FExp | "async" Block ;
FExp  = Exp  | "fun""(" [DParamList] ")" RType Block;
Exp   = Rel [Exp1];
Exp1  = "==" Rel [Exp1] | "!=" Rel [Exp1];
 
Rel   = Expr [Reltail]  ;
Reltail   = "<" Expr | "<=" Expr | ">=" Expr | ">" Expr;
Expr  = Term [Expr1];
Expr1  = "+" Term [Expr1]  | "-" Term [Expr1];


Term  = Unary [Term1];
Term1  = "*" Unary [Term1] | "/" Unary [Term1];

Unary = "!" Unary| "-" Unary| Factor;

Factor = Atom | "(" Exp ")" | CFunc ;


Atom   = NUMBER | IDEN | BOOLV ;
CFunc  = Ide "("[ParamList]")"";";

ParamList = FExp [ParamList1] ;
ParamList1 = "," FExp [ParamList1] ;

Ide = IDEN;

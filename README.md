#FunW@P - An Imperative-Functional Programming Language

##Slide and Other Materials
* Our Slide:
  * [slide Agile Teamworker](http://1drv.ms/1IUn2yB)
  * [slide of FunW@P – The Project](http://1drv.ms/1ySTo4I)
  * [Text of the Exercise](http://1drv.ms/1IUnygb)
* User Guide:
  * [C# Programming Guide](http://msdn.microsoft.com/it-it/library/67ef8sbd.aspx)
  * [Coco/r User Manual](http://www.ssw.uni-linz.ac.at/Coco/Doc/UserManual.pdf)
  * [F# Language Reference](http://msdn.microsoft.com/en-us/library/dd233181.aspx)
* Tutorial:
  * [C# Tutorial list](http://msdn.microsoft.com/en-us/library/aa288436%28v=vs.71%29.aspx)
  * [Coco/r Tutorial](http://structured-parsing.wikidot.com/coco-r-parser-with-internal-scanner-part-1)
  * [F# Walkthrough](http://msdn.microsoft.com/en-us/library/dd233160.aspx)

##The grammar
####Token
```
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
```

####Productions
```
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
```

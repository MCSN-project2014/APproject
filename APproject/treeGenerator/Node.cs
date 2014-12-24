using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APproject.treeGenerator
{
    class  Node
    {
        private String nameConstruct;

        public Node(String n) 
        { nameConstruct = n; }

        public Node() 
        { }
        public String  getname()
        { return nameConstruct;}

    }
    
    class Stmt : Node 
    { 
        public Stmt () : base ()
        { }
    }

    class Exp : Stmt
    {
        private Types rType;    // for future type cecking : bool or int
        private String expType ; // Aexp or Exp or FExp

        public Exp ( Types rtype, String etype){
     
            rType = rtype;
            expType = etype;
        }

        public Types getReturnType()
        {
            return rType;
        }
    }
    


    class If : Stmt
    {
        private Exp expr;
        private Stmt stmt;
        
        public If( Exp e, Stmt s)
        {
            expr = e;
            stmt = s;
            if (expr.getReturnType() != Types.boolean) throw new Exception("Incorrect Type");
        }

    }

    class Else : Stmt
    {
        private Exp expr;
        private Stmt stmt1;
        private Stmt stmt2;

        public Else(Exp e, Stmt s1, Stmt s2)
        {
            expr = e; 
            stmt1 = s1;  
            stmt2=s2;
            if (expr.getReturnType() != Types.boolean) throw new Exception("Incorrect Type");
        }

    }
    class For : Stmt
    {
        private Exp exprStart;
        private Exp exprMiddle;
        private Exp stmtEnd;
        private Stmt stmtf;
        
        public For(Exp eStart, Exp eMiddle, Exp eEnd, Stmt s)
        {
            exprStart = eStart;
            exprMiddle = eMiddle;
            stmtEnd = eEnd;
            stmtf = s;
           // if (expr.getType() != Types.boolean) throw new Exception("Incorrect Type");
        }

    }

    class Decl : Stmt 
    {
        public String ide;
        public Exp exp;
        public String declType;    // declaration with async{} keyword or not 

        public Decl(String id, Exp expd, String dType)
        {
            ide = id;
            exp = expd;
            declType = dType;
        }

    }
 

   

}

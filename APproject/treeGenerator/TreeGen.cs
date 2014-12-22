using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APproject.treeGenerator
{
    class TreeGen
    {
        public Node root;

        public TreeGen(){}
    

        public Node createExp( String n, Types rtype, String etype){ return new Exp(n,rtype,etype);}
        public Node createIf(Exp e, Stmt s) {return new If( e, s );}
        public Node createElse(Exp e, Stmt s1, Stmt s2){return new Else( e, s1, s2);}
        public Node createFor(Exp eStart, Exp eMiddle, Exp eEnd, Stmt s) { return new For(eStart, eMiddle, eEnd, s); }
        public Node createDecl(String id, Exp expd, String dType){return new Decl(id, expd,  dType);}
        }
    }
}

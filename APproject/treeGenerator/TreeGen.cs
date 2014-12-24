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
        public Node currentNode;
        public TreeGen()
        {
            root = new Node();
            currentNode = root;
        }

        public Exp createExp(Types rtype, String etype)
        { return new Exp(rtype, etype); }

        public If createIf(Exp e, Stmt s)
        { return new If(e, s); }

        public Else createElse(Exp e, Stmt s1, Stmt s2)
        { return new Else(e, s1, s2); }

        public For createFor(Exp eStart, Exp eMiddle, Exp eEnd, Stmt s)
        { return new For(eStart, eMiddle, eEnd, s); }

        public Decl createDecl(String id, Exp expd, String dType)
        { return new Decl(id, expd, dType); }
    }
}
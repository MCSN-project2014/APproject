using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace APproject.FSCodeGenerator
{
    class FSCodeGenTestModule
    {
        static public Node createASTIf( )
        {
            Node If = new Node ( Labels.If);
			Node condition = new Node (Labels.Gte);
			If.addChildren (condition);
			Obj term1 = new Obj ();
			term1.type = Types.integer;
            term1.name = "s";
			term1.kind = Kinds.var;
			Obj term2 = new Obj ();
			term2.type = Types.integer;
			term2.kind = Kinds.var;
            term2.name = "z";
			condition.addChildren (new Node(new Term(term1)));
			condition.addChildren (new Node(new Term(term2)));
			Node then = new Node (Labels.Block);
			If.addChildren (then);
			Obj term3 = new Obj ();
			term3.type = Types.integer;
			term3.kind = Kinds.var;
            term3.name = "z";
			then.addChildren (new Node(new Term (term3)));

            return If;

        }
        static public Node createAST()
        {    /** create a sample AST for:
              *
              * fun add ( x int , y int ){  //need the Parameters Node label
              *     return x + y;
              * }
              * 
              * fun minus ( x int , y int ){  
              *     return x-y;
              * }
              * 
              * fun main (){
              *     var t = 20;
              *     var r = 10
              *     var d = add( t , r);
              *     if  t > 20 {
              *       r = 0; 
              *     }
              *     else r = 1;
              * }
              * 
              * */
          //  Node sum = new Node(Labels.FunDecl);
           // Node minus = new Node(Labels.FunDecl);

			Node main = new Node (Labels.Main);

            Node assT = new Node(new Term( Labels.AssigDecl);
            assT.term.variable.name = "t";
            Node valueT = new Node(Labels.Term);
            valueT.term.integer = 20;
            assT.addChildren(valueT);
            Node assR = new Node(Labels.AssigDecl);
            assT.term.variable.name = "r";
            Node valueR = new Node(Labels.Term);
            valueR.term.integer = 10;
            assT.addChildren(valueR);


            Node If = new Node(Labels.If);
			Node condition = new Node (Labels.Gte);
            Node T = new Node(Labels.Term);
            T.term.variable.name = "t";
            Node ceckT = new Node(Labels.Term);
            valueT.term.integer = 20;
			condition.addChildren (T);
            condition.addChildren(ceckT);
            Node then = new Node(Labels.Block);
            Node assignR = new Node(Labels.AssigDecl);
            assignR.term.variable.name = "r";
            Node valR = new Node(Labels.Term);
            valR.term.integer = 0;
            then.addChildren(assignR);
            then.addChildren(valR);
            Node elsee = new Node(Labels.Block);
            Node assignR1 = new Node(Labels.AssigDecl);
            assignR1.term.variable.name = "r";
            Node valR1 = new Node(Labels.Term);
            valR1.term.integer = 1;
            assignR1.addChildren(valR1);
            elsee.addChildren(assignR1);

            If.addChildren(condition);
            If.addChildren(then);
            If.addChildren(elsee);



            main.addChildren(assT);
            main.addChildren(assR);
            main.addChildren(If);

            return main;
        }

        static public Node createAST1()
        {
            Term t1 = new Term(42);
            Term t2 = new Term(10);
            Node min = new Node(Labels.Minus);
            Node plus = new Node(Labels.Plus);
            Node NodeT1 = new Node(t1);
            Node NodeT2 = new Node(t2);

            plus.addChildren(NodeT1);
            plus.addChildren(min);
            min.addChildren(NodeT1);
            min.addChildren(NodeT2);

            return plus;

        }

        static public Node createAST2()
        {
            Term t1 = new Term(42);
            Term t2 = new Term(10);
            Node min = new Node(Labels.Minus);
            Node async = new Node(Labels.Async);
            Node NodeT1 = new Node(t1);
            Node NodeT2 = new Node(t2);

            async.addChildren(min);
            min.addChildren(NodeT1);
            min.addChildren(NodeT2);

            return async;

        }
        public void printAST(Node node)
        {
            if (node != null)
            {
                if (node.term != null)
                    Console.WriteLine(node.term);
                else
                    Console.WriteLine(node.label);
                foreach (Node n in node.getChildren())
                {
                    printAST(n);
                }
            }
        }

        static void Main(string[] args)
        {
            String fileName = "traslated_file";
            FSCodeGen gen = new FSCodeGen(fileName);
            //Node root = createAST2();
          //  gen.translate(root);

            //Node root = createASTIf();
            //gen.translate(root);

            Node root = createAST();
            gen.translate(root);
        }

    }
}

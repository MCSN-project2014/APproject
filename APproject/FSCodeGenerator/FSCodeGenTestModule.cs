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

        static public Node createASTif()
        { /* if ( t == False ){
           *    s = 0;
           *    }
           *  else s = 1;
           * 
           * */
            Node If = new Node(Labels.If);

            Node condition = new Node(Labels.Eq);
            condition.children.Add(new Term(new Obj { name = "t" }));
            condition.children.Add(new Term(true));

            Node then = new Node(Labels.Block);
            Node assign = new Node(Labels.Assig);
            assign.addChildren(new Term(new Obj { name = "s" }));
            assign.addChildren(new Term(0));
            then.addChildren(assign);

            Node Else = new Node(Labels.Block);
            Node assign1 = new Node(Labels.Assig);
            Else.addChildren(assign1);
            assign1.addChildren(new Term(new Obj { name = "s" }));
            assign1.addChildren(new Term(1));

            If.addChildren(condition);
            If.addChildren(then);
            If.addChildren(Else);

            return If;

        }

        static public Node createASTfunDecl()
        {
            /* 
             * fun add ( x int, y int ) int {
             *  return  x + y
             *  }
             * 
             * fun main (){
             *     var x = 20;
             *     var y = 10
             *     var r = add( x, y);
             *
             * */

            Node program = new Node(Labels.Program);
            String sumObj = "sum";
            Node sumDec = new Node(Labels.FunDecl, sumObj);
            Node block = new Node(Labels.Block);
            Node returnType = new Node(Labels.Return);
            returnType.addChildren(new Term(new Obj { name = "int" }));
            Node returnSum = new Node(Labels.Return);
            Node sumOp = new Node(Labels.Plus);
            Obj varx = new Obj { name = "x", type = Types.integer };
            Obj vary = new Obj { name = "y" };
            returnSum.addChildren(sumOp);
            sumDec.addChildren(block);
            sumDec.addChildren(returnType);
            sumDec.addChildren(new Term(varx));
            sumDec.addChildren(new Term(vary));
            block.addChildren(sumOp);
            sumOp.addChildren(new Term(varx));
            sumOp.addChildren(new Term(vary));

            program.addChildren(sumDec);

            Node main = new Node(Labels.Main);

            Node assX = new Node(Labels.AssigDecl);
            Term X = new Term(new Obj { name = "x" });
            Term valueX = new Term("20");
            assX.addChildren(X);
            assX.addChildren(valueX);
            Node assY = new Node(Labels.AssigDecl);
            Term Y = new Term(new Obj { name = "y" });
            Term valueY = new Term("10");
            assY.addChildren(Y);
            assY.addChildren(valueY);
            Node assFunSum = new Node(Labels.AssigDecl);
            Term R = new Term(new Obj { name = "r" });
            Node sumCall = new Node(Labels.FunCall);
            sumCall.addChildren(new Term(new Obj { name = "sum" }));
            sumCall.addChildren(new Term(new Obj { name = "x" }));
            sumCall.addChildren(new Term(new Obj { name = "y" }));
            assFunSum.addChildren(R);
            assFunSum.addChildren(sumCall);

            main.addChildren(assX);
            main.addChildren(assY);
            main.addChildren(assFunSum);

            program.addChildren(main);
            return program;


        }
      /*
        //static public Node createAST1()
        //{
        //    Term t1 = new Term(42);
        //    Term t2 = new Term(10);
        //    Node min = new Node(Labels.Minus);
        //    Node plus = new Node(Labels.Plus);
        //    Node NodeT1 = new Node(t1);
        //    Node NodeT2 = new Node(t2);

        //    plus.addChildren(NodeT1);
        //    plus.addChildren(min);
        //    min.addChildren(NodeT1);
        //    min.addChildren(NodeT2);

        //    return plus;

        //}

        //static public Node createAST2()
        //{
        //    Term t1 = new Term(42);
        //    Term t2 = new Term(10);
        //    Node min = new Node(Labels.Minus);
        //    Node async = new Node(Labels.Async);
        //    Node NodeT1 = new Node(t1);
        //    Node NodeT2 = new Node(t2);

        //    async.addChildren(min);
        //    min.addChildren(NodeT1);
        //    min.addChildren(NodeT2);

        //    return async;

        //}
        //public void printAST(Node node)
        //{
        //    if (node != null)
        //    {
        //        if (node.term != null)
        //            Console.WriteLine(node.term);
        //        else
        //            Console.WriteLine(node.label);
        //        foreach (Node n in node.getChildren())
        //        {
        //            printAST(n);
        //        }
        //    }
        //}
*/
        static void Main(string[] args)
        {
            String fileName = "traslated_file";
            FSCodeGen gen = new FSCodeGen(fileName);
            //  Node root = createAST2();
            //  gen.translate(root);

           // Node root = createASTif();
            //gen.translate(root);

            Node root = createASTfunDecl();
            gen.translate(root);

            //Node root = createAST();
            // gen.translate(root);
        }

    }
}

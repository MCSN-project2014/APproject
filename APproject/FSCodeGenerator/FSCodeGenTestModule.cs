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
        {   // fun add ( x int, y int ) int {
            //  
            // return  x + y
            // 
            //  }
            // 
     
        	Node program = new Node(Labels.Program);
            String sumObj =  "sum" ;
            Node sumDec = new Node(Labels.FunDecl, sumObj);
            Node block = new Node ( Labels.Block);
            Node returnType = new Node(Labels.Return);
            returnType.addChildren(new Term(new Obj { name  = "int" }));
            Node returnSum = new Node ( Labels.Return);
            Node sumOp = new Node( Labels.Plus  );
            Obj varx = new Obj{ name = "x", type= Types.integer};
            Obj vary = new Obj{ name = "y"};
            returnSum.addChildren(sumOp);
            sumDec.addChildren( block);
            sumDec.addChildren( returnType);
            sumDec.addChildren( new Term(varx));
            sumDec.addChildren( new Term(vary));
            block.addChildren(sumOp);
            sumOp.addChildren(new Term(varx));
            sumOp.addChildren(new Term(vary));
           
            program.addChildren(sumDec);
            return program;
/*
			Obj fun = new Obj{ name = "test" };
			Node funDec = new Node (Labels.FunDecl, fun);
			program.addChildren (funDec);
			Node block1 = new Node (Labels.Block);
			funDec.addChildren (block1);
			Obj varA = new Obj{ name = "a" };
			Obj varB = new Obj{ name = "b" };
			funDec.addChildren (new Term (varA));
			funDec.addChildren (new Term (varB));
			Node print = new Node (Labels.Print);
			block1.addChildren (print);
			Node minus = new Node (Labels.Minus);
			minus.addChildren (new Term(varA));
			minus.addChildren (new Term (varB));
			print.addChildren (minus);

			Node main = new Node (Labels.Main);
			program.addChildren (main);
			Node assDec = new Node (Labels.AssigDecl);
			main.addChildren (assDec);
			assDec.addChildren (new Term (new Obj{ name = "c" }));
			Node call = new Node (Labels.FunCall, fun);
			call.addChildren (new Term (7));
			call.addChildren (new Term (3));
			assDec.addChildren (call);

			return program;*/
         

        }
        /*
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

        /*	Node main = new Node (Labels.Main);

            Node assT = new Node(Labels.AssigDecl);
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
        */
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

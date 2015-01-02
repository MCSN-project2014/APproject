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
        static public Node createASTfor()
        {/**
          *  var a int = 0;
          *  
          *  for (int i=0 ; i < 10 ; i++ ) {
          *     a  = a + i ;
          *  }
          * 
          * */

            Node program = new Node(Labels.Program);
            Node assA = new Node(Labels.AssigDecl);
            Term A = new Term(new Obj { name = "a" });
            Term valueA = new Term("0");
            assA.addChildren(A);
            assA.addChildren(valueA);

            Node For = new Node(Labels.For);
            Node assFor = new Node(Labels.Assig);
            Term X = new Term(new Obj { name = "i" });
            Term valueX = new Term("0");
            assFor.addChildren(X);
            assFor.addChildren(valueX);
            Node expFor = new Node(Labels.Gt);
            expFor.addChildren(new Term(new Obj { name = "i" }));
            expFor.addChildren(new Term(20));

            Node block = new Node(Labels.Block);
            Node assBlock = new Node(Labels.Assig);
            Term Ablock = new Term(new Obj { name = "a" });
            Node ASum = new Node(Labels.Plus);
            ASum.addChildren(new Term(new Obj { name = "a" }));
            ASum.addChildren(new Term(new Obj { name = "i" }));
            assBlock.addChildren(Ablock);
            assBlock.addChildren(ASum);
            block.addChildren(assBlock);

            For.addChildren(assFor);
            For.addChildren(expFor);
            For.addChildren(new Node(Labels.Assig)); // it is not used in the translation phase to f#
            For.addChildren(block);

            program.addChildren(assA);
            program.addChildren(For);
            return program;

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
            Obj sumObj = new Obj() { name = "sum" };
            Node sumDec = new Node(Labels.FunDecl, sumObj);
            Node block = new Node(Labels.Block);
            Node returnSum = new Node(Labels.Return);
            Node sumOp = new Node(Labels.Plus);
            Obj varx = new Obj { name = "x", type = Types.integer };
            Obj vary = new Obj { name = "y" };
            returnSum.addChildren(sumOp);
            sumDec.addChildren(block);
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
            Node sumCall = new Node(Labels.FunCall, sumObj);
            sumCall.addChildren(new Term("x"));
            sumCall.addChildren(new Term("y"));
            assFunSum.addChildren(R);
            assFunSum.addChildren(sumCall);

            main.addChildren(assX);
            main.addChildren(assY);
            main.addChildren(assFunSum);

            program.addChildren(main);
            return program;


        }


        static public Node createASTasync()
        { /*
           * 
           * fun fib (n int) int {
           *    var a, b int;
           *          //We do not wait for fib(n-1)
           *    a = async{return bib(n-1)}
           *          // ... because we perform fib(n-2) in parallel
           *    b = async{return bib(n-2)}
           *         // but here we wait ....
           *    return a + b;
           * }
           * 
           **/
            Node program = new Node(Labels.Program);
            String fibObj = "Fib";
            Node fibDec = new Node(Labels.FunDecl, fibObj);
            Obj varN = new Obj{ name = "n" };
            Node returnType = new Node(Labels.Return);
            returnType.addChildren(new Term(new Obj { name = "int" }));
            Node block = new Node(Labels.Block);

            Node declA = new Node(Labels.Decl);
            Node declB = new Node(Labels.Decl);
            Term A = new Term(new Obj { name = "a" });
            Term B = new Term(new Obj { name = "b" });
            declA.addChildren(A);
            declB.addChildren(B);
            Node assigDeclA = new Node(Labels.Assig);
            Node assigDeclB = new Node(Labels.Assig);
            Node blockAsyncA = new Node(Labels.Async);
            Node returnAsyncA = new Node(Labels.Return);
            Node funCallA = new Node(Labels.FunCall);
            funCallA.addChildren(new Term("bib"));
            funCallA.addChildren(new Term("n-1"));
            returnAsyncA.addChildren(funCallA);
            blockAsyncA.addChildren(returnAsyncA);

            Node blockAsyncB = new Node(Labels.Async);
            Node returnAsyncB = new Node(Labels.Return);
            Node funCallB = new Node(Labels.FunCall);
            funCallB.addChildren(new Term("bib"));
            funCallB.addChildren(new Term("n-2"));
            returnAsyncB.addChildren(funCallB);
            blockAsyncB.addChildren(returnAsyncB);

            assigDeclA.addChildren(new Term("a"));
            assigDeclA.addChildren(blockAsyncA);

            assigDeclB.addChildren(new Term("b"));
            assigDeclB.addChildren(blockAsyncB);

            Node returnFib = new Node(Labels.Return);
            Node sumAB = new Node(Labels.Plus);
            sumAB.addChildren(new Term(new Obj { name = "a" }));
            sumAB.addChildren(new Term(new Obj { name = "b" }));

            block.addChildren(declA);
            block.addChildren(declB);
            block.addChildren(assigDeclA);
            block.addChildren(assigDeclB);
            block.addChildren(sumAB);

            fibDec.addChildren(block);
            fibDec.addChildren(new Term(varN));
            program.addChildren(fibDec);


            return program;
        }

        public static ASTNode  createASTafun()
        {

            /**
             * let a = fun(x int) int {
             *              sum + x
             * 
             * 
             * **/

            Node program = new Node(Labels.Program);
            Node assDeclAfun = new Node(Labels.AssigDecl);
           
            Node afun = new Node(Labels.Afun);
            Node blockAfun = new Node(Labels.Block);
            Node sum = new Node( Labels.Plus);
            sum.addChildren(new Term("sum"));
            sum.addChildren(new Term("x"));
            blockAfun.addChildren(sum);
            Node returnType = new Node(Labels.Return);
            returnType.addChildren(new Term("int"));

            afun.addChildren(blockAfun );
            afun.addChildren(new Term("x"));

            assDeclAfun.addChildren(new Term(" a"));
            assDeclAfun.addChildren(afun);
            

            program.addChildren(assDeclAfun);
            return program;
        }
        
        public static void printAST(ASTNode node)
        {
            if (node != null)
            {
                if (node.isTerminal())
                    Console.WriteLine(node);
                else
                {
                    Console.WriteLine(node);
                    foreach (ASTNode n in node.children)
                        printAST(n);
                }
            }
        }

        static void test(ASTNode root)
        {
            String fileName = "traslated_file";
            FSCodeGen gen = new FSCodeGen(fileName);
           // printAST(root);
            gen.translate(root);
            Console.ReadKey();
        }

        static void Main(string[] args)
        {

            // test(createASTif());
           //  test(createASTfunDecl());
           //  test(createASTfor());
            test(createASTasync());
           // test(createASTafun());



        }
    }
}


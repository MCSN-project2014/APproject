using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APproject.FSCodeGenerator
{   


    class MainTestFSCodeGen
    {
        /// <summary>
        /// It creates the first AST for the example:
        /// 
        /// fun main (){
        ///     var a int = 0;
        ///     var b int = 0;
        ///     var a_eq_b bool = a == b;
        ///     
        ///     for i = 0 ; i < 32; i++ {
        ///         a += i;
        ///     
        ///      if a_eq_b && a == 0 {
        ///         println("a is 0, so is b");
        ///      } else {
        ///         println("at leas one is not 0!");
        ///      }
        /// }
        /// </summary>
        static private Node AST1()
        {
            Node prog = new Node(Labels.Program);
            Node main = new Node(Labels.Main);
            prog.addChildren(main);

            // var a int = 0;
            // var b int = 0;
            Term varA = new Term(new Obj { name = "a" });
            Term varB = new Term(new Obj { name = "b" });
            Node assigA = new Node(Labels.AssigDecl);
            Node assigB = new Node(Labels.AssigDecl);
            assigA.addChildren(varA);
            assigB.addChildren(varB);
            assigA.addChildren(new Term(0));
            assigB.addChildren(new Term(0));

            // var a_eq_b bool = a == b;
            Term varAEqB = new Term(new Obj { name = "a_eq_b" });
            Node equiv = new Node(Labels.Eq);
            equiv.addChildren(varA);
            equiv.addChildren(varB);
            Node assigEquiv = new Node(Labels.AssigDecl);
            assigEquiv.addChildren(varAEqB);
            assigEquiv.addChildren(equiv);
            
            // ***********************TODO************************
            //for i = 0 ; i < 32; i++ {
            //  a += i;
            //************************TODO************************

            //if a_eq_b && a == 0 {
            //        println("a is 0, so is b");
            //    } else {
            //     println("at leas one is not 0!");
            //}

            Node ifN = new Node(Labels.If);
            Node and = new Node(Labels.And);
            Node isAZero = new Node(Labels.Eq);
            isAZero.addChildren(varA);
            isAZero.addChildren(new Term(0));
            and.addChildren(varAEqB);
            and.addChildren(isAZero);

            Node block1 = new Node(Labels.Block);
            Node block2 = new Node(Labels.Block);
            Node print1 = new Node(Labels.Print);
            Node print2 = new Node(Labels.Print);
            print1.addChildren(new Term(new Obj{name = " \"a is 0, so is b\" "}));
            print2.addChildren(new Term(new Obj{name = " \"either a or b is not zero\" "}));
            ifN.addChildren(and);
            block1.addChildren(print1);
            block2.addChildren(print2);
            ifN.addChildren(block1);
            ifN.addChildren(block2);

            main.addChildren(assigA);
            main.addChildren(assigB);
            main.addChildren(assigEquiv);
            main.addChildren(ifN);

            return prog;
        }

        static void Main(string[] args)
        {
            String fileName = "traslated_file";
            FSCodeGen gen = new FSCodeGen(fileName);

            Node root = AST1();
            gen.translate(root);
        }


    }
}

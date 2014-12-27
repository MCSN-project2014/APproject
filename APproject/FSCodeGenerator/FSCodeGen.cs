using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace APproject.FSCodeGenerator
{

    ///<summary>
    ///This class converts an AST of funW@P into the corresponding F#
    ///code, simply by visiting the tree.
    ///Output is written into a outputFileName.fs file, 
    ///result of the translation.
    ///</summary>
    public class FSCodeGen
    {
        private StreamWriter fileWriter;
        private int indentationLevel; //it stores the number of \t needed to get the perfect indentation ;)
        private string fileName;

        public FSCodeGen(string outputFileName)
        {
            indentationLevel = 0;
            fileName = outputFileName + ".fs";

            if (outputFileName == string.Empty)
            {
                Console.WriteLine("Invalid File Name");
            }
            else
            {
                try
                {
                    FileStream output = new FileStream(outputFileName, FileMode.OpenOrCreate, FileAccess.Write);
                }
                catch (IOException)
                {
                    Console.WriteLine("Error while creating the new file " + outputFileName + ".fs");
                }
            }
        }

        ///<summary>
        /// This method calls the necessary ones in order to translate 
        /// the AST into F# code.
        /// It is recursively called by all other methods.
        /// Just a big switch case.
        ///</summary>
        /// <param name="Node n">n.</param>
        public void translate(Node n) {
            switch (n.label)
            {
                case Labels.Main: 
                    translateMain(n);
                    break;
                case Labels.Block:
                    translateBlock(n);
                    break;
                case Labels.FunDecl: 
                    translateFunDecl(n);
                    break;
                case Labels.If:
                    translateIf(n);
                    break;
                case Labels.While: 
                    translateWhile(n);
                    break;
                case Labels.Return: 
                    translateReturn(n);
                    break;
                case Labels.Assig: 
                    translateAssig(n);
                    break;
                case Labels.Decl: 
                    translateDecl(n);
                    break;
                case Labels.AssigDecl: 
                    translateAssigDecl(n);
                    break;
                case Labels.Print: 
                    translatePrint(n);
                    break;
                case Labels.For: 
                    translateFor(n);
                    break;
                case Labels.Async: 
                    translateAsync(n);
                    break;
                case Labels.Afun: 
                    translateAfun(n);
                    break;
                case Labels.Plus: 
                    translateOp("+", n);
                    break;
                case Labels.Mul:
                    translateOp("*", n);
                    break;
                case Labels.Minus:
                    translateOp("-", n);
                    break;
                case Labels.Div:
                    translateOp("/", n);
                    break;
                case Labels.Gt:
                    translateOp(">", n);
                    break;
                case Labels.Gte:
                    translateOp(">=", n);
                    break;
                case Labels.Lt:
                    translateOp("<", n);
                    break;
                case Labels.Lte:
                    translateOp("<=", n);
                    break;
                case Labels.Eq:
                    translateOp("==", n);
                    break;
                default : 
                    break;
            }
            fileWriter.Close();
        }

        ///<summary>
        ///This is a helper method which calls the method 'translate'
        ///if needed or prints the content of a node if it's a terminal
        ///one.
        ///</summary>
        ///<param name="n">n.</param>
        private void translateRecursive (Node n){
             if (n.isTerminal())
                safeWrite(n.term.ToString());
            else translate(n);
        }

        /// <summary>
        /// This is a helper method which allows to make a safe use of 
        /// the file being written.
        /// </summary>
        /// <param name="s">String to be written within the output file.</param>
        private void safeWrite(string s)
        {
            using (fileWriter = new StreamWriter(fileName, true))
            {
                fileWriter.Write(s);
            }
        }

        /// <summary>
        /// This method prints n \t's according to the passed parameter. 
        /// </summary>
        /// <param name="n">Indentation level to be used.</param>
        private void indent(int n)
        {
            for (int i = 0; i < n; i++)
            {
                safeWrite("\t");
            }
        }

        public void translateMain(Node n)
        {
            foreach ( Node c in n.getChildren() )
            {
                translateRecursive(c);
            }
            
        }
        public void  translateBlock(Node n)
        {   indentationLevel++;
            List<Node> children = n.getChildren();
            foreach( Node c in children)
            {
                for ( int i =0 ; i < indentationLevel; indentationLevel++)
                {
                    safeWrite("\t");
                }
                 translateRecursive(c);
                 safeWrite("\n");
            }
            indentationLevel--;
           
        }

        public void translateFunDecl(Node n)
        {   // fun add ( x int, y int ) 
            //{  return  x + y }
            // let add x y =
            //      x+y
            List<Node> children = n.getChildren(); 
            safeWrite("let ");
            translateRecursive(n);
            translateRecursive(children.ElementAt(0)); // <parameters>
            safeWrite(" = \n");
            translateRecursive(children.ElementAt(1)); // <block> 
             
        }

        public void translateFun(Node n)
        {
            List<Node> children = n.getChildren();
            safeWrite("for ");
            translateRecursive(children.ElementAt(0)); 
            safeWrite(" in ");
            translateRecursive(children.ElementAt(1));  
            safeWrite(" do \n ");
            safeWrite("\t");
            translateRecursive(children.ElementAt(3));  // for statement block 

        }
    
        public void translateIf(Node n)
        {
            List<Node> children = n.getChildren();
            safeWrite("if ");
            translateRecursive( children.ElementAt(0));
            safeWrite(" then ");
            translateRecursive( children.ElementAt(1));
            safeWrite("\n");
            if( children.Count == 3)
            {
                safeWrite("else");
                translateRecursive(children.ElementAt(2));
                safeWrite("\n");
            }
        
        }

        public void translateWhile(Node n)
        {
            List<Node> children = n.getChildren();
            safeWrite("While ");
            translateRecursive(children.ElementAt(0));
            safeWrite("do \n");
            translateRecursive(children.ElementAt(1));  
        }

        public void translateReturn(Node n)
        {
            translateRecursive(n.getChildren().ElementAt(0));
        }

        public void translateAssig(Node n)
        { 
            List<Node> children = n.getChildren();
            translateRecursive(children.ElementAt(0));
            safeWrite(" = ");
            translateRecursive(children.ElementAt(1));
        }

        public void translateDecl(Node n)
        {
            safeWrite("let ");
            translateRecursive(n.getChildren().ElementAt(0));
        }

        public void translateAssigDecl(Node n)
        {
            List<Node> children = n.getChildren();
            safeWrite("let ");
            translateRecursive( n ); // n contains the variable name declared
            safeWrite(" = ");
            translateRecursive(children.ElementAt(1));
        }

        public void translatePrint(Node n)
        {
            safeWrite("\n");
            indent(indentationLevel);
            safeWrite("printfn (");
            safeWrite(n.getChildren().ElementAt(0).term.ToString());
            safeWrite(")\n");
        }

        public void translateFor(Node n)
        {

        }

        public void translateAsync(Node n)
        {
            safeWrite("async {\n");
            indentationLevel++;
            indent(indentationLevel);
            translateRecursive(n.getChildren().ElementAt(0));
            safeWrite("\n");
            indentationLevel--;
            safeWrite("}\n");
        }

        public void translateAfun(Node n)
        {
            //few doubts about the implementation :P
        }

        /// <summary>
        /// This method translates into F# all arithmetic and
        /// boolean expressions.
        /// </summary>
        /// <param name="op">The symbol of the operator within a string.</param>
        /// <param name="n">The node.</param>
        public void translateOp(string op, Node n)
        {
            List<Node> children = n.getChildren();
            Node first = children.ElementAt(0);
            Node second = children.ElementAt(1);

            translateRecursive(first);

            safeWrite(" " + op + " ");

            translateRecursive(second);

        }

    }
}

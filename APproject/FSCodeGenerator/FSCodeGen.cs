using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace APproject
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
        private int indentationLevel; //it stores the number of \s needed to get the perfect indentation ;)
        private string fileName;
        private int asyncTasksCounter;
        Environment environment;

        public FSCodeGen(string outputFileName)
        {
            indentationLevel = 0;
            asyncTasksCounter = 0;
            fileName = outputFileName + ".fs";
            environment = new Environment();

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
        public void translate(ASTNode n) {
            switch (n.label)
            {
                case Labels.Program :
                    translateProgram(n);
                    break;
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
                case Labels.Read:
                    translateRead(n);
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
                    translateOp("=", n);
                    break;
                case Labels.FunCall:
                    translateFunCall(n);
                    break;
                case Labels.And:
                    translateOp("&&", n);
                    break;
                case Labels.Or:
                    translateOp("||", n);
                    break;
                case Labels.NotEq:
                    translateOp("<>", n);
                    break;
                default : 
                    break;
            }
        }

        ///<summary>
        ///This is a helper method which calls the method 'translate'
        ///if needed or prints the content of a node if it's a terminal
        ///one.
        ///</summary>
        ///<param name="n">n.</param>
        private void translateRecursive (ASTNode n){
             if (n.isTerminal())
                safeWrite(n.ToString()); 
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
        /// This method prints n \s's according to the passed parameter. 
        /// http://msdn.microsoft.com/en-us/library/dd233191.aspx
        /// </summary>
        /// <param name="n">Indentation level to be used.</param>
        private void indent(int n)
        {
            for (int i = 0; i < n; i++)
            {
                safeWrite("    ");
            }
        }

        /// <summary>
        /// This method translates all the function declarations and the 
        /// main, recursively.
        /// </summary>
        /// <param name="n">the Program node</param>
        public void  translateProgram(ASTNode n)
        {
            safeWrite("open System\n");
            safeWrite("open System.IO\n");
            foreach ( ASTNode c in n.children )
            {
                translateRecursive( c );
            }
        }

        /// <summary>
        /// This method translates the main.
        /// </summary>
        /// <param name="n">the Main node</param>
        public void translateMain(ASTNode n)
        {
            environment.addScope();

           
            foreach ( Node c in n.children )
            {
                translateRecursive(c);
            }

            environment.removeScope();
            
        }
        /// <summary>
        /// This method translates the Block node and 
        /// correctly indent it, by calling the method indent().
        /// </summary>
        /// <param name="n">the Block node</param>
        public void translateBlock(ASTNode n)
        {   
            List<ASTNode> children = n.children;
           
            environment.addScope();

             if (n.parent.label != Labels.Main)
             {
                indentationLevel++;
                foreach( Node c in children)
                {
                    indent(indentationLevel);
                    translateRecursive(c);  
                }
                indentationLevel--;
                safeWrite("\n");
             }
             else
             {
                 foreach (Node c in children)
                 {
                   translateRecursive(c);
                 }
             }
  
           environment.removeScope();
        }

        /// <summary>
        /// This method translates a function declaration in F#.
        /// The first child is the block Node, 
        /// the other children are the parameters of the function.
        /// </summary>
        /// <param name="n">Node representing a function declaration.</param>
        /// 
        public void translateFunDecl(ASTNode n)
        {
            string functionName = ((Obj)n.value).name;

            //environment.addUpdateValue((Obj)n.value, n.value);

            safeWrite("let ");
            safeWrite(functionName);
            translateParameters(1, n);
            safeWrite(" = \n");
            translateRecursive(n.children.ElementAt(0));
            safeWrite("\n");

        }
     

        /// <summary>
        /// This is a helper method, translating the function
        /// parameters.
        /// </summary>
        /// <param name="initPar"></param>
        /// <param name="n"></param>
        private void translateParameters(int initPar, ASTNode n)
        {
            List<ASTNode> parameters = n.children;
            {
                for (int i = initPar; i < parameters.Count; i++)
                {
                    safeWrite(" ");
                    ASTNode temp = parameters.ElementAt(i);
                    translateRecursive(temp);
                    safeWrite(" ");
                }
            }

        }

        /// <summary>
        /// This method translates the If node and 
        /// its children, recursively.
        /// </summary>
        /// <param name="n">the If node</param>
        public void translateIf(ASTNode n)
        {
            List<ASTNode> children = n.children;

            safeWrite("if ");
            translateRecursive(children.ElementAt(0));
            safeWrite(" then\n");
            translateRecursive(children.ElementAt(1));
            if(children.Count == 3)
            {
                safeWrite("else\n");
                translateRecursive(children.ElementAt(2));
                safeWrite("\n");
            }
        }
        /// <summary>
        /// This method translates the While node and 
        /// its children, recursively.
        /// </summary>
        /// <param name="n">the While node</param>
        public void translateWhile(ASTNode n)
        {
            List<ASTNode> children = n.children;
            safeWrite("while ");
            translateRecursive(children.ElementAt(0));
            safeWrite(" do\n");
            translateRecursive(children.ElementAt(1));  
        }
        /// <summary>
        /// This method translates the Return node
        /// (F# has no explicit "return" statement").
        /// </summary>
        /// <param name="n">the Return node</param>
        public void translateReturn(ASTNode n)
        {
            safeWrite("\n");
            indent(indentationLevel);
            translateRecursive(n.children.ElementAt(0));
        }

        /// <summary>
        /// This method translates the Assignment node and 
        /// its children, recursively.
        /// </summary>
        /// <param name="n">the Assignment node</param>
        public void translateAssig(ASTNode n)
        {
            List<ASTNode> children = n.children;
            translateRecursive(children.ElementAt(0));
            safeWrite(" <- ");
            translateRecursive(children.ElementAt(1));
        }

        /// <summary>
        /// This method translates the Declaration node and 
        /// its children, recursively. In F# all mutable variables
        /// MUST be initialized: thus int variables are set to 0,
        /// bool variables to true.
        /// </summary>
        /// <param name="n">the declaration node</param>
        public void translateDecl(ASTNode n)
        {   
            safeWrite("let mutable ");

            translateRecursive(n.children.ElementAt(0));

            if (n.children.ElementAt(0).type == Types.integer)
            {
                safeWrite(" = 0\n"); // integer are initialized to '0'
            }
            else if (n.children.ElementAt(0).type == Types.boolean)
            {
                safeWrite(" = true\n"); // bool are initialized to 'true'
            }
            else safeWrite(" = Unchecked.defaultof<'a>\n");
        }

        /// <summary>
        /// This method translates the Declaration/Assigment node and 
        /// its children, recursively. 
        /// </summary>
        /// <param name="n">the declaration/assignment node</param>
        public void translateAssigDecl(ASTNode n)
        {
            List<ASTNode> children = n.children;
            safeWrite("let mutable ");
            translateRecursive(children.ElementAt(0)); // contains the variable name 
            safeWrite(" = ");
            translateRecursive(children.ElementAt(1));
            safeWrite("\n");
        }

        /// <summary>
        /// This method translates the call of function node and 
        /// its children, recursively. 
        /// </summary>
        /// <param name="n">the function call node</param>
        /// 
        /****************** !! Still RECURSIVE FUNCTION to be translated !! **********/
        public void translateFunCall( ASTNode n)
        {
            List<ASTNode> children = n.children;
            if (n.value.GetType() == typeof(Obj))
            {
              safeWrite(((Obj)n.value).name + " ");
            }

            for (int i = 0; i < children.Count(); i++)  // parameters of the function
            {
                safeWrite(" ");
                translateRecursive(children.ElementAt(i));
                safeWrite(" ");
            }

        }

        /// <summary>
        /// This method translates a println() call node into the 
        /// correspondent printfn() of F#. 
        /// </summary>
        /// <param name="n">the println call node</param>
        /// 
        public void translatePrint(ASTNode n)
        {
            safeWrite("\n");
            indent(indentationLevel);
            safeWrite("Console.WriteLine(");
            translateRecursive(n.children.ElementAt(0));
            safeWrite(")\n");
        }
        }

        /// <summary>
        /// This method translates a readln() call node into the 
        /// correspondent Console.ReadLine() of F#. 
        /// </summary>
        /// <param name="n">the readln call node</param>
        /// 
        public void translateRead(ASTNode n)
        {
            if (n.parent.children.ElementAt(0).type == Types.integer)
            {
                safeWrite("Convert.ToInt32(Console.ReadLine())\n");
            }
            else
            {
                safeWrite("Console.ReadLine()\n");
            }
        }

        /// <summary>
        /// Translate for statement from funW@p to F# syntax.
        /// The syntax in f# is :
        /// for pattern in enumerable-expression do
        ///    body-expression
        /// </summary>
        /// <param name="n">Node represents a For statement.</param>
        public void translateFor(ASTNode n)
        {
            /**
             *  for i := 0; i < 10; i++ 
             *  { a += i }
             * 
             * for i in 0 .. 10 do
             *     <block>
             *    
             * */
            List<ASTNode> children = n.children;
            safeWrite("for ");
            ASTNode assFor = children.ElementAt(0);
            Term pattern = (Term)assFor.children.ElementAt(0);
            safeWrite(pattern.ToString());                 
            safeWrite(" in ");
            Term valueStart = (Term) assFor.children.ElementAt(1);
            safeWrite(valueStart.ToString());
            safeWrite(" .. ");
            ASTNode expFor = children.ElementAt(1);
            Term valueExp = (Term)expFor.children.ElementAt(1);
            translateRecursive(valueExp);
            safeWrite(" do \n ");
            translateRecursive(children.ElementAt(3));  // block 

        }
        /// <summary>
        /// This method translates the async node inthe f# syntax
        /// </summary>
        /// <param name="n">A Afun node .</param>

        public void translateAsync(ASTNode n)
        {

            /* var a int = async{...}
             * b = async{...}
             * a + b
             * safeWrite("Async.RunSynchronously(async { return ");
            translateRecursive(n.children.ElementAt(0));
            safeWrite("})");
            */
            string taskName = "task" + (asyncTasksCounter++);
            safeWrite("let " + taskName + " = Async.StartAsTask( async{ return ");

            // insert taskName in memory with name of the variable 
            // associate result to variable
            // use the variable
            translateRecursive(n.children.ElementAt(0));
            safeWrite("})");

        }

        /// <summary>
        /// This method translates into F# the anonymous function.
        /// The first child is the block, the second the return type(if there),
        /// other children are the parameters.
        /// </summary>
        /// <param name="n">The node representing an anonymous fuunction.</param>
        /// 
        public void translateAfun(ASTNode n)
        {

            List<ASTNode> children = n.children; 
            int numElement = children.Count;
            safeWrite("fun ");
            if (numElement >= 2 && children.ElementAt(1).label==Labels.Return ){
                translateParameters(2 , n);
                safeWrite(" : ");
                translateRecursive(children.ElementAt(1)); 
                safeWrite( " -> \n");
                translateRecursive(children.ElementAt(0));
            }

            if (numElement >= 2 && children.ElementAt(1).label != Labels.Return)
            {
                translateParameters(1, n);
                safeWrite(" -> \n");
                translateRecursive(children.ElementAt(0));
            }
        

        }

        /// <summary>
        /// This method translates into F# all binary arithmetic and
        /// boolean expressions.
        /// </summary>
        /// <param name="op">The symbol of the operator within a string.</param>
        /// <param name="n">The node.</param>
        public void translateOp(string op, ASTNode n)
        {
            List<ASTNode> children = n.children;
            ASTNode first = children.ElementAt(0);
            ASTNode second = children.ElementAt(1);

            translateRecursive(first);

            safeWrite(" " + op + " ");

            translateRecursive(second);

        }
    }
}

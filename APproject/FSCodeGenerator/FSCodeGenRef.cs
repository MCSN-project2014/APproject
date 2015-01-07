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
    public class FSCodeGenRef
    {
        //private StreamWriter fileWriter;
        private FileStream output;
        private int indentationLevel; //it stores the number of \s needed to get the perfect indentation ;)
        private string fileName;
        private int asyncTasksCounter;
        private bool bang; //it indicates whether the operator bang! must be used or not
        Dictionary<string, Tuple<ASTNode, List<string>>> funDeclarations; //contains the reference to the nodes representing funDecl's
        Environment environment;

        public FSCodeGenRef(string outputFileName)
        {
            indentationLevel = 0;
            asyncTasksCounter = 0;
            fileName = outputFileName + ".fs";
            environment = new Environment();
            bang = false;

            funDeclarations = new Dictionary<string, Tuple<ASTNode, List<string>>>();

            if (outputFileName == string.Empty)
            {
                Console.WriteLine("Invalid File Name");
            }
            else
            {
                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
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
        public void translate(ASTNode n)
        {
            switch (n.label)
            {
                case Labels.Program:
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
                //  case Labels.Dsync:
                //     translateDsync(n);
                //    break;
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
                case Labels.Negativ:
                    translateNegativ(n);
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
                default:
                    break;
            }
        }

        ///<summary>
        ///This is a helper method which calls the method 'translate'
        ///if needed or prints the content of a node if it's a terminal
        ///one.
        ///</summary>
        ///<param name="n">n.</param>
        private void translateRecursive(ASTNode n)
        {
            if (n.isTerminal())
                
                
                if ((n.value) is Obj && ((Obj)n.value).kind == Kinds.var && bang)
                    safeWrite("!" + n.ToString());
                else
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
            using (var fileWriter = new StreamWriter(fileName, true))
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
        public void translateProgram(ASTNode n)
        {
            safeWrite("open System\n");
            safeWrite("open System.IO\n");
            safeWrite("open System.Threading.Tasks\n");
            safeWrite("\n");
            foreach (ASTNode c in n.children)
            {
                translateRecursive(c);
            }
        }

        /// <summary>
        /// This method translates the main.
        /// </summary>
        /// <param name="n">the Main node</param>
        public void translateMain(ASTNode n)
        {
            environment.addScope();

            safeWrite("\n[<EntryPoint>]\nlet main argv = \n");
            indentationLevel++;
            indent(indentationLevel);
            safeWrite("let _task = ref( Map.empty )\n");
            indentationLevel--;
            foreach (Node c in n.children)
            {
                translateRecursive(c);
            }
            indentationLevel++;
            indent(indentationLevel);
            safeWrite("Console.ReadLine()|>ignore\n");
            indent(indentationLevel);
            safeWrite("0\n");
            indentationLevel--;

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

            //if (n.parent.label != Labels.Main)
            //{
            // safeWrite("\n");
            indentationLevel++;
            foreach (Node c in children)
            {

                indent(indentationLevel);
                translateRecursive(c);
            }
            indentationLevel--;

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

            environment.addScope();

            //funDeclarations.Add(functionName, ); //stores the couple < functionName, DeclNode >, useful for dsync

            if (((Obj)n.value).recursive == true)
            {
                safeWrite("let rec ");
                safeWrite(functionName);
                List<string> nameParameters = translateParameters(1, n);
                safeWrite(" =");
                if (nameParameters.Count == 0)
                    safeWrite("\n");
                translateMutableParameters(nameParameters);
                translateRecursive(n.children.ElementAt(0));
                safeWrite("\n");
            }
            else
            {
                safeWrite("let ");
                safeWrite(functionName);
                List<string> nameParameters = translateParameters(1, n);
                safeWrite(" =");
                if (nameParameters.Count == 0)
                    safeWrite("\n");
                translateMutableParameters(nameParameters);
                translateRecursive(n.children.ElementAt(0));
                safeWrite("\n");
            }

            environment.removeScope();
        }


        /// <summary>
        /// This is a helper method, translating the paramters function
        /// as mutable variable.
        /// </summary>
        /// <param name="iparList"></param>
        private void translateMutableParameters(List<string> parList)
        {
            indentationLevel++;
            foreach (string s in parList)
            {
                safeWrite("\n");
                indent(indentationLevel);
                string mutableDecl = "let " + s + " = ref(" + s + ")";
                safeWrite(mutableDecl);
            }
            
            if (parList.Count!=0)
                safeWrite("\n");
            indentationLevel--;
            
        }


        /// <summary>
        /// This is a helper method, translating the function
        /// parameters.
        /// </summary>
        /// <param name="initPar"></param>
        /// <param name="n"></param>
        private List<string> translateParameters(int initPar, ASTNode n)
        {
            List<ASTNode> parameters = n.children;
            List<string> nameParameters = new List<string>();
            {
                for (int i = initPar; i < parameters.Count; i++)
                {
                    safeWrite(" ");
                    ASTNode temp = parameters.ElementAt(i);
                    nameParameters.Add(temp.ToString());
                    translateRecursive(temp);
                    safeWrite(" ");
                }
                if (parameters.Count == 1)
                {
                    safeWrite("()");
                }
                return nameParameters;
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
            bang = true;
            translateRecursive(children.ElementAt(0));
            bang = false;
            safeWrite(" then\n");
            translateRecursive(children.ElementAt(1));
            if (children.Count == 3)
            {
                indent(indentationLevel);
                safeWrite("else\n");
                translateRecursive(children.ElementAt(2));
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
            bang = true;
            translateRecursive(children.ElementAt(0));
            bang = false;
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
            //safeWrite("\n");
            // indent(indentationLevel);
            bang = true;
            translateRecursive(n.children.ElementAt(0));
            bang = false;
        }

        /// <summary>
        /// This method translates the Assignment node and 
        /// its children, recursively.
        /// </summary>
        /// <param name="n">the Assignment node</param>
        public void translateAssig(ASTNode n)
        {
            List<ASTNode> children = n.children;

            if (children[1].label != Labels.Async)
            {
                translateRecursive(children.ElementAt(0));
                safeWrite(" := ");
            }

            bang = true;
            translateRecursive(children.ElementAt(1));
            bang = false;
            safeWrite("\n");
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
            for (int i = 0; i < n.children.Count; i++)
            {
                if (i > 0)
                    indent(indentationLevel); //fix indentation for multiple declarations like var x, y, z int;

                safeWrite("let ");
                translateRecursive(n.children.ElementAt(i));
                if (n.children.ElementAt(i).type == Types.integer)
                {
                    safeWrite(" = ref (0)\n"); // integer are initialized to '0'
                }
                else if (n.children.ElementAt(i).type == Types.boolean)
                {
                    safeWrite(" = ref (true)\n"); // bool are initialized to 'true'
                }
                else
                {
                    safeWrite(" = ref (Unchecked.defaultof<'a>)\n");
                }
                
            }
        }

        /// <summary>
        /// This method translates the Declaration/Assigment node and 
        /// its children, recursively. 
        /// </summary>
        /// <param name="n">the declaration/assignment node</param>
        public void translateAssigDecl(ASTNode n)
        {
            List<ASTNode> children = n.children;
            if (children[1].label != Labels.Async)
            {
                safeWrite("let ");
                translateRecursive(children.ElementAt(0)); // contains the variable name 
                safeWrite(" = ref(");
            }
            bang = true;
            translateRecursive(children.ElementAt(1));
            bang = false;
            safeWrite(")\n");
        }

        /// <summary>
        /// This method translates the call of function node and 
        /// its children, recursively. 
        /// </summary>
        /// <param name="n">the function call node</param>
        /// 
        public void translateFunCall(ASTNode n)
        {
            List<ASTNode> children = n.children;
            dynamic tmp = n.value;
            if (tmp.kind == Kinds.proc)
                safeWrite(tmp.name + " ");
            else
                safeWrite("!"+tmp.name + " ");

            for (int i = 0; i < children.Count(); i++)  // parameters of the function
            {
                safeWrite("(");
                bang = true;
                translateRecursive(children.ElementAt(i));
                bang = false;
                safeWrite(")");
            }

            if (children.Count == 0)
            {
                safeWrite("()");
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
            //indent(indentationLevel);
            safeWrite("Console.WriteLine( ");
            bang = true;
            translateRecursive(n.children.ElementAt(0));
            bang = false;
            safeWrite(" )\n");
        }

        /// <summary>
        /// This method translates a readln() call node into the 
        /// correspondent Console.ReadLine() of F#. 
        /// </summary>
        /// <param name="n">the readln call node</param>
        /// 
        public void translateRead(ASTNode n)
        {
            safeWrite("Convert.ToInt32(Console.ReadLine())");
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
            Term valueStart = (Term)assFor.children.ElementAt(1);
            safeWrite(valueStart.ToString());
            safeWrite(" .. ");
            ASTNode expFor = children.ElementAt(1);
            Term valueExp = (Term)expFor.children.ElementAt(1);
            translateRecursive(valueExp);
            safeWrite(" do \n");
            translateRecursive(children.ElementAt(3));  // block 

        }
        /// <summary>
        /// This method translates the async node inthe f# syntax
        /// </summary>
        /// <param name="n">A Afun node .</param>

        public void translateAsync(ASTNode n)
        {
            ASTNode sisterNode = n.parent.children[0];
            environment.addUpdateValue(((Obj)sisterNode.value), true);

            safeWrite("_task := (!_task).Add(" + ((Obj)sisterNode.value).name + ", Async.StartAsTask( async{ return ");
            translateRecursive(n.children[0]);
            safeWrite("}))");

        }

        /// <summary>
        /// This method produces the F# code performing 
        /// the HTTP POST request for the dsync in funW@P.
        /// </summary>
        /// <param name="n">The node Dsync.</param>

        /*
         * 
        public void translateDsync(ASTNode n)
        {
            string url = n.children[0].name;
        }

       */
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
            bang = false;
            var paramList = translateParameters(1, n);
            safeWrite(" ->\n");
            

            indentationLevel++;
            foreach (string tmp in paramList)
            {
                indent(indentationLevel);
                safeWrite("let " + tmp + " = " + "ref( " + tmp + " )\n");     
            }
            indentationLevel--;

            translateRecursive(children[0]);
            


            ////if (numElement >= 2 && children.ElementAt(1).label == Labels.Return)
            //{
            //    translateParameters(2, n);
                
            //    //translateRecursive(children.ElementAt(1));
            //    safeWrite(" -> \n");
            //    translateRecursive(children.ElementAt(0));
            //}

            //if (numElement >= 2 && children.ElementAt(1).label != Labels.Return)
            //{   

            //    translateParameters(1, n);
            //    safeWrite(" -> \n");
            //    translateRecursive(children.ElementAt(0));
            //}


        }
        /// <summary>
        /// This method translates into F#  the unary minus.
        /// </summary>
        /// <param name="n">The node.</param>
        public void translateNegativ(ASTNode n)
        {
            safeWrite("-");
            translateRecursive(n.children.ElementAt(0));
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



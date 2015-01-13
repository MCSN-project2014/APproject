using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace APproject
{
    ///<summary>
    ///This class converts an AST of funW@P into the corresponding F# code,
    ///simply by visiting the tree, according to a breadth-first fashion.
    ///The output is written into a outputFileName.fs file, 
    ///result of the translation.
    ///</summary>
    public class FSCodeGen
    {

        private int indentationLevel;           //it stores the number of \s needed to get the perfect F# indentation ;)
        private string fileName;
        private bool bang;                      //it indicates whether the operator bang! must be used or not in the translation
        private Environment environment;        //it keeps variables that must be translated differently in F#(same Environment class of interpreter)
		private Dictionary<Obj,ASTNode> decFunctions; 
		private int indexPar;                   //counter for generating unique variable names.

        ///<summary>
        ///The constructor of FSCodeGenRef. 
        ///The outputFileName.fs file is created.
        ///If the file is already there, the old version
        ///is deleted.
        ///</summary> 
		public FSCodeGen(string outputFileName)
        {
			indexPar = 0;
			decFunctions = new Dictionary<Obj,ASTNode> ();
            indentationLevel = 0;
            fileName = outputFileName + ".fs";
            environment = new Environment();
            bang = false;

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
        /// It is recursively called by all other methods, by using
        /// the translateRecursive() method below.
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
                case Labels.Bracket:
                    translateBracket(n);
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
        ///if needed or prints the correct string if a node
        ///is a terminal one.
        ///</summary>
        ///<param name="n">n.</param>
        private void translateRecursive(ASTNode n)
        {
         //terminal synchronously assigned
            if (n.isTerminal())
            {
				if (n.value is Obj)
                {
					var tmpVar = (Obj)n.value;
					object memValue;
					if (environment.TryGetValue(tmpVar, out memValue) && (bool)memValue && bang) {
						safeWrite ("_task_" + n.ToString () + ".Result");
					}else if (tmpVar.kind == Kinds.var && bang)
                        safeWrite("!" + n.ToString());
                    else
                        safeWrite(n.ToString());
                }
                else if (n.value is string)
                    safeWrite("\"" + n + "\"");
                else safeWrite(n.ToString());
            }
			else translate(n);
        }

        /// <summary>
        /// This is a helper method which writes a string in the F# file.
        /// It doesn't introduce any indentation or white spaces.
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
        /// This method writes a number of white spaces and followed 
        /// by the string in the F# file.
        /// It doesn't write the new line \n.
        /// </summary>
        /// <param name="s">String to be written within the output file.</param>

		private void safeWriteIndent(string s){
			var tmp = indent () + s;
			safeWrite (tmp);
		}

        /// <summary>
        /// This method prints four spaces according to the 
        /// indentationLevel content.
        /// http://msdn.microsoft.com/en-us/library/dd233191.aspx
        /// </summary>
		private string indent()
        {
			string res = "";
			for (int i = 0; i < indentationLevel; i++)
                res+="    ";
			return res;

        }

        /// <summary>
        /// This method translates all the function declarations and the 
        /// main, recursively. It prints the needed "open" statements in F#.
        /// </summary>
        /// <param name="n">The Program node.</param>
        public void translateProgram(ASTNode n)
        {
            safeWriteIndent("open System\n");
            safeWriteIndent("open System.IO\n");
            safeWriteIndent("open System.Threading.Tasks\n");
            safeWriteIndent("open funwaputility.PostMethods\n");
            safeWriteIndent("open funwaputility.Readline\n");   
            safeWriteIndent("\n");
 
            foreach (ASTNode c in n.children)
            {
                translateRecursive(c);
            }
        }
      

        /// <summary>
        /// This method translates the Main.
        /// </summary>
        /// <param name="n">The Main node.</param>
        public void translateMain(ASTNode n)
        {
            environment.addScope();

            safeWriteIndent("\n[<EntryPoint>]\nlet main argv = \n");
            
			indentationLevel++;

			foreach (Node c in n.children)
                translateRecursive(c);
  			
            safeWriteIndent("Console.ReadLine()|>ignore\n");
            safeWriteIndent("0\n");
			indentationLevel--;

            environment.removeScope();

        }

        /// <summary>
        /// This method translates a Block node.
        /// </summary>
        /// <param name="n">The Block node</param>
        public void translateBlock(ASTNode n)
        {
            List<ASTNode> children = n.children;

            environment.addScope();

            indentationLevel++;
            foreach (Node c in children)
            {
                translateRecursive(c);
            }
            indentationLevel--;

            environment.removeScope();
        }

        /// <summary>
        /// This method translates a function declaration in F#.
        /// The first child is the block Node, the other children are the 
        /// parameters of the function itslef.
        /// If the function is recursive the "rec" keyword is used.
        /// </summary>
        /// <param name="n">The FunDecl node.</param>
        /// 
        public void translateFunDecl(ASTNode n)
        {
            environment.addScope();

			decFunctions.Add ((Obj)n.value, n);
         	
			safeWriteIndent ("let "+( ((Obj)n.value).recursive ? "rec " :  " ") + ((Obj)(n.value)).name);
			List<string> nameParameters = translateParameters (n);
			safeWrite (" =");
			safeWrite("\n");
			indentationLevel++;
			translatePassedParameters (nameParameters);
			indentationLevel--;
			translateRecursive (n.children[0]);
			safeWrite ("\n");

            environment.removeScope();
        }


        /// <summary>
        /// This is a helper method, translating the function parameters
        /// that may be used within a function call.
        /// </summary>
        /// <param name="iparList">List of parameters to be printed out.</param>
        private void translatePassedParameters(List<string> parList)
        {
            foreach (string s in parList)
				safeWriteIndent("let " + s + " = ref(" + s + ")\n");
        }


        /// <summary>
        /// This is a helper method, translating the formal
        /// function parameters.
        /// </summary>
        /// <param name="initPar"></param>
        /// <param name="n"></param>
        private List<string> translateParameters(ASTNode n)
        {
            List<ASTNode> parameters = n.children;
            List<string> nameParameters = new List<string>();
            {
                for (int i = 1; i < parameters.Count; i++)
                {
                    ASTNode temp = parameters[i];
					nameParameters.Add(temp.ToString());
                    safeWrite(" "+temp+" ");
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
        /// <param name="n">The If node.</param>
        public void translateIf(ASTNode n)
        {
            List<ASTNode> children = n.children;

            safeWriteIndent("if ");
            bang = true;
            translateRecursive(children.ElementAt(0));
            bang = false;
            safeWrite(" then\n");
            translateRecursive(children.ElementAt(1));
            if (children.Count == 3)
            {
                safeWriteIndent("else\n");
                translateRecursive(children.ElementAt(2));
            }
        }

        /// <summary>
        /// This method translates the While node and 
        /// its children, recursively.
        /// </summary>
        /// <param name="n">The While node</param>
        public void translateWhile(ASTNode n)
        {
            List<ASTNode> children = n.children;
            safeWriteIndent("while ");
            bang = true;
            translateRecursive(children[0]);
            bang = false;
            safeWrite(" do\n");
            translateRecursive(children.ElementAt(1));
        }

        /// <summary>
        /// This method translates the Return node
        /// (F# has no explicit "return" statement").
        /// </summary>
        /// <param name="n">The Return node</param>
        public void translateReturn(ASTNode n)
        {
            bang = true;
			safeWriteIndent ("");
            translateRecursive(n.children[0]);
            safeWrite("\n");
            bang = false;
        }

        /// <summary>
        /// This method translates the Assignment node and 
        /// its children, recursively.
        /// If the second child is an Async node it calls recursively the 
        /// translator for an Async node if it is a Dasync node, 
        /// same for a Dasync code.
        /// </summary>
        /// <param name="n">The Assignment node.</param>
        public void translateAssig(ASTNode n)
        {
            List<ASTNode> children = n.children;
			switch (children[1].label)
            {
			case Labels.Async:
				createAsync (children [1].children [0], (Obj)children [0].value);
				break;
			case Labels.Dsync:
                 //funCall, varAsync, url
                createDAsync(children[1].children[1], (Obj)children[0].value, children[1].children[0].ToString());
				break;
			default:
				environment.addUpdateValue ((Obj)children [0].value, false);

				safeWriteIndent (children [0] + " := ");
				bang = true;
				translateRecursive (children.ElementAt (1));
				bang = false;
				safeWrite ("\n");
				break;
			}
            
        }

        /// <summary>
        /// This method translates the Declaration node and 
        /// its children, recursively. In F# all ref variables
        /// MUST be initialized: thus int variables are set 
        /// by default to 0, bool variables to 'true'.
        /// Same happens with the funW@P interpreter.
        /// </summary>
        /// <param name="n">the declaration node</param>
        public void translateDecl(ASTNode n)
        {
			foreach(ASTNode item in n.children)
            {
			    safeWriteIndent("let " + item + " = ref (" +(item.type == Types.integer ? "0" : "true") + ")\n");
        	}

			foreach (ASTNode item in n.children) {
				var vartmp = (Obj)item.value;
				if (vartmp.isUsedInAsync)
					createStructureAsync (vartmp);
                if(vartmp.isUsedInDasync)
                    createStructureAsync(vartmp);
			}
		}

        /// <summary>
        /// Private method creating the Task to be associated
        /// to a variable which is objet of an Async assignment.
        /// </summary>
        /// <param name="var">The variable Obj.</param>

		private void createStructureAsync(Obj var){
			safeWriteIndent ("let mutable " + "_task_" + var.name +
				" = Async.StartAsTask( async{ return " +
				(var.type == Types.integer ? "0" : "true") + "})\n");
			environment.addUpdateValue (var, true);
		}


        /// <summary>
        /// This method writes the F# code for the async operation.
        /// </summary>
        /// <param name="funCall">FunCall node in the async.</param>
        /// <param name="varAsync">The variable in wich the result will be stored.</param>
		private void createAsync (ASTNode funCall, Obj varAsync){

			createTmpParameter (funCall);

			var funObj = (Obj)funCall.value;
			safeWriteIndent("_task_" + varAsync.name + " <- Async.StartAsTask( async{ return "+funObj.name+" ");

			int i=0;
			foreach (ASTNode node in funCall.children) {
				safeWrite ("_par_"+ node+ indexPar + (i++ < funCall.children.Count-1 ? " ":""));
                indexPar++;
			}
			if (funCall.children.Count == 0)
				safeWrite ("()");
			safeWrite(" })\n");
			
		}


        /// <summary>
        /// Helper method to translate the actual
        /// parameter of a function call in F#.
        /// </summary>
        /// <param name="funCall">FunCall node.</param>

		private List<string> createTmpParameter(ASTNode funCall){
            var tmpIndex = indexPar;
			var actual = new List<string>();
			foreach (ASTNode node in funCall.children) {
                var varName = "_par_"  + node + tmpIndex++ ;
                safeWriteIndent("let " + varName + (node.type == Types.integer ? " : int " : " : bool ") + " = ");
				actual.Add (varName);
				bang = true;
				translateRecursive (node);
				bang = false;
				safeWrite ("\n");
			}
			return actual;
		}

        /// <summary>
        /// This method translates a Dasync into F# code.
        /// It converts the parameters of the Dasync function and it's body 
        /// in a JSON string.
        /// It calls the method for the POST operation (both for bool and int functions).
        /// It writes a task in result to a Dasync operation, 
        /// with the url specified and the json structure.
        /// </summary>
        /// <param name="funCall">FunCall node in the Dasync structure.</param>
        /// <param name="varDAsync">Variable to assign the returned dasync value.</param>
        /// <param name="url">String representing the url, where the function will be executed</param>
		private void createDAsync (ASTNode funCall, Obj varDAsync, string url){
			List<string> actual = createTmpParameter (funCall);

			ASTNode funDec;
			if (decFunctions.TryGetValue ((Obj)funCall.value, out funDec)) {

				var formal = new List<string> ();
				foreach (var item in funDec.children) {
					if (item.isTerminal())
						formal.Add (item.ToString ());
				}
				var block = funDec.children[0];
				block.parent = null;

				string data = HelperJson.SerializeWithEscape (actual, formal, block);
                safeWriteIndent("let tempJsonData = \"" +  data + "\"\n");
                var nameTaskDasync = "_task_" + varDAsync.name;
                var postAsyncType = (varDAsync.type==Types.integer? "getPostAsyncInt": "getPostAsyncBool");
                safeWriteIndent(nameTaskDasync +" <- Async.StartAsTask( "+postAsyncType +"( !" + url + ",tempJsonData ))\n");
				//Console.WriteLine(data.Replace("\\\"","'"));

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
			var tmpVar = (Obj)children [0].value;
			if (tmpVar.isUsedInAsync){
				createStructureAsync (tmpVar);
			}

			if (children [1].label != Labels.Async) {
				safeWriteIndent ("let "+children[0]+" = ref(");
			}

            bang = true;
            translateRecursive(children[1]);
            bang = false;

			if (children[1].label != Labels.Async) safeWrite(")");
			safeWrite("\n");
        }

        /// <summary>
        /// This method translates the call of function node and 
        /// its children, recursively. 
        /// </summary>
        /// <param name="n">the FunCall node</param>
        /// 
        public void translateFunCall(ASTNode n)
        {
            List<ASTNode> children = n.children;
            dynamic tmp = n.value;
            if (tmp.kind == Kinds.fundec)
                safeWrite(tmp.name + " ");
            else
                safeWrite("!"+tmp.name + " ");

			foreach(ASTNode item in children)  // parameters of the function
            {
                safeWrite("(");
                bang = true;
				translateRecursive(item);
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
        /// <param name="n">the Println node</param>
        /// 
        public void translatePrint(ASTNode n)
        {
            safeWriteIndent("Console.WriteLine( ");
            bang = true;
            translateRecursive(n.children[0]);
            bang = false;
            safeWrite(" )\n");
        }

        /// <summary>
        /// This method translates a readln() call node into the 
        /// correspondent F# _readln() which is defined in the 
        /// funwaputilities.dll library as:
        /// 
            ///let _readln() =
            ///     let mutable tmp = true
            ///     let input = ref(0)
            ///     while tmp do
            ///     try
            ///        input := Convert.ToInt32(Console.ReadLine())
            ///        tmp <- false;
            ///    with
            ///    | :? System.FormatException as ex ->
            ///        Console.WriteLine("funW@P->F# - Only integer input is allowed. Try again.")
            ///!input
        ///
        /// </summary>
        /// <param name="n">the readln call node</param>
        /// 
        public void translateRead(ASTNode n)
        {
            safeWrite("_readln()");
        }
 

        /// <summary>
        /// This method translates into F# an anonymous function.
        /// The first child is the block, the second the return type(if there is one),
        /// the other children are the parameters.
        /// </summary>
        /// <param name="n">The Afun node.</param>
        /// 
        public void translateAfun(ASTNode n)
        {

            List<ASTNode> children = n.children;
           
            //int numElement = children.Count;
            safeWrite("fun ");
            bang = false;
            var paramList = translateParameters(n);
            safeWrite(" ->\n");
            

            indentationLevel++;
			translatePassedParameters (paramList);
            indentationLevel--;
            translateRecursive(children[0]);
        }

        /// <summary>
        /// This method translates into F#  the unary minus.
        /// </summary>
        /// <param name="n">The Negativ node.</param>
        public void translateNegativ(ASTNode n)
        {
            safeWrite("-");
            translateRecursive(n.children[0]);
        }


        /// <summary>
        /// This method translates into F# the brackets in a 
        /// boolean expressions.
        /// </summary>
        /// <param name="n">The bracket node.</param>
        public void translateBracket( ASTNode n )
        {
            safeWrite("(");
			translateRecursive(n.children[0]);
            safeWrite(")");
        }

        /// <summary>
        /// This method translates into F# all binary arithmetic and
        /// boolean expressions.
        /// </summary>
        /// <param name="op">The symbol of the operator within a string.</param>
        /// <param name="n">The Op node.</param>
        public void translateOp(string op, ASTNode n)
        {
            List<ASTNode> children = n.children;
			translateRecursive(children[0]);

            safeWrite(" " + op + " ");

			translateRecursive(children[1]);

        }
    }
}



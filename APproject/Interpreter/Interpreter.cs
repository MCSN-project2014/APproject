using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace APproject
{
	public class Interpreter
	{
		const string CONSOL_PRINT = "funW@P console: ";
		const string CONSOL_READ = "funW@P read: ";
		const string CONSOL_INFO = "funW@P info: ";

		//MemoryFunction funMem;
		private Dictionary<Obj,ASTNode> function;
		//Obj main;
		ASTNode startNode;

		public Interpreter( ASTNode node){
			startNode = node;
			//main = new Obj{ name = "main" };
			function = new Dictionary<Obj, ASTNode> ();
		}
			
		public void Start (){
			Console.WriteLine ("\nINTERPRETER START:");
			try{
				if (startNode.label == Labels.Program){
					foreach (ASTNode node in startNode.children) {
						if (node.label == Labels.FunDecl)
							function.Add ((Obj)node.value, node);
						if (node.label == Labels.Main) {
							//funMem.addNameSpace (main);
							var mem = new Environment ();
							mem.addScope ();
							Interpret (node, mem);
							mem.removeScope ();
						}
					}
				}
			}catch(InterpreterException e){
				Console.WriteLine (e.Message);
			}
		}

		public void Start(List<Dictionary<string,object>> parameter){
			try{
				if (startNode.label == Labels.Block){
					var mem = new Environment();
					mem.addScope();

					foreach(var item in parameter){
						var keys = item.Keys.GetEnumerator();
						keys.MoveNext();
						var key = keys.Current;
						object value = item[key];
						mem.addUpdateValue(new Obj{name=key}, value);
					}
					Console.Write(Interpret(startNode,mem));
				}
			}catch(InterpreterException e){
				Console.WriteLine (e.Message);
			}

		}

		private object Interpret (ASTNode node, Environment actualMemory){
			if (!node.isTerminal ()) {
				List<ASTNode> children = node.children;
				bool condition;
				switch (node.label) {
				case Labels.Main:
					foreach (Node n in children)
						Interpret (n, actualMemory);
					break;
				case Labels.Block:
					actualMemory.addScope ();
					object ret = null;
					foreach (Node n in children) {
						ret = Interpret (n, actualMemory);
						if (ret != null)
							break;
					}
					actualMemory.removeScope ();
					return ret;
				case Labels.If:
					condition = InterpretCondition (children [0], actualMemory);
					if (condition) {
						return Interpret (children [1], actualMemory);
					} else if (children.Count > 2)
						return Interpret (children [2], actualMemory);
					break;
				case Labels.While:
					condition = InterpretCondition (children [0], actualMemory);
					while (condition) {
						ret = Interpret (children [1], actualMemory);
						if (ret!=null)
							return ret;
						condition = InterpretCondition (children [0], actualMemory);
					}
					break;
				case Labels.Decl:
                    foreach (ASTNode n in children)
                    {
                        if (n.type == Types.boolean)
                            actualMemory.addUpdateValue((Obj)n.value, true);
                        if (n.type == Types.integer)
                            actualMemory.addUpdateValue((Obj)n.value, 0);
                    }
					break;
				case Labels.AssigDecl:
					Assignment (children, actualMemory);
					break;
				case Labels.Assig:
					Assignment (children, actualMemory);
					break;
				case Labels.Print:
                    if (children[0].isTerminal() && children[0].value is string)
                    {
                        /*char[] quotes = new char[1];
                        quotes[0] = '"';
                        string tmpString = ((string)children[0].value).TrimStart(quotes);
                        tmpString = tmpString.TrimEnd(quotes);*/
						Console.WriteLine(CONSOL_PRINT + children[0]);
                    }
					else{
						var tmp2 = InterpretExp(children[0], actualMemory);
						if (tmp2 is bool || tmp2 is int)
							Console.WriteLine (CONSOL_PRINT + Convert.ToString (tmp2));
						else
							throw new CantPrintFunction (children[0].column, children[0].line);
					}
					break;
				case Labels.Return:
					object tmp = InterpretExp (children [0], actualMemory);
					if (tmp is Tuple<ASTNode,Environment>){
						var afunTuple = (Tuple<ASTNode,Environment>) tmp;
						return new Tuple<ASTNode,Environment>(afunTuple.Item1,afunTuple.Item2.CloneMemory());
					}
					return tmp;
				}
			} 
			return null;
		}

		private void Assignment(List<ASTNode> children, Environment actualMemory){
			object value = InterpretExp (children [1], actualMemory);
			Obj variable = (Obj) children[0].value;
			if (value is Tuple<ASTNode,Environment>) {
				var tuple = (Tuple<ASTNode,Environment>)value;
				actualMemory.addUpdateValue (variable, tuple.Item1, tuple.Item2);
			}else
				actualMemory.addUpdateValue (variable, value);
		}

		private bool InterpretCondition (ASTNode node, Environment actualMemory)
		{
			return (bool) InterpretExp(node,actualMemory);
		}

		private int InterpretExpInt (ASTNode node, Environment actualMemory){
			return (int) InterpretExp(node,actualMemory);
		}

		private object InterpretExp (ASTNode node, Environment actualMemory)
		{
			if (node.isTerminal ()) {
				if (node.value is Obj) {
					object value = actualMemory.GetValue ((Obj)node.value);
					if (value is Task<object>) {
						Console.WriteLine (CONSOL_INFO + "while computing '" + node + "'...");
						return ((Task<object>)value).Result;
					} else if (value is Task<HttpResponseMessage>) {
						Console.WriteLine (CONSOL_INFO + "while computing '" + node + "' on server...");
						return HelperHttpClient.WaitResult ((Task<HttpResponseMessage>)value);
					}else
						return value;
				}else
					return node.value;
			} else {
				List<ASTNode> children = node.children;
				switch (node.label) {
				case Labels.Plus:
					return InterpretExpInt (children [0], actualMemory) + InterpretExpInt (children [1],actualMemory);
				case Labels.Minus:
					return InterpretExpInt (children [0],actualMemory) - InterpretExpInt (children [1],actualMemory);
				case Labels.Mul:
					return InterpretExpInt (children [0],actualMemory) * InterpretExpInt (children [1],actualMemory);
				case Labels.Div:
					return InterpretExpInt (children [0],actualMemory) / InterpretExpInt (children [1],actualMemory);
				case Labels.Eq:
					var tmp = InterpretExp (children [0], actualMemory);
					if (tmp is bool)
						return (bool) tmp == InterpretCondition (children [1],actualMemory);
					else
						return (int) tmp == InterpretExpInt (children [1],actualMemory);
                case Labels.Negativ:
                    return -(InterpretExpInt(children[0], actualMemory));                        
				case Labels.NotEq:
					var tmp1 = InterpretExp (children [0], actualMemory);
					if (tmp1 is bool)
						return (bool) tmp1 != InterpretCondition (children [1],actualMemory);
					else
						return (int) tmp1 != InterpretExpInt (children [1],actualMemory);				
				case Labels.Gt:
					return InterpretExpInt (children [0],actualMemory) > InterpretExpInt (children [1],actualMemory);
				case Labels.Gte:
					return InterpretExpInt (children [0],actualMemory) >= InterpretExpInt (children [1],actualMemory);
				case Labels.Lt:
					return InterpretExpInt (children [0],actualMemory) < InterpretExpInt (children [1],actualMemory);
				case Labels.Lte:
					return InterpretExpInt (children [0],actualMemory) <= InterpretExpInt (children [1],actualMemory);
				case Labels.And:
					return InterpretCondition (children [0], actualMemory) && InterpretCondition (children [1], actualMemory);
				case Labels.Or:
					return InterpretCondition (children [0], actualMemory) || InterpretCondition (children [1], actualMemory);
				case Labels.Bracket:
					return InterpretExp (children [0], actualMemory);
				case Labels.FunCall:
					return FunctionCall (FunParameterPassing(node,actualMemory));
				case Labels.Afun:
					return new Tuple<ASTNode,Environment> (node,actualMemory);
				case Labels.Read:
					Console.Write (CONSOL_READ);
					string read = Console.ReadLine ();
					try {
						Console.Write ("\n");
						return Convert.ToInt32 (read);
					} catch (FormatException) {
						throw new ReadNotIntegerValue (node.column, node.line);
					}
				case Labels.Async:
					var fun = FunParameterPassing (node.children[0], actualMemory);
					Task<object> task = Task.Run (() => {
						//Thread.Sleep(2000);
						return FunctionCall (fun);
					});
					return task;
				case Labels.Dsync:
					var parChildren = children [1].children;
					var funObj = (Obj)children [1].value;
					var formal = new List<string> ();
					var actual = new List<string> ();
					ASTNode funNode;
					if (function.TryGetValue (funObj, out funNode)) {
						int i = 1;
						foreach (var par in parChildren) {
							formal.Add (funNode.children[i].ToString());
							actual.Add (Convert.ToString(InterpretExp (par, actualMemory)));
							i++;
						}
						string json = HelperJson.Serialize(actual, formal, funNode.children [0]);
						var url = (string)actualMemory.GetValue ((Obj)children [0].value);
						//url = url.Substring(1,url.Length-2);
						return HelperHttpClient.PostAsyncRequest (url, json);
					}
					return null;
				default:
					return null;
				}
			}
		}

		private Tuple<ASTNode,Environment> FunParameterPassing(ASTNode node, Environment actualMemory){
			Obj funName = (Obj)node.value;
			ASTNode funNode;
			Environment funMem;
			if (function.TryGetValue (funName, out funNode)) {
				funMem = new Environment ();
			} else {
				var afun = (Tuple<ASTNode,Environment>) actualMemory.GetValue (funName); 
				funNode = afun.Item1;
				funMem = afun.Item2;
			}
			funMem.addScope ();
			int i = 1;
			foreach (ASTNode actual in node.children) {
				funMem.addUpdateValue ((Obj)funNode.children [i].value, InterpretExp (actual, actualMemory));
				i++;
			}
			return new Tuple<ASTNode,Environment> (funNode, funMem);
		}

		private object FunctionCall(Tuple<ASTNode,Environment> fun){
			object ret = Interpret (fun.Item1.children [0], fun.Item2);
			fun.Item2.removeScope ();
			return ret;
		}

	}
}